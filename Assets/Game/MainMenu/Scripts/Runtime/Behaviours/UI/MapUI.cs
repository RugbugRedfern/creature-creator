using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MapUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Menu mapMenu;
        [SerializeField] private Image screenshotImg;
        [SerializeField] private Sprite[] screenshots;
        [SerializeField] private GameObject lockedIcon;
        #endregion

        #region Methods
        public void View(Transform anchor)
        {
            mapMenu.transform.position = anchor.position;
            mapMenu.Open();
        }
        public void Hide()
        {
            mapMenu.Close();
        }

        public void OnMapChanged(int option)
        {
            screenshotImg.sprite = screenshots[option];

            string mapId = $"map_unlocked_{(Map)option}".ToLower();
            lockedIcon.SetActive(PlayerPrefs.GetInt(mapId) == 0);
        }
        #endregion
    }
}