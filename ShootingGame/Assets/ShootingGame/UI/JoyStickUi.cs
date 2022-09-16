using UnityEngine;

namespace ShootingGame.UI
{
    public class JoyStickUi : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(UiManager.Instance.gamePlayView.transform, false);
        }
    }
}
