using System.Collections;
using ShootingGame.Player;
using UnityEngine;

namespace ShootingGame
{
    public class HealthBooster : MonoBehaviour
    {
        public GameObject plus;
        public BoxCollider boxCollider;

        private void Start()
        {
            plus.SetActive(true);
            boxCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerController playerController = other.transform.GetComponent<PlayerController>();
            if (playerController!=null)
            {
                playerController.BoostHealth();
                SetInActive();
            
            }
        }
   
        private void SetInActive()
        {
            StartCoroutine(ActiveBooster());
            plus.SetActive(false);
            boxCollider.enabled = false;
        
        }
    
        private IEnumerator ActiveBooster()
        {
            yield return new WaitForSeconds(5);
            plus.SetActive(true);
            boxCollider.enabled = true;
        }
    }
}
