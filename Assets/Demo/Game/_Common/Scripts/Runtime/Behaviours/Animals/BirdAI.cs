// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BirdAI : AnimalAI
    {
        #region Methods
        public void Frighten(Collider col)
        {
            CreatureBase other = col.GetComponent<CreatureBase>();
            if (other != null && other != Creature)
            {
                ChangeState("FLY");
            }
        }
        #endregion

        #region Nested
        [Serializable]
        public class Flying : BaseState
        {
            [SerializeField] public Transform perchPoints;
            [SerializeField] private float flightSpeed;
            [SerializeField] private float flightHeight;
            [SerializeField] private AnimationCurve flightPath;
            [SerializeField] private float shockTime;
            [SerializeField] private float minDistanceFromCreature;
            [SerializeField] private CreatureEffector.Sound[] flapSounds;

            public BirdAI BirdAI => StateMachine as BirdAI;

            public Transform RandomPerchPoint
            {
                get
                {
                    List<Transform> points = new List<Transform>();
                    foreach (Transform point in perchPoints)
                    {
                        bool isFarEnough = true;
                        foreach (CreatureBase creature in FindObjectsOfType<CreatureBase>())
                        {
                            if (!Vector3Utility.SqrDistanceComp(creature.transform.position, point.position, minDistanceFromCreature))
                            {
                                isFarEnough = false;
                                break;
                            }
                        }
                        if (isFarEnough)
                        {
                            points.Add(point);
                        }
                    }

                    return points[UnityEngine.Random.Range(0, points.Count)];
                }
            }

            public override void Enter()
            {
                BirdAI.StartCoroutine(FlyToPositionRoutine(RandomPerchPoint.position));
                BirdAI.Params.SetBool("Body_IsFlying", true);
                BirdAI.Animator.GetBehaviour<Animations.Flying>().OnFlap += OnFlap;
            }
            public override void Exit()
            {
                BirdAI.Params.SetBool("Body_IsFlying", false);
                BirdAI.Animator.GetBehaviour<Animations.Flying>().OnFlap -= OnFlap;
            }

            private IEnumerator FlyToPositionRoutine(Vector3 to)
            {     
                // Shock
                BirdAI.Params.SetBool("Eye_IsDilated", true);

                yield return new WaitForSeconds(shockTime);

                // Turn away
                Quaternion cur = BirdAI.transform.rotation;
                Quaternion tar = Quaternion.LookRotation(Vector3.ProjectOnPlane(to - BirdAI.transform.position, Vector3.up));
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    BirdAI.transform.rotation = Quaternion.Slerp(cur, tar, progress);
                }, 1f);

                // Fly away
                Vector3 from = BirdAI.transform.position;
                float flightDistance = Vector3.Distance(from, to);
                float flightTime = flightDistance / flightSpeed;
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    BirdAI.transform.position = Vector3.Lerp(from, to, progress) + (Vector3.up * (flightHeight * flightPath.Evaluate(progress)));
                }, flightTime);

                // Perch
                BirdAI.Params.SetBool("Eye_IsDilated", false);
                BirdAI.ChangeState("IDL");
            }

            public void OnFlap()
            {
                BirdAI.Creature.Effector.PlaySound(flapSounds);
            }
        }
        #endregion
    }
}