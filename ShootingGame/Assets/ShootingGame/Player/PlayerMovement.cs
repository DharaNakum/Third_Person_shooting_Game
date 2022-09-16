using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace ShootingGame.Player
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        #region PUBLIC_VARS
            public CharacterController controller;
            public float moveSpeed;
            public Animator animator;
            public Joystick joystick;
        #endregion

        #region PRIVATE_VARS
            private Transform m_MainCamera;
            public float rotationSmoothTime = 0.25f;
            private float m_TurnSmoothVelocity;
            private float m_VerticalVelocity;
            private float m_Gravity;
            private Joystick m_Joystick;
            private float m_MaxJumpHight = 2.0f;
            private bool m_IsJumping;
            private float m_InitialJumpVelocity;
            private float m_Timer;
            private Vector3 m_Direction;
            private float m_Horizontal;
            private float m_Vertical;
            private Joystick m_InputJoystick;
            private static readonly int Jump = Animator.StringToHash("Jump");
            private static readonly int Death = Animator.StringToHash("Death");
            private static readonly int Speed = Animator.StringToHash("Speed");
        #endregion

        #region UNITY_CALLBACKS
            private void Start()
            {
                if(!photonView.IsMine)
                {
                    return;
                }
                if (UnityEngine.Camera.main != null)
                {
                    m_MainCamera = UnityEngine.Camera.main.transform;
                }
                if (HandleGravity() > -10.0f)
                {
                    m_VerticalVelocity = HandleGravity();
                    controller.Move(new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime );
                }
                if (photonView.IsMine)
                {
                    m_InputJoystick = Instantiate(joystick);
                }
                m_IsJumping = false;
            }

            private void Update()
            {
                if(!photonView.IsMine)
                {
                    return;
                }
                if (!PlayerController.Instance.isMoving)
                {
                    return;
                }
                JumpPlayer();
                Move();
            }
            private void LateUpdate()
            {
                if (!photonView.IsMine)
                {
                    return;
                }
                if(m_Direction.magnitude<0.01f && Input.GetMouseButton(0))
                {
                    float targetAngle =  m_MainCamera.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, rotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                }
            }
        #endregion

        #region PUBLIC_FUNCTIONS]
            public void TurnPlayerTowardsCamera()
            {
                float targetAngle =  m_MainCamera.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
            public void ActiveDeathState(bool isActive)
            {
                photonView.RPC("SetCharacterControllerActive", RpcTarget.All, isActive);
            }
            [PunRPC]
            public void SetCharacterControllerActive(bool isActive)
            {
                controller.enabled = isActive;
                PlayDeathAnimation(!isActive);
                StartCoroutine(StopAnimation(isActive));
            }
        #endregion

        #region PRIVATE_FUNCTIONS
            private void JumpPlayer()
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool(Jump, true);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    animator.SetBool(Jump, false);
                }

                m_VerticalVelocity += Physics.gravity.y * Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Space) && m_Timer > 0.8f)
                {
                    m_Timer = 0;
                    m_VerticalVelocity = 3.5f;
                }

                m_Timer += Time.deltaTime;
                controller.Move(new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime);
            }
            private void Move()
            {
                if (PlayerController.Instance.isMobileInputEnable)
                {
                    m_Horizontal = m_InputJoystick.Horizontal;
                    m_Vertical = m_InputJoystick.Vertical;
                }
                else
                {
                     m_Horizontal = Input.GetAxis("Horizontal");
                     m_Vertical = Input.GetAxis("Vertical");
                }
                if (PlayerController.Instance.isFiring)
                {
                    if (m_Vertical < 0)
                    {
                        m_Horizontal = 0;
                        m_Vertical = 0;
                    }
                }
                m_Direction = new Vector3(m_Horizontal, 0, m_Vertical);
                if(m_Direction.magnitude>=0.01f)
                {
                    float targetAngle = Mathf.Atan2(m_Direction.x, m_Direction.z) * Mathf.Rad2Deg+ m_MainCamera.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, rotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move((moveDir.normalized * moveSpeed * Time.deltaTime) +
                                    new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime );
                }
                animator.SetFloat(Speed,m_Direction.magnitude*2.0f);
            }
            private float HandleGravity()
            {
                float gravity;
                if (controller.isGrounded)
                {
                    gravity = -0.5f;
                }
                else
                {
                    gravity = -9.8f;
                }

                return gravity;
            }

            private void PlayDeathAnimation(bool isPlay)
            {
                animator.SetBool(Death , isPlay);
            }
            private void ActivePlayer()
            {
                PlayerController.Instance.SpawnPlayerAfterDeath();
                controller.enabled = true;
                animator.enabled = true;
                animator.SetBool(Death, false);
                animator.SetFloat(Speed, 0);
            }
        #endregion

        #region CO-ROUTINES
            private IEnumerator StopAnimation(bool isActive)
            {
                yield return new WaitForSeconds(4.5f);
                animator.enabled = isActive;
                yield return new WaitForSeconds(5f);
                ActivePlayer();
            }
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion
    }
}
