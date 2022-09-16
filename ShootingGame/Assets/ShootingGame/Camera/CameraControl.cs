using Cinemachine;
using Photon.Pun;
using ShootingGame.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootingGame.Camera
{
    public class CameraControl : MonoBehaviour
    {
        #region PUBLIC_VARS
            public GameObject player;
            public CinemachineFreeLook cmFreeLook;
            public MiniMap miniMap;
        #endregion

        #region PRIVATE_VARS
            private Vector3 m_CurrentVal;
            private GameObject m_Target;
            private GameObject m_GunPoint;
        #endregion

        #region UNITY_CALLBACKS
        #endregion

        #region PUBLIC_FUNCTIONS
            public void GetTargetPlayer()
            {
                player = GetLocalPlayer();
                GameManager.Instance.myPlayer = player;
                m_Target = player.GetComponent<PlayerController>().target;
                m_GunPoint = player.GetComponent<PlayerController>().gunPoint;
                if(player==null)
                {
                    this.gameObject.SetActive(false);
                    return;
                }
                cmFreeLook.Follow = player.transform;
                cmFreeLook.LookAt = m_Target.transform;
                miniMap.SetPlayer(player.transform);
            }

            public void LookAtGunPoint()
            {
                cmFreeLook.LookAt = m_GunPoint.transform;
            }

            public void LookAtTarget()
            {
                cmFreeLook.LookAt = m_Target.transform;
            }
        #endregion

        #region PRIVATE_FUNCTIONS
            private GameObject GetLocalPlayer()
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject player in players)
                {
                    if(player.GetComponent<PhotonView>().IsMine)
                    {
                        return player;
                    }
                }
                return null;
            }
        #endregion

        #region CO-ROUTINES
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion
        
    
       

      
        
    
    }
}
