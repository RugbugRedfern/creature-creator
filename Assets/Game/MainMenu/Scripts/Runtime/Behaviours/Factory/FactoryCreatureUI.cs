using Steamworks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryCreatureUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI upVotesText;
        [SerializeField] private Image previewImg;
        [SerializeField] private Button pageBtn;
        [SerializeField] private Button subscribeBtn;
        [SerializeField] private Button likeBtn;
        [Space]
        [SerializeField] private GameObject info;
        [SerializeField] private GameObject refreshIcon;
        [SerializeField] private GameObject errorIcon;
        #endregion

        #region Methods
        public void Setup(string name, uint upVotes, PublishedFileId_t id)
        {
            nameText.text = name;
            upVotesText.text = upVotes.ToString();

            pageBtn.onClick.AddListener(delegate
            {
                SteamFriends.ActivateGameOverlayToWebPage($"steam://url/CommunityFilePage/{id}");
            });
            likeBtn.onClick.AddListener(delegate
            {
                SteamUGC.SetUserItemVote(id, true);
                upVotesText.text = $"{upVotes + 1}";
            });
            subscribeBtn.onClick.AddListener(delegate
            {
                SteamFriends.ActivateGameOverlayToWebPage($"steam://url/CommunityFilePage/{id}");
                SteamUGC.SubscribeItem(id);
            });
        }

        public void SetPreview(string url)
        {
            StartCoroutine(SetPreviewRoutine(url));
        }
        private IEnumerator SetPreviewRoutine(string url)
        {
            refreshIcon.SetActive(true);

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D preview = ((DownloadHandlerTexture)request.downloadHandler).texture;
                previewImg.sprite = Sprite.Create(preview, new Rect(0, 0, preview.width, preview.height), new Vector2(0.5f, 0.5f));
                info.SetActive(true);
            }
            else
            {
                errorIcon.SetActive(true);
            }
            refreshIcon.SetActive(false);
        }
        #endregion
    }
}