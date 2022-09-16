using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingGame.UI
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        
        bool m_IsConnecting;
        
        #endregion
        
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #region Private Fields

        
        string gameVersion = "1";


        #endregion


        #region MonoBehaviour CallBacks
        
        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        void Start()
        {
            Application.targetFrameRate = 60;
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }


        #endregion


        #region Public Methods
        
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                m_IsConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }


        #endregion
    
        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            if (m_IsConnecting)
            {
                SceneManager.LoadScene("Lobby");
                m_IsConnecting = false;
            }
        }
    
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            m_IsConnecting = false;
        }


        #endregion
    }
}
