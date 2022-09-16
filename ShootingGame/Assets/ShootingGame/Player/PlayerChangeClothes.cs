using UnityEngine;

namespace ShootingGame.Player
{
    public class PlayerChangeClothes : MonoBehaviour
    {
        #region PUBLIC_VARS
            public static PlayerChangeClothes Instance;
            public SkinnedMeshRenderer bodyMesh;
            public SkinnedMeshRenderer headMesh;
            public Material[] skins;
        #endregion

        #region PRIVATE_VARS
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        #endregion
        
        #region PUBLIC_FUNCTIONS
        public void ChangeSkin(int index)
        {
            if (index == 0)
            {
                return;
            }

            index--;
            bodyMesh.material = skins[index];
            headMesh.material = skins[index];
        }
        #endregion

        #region PRIVATE_FUNCTIONS
        #endregion

        #region CO-ROUTINES
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion
    }
}
