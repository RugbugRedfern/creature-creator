using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MouseHintDrag : MouseHint
    {
        #region Fields
        [Header("Desktop")]
        [SerializeField] private Hint lmb;
        [SerializeField] private Hint rmb;

        [Header("Handheld")]
        [SerializeField] private Hint finger;
        #endregion

        #region Methods
        public void Setup(int button, Transform sT, Transform eT, bool sInWorld, bool eInWorld, float t1 = 1f, float t2 = 2f, float t3 = 1f)
        {
            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                Setup(button == 0 ? lmb : rmb);
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                Setup(finger);
            }

            StartCoroutine(AnimateRoutine(sT, eT, sInWorld, eInWorld, t1, t2, t3));
        }

        private IEnumerator AnimateRoutine(Transform sT, Transform eT, bool sInWorld, bool eInWorld, float t1, float t2, float t3)
        {
            while (true)
            {
                if (sT == null || eT == null) yield break;

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    transform.position = GetPosition(sT, sInWorld);
                    transform.localScale = Vector3.Lerp(Vector3.one, 0.9f * Vector3.one, p);
                },
                t1);
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    transform.position = Vector3.Lerp(GetPosition(sT, sInWorld), GetPosition(eT, eInWorld), p);
                },
                t2);
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    transform.position = GetPosition(eT, eInWorld);
                    transform.localScale = Vector3.Lerp(0.9f * Vector3.one, Vector3.one, p);
                },
                t3);
            }
        }
        #endregion
    }
}