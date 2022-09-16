using System;
using ShootingGame.Player;
using UnityEngine;
using UnityEngine.UI;

namespace ShootingGame.PlayFab
{
    public class MenuController : MonoBehaviour
    {
        #region PUBLIC_VARS

        public static MenuController Instance;
        public GameObject shopPanel;
        public GameObject lobbyPanel;
        public GameObject leaderBoardPanel;
        public GameObject[] buttonLocks;
        public Button[] unlockedButtons;

        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            SetUpStore();
        }

        #endregion

        #region PUBLIC_FUNCTIONS
        public void SetUpStore()
        {
            for (int i = 0; i < PersistentData.Instance.allSkins.Length; i++)
            {
                buttonLocks[i].SetActive(!PersistentData.Instance.allSkins[i]);
                unlockedButtons[i].interactable = PersistentData.Instance.allSkins[i];
            }
        }

        public void UnlockSkin(int index)
        {
            PersistentData.Instance.allSkins[index] = true;
            PlayFabManager.Instance.SetUserData(PersistentData.Instance.SkinsDataToString());
            SetUpStore();
        }

        public void OpenShop()
        {
            lobbyPanel.SetActive(false);
            leaderBoardPanel.SetActive(false);
            shopPanel.SetActive(true);
            
        }

        public void CloseShop()
        {
            lobbyPanel.SetActive(true);
            shopPanel.SetActive(false);
        }

        public void SetMySkin(int whichSkin)
        {
            PersistentData.Instance.mySkin = whichSkin;
            PlayerChangeClothes.Instance.ChangeSkin(whichSkin);
            Debug.Log(whichSkin);
        }
        #endregion

        #region PRIVATE_FUNCTIONS

        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS

        #endregion

       
    }
}
