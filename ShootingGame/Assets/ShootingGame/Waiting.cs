using Photon.Pun;
using UnityEngine;

namespace ShootingGame
{
    public class Waiting : MonoBehaviour
    {
        private bool isTrue;
        // Start is called before the first frame update
        void Start()
        {
            isTrue = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTrue)
            {
                return;
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                PhotonNetwork.LoadLevel("City");
                Debug.Log("load city");
                isTrue = true;
            }
        }
    
    }
}
