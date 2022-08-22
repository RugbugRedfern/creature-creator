// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MiscUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CanvasGroup m_UI;
        private bool m_IsVisible = true;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            InformationDialog.Inform("Early Access", "Please note that this game is in Early Access! If you encounter a bug, please consider reporting it in the community Discord server. Thank you!", okay: "View Roadmap", onOkay: delegate
            {
                RoadmapMenu.Instance.Open();
            });
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U))
            {
                m_IsVisible = !m_IsVisible;
                StartCoroutine(m_UI.Fade(m_IsVisible, 0.25f));
            }
        }

        public void SubscribeToYouTubeChannel()
        {
            Application.OpenURL("https://www.youtube.com/channel/UCGLR3v7NaV1t92dnzWZNSKA?sub_confirmation=1");
        }
        public void FollowTwitterAccount()
        {
            Application.OpenURL("https://twitter.com/daniellochner");
        }
        public void JoinDiscordServer()
        {
            Application.OpenURL("https://discord.gg/sJysbdu");
        }
        public void ViewGitHubSourceCode()
        {
            Application.OpenURL("https://github.com/daniellochner/creature-creator");
            RoadmapMenu.Instance.Open();

            //DateTime releaseDate = new DateTime(2022, 8, 7);
            //TimeSpan diff = releaseDate - DateTime.Now;
            //if (diff > TimeSpan.Zero)
            //{
            //    InformationDialog.Inform("Source Code", $"The source code to the game itself will release separately in:<br>{diff.Days} days, {diff.Hours} hours, {diff.Minutes} minutes and {diff.Seconds} seconds.");
            //}
            //else
            //{
            //    Application.OpenURL("https://github.com/daniellochner/creature-creator-demo");
            //}
        }
        public void Quit()
        {
            ConfirmationDialog.Confirm("Quit", "Are you sure you want to exit this application?", onYes: Application.Quit);
        }
        #endregion
    }
}