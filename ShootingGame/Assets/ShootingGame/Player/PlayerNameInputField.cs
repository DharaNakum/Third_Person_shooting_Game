using Photon.Pun;
using TMPro;
using UnityEngine;

namespace ShootingGame.Player
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants


        // Store the PlayerPref Key to avoid typos
        const string PlayerNamePrefKey = "PlayerName";


        #endregion
    
    


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start () 
        {
            string defaultName = string.Empty;
            TMP_InputField _inputField = this.GetComponent<TMP_InputField>(); 
        
            if (_inputField!=null)
            {
                if (PlayerPrefs.HasKey(PlayerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }


            PhotonNetwork.NickName =  defaultName;
            //Debug.Log("input field  " + _inputField.text );
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // #Important
            // Debug.Log(value);
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;
       

            PlayerPrefs.SetString(PlayerNamePrefKey,value);
        }


        #endregion
    }
}
