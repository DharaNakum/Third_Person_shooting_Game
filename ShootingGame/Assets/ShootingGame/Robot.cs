using Photon.Pun;
using ShootingGame.Player;
using ShootingGame.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace ShootingGame
{
    public class Robot : MonoBehaviourPunCallbacks
    {
        public GameObject followTarget;
        public NavMeshAgent robotAgent;
        public SphereCollider sphereCollider;
        public BoxCollider boxCollider;
        public PlayerController targetEnemy;

        private void Start()
        {
            sphereCollider.enabled = false;
        }

        /*public void SetPositionAndTarget(Vector3 pos)
        {
            this.transform.position = pos;
        }*/
        private void Update()
        {
            if (followTarget == null)
            {
                return;
            }
            if (!photonView.IsMine)
            {
                return;
            }
            robotAgent.SetDestination(followTarget.transform.position + new Vector3(1,0,1));
            if (targetEnemy != null)
            {
                Vector3 relativePos = targetEnemy.transform.position - transform.position;
                Quaternion lookAt = quaternion.LookRotation(relativePos, Vector3.up);
                transform.rotation = lookAt;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Blast();
                }
            }
        }

        public void Blast()
        {
            if(targetEnemy!=null)
            {
                if (boxCollider.enabled == false)
                {
                    if (targetEnemy.GetComponent<CharacterController>().enabled)
                    {
                        followTarget.GetComponent<PlayerController>().CalculateKillCount(targetEnemy,0.3f);
                        targetEnemy.RpcBlastCallBack();
                    }
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (followTarget == null)
            {
                PlayerController playerController = other.transform.GetComponent<PlayerController>();
                if (playerController!=null)
                {
                    followTarget = other.transform.gameObject;
                    if (photonView.Owner != followTarget.GetComponent<PhotonView>().Owner)
                    {
                        base.photonView.RequestOwnership();
                    }
                    sphereCollider.enabled = true;
                    boxCollider.enabled = false;
                }
            }
            else
            {
                if (other.GetComponent<PhotonView>().Owner != followTarget.GetComponent<PhotonView>().Owner)
                {
                    Debug.Log(other.name);
                    PlayerController controller = other.transform.GetComponent<PlayerController>();
                    if (controller != null)
                    {
                        targetEnemy = other.gameObject.GetComponent<PlayerController>();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (followTarget != null)
            {
                if (other.GetComponent<PhotonView>().Owner != followTarget.GetComponent<PhotonView>().Owner)
                {
                    Debug.Log(other.name);
                    PlayerController controller = other.transform.GetComponent<PlayerController>();
                    if (controller != null)
                    {
                        targetEnemy = null;
                    }
                }
            }
        }
    }
}
