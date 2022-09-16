using ShootingGame.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShootingGame.UI
{
    public class PlayerUi : MonoBehaviour
    {
        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private TMP_Text playerNameText;
        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;
        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f,30f,0f);
    
        float m_CharacterControllerHeight = 0f;
        Transform m_TargetTransform;
        Renderer m_TargetRenderer;
        CanvasGroup m_CanvasGroup;
        Vector3 m_TargetPosition;
    
        private PlayerController m_Target;
        void Awake()
        {
            transform.SetParent(UiManager.Instance.gamePlayView.transform, false);
            //this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            m_CanvasGroup = this.GetComponent<CanvasGroup>();
        }
        void Update()
        {
        
            if (m_Target == null)
            {
                Destroy(this.gameObject);
                return;
            }
            // Reflect the Player Health
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = m_Target.health;
            }
        }
        void LateUpdate()
        {
// Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (m_TargetRenderer!=null)
            {
                this.m_CanvasGroup.alpha = m_TargetRenderer.isVisible ? 1f : 0f;
            }


// #Critical
// Follow the Target GameObject on screen.
            if (m_TargetTransform != null)
            {
                m_TargetPosition = m_TargetTransform.position;
                m_TargetPosition.y += m_CharacterControllerHeight;
                this.transform.position = UnityEngine.Camera.main.WorldToScreenPoint (m_TargetPosition) + screenOffset;
            }
        }
    
        public void SetTarget(PlayerController _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            m_Target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = m_Target.photonView.Owner.NickName;
            }
        
            m_TargetTransform = this.m_Target.GetComponent<Transform>();
            m_TargetRenderer = this.m_Target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController> ();
// Get data from the Player that won't change during the lifetime of this Component
            if (characterController != null)
            {
                m_CharacterControllerHeight = characterController.height;
            }
        }
    }
}
