using UnityEngine;

namespace ShootingGame
{
    public class MiniMap : MonoBehaviour
    {
        public Transform targetPlayer;

        public void SetPlayer(Transform player)
        {
            targetPlayer = player;
        }

        private void LateUpdate()
        {
            if (targetPlayer == null)
            {
                return;
            }
            Vector3 newPosition = targetPlayer.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            //transform.rotation = Quaternion.Euler(90,targetPlayer.rotation.y,0);
        
        }
    }
}
