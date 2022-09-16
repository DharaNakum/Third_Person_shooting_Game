using UnityEngine;

namespace ShootingGame.UI.Views
{
    public class BaseView : MonoBehaviour
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        #endregion

        #region UNITY_CALLBACKS
        #endregion
        
        #region PUBLIC_FUNCTIONS
            public void ShowView()
            {
                gameObject.SetActive(true);
            }
            public void HideView()
            {
                gameObject.SetActive(false);
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
