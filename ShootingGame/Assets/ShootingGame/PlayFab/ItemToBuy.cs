using PlayFab;
using PlayFab.ClientModels;
using ShootingGame.Player;
using UnityEngine;

namespace ShootingGame.PlayFab
{
    public class ItemToBuy : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS

        public void BuyItem(int coinPrice)
        {
            var request = new SubtractUserVirtualCurrencyRequest
            {
                VirtualCurrency = "CN",
                Amount = coinPrice
            };
            PlayFabClientAPI.SubtractUserVirtualCurrency(request,OnSubtractCoinSuccess,OnError);
        }

        private void OnError(PlayFabError obj)
        {
            Debug.Log("Error : " + obj.ErrorMessage);
        }

        private void OnSubtractCoinSuccess(ModifyUserVirtualCurrencyResult obj)
        {
            Debug.Log("bought item!");
            PlayFabManager.Instance.GeVirtualCurrency();
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
