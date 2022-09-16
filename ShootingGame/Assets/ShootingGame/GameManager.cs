using Photon.Pun;
using ShootingGame.Player;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ShootingGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region PUBLIC_VARS
        public static GameManager Instance;
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public GameObject myPlayer;
        public GameObject catPrefab;
        public GameObject birdPrefab;
        public Robot robot;
        #endregion

        #region PRIVATE_VARS
        #endregion

        #region UNITY_CALLBACKS
        void Start()
        {
            Application.targetFrameRate = 60;
            Instance = this;
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                SpawnPlayer();
                if (PlayerController.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    SpawnPlayer();
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        #endregion
        

        #region PRIVATE_FUNCTIONS
        private void SpawnPlayer()
        {
            float randomX = UnityEngine.Random.Range(-40 , 40);
            float randomZ = UnityEngine.Random.Range(-3 , 2);
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(randomX, 10f, randomZ), Quaternion.identity, 0);
            PhotonNetwork.Instantiate(this.catPrefab.name, new Vector3(randomX-0.5f, 5.1f, randomZ-1f), Quaternion.identity, 0);
            PhotonNetwork.Instantiate(this.birdPrefab.name, new Vector3(randomX+0.2f, 6.7f, randomZ +0.0f), Quaternion.identity, 0);
        }
        #endregion

        #region CO-ROUTINES
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion
        
        #region Photon Callbacks
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        #endregion


        #region Public Methods
        public void LeaveRoom()
        {
            Debug.Log("LeaveRoom");
            PhotonNetwork.LeaveRoom();
        }
        #endregion
    }
}
