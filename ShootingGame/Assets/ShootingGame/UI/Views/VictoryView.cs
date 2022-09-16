using UnityEngine;

namespace ShootingGame.UI.Views
{
    public class VictoryView : BaseView
    {
        public void OnOkClick()
        {
            GameManager.Instance.LeaveRoom();
        }
    }
}
