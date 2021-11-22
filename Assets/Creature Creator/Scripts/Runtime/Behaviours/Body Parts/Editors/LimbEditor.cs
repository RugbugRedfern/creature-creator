﻿// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LimbEditor : BodyPartEditor
    {
        #region Fields
        private MeshRenderer[] toolRenderers;
        private SphereCollider[] toolColliders;
        #endregion

        #region Properties
        public LimbConstructor LimbConstructor => BodyPartConstructor as LimbConstructor;
        public LimbEditor FlippedLimb => Flipped as LimbEditor;

        public override bool IsInteractable
        {
            get => base.IsInteractable;
            set
            {
                base.IsInteractable = value;

                if (LimbConstructor.ConnectedExtremity != null)
                {
                    LimbConstructor.ConnectedExtremity.GetComponent<ExtremityEditor>().IsInteractable = value;
                }
            }
        }
        public override bool CanCopy
        {
            get
            {
                bool canCopy = base.CanCopy;

                if (LimbConstructor.ConnectedExtremity != null)
                {
                    canCopy &= EditorManager.Instance.CanAddBodyPart(LimbConstructor.ConnectedExtremity.AttachedBodyPart.bodyPartID);
                }

                return canCopy;
            }
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            toolRenderers = LimbConstructor.Root.GetComponentsInChildren<MeshRenderer>();
            toolColliders = LimbConstructor.Root.GetComponentsInChildren<SphereCollider>();
        }

        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            SetupInteraction();
            SetupBones();
            SetupConstructor();
        }
        private void SetupInteraction()
        {
            Hover.OnEnter.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    SetBonesVisibility(true);
                }
            });
            Hover.OnExit.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0) && !IsSelected)
                {
                    SetBonesVisibility(false);
                }
            });

            Scroll.OnScrollUp.RemoveAllListeners();
            Scroll.OnScrollUp.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    LimbConstructor.AddWeight(0, 5);
                    FlippedLimb.LimbConstructor.AddWeight(0, 5);
                    CreatureEditor.IsDirty = true;
                }
            });
            Scroll.OnScrollDown.RemoveAllListeners();
            Scroll.OnScrollDown.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    LimbConstructor.RemoveWeight(0, 5);
                    FlippedLimb.LimbConstructor.RemoveWeight(0, 5);
                    CreatureEditor.IsDirty = true;
                }
            });

            Drag.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    UpdateMeshCollider();
                    FlippedLimb.UpdateMeshCollider();
                }
            });
            Drag.OnEndDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    ExtremityConstructor connectedExtremity = LimbConstructor.ConnectedExtremity;
                    if (connectedExtremity != null)
                    {
                        connectedExtremity.transform.parent = connectedExtremity.Flipped.transform.parent = transform.parent;
                        connectedExtremity.UpdateAttachmentConfiguration();
                    }
                }
            });

            Select.OnSelect.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    SetBonesVisibility(IsSelected);
                }
            });
        }
        private void SetupBones()
        {
            for (int i = 0; i < LimbConstructor.Bones.Length; i++)
            {
                int index = i;

                Transform bone = LimbConstructor.Bones[index];

                if (index > 0)
                {
                    Scroll boneScroll = bone.gameObject.GetComponent<Scroll>();
                    boneScroll.OnScrollUp.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            LimbConstructor.AddWeight(index, 5);
                            FlippedLimb.LimbConstructor.AddWeight(index, 5);
                            CreatureEditor.IsDirty = true;
                        }
                    });
                    boneScroll.OnScrollDown.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            LimbConstructor.RemoveWeight(index, 5);
                            FlippedLimb.LimbConstructor.RemoveWeight(index, 5);
                            CreatureEditor.IsDirty = true;
                        }
                    });

                    Hover boneHover = bone.gameObject.GetComponent<Hover>();
                    boneHover.OnEnter.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                        {
                            CreatureEditor.CameraOrbit.Freeze();
                            SetBonesVisibility(true);
                        }
                    });
                    boneHover.OnExit.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                        {
                            CreatureEditor.CameraOrbit.Unfreeze();

                            if (!IsSelected)
                            {
                                SetBonesVisibility(false);
                            }
                        }
                    });

                    Drag boneDrag = bone.gameObject.GetComponent<Drag>();
                    boneDrag.world = Drag.world;
                    boneDrag.cylinderRadius = CreatureEditor.CreatureConstructor.MaxRadius;
                    if (index < LimbConstructor.Bones.Length - 1 || this is ArmEditor)
                    {
                        boneDrag.cylinderHeight = CreatureEditor.CreatureConstructor.MaxHeight;
                    }
                    boneDrag.OnPress.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            CreatureEditor.CameraOrbit.Freeze();

                            //if (index < LimbConstructor.Bones.Length - 1)
                            //{
                            //    Vector3 previousPos = LimbConstructor.Bones[index - 1].position;
                            //    Vector3 nextPos = LimbConstructor.Bones[index + 1].position;

                            //    boneDrag.maxDistance = Vector3.Distance(previousPos, nextPos) / 2f;
                            //    boneDrag.ClampFromPosition = (previousPos + nextPos) / 2f;
                            //}
                        }
                    });
                    boneDrag.OnDrag.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            Vector3 localBonePosition = CreatureEditor.transform.InverseTransformPoint(bone.position);
                            localBonePosition.x *= -1;
                            Vector3 worldBonePosition = CreatureEditor.transform.TransformPoint(localBonePosition);

                            FlippedLimb.LimbConstructor.Bones[index].position = worldBonePosition;
                        }
                    });
                    boneDrag.OnRelease.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            if (!boneHover.IsOver && !Hover.IsOver)
                            {
                                CreatureEditor.CameraOrbit.Unfreeze();
                            }

                            UpdateMeshCollider();
                            FlippedLimb.UpdateMeshCollider();

                            CreatureEditor.IsDirty = true;
                        }
                    });
                }
            }

            UpdateMeshCollider();
            SetBonesVisibility(false);
        }
        private void SetupConstructor()
        {
            LimbConstructor.OnSetWeight += delegate(int index, float weight)
            {
                toolRenderers[index].transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2, weight / 100f);

                UpdateMeshCollider();
            };
        }

        public override BodyPartEditor Copy()
        {
            LimbEditor copiedLE = base.Copy() as LimbEditor;

            // Limb
            LimbConstructor copiedLC = copiedLE.LimbConstructor;
            for (int i = 0; i < copiedLC.AttachedLimb.bones.Count; i++)
            {
                copiedLC.Bones[i].position = LimbConstructor.Bones[i].position;
                copiedLC.Bones[i].rotation = LimbConstructor.Bones[i].rotation;
                copiedLC.SetWeight(i, LimbConstructor.GetWeight(i));
            }

            // Connected Extremity
            ExtremityConstructor connectedExtremity = LimbConstructor.ConnectedExtremity;
            if (connectedExtremity != null)
            {
                ExtremityEditor copiedEE = connectedExtremity.GetComponent<ExtremityEditor>().Copy() as ExtremityEditor;

                copiedEE.Drag.IsDragging = copiedEE.Drag.IsPressing = false;
                copiedEE.IsSelected = false;

                copiedEE.ExtremityConstructor.ConnectToLimb(copiedLE.LimbConstructor);
                copiedEE.ExtremityConstructor.Flip();
            }

            return copiedLE;
        }

        public override void Deselect()
        {
            base.Deselect();

            SetBonesVisibility(false);
        }

        public void SetBonesVisibility(bool isVisible)
        {
            for (int i = 0; i < LimbConstructor.Bones.Length; i++)
            {
                toolRenderers[i].enabled = isVisible;
                toolColliders[i].enabled = isVisible;
            }
            toolColliders[0].enabled = false;
        }
        #endregion
    }
}