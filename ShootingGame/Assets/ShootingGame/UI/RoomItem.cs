using TMPro;
using UnityEngine;

namespace ShootingGame.UI
{
    public class RoomItem : MonoBehaviour
    {
        public TMP_Text roomName;
        public LobbyManager manager;

        private void Start()
        {
            manager = FindObjectOfType<LobbyManager>();
        }

        public void SetRoomName(string nameOfRoom)
        {
            roomName.text = nameOfRoom;
        }

        public void OnJoinRoomClick()
        {
            manager.JoinRoom(roomName.text);
        }
    }
}
