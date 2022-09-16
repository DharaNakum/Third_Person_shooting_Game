using System;
using ShootingGame.UI.Views;
using Unity.VisualScripting;
using UnityEngine;

namespace ShootingGame.UI
{
    public class UiManager : MonoBehaviour
    {
        #region PUBLIC_VARS
            public static UiManager Instance;
            public GamePlayView gamePlayView;
            public VictoryView victoryView;
            public DefeatedView defeatedView;
        #endregion
        
        #region PRIVATE_VARS
        
        #endregion
        
        #region UNITY_CALLBACKS
            void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                }
            }
            private void Start()
            {
                victoryView.HideView();
                defeatedView.HideView();
            }
        #endregion
        
        #region PUBLIC_FUNCTIONS
            public void ActiveVictoryPanel()
            {
                gamePlayView.HideView();
                victoryView.ShowView();
            }
            public void ActiveDefeatedPanel()
            {
                gamePlayView.HideView();
                defeatedView.ShowView();
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
