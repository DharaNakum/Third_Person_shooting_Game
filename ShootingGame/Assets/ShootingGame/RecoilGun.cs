using System.Collections;
using UnityEngine;

namespace ShootingGame
{
    public class RecoilGun : MonoBehaviour
    {
        private float m_Time;
        private Vector3 m_LastPosition;
        private Quaternion m_LastRotation;
        private Vector3 m_OriginalPosition;
        private Quaternion m_OriginalRotation;
        public float traumaExponent = 1;
        public Vector3 maximumAngularShake = Vector3.one * 0.05f;
        public Vector3 maximumTranslationShake = Vector3.one * 0.07f;

        private void Start()
        {
            m_LastPosition = transform.localPosition;
            m_LastRotation = transform.rotation;
            m_OriginalPosition = m_LastPosition;
            m_OriginalRotation = transform.rotation;
            maximumAngularShake = Vector3.one * 0.05f;
            maximumTranslationShake = Vector3.one * 0.07f;
        }

        public void Recoil()
        {
            StartCoroutine(GunRecoil(0.1f));
        }

        private IEnumerator GunRecoil(float speed)
        {
            m_Time = 1 / speed;
            Vector3 previousPosition =  m_LastPosition;
            Quaternion previousRotation =  m_LastRotation;
            for (float i = 0; i <= 1; i += (Time.deltaTime * m_Time))
            {
                previousRotation = m_LastRotation;
                previousPosition = m_LastPosition;
                m_LastPosition = new Vector3(
                    maximumTranslationShake.x * (Mathf.PerlinNoise(0, Time.time * 25) * 2 - 1),
                    previousPosition.y,
                    previousPosition.z
                ) * 0.5f;
            
                transform.localPosition += m_LastPosition - previousPosition;
                yield return new WaitForSeconds(0.005f);
            }

            transform.localPosition = m_OriginalPosition;
            transform.localRotation = m_OriginalRotation;
        }
    }
}
