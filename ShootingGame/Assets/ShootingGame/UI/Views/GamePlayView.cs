using System;
using System.Collections;
using Photon.Pun;
using ShootingGame.Player;
using UnityEngine;
using UnityEngine.UI;

namespace ShootingGame.UI.Views
{
    public class GamePlayView : BaseView
    {
        #region PUBLIC_VARS
            public Text timerText;
            public Text killCount;
            public Text deathCount;
        #endregion

        #region PRIVATE_VARS
        #endregion

        #region UNITY_CALLBACKS
        #endregion

        #region PUBLIC_FUNCTIONS
            public void OnCatButtonClick()
            {
                Debug.Log("OnCatButtonClick");
                PlayerController.Instance.ActiveCat();
            }

            public void OnBirdButtonClick()
            {
                PlayerController.Instance.ActiveBird();
            }

            public void OnRobotButtonClick()
            {
                if (GameManager.Instance.robot.photonView.IsMine)
                {
                    GameManager.Instance.robot.Blast();
                }
            }

            public void SetTimer(int count)
            {
                timerText.text = $"{count / 60:00} : {count % 60:00}";
            }

            public void SetKillCount(int count)
            {
                killCount.text = count.ToString();
            }

            public void SetDeathCount(int count)
            {
                deathCount.text = count.ToString();
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
