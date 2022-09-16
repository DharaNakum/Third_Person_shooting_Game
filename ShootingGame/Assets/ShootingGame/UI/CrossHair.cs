using UnityEngine;

namespace ShootingGame.UI
{
    public class CrossHair : MonoBehaviour
    {
        void Awake()
        {
            transform.SetParent(UiManager.Instance.gamePlayView.transform, false);
        }
    }
}
