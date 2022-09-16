using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ShootingGame.PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootingGame.UI
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [FormerlySerializedAs("_roomInputField")] public TMP_InputField roomInputField;

        public RoomItem roomItemPrefab;
        private List<RoomItem> roomItemList = new List<RoomItem>();

        public Transform contentObject;
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
            PhotonNetwork.JoinLobby();
            PlayFabManager.Instance. GetTitleData();
            PlayFabManager.Instance.GeVirtualCurrency();
            PlayFabManager.Instance.GettingProfileInfo();
            //_inputField = this.GetComponent<InputField>();
        }

        public void OnCreateRoomClick()
        {
            if (roomInputField.text.Length <= 0)
            {
                return;
            }
            PhotonNetwork.CreateRoom(roomInputField.text,new RoomOptions(){MaxPlayers = 4});
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            //base.OnCreateRoomFailed(returnCode, message);
            Debug.Log("Enter another name");
            //SceneManager.LoadScene("Lobby");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel("Waiting");
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            UpdateRoomList(roomList);
        }

        private void UpdateRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomItem Item in roomItemList)
            {
                Destroy(Item.gameObject);
            }
            roomItemList.Clear();
            foreach (RoomInfo room in roomList)
            {
                Debug.Log(room.Name);
                RoomItem newRoomItem = Instantiate(roomItemPrefab,contentObject);
                newRoomItem.SetRoomName(room.Name);
                roomItemList.Add(newRoomItem);
            }
            //Debug.Log("*** room list update " + roomList[roomList.Count-1]);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void JoinRoom(string roomNameText)
        {
            //throw new System.NotImplementedException();
            PhotonNetwork.JoinRoom(roomNameText);
        }
    }
}
