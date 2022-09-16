using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using ShootingGame.Player;
namespace ShootingGame.Pat
{
    public class Bird : MonoBehaviourPunCallbacks
    {
        #region PUBLIC_VARS
            public Animator birdAnimator;
            public GameObject birdTarget;
            public NavMeshAgent birdAgent;
            public Vector3 birdOffset;
        #endregion

        #region PRIVATE_VARS
            private UnityEngine.Camera m_MyCamera;
            private bool m_Peaking;
            private bool m_Peaked;
            private PlayerController m_EnemyPlayerController;
            private int m_RandomNumber;
            private bool m_IsRandomFly;
            private float m_Time;
            private int m_LayerA;
            private int m_LayerB;
            private int m_LayerMaskCombined;
            private static readonly int Blend = Animator.StringToHash("Blend");

            #endregion

        #region UNITY_CALLBACKS
            void Start()
            {
                if(!photonView.IsMine)
                {
                    return;
                }
                if(GameManager.Instance.myPlayer != null)
                {
                    birdTarget = GameManager.Instance.myPlayer;
                }
                InitializeVariable();
            }
            void Update()
            {
                if(!photonView.IsMine)
                {
                    return;
                }
                if (m_IsRandomFly)
                {
                    return;
                }
            
                if (m_Peaking && birdAgent.remainingDistance<0.001)
                {
                    m_EnemyPlayerController.RpcFireCllBack();
                    PlayerController.Instance.CalculateKillCount(m_EnemyPlayerController,0.1f);
                    m_Peaking = false;
                }
                BirdTarget();
                BirdAnimation();
                BirdInput();
            }
            #endregion

        #region PUBLIC_FUNCTIONS
        
            
            public void RandomFly()
            {
                //birdAgent.SetDestination(new Vector3(-10, transform.position.y, 10));
                /*Debug.Log("randomfly");
            if (m_RandomNumber > 0)
            {
                m_RandomNumber--;
                float randomTime = Random.Range(5f, 8f);
                StartCoroutine(FlyRandomly(randomTime));
            }
            else
            {
                m_IsRandomFly = false;
                birdAgent.baseOffset = 0.5f;
            }*/
                float randomTime = Random.Range(5f, 8f);
                StartCoroutine(FlyRandomly(randomTime));
            }
        #endregion

        #region PRIVATE_FUNCTIONS
            private void InitializeVariable()
            {
                m_MyCamera = UnityEngine.Camera.main;
                birdOffset = new Vector3(0.2f, 0, -0.2f);
                m_Peaking = false;
                m_IsRandomFly = false;
                m_LayerA = 8;
                m_LayerB = 9;
                m_LayerMaskCombined = (1 << m_LayerA) | (1 << m_LayerB);
                m_LayerMaskCombined = ~m_LayerMaskCombined;
            }
            private void BirdAnimation()
            {
                if (birdAgent.remainingDistance > 0.1f)
                {
                    birdAnimator.SetFloat(Blend, 1);
                }
                else
                {
                    birdAnimator.SetFloat(Blend, birdAgent.remainingDistance);
                }
            }
            private void BirdTarget()
            {
                if (!m_Peaking)
                {
                    birdAgent.SetDestination(birdTarget.transform.position + birdOffset);
                }
                else
                {
                    if (m_EnemyPlayerController != null)
                    {
                        birdAgent.SetDestination(m_EnemyPlayerController.transform.position +
                                                 new Vector3(birdOffset.x - 0.2f, birdOffset.y, 0));
                    }
                }
            }
            private void BirdInput()
            {
                if (Input.touchCount > 0 || Input.GetMouseButtonDown(1))
                {
                    if (!birdTarget.GetComponent<PlayerController>().isBirdActive)
                    {
                        return;
                    }

                    AttackByBird();
                }
            }
            private void AttackByBird()
            {
                Ray ray = m_MyCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,Mathf.Infinity,m_LayerMaskCombined))
                {
                    m_EnemyPlayerController = hit.transform.gameObject.GetComponent<PlayerController>();
                    if (m_EnemyPlayerController.photonView.IsMine)
                    {
                        m_EnemyPlayerController = null;
                        return;
                    }
                    if (m_EnemyPlayerController != null)
                    {
                        m_Peaking = true;
                        birdAgent.SetDestination(hit.transform.position + new Vector3(birdOffset.x-0.2f, birdOffset.y, 0));
                    }
                }
            }
        #endregion

        #region CO-ROUTINES
            private IEnumerator FlyRandomly(float speed)
            {
                m_IsRandomFly = true;
                Debug.Log("coroutine");
                m_Time = 1 / speed;
                /*float positionX = m_EnemyPlayerManager.transform.position.x;
            float positionZ = m_EnemyPlayerManager.transform.position.z;
            float randomX = Random.Range(-40 , 40);
            float randomZ = Random.Range(-40 , 40);*/
                float offset = Random.Range(0.6f, 1.3f);
                //Debug.Log(new Vector3(10,transform.position.y,-10));
                birdAgent.SetDestination(new Vector3(-10, transform.position.y, 10));
                for (float i = 0; i <= 1; i += (Time.deltaTime * m_Time))
                {
                    birdAgent.baseOffset = Mathf.Lerp(birdAgent.baseOffset, offset, i);
                    //Debug.Log(birdAgent.baseOffset);
                    yield return null;
                }
            
                //RandomFly();
            }
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion

    }
}
