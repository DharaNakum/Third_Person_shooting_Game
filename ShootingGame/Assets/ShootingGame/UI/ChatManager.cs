using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShootingGame.UI
{
    public class ChatManager : MonoBehaviour,IChatClientListener
    {
        public TMP_InputField  _inputField;
        ChatClient chatClient;
        private List<RoomItem> m_MessageList = new List<RoomItem>();
        public string userID;
        public Transform messageContainer;
        public Text currentChannelText;
        private string m_CurrentChannelTextString ;
        void Start()
        {
            chatClient = new ChatClient( this );
            userID = PhotonNetwork.NickName;
            // Set your favourite region. "EU", "US", and "ASIA" are currently supported.
            chatClient.ChatRegion = "EU";
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
        }

        // Update is called once per frame
        void Update()
        {
            chatClient.Service();
        }

        public void OnSendClick()
        {
            if (_inputField.text != null)
            {
                chatClient.PublishMessage( "channelA", _inputField.text );
            }
            
        }
        
        public void DebugReturn(DebugLevel level, string message)
        {
            
        }

        public void OnDisconnected()
        {
            //throw new System.NotImplementedException();
        }

        public void OnConnected()
        {
            chatClient.Subscribe( new string[] { "channelA" } );
            //throw new System.NotImplementedException();
        }

        public void OnChatStateChange(ChatState state)
        {
            //throw new System.NotImplementedException();
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            //throw new System.NotImplementedException();
            string msgs = "";
            for ( int i = 0; i < senders.Length; i++ )
            {
                msgs = string.Format("{0}{1}:{2}, ", msgs, senders[i], messages[i]);
                m_CurrentChannelTextString += msgs;
            }
            //Console.WriteLine( "OnGetMessages: {0} ({1}) > {2}", channelName, senders.Length, msgs );
            Debug.Log("OnGetMessages: {0} ({1}) > {2}"+ channelName + " " + senders.Length+ " " + msgs);
            currentChannelText.text = m_CurrentChannelTextString;
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //throw new System.NotImplementedException();
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUnsubscribed(string[] channels)
        {
            //throw new System.NotImplementedException();
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUserSubscribed(string channel, string user)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            //throw new System.NotImplementedException();
        }
    }
}
