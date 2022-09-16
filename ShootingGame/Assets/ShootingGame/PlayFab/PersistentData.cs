using System;
using UnityEngine;


namespace ShootingGame.PlayFab
{
    public class PersistentData : MonoBehaviour
    {
        #region PUBLIC_VARS
        public static PersistentData Instance;
        public bool[] allSkins;
        public int mySkin;

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
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        #region PUBLIC_FUNCTIONS

        public void SkinsStringToData(string skinsIn)
        {
            for (int i = 0; i < skinsIn.Length; i++)
            {
                if (int.Parse(skinsIn[i].ToString()) > 0)
                {
                    allSkins[i] = true;
                }
                else
                {
                    allSkins[i] = false;
                }
            }

            MenuController.Instance.SetUpStore();
        }

        public string SkinsDataToString()
        {
            string toString = "";
            for (int i = 0; i < allSkins.Length; i++)
            {
                if (allSkins[i] == true)
                {
                    toString += "1";
                }
                else
                {
                    toString += "0";
                }
            }
            return toString;
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
