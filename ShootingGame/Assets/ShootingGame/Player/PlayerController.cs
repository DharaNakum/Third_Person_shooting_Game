using System.Collections;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using ShootingGame.Camera;
using ShootingGame.PlayFab;
using ShootingGame.UI;
using UnityEngine;

namespace ShootingGame.Player
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region PUBLIC_VARS

        public static PlayerController Instance;
        public ParticleSystem fireParticle;
        public ParticleSystem frozenParticles;
        public ParticleSystem deathParticles;
        public ParticleSystem blastParticle;
        public PlayerMovement playerMovement;
        public RecoilGun recoilGun;
        public GameObject playerUiPrefab;
        public GameObject crossHairPrefab;
        public GameObject damageEffectPrefab;
        public static GameObject LocalPlayerInstance;
        public GameObject target;
        public GameObject gunPoint;
        public int duration;
        public bool isMobileInputEnable;
        [HideInInspector] public int deathCount;
        [HideInInspector] public int killCount;
        [HideInInspector] public int score;
        [HideInInspector] public float health = 1f;
        [HideInInspector] public bool isMoving;
        [HideInInspector] public bool isFiring;
        [HideInInspector] public bool isCatActive;
        [HideInInspector] public bool isBirdActive;
        public SkinnedMeshRenderer bodyMesh;
        public SkinnedMeshRenderer headMesh;
        public Material[] skins;
        #endregion

        #region Private Fields

        private bool m_IsExplosion;
        private GameObject m_DamageEffect;
        private float m_Time;
        private int m_LayerA;
        private int m_LayerB;
        private int m_LayerMaskCombined;
        private Vector3 m_CameraForward;
        private bool m_IsInitialize;
        private bool m_IsDeath;
        private bool m_IsKilled;
        private int m_RemainingDuration;
        private CmFreeLookCamera m_CmFreeLookCamera;
        private CameraControl m_CameraControl;
        private UnityEngine.Camera m_Camera;
        private int m_MySkin;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            m_Camera = UnityEngine.Camera.main;
            if (photonView.IsMine)
            {
                PlayerController.LocalPlayerInstance = this.gameObject;
                if (m_Camera != null)
                {
                    m_CameraControl = m_Camera.gameObject.GetComponent<CameraControl>();
                    m_CmFreeLookCamera = m_CameraControl.cmFreeLook.GetComponent<CmFreeLookCamera>();
                }

                //score = PlayFabManager.Instance.GetScore();
            }

            if (gunPoint == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                gunPoint.SetActive(false);
            }

            InitializeVariable();
        }

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (m_CameraControl != null)
            {
                m_CameraControl.GetTargetPlayer();
            }
            //PlayerChangeClothes.Instance.ChangeSkin(PersistentData.Instance.mySkin);
            if (photonView.IsMine)
            {
                m_MySkin = PersistentData.Instance.mySkin;
                //ChangePlayerSkin(m_MySkin);
                photonView.RPC("ChangeSkin",RpcTarget.AllBuffered,m_MySkin);
            }
            
            /*if (photonView.IsMine)
            {
                photonView.RPC("ChangePlayerSkin",RpcTarget.Others,PersistentData.Instance.mySkin);
            }*/
            PlayerUi();
            InstantiateCrossHair();
            InstantiateDamageEffect();
            StartTimer();
        }

        void Update()
        {
            if (m_IsDeath)
            {
                return;
            }

            if (photonView.IsMine)
            {
                if (isMoving)
                {
                    ProcessInputs();
                }
            }

            m_Time += Time.deltaTime;
            if (gunPoint != null && isFiring != gunPoint.activeInHierarchy)
            {
                gunPoint.SetActive(isFiring);
            }

            if (gunPoint.activeSelf && isFiring)
            {
                if (m_Camera != null)
                {
                    m_CameraForward = m_Camera.transform.forward;
                }

                m_IsInitialize = true;
                Fire();
            }
        }
       

        #endregion

        #region PUBLIC_FUNCTIONS

        [PunRPC]
        public void ShowResult()
        {
            if (killCount >= deathCount)
            {
                UiManager.Instance.ActiveVictoryPanel();
                PlayFabManager.Instance.GrantVirtualCurrency(10);
            }
            else
            {
                UiManager.Instance.ActiveDefeatedPanel();
                PlayFabManager.Instance.GrantVirtualCurrency(5);
            }
            PlayFabManager.Instance.SendLeaderBoard(score);
        }
        [PunRPC]
        public void ChangeSkin(int index)
        {
            if (index == 0)
            {
                return;
            }

            index--;
            bodyMesh.material = skins[index];
            headMesh.material = skins[index];
        }

        public void RpcFrozenCllBack()
        {
            if (m_IsDeath)
            {
                return;
            }

            photonView.RPC("Frozen", RpcTarget.All);
        }

        [PunRPC]
        public void RpcFireCllBack()
        {
            if (m_IsDeath)
            {
                return;
            }

            photonView.RPC("ApplyDamage", RpcTarget.All);
        }

        [PunRPC]
        public void ApplyDamage()
        {
            if (m_IsDeath)
            {
                return;
            }

            deathParticles.Play();
            if (!photonView.IsMine)
            {
                return;
            }

            health -= 0.1f;
            if (health <= 0f)
            {
                if (m_IsDeath)
                {
                    return;
                }

                deathCount++;
                //score -= 10;
                m_IsDeath = true;
                isMoving = false;
                playerMovement.ActiveDeathState(false);
                UiManager.Instance.gamePlayView.SetDeathCount(deathCount);
            }

            m_DamageEffect.SetActive(true);
            StartCoroutine(nameof(DamageEffectActiveTime));
        }

        public void RpcBlastCallBack()
        {
            if (m_IsDeath)
            {
                return;
            }

            photonView.RPC("BlastDamage", RpcTarget.All);
        }

        public void BoostHealth()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            health = 1.0f;
        }

        public void StartShooting()
        {
            if (!isFiring)
            {
                m_Time = 0;
                isFiring = true;
                playerMovement.TurnPlayerTowardsCamera();
                m_CameraControl.LookAtGunPoint();
                m_CmFreeLookCamera.ZoomIn();
            }
        }

        public void StopShooting()
        {
            if (isFiring)
            {
                isFiring = false;
                m_CameraControl.LookAtTarget();
                m_CmFreeLookCamera.ZoomOut();
            }
        }

        public void ActiveBird()
        {
            isCatActive = false;
            isBirdActive = true;
            StartCoroutine(DeactivatePats());
        }

        public void ActiveCat()
        {
            isCatActive = true;
            isBirdActive = false;
            StartCoroutine(DeactivatePats());
        }

        public void SpawnPlayerAfterDeath()
        {
            float randomX = UnityEngine.Random.Range(-40, 40);
            float randomZ = UnityEngine.Random.Range(-3, 2);
            gameObject.transform.position = new Vector3(randomX, 10f, randomZ);
            m_IsDeath = false;
            isMoving = true;
            health = 1;
        }
        public void GetLeaderBoard()
        {
            var requestLeaderBoard = new GetLeaderboardRequest{StartPosition = 0,StatisticName = "Score"};
            PlayFabClientAPI.GetLeaderboard(requestLeaderBoard,OnGetLeaderBoard,null);
        }

        private void OnGetLeaderBoard(GetLeaderboardResult result)
        {
            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                Debug.Log(photonView.IsMine);
                if (player.DisplayName == PhotonNetwork.NickName)
                {
                    Debug.Log("1235246578uybh dsxcweasd x "+player.StatValue);
                    score = player.StatValue;
                }
            }
        }

        public void CalculateKillCount(PlayerController playerController,float damage)
        {
            Debug.Log("Health " + playerController.health);
            if (playerController != null)
            {
                if (playerController.health > 0.2f)
                {
                    m_IsKilled = false;
                }
            }

            if (playerController != null)
            {
                if (playerController.health - damage < 0.09999995 && !m_IsKilled)
                {
                    Debug.Log("*** Health " + playerController.health + " " + ( playerController.health - damage));
                    m_IsKilled = true;
                    killCount++;
                    score += 10;
                    UiManager.Instance.gamePlayView.SetKillCount(killCount);
                }
            }
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Fire()
        {
            if (m_Time > 0.25f)
            {
                m_Time = 0;
                if (fireParticle != null)
                {
                    fireParticle.Play();
                }

                if (photonView.IsMine)
                {
                    GenerateRay();
                }
            }
        }

        private void GenerateRay()
        {
            RaycastHit hit;
            Debug.DrawRay(gunPoint.transform.position, m_CameraForward, Color.green, 10.0f);
            if (Physics.Raycast(gunPoint.transform.position, m_CameraForward, out hit, Mathf.Infinity,
                m_LayerMaskCombined))
            {
                PlayerController playerController = hit.transform.gameObject.GetComponent<PlayerController>();
                CalculateKillCount(playerController,0.1f);
                if (playerController != null)
                {
                    playerController.RpcFireCllBack();
                }
            }

            recoilGun.Recoil();
        }

        private IEnumerator DamageEffectActiveTime()
        {
            yield return new WaitForSeconds(0.2f);
            m_DamageEffect.SetActive(false);
        }

        private void Being(int seconds)
        {
            m_RemainingDuration = seconds;
            StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while (m_RemainingDuration >= 0)
            {
                UiManager.Instance.gamePlayView.SetTimer(m_RemainingDuration);
                m_RemainingDuration--;
                yield return new WaitForSeconds(1f);
            }
        }

        [PunRPC]
        private void Frozen()
        {
            frozenParticles.Play();
            isMoving = false;
            StartCoroutine(StopMovement());
        }

        private void PlayerUi()
        {
            if (playerUiPrefab != null)
            {
                GameObject uiGo = Instantiate(playerUiPrefab);
                uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }

        private void StartTimer()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Being(duration);
                StartCoroutine(Timer(duration));
            }
        }

        [PunRPC]
        private void BlastDamage()
        {
            blastParticle.Play();
            if (!photonView.IsMine)
            {
                return;
            }

            if (m_IsDeath)
            {
                return;
            }

            health -= 0.3f;
            if (health <= 0f)
            {
                if (m_IsDeath)
                {
                    return;
                }

                deathCount++;
                //score -= 10;
                m_IsDeath = true;
                isMoving = false;
                playerMovement.ActiveDeathState(false);
                UiManager.Instance.gamePlayView.SetDeathCount(deathCount);
            }

            m_DamageEffect.SetActive(true);
            StartCoroutine(nameof(DamageEffectActiveTime));
        }

        private void InitializeVariable()
        {
            isCatActive = false;
            isBirdActive = false;
            isFiring = false;
            isMoving = true;
            m_IsDeath = false;
            m_Time = 0.01f;
            m_LayerA = 8;
            m_LayerB = 9;
            m_LayerMaskCombined = (1 << m_LayerA) | (1 << m_LayerB);
            m_LayerMaskCombined = ~m_LayerMaskCombined;
            m_IsInitialize = true;
            killCount = 0;
            deathCount = 0;
            GetLeaderBoard();
            UiManager.Instance.gamePlayView.SetKillCount(killCount);
        }
        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartShooting();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                StopShooting();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ActiveCat();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                ActiveBird();
            }
        }

        private void InstantiateCrossHair()
        {
            if (photonView.IsMine)
            {
                Instantiate(this.crossHairPrefab);
            }
        }

        private void InstantiateDamageEffect()
        {
            if (photonView.IsMine)
            {
                m_DamageEffect = Instantiate(this.damageEffectPrefab);
                m_DamageEffect.SetActive(false);
            }
        }

        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isFiring);
                stream.SendNext(health);
                stream.SendNext(m_RemainingDuration);
            }
            else
            {
                this.isFiring = (bool) stream.ReceiveNext();
                this.health = (float) stream.ReceiveNext();
                this.m_RemainingDuration = (int) stream.ReceiveNext();
                if (!photonView.IsMine && PhotonNetwork.IsMasterClient)
                {

                }
                else
                {
                    UiManager.Instance.gamePlayView.SetTimer(m_RemainingDuration);
                }
            }
        }

        #endregion

        #region CO-ROUTINES

        private IEnumerator Timer(int time)
        {
            yield return new WaitForSeconds(time);
            photonView.RPC("ShowResult", RpcTarget.All);
        }

        private IEnumerator StopMovement()
        {
            yield return new WaitForSeconds(5);
            frozenParticles.Stop();
            isMoving = true;
        }

        private IEnumerator DeactivatePats()
        {
            yield return new WaitForSeconds(10.0f);
            isCatActive = false;
            isBirdActive = false;
        }

        #endregion

        #region Custom

        #endregion
    }
}
