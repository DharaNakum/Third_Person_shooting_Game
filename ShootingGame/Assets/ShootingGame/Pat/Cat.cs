using Photon.Pun;
using ShootingGame.Player;
using UnityEngine;
using UnityEngine.AI;

namespace ShootingGame.Pat
{
    public class Cat : MonoBehaviourPunCallbacks
    {
        #region PUBLIC_VARS
            public Animator animator;
            public Vector3 offset;
            private UnityEngine.Camera m_MyCamera;
            public NavMeshAgent agent;
            public GameObject target;
        #endregion

        #region PRIVATE_VARS
            private bool m_IsBarking;
            private bool m_Barked;
            private PlayerController m_EnemyPlayerController;
            private int m_LayerA;
            private int m_LayerB;
            private int m_LayerMaskCombined;
            private static readonly int Speed = Animator.StringToHash("Speed");

            #endregion

        #region UNITY_CALLBACKS
            private void Start()
            {
                if (!photonView.IsMine)
                {
                    return;
                }

                m_MyCamera = UnityEngine.Camera.main;
                if (GameManager.Instance.myPlayer != null)
                {
                    target = GameManager.Instance.myPlayer;
                }

                InitializeVariable();
            }
            private void Update()
            {
                if (!photonView.IsMine)
                {
                    return;
                }
                if (m_IsBarking && agent.remainingDistance < 0.1 && !m_Barked)
                {
                    m_EnemyPlayerController.RpcFrozenCllBack();
                    m_Barked = true;
                }
                if (m_IsBarking && agent.remainingDistance < 0.001)
                {
                    m_IsBarking = false;
                    m_Barked = false;
                }
                CatTarget();
                animator.SetFloat(Speed, agent.remainingDistance * 2);
                CatInput();
            }
        #endregion

        #region PUBLIC_FUNCTIONS
        #endregion

        #region PRIVATE_FUNCTIONS
            private void CatTarget()
            {
                if (!m_IsBarking)
                {
                    agent.SetDestination(target.transform.position + offset);
                }
                else
                {
                    if (m_EnemyPlayerController != null)
                    {
                        agent.SetDestination(m_EnemyPlayerController.transform.position +
                                             new Vector3(offset.x + 1, offset.y, 0));
                    }
                }
            }
            
            private void InitializeVariable()
            {
                m_IsBarking = false;
                m_Barked = false;
                m_LayerA = 8;
                m_LayerB = 9;
                m_LayerMaskCombined = (1 << m_LayerA) | (1 << m_LayerB);
                m_LayerMaskCombined = ~m_LayerMaskCombined;
            }
            
            private void CatInput()
            {
                if (Input.touchCount > 0 || Input.GetMouseButtonDown(1))
                {
                    if (!PlayerController.Instance.isCatActive)
                    {
                        return;
                    }

                    AttackByCat();
                }
            }

            private void AttackByCat()
            {
                Ray ray = m_MyCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMaskCombined))
                {
                    Debug.Log(hit.transform.name);
                    m_EnemyPlayerController = hit.transform.gameObject.GetComponent<PlayerController>();
                    if (m_EnemyPlayerController.photonView.IsMine)
                    {
                        m_EnemyPlayerController = null;
                        return;
                    }

                    if (m_EnemyPlayerController != null)
                    {
                        m_IsBarking = true;
                        agent.SetDestination(hit.transform.position + new Vector3(offset.x + 1, offset.y, 0));
                    }
                }
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
