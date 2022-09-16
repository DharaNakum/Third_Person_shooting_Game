using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using ShootingGame.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ShootingGame.PlayFab
{
    public class PlayFabManager : MonoBehaviour
    {
        public static PlayFabManager Instance;

        public GameObject leaderBoardPanel;
        public GameObject listingPrefab;
        public Transform listingContainer;
        public Text messageText;
        public Text coinValueText;
        public Image coinLoading;
        public RawImage rowImage;

        private string m_MyId;
        
        #region UNITY_CALLBACKS
        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            coinLoading.enabled = false;
        }
        
        #endregion
        
        #region Login
        public void Login()
        {
            var request = new LoginWithCustomIDRequest{CustomId = SystemInfo.deviceUniqueIdentifier,CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request,OnSuccess,OnError);
        }
    
        private void OnError(PlayFabError obj)
        {
            Debug.Log("Error");
        }

        private void OnSuccess(LoginResult obj)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest{DisplayName = PhotonNetwork.NickName},OnDisplayName,OnError);
            Debug.Log("success");
            m_MyId = obj.PlayFabId;
            GatPlayerData();
        }

        private void OnDisplayName(UpdateUserTitleDisplayNameResult obj)
        {
            
        }

        #endregion
        
        #region LeaderBoard

        public void GetLeaderBoard()
        {
            var requestLeaderBoard = new GetLeaderboardRequest{StartPosition = 0,StatisticName = "Score"};
            PlayFabClientAPI.GetLeaderboard(requestLeaderBoard,OnGetLeaderBoard,OnErrorLeaderBoard);
        }

        private void OnErrorLeaderBoard(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }

        private void OnGetLeaderBoard(GetLeaderboardResult result)
        {
            foreach (Transform child in listingContainer)
            {
                Destroy(child.gameObject);
            }
            leaderBoardPanel.SetActive(true);
            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                GameObject tempListing = Instantiate(listingPrefab, listingContainer);
                LeaderBoardList leaderBoardList = tempListing.GetComponent<LeaderBoardList>();
                leaderBoardList.displayName.text = player.DisplayName;
                leaderBoardList.score.text = player.StatValue.ToString();
            }
        }
        

        public void SendLeaderBoard(int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = "Score",
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request,OnLeaderBoardUpdate,OnError);
        }

        private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult obj)
        {
            Debug.Log("LeaderBoard updated");
        }

        public void OnCloseClick()
        {
            leaderBoardPanel.SetActive(false);
        }

        #endregion
        #region PlayerData

        public void GatPlayerData()
        {
            Debug.Log("GatPlayerData");
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = m_MyId,
                Keys = null
            },UserDataSuccess,OnErrorLeaderBoard );
        }

        private void UserDataSuccess(GetUserDataResult result)
        {
            if (result.Data == null || !result.Data.ContainsKey("Skins"))
            {
                Debug.Log("Skins not set");
            }
            else
            {
                Debug.Log(result.Data["Skins"].Value);
                PersistentData.Instance.SkinsStringToData(result.Data["Skins"].Value);
            }
        }

        public void SetUserData(string skinsData)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string,string>()
                {
                    {"Skins",skinsData}
                }
            },SetDataSuccess,OnErrorLeaderBoard);
        }

        private void SetDataSuccess(UpdateUserDataResult obj)
        {
            Debug.Log(obj.DataVersion);
        }

        #endregion
        
        #region TitleData

        public void GetTitleData()
        {
            Debug.Log("getTitleData");
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleReceived,OnErrorLeaderBoard);
        }

        private void OnTitleReceived(GetTitleDataResult obj)
        {
            if (obj.Data == null || obj.Data.ContainsKey("Message") == false)
            {
                Debug.Log("no Message");
                return;
            }

            messageText.text = obj.Data["Message"];
        }

        #endregion
        
        #region virtualCurrency

        public void GeVirtualCurrency()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess,OnErrorLeaderBoard);
            coinLoading.enabled = true;
        }

        private void OnGetUserInventorySuccess(GetUserInventoryResult obj)
        {
            int coins = obj.VirtualCurrency["CN"];
            coinValueText.text = coins.ToString();
            coinLoading.enabled = false;
        }

        public void GrantVirtualCurrency(int coins)
        {
            var request = new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = "CN",
                Amount = coins
            };
            PlayFabClientAPI.AddUserVirtualCurrency(request,OnGrantVirtualCurrencySuccess,OnErrorLeaderBoard);
        }

        private void OnGrantVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult obj)
        {
            Debug.Log("currency Added");
        }

        #endregion

        public void GettingProfileInfo()
        {
           
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
               OnIdGetSuccess, error => Debug.Log("Failed To Get Playfab Id"));
        }

        private void OnIdGetSuccess(GetAccountInfoResult obj)
        {
            var userId = obj.AccountInfo.PlayFabId.ToString();
            GetPlayerProfile(userId);
        }

        void GetPlayerProfile(string playFabId) {
            PlayFabClientAPI.GetPlayerProfile( new GetPlayerProfileRequest() {
                    PlayFabId = playFabId,
                    ProfileConstraints = new PlayerProfileViewConstraints() {
                        ShowAvatarUrl = true
                    }
                },
                OnUrlGet,
                OnError);
        }

        private void OnUrlGet(GetPlayerProfileResult obj)
        {
            Debug.Log(obj.PlayerProfile.AvatarUrl);
            StartCoroutine(DownloadImage(obj.PlayerProfile.AvatarUrl));
        }
        IEnumerator DownloadImage(string MediaUrl)
        {   
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) 
                Debug.Log(request.error);
            else{
                Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
                rowImage.texture = tex;
            }
        } 
        
    }
}
