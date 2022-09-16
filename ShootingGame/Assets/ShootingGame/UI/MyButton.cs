using ShootingGame.Player;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShootingGame.UI
{
    public class MyButton : Button
    {

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            PlayerController.Instance.StartShooting();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            PlayerController.Instance.StopShooting();
        }
    }
}