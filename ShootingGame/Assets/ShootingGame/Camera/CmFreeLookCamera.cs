using System.Collections;
using Cinemachine;
using UnityEngine;

namespace ShootingGame.Camera
{
    public class CmFreeLookCamera : MonoBehaviour
    {
        public CinemachineFreeLook freeLookCamera;
        public float midOrbitOffset;
        public float otherOrbitOffset;
        public float heightOffset;
        public float speed = 0.4f;
        public FixedTouchField fixedTouchField;
        private float m_Time;
        private float m_CurrentMidRadius;
        private float m_NewMidRadius;
        private float m_CurrentTopRadius;
        private float m_NewTopRadius;
        private float m_CurrentBottomRadius;
        private float m_CurrentMiddleHeight;
        private float m_NewBottomRadius;
        private float m_NewMiddleHeight;
        void Start(){
            CinemachineCore.GetInputAxis = GetAxisCustom;
            midOrbitOffset = 3.2f;
            otherOrbitOffset = 1.0f;

            speed = 0.4f;
        }
        public float GetAxisCustom(string axisName){
            if(axisName == "Mouse X"){
                if (Input.GetMouseButton(0)){
                    return -(fixedTouchField.TouchDist.x/10);
                } else{
                    return 0;
                }
            }
            if (axisName == "Mouse Y"){
                if (Input.GetMouseButton(0)){
                    return -(fixedTouchField.TouchDist.y/10);
                } else{
                    return 0;
                }
            }
            return UnityEngine.Input.GetAxis(axisName);
        }

        public void ZoomIn()
        {
            m_CurrentMidRadius = freeLookCamera.m_Orbits[1].m_Radius;
            m_CurrentTopRadius = freeLookCamera.m_Orbits[0].m_Radius;
            m_CurrentBottomRadius = freeLookCamera.m_Orbits[2].m_Radius;
            m_CurrentMiddleHeight = freeLookCamera.m_Orbits[1].m_Height;
            m_NewTopRadius = m_CurrentTopRadius - otherOrbitOffset;
            m_NewMidRadius = m_CurrentMidRadius - midOrbitOffset;
            m_NewBottomRadius = m_CurrentBottomRadius - otherOrbitOffset;
            m_NewMiddleHeight = m_CurrentMiddleHeight - heightOffset;
            StartCoroutine(ZoomCamera());
        }
        public void ZoomOut()
        {
            //freeLookCamera.m_Orbits[1].m_Radius = 5;
            m_CurrentMidRadius = freeLookCamera.m_Orbits[1].m_Radius;
            m_CurrentTopRadius = freeLookCamera.m_Orbits[0].m_Radius;
            m_CurrentBottomRadius = freeLookCamera.m_Orbits[2].m_Radius;
            m_CurrentMiddleHeight = freeLookCamera.m_Orbits[1].m_Height;
            m_NewTopRadius = m_CurrentTopRadius + otherOrbitOffset;
            m_NewMidRadius = m_CurrentMidRadius + midOrbitOffset;
            m_NewBottomRadius = m_CurrentBottomRadius + otherOrbitOffset;
            m_NewMiddleHeight = m_CurrentMiddleHeight + heightOffset;
            StartCoroutine("ZoomCamera");
        }

        public IEnumerator ZoomCamera()
        {
            m_Time = 1 / speed;
            for (float i = 0.0f; i <= 1; i += (Time.deltaTime * m_Time))
            {
                //Debug.Log("Value of i " + i);
                m_CurrentTopRadius = Mathf.Lerp(m_CurrentTopRadius, m_NewTopRadius, i);
                m_CurrentMidRadius = Mathf.Lerp(m_CurrentMidRadius, m_NewMidRadius, i);
                m_CurrentBottomRadius = Mathf.Lerp(m_CurrentBottomRadius, m_NewBottomRadius, i);
                m_CurrentMiddleHeight = Mathf.Lerp(m_CurrentMiddleHeight, m_NewMiddleHeight, i);
                freeLookCamera.m_Orbits[0].m_Radius = m_CurrentBottomRadius;
                freeLookCamera.m_Orbits[1].m_Radius = m_CurrentMidRadius;
                freeLookCamera.m_Orbits[2].m_Radius = m_CurrentBottomRadius;
                freeLookCamera.m_Orbits[1].m_Height = m_CurrentMiddleHeight;
                yield return null;
            }
            freeLookCamera.m_Orbits[0].m_Radius = m_NewBottomRadius;
            freeLookCamera.m_Orbits[1].m_Radius = m_NewMidRadius;
            freeLookCamera.m_Orbits[2].m_Radius = m_NewTopRadius;
        }
    }
}
