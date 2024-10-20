using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AdvanceManager : MonoBehaviourSingleton<AdvanceManager>
    {
        #region Fields
        [SerializeField] private UnlockableBodyPart[] bodyParts;
        [SerializeField] private UnlockablePattern[] patterns;
        [SerializeField] private Quest[] quests;
        [SerializeField] private Battle[] battles;
        [SerializeField] private Teleport[] teleports;
        [SerializeField] private Map nextMap = Map.ComingSoon;
        [Space]
        [SerializeField] private GameObject progress;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private TextMeshProUGUI patternsText;
        [SerializeField] private TextMeshProUGUI questsText;
        [SerializeField] private WorldHint worldHintPrefab;

        private int unlockedBodyParts, unlockedPatterns, completedQuests;
        private bool isAllowedToAdvance = false;
        #endregion

        #region Properties
        public int TotalBodyParts => bodyParts.Length;
        public int TotalPatterns => patterns.Length;
        public int TotalQuests => quests.Length;

        public int NumUnlockedBodyParts
        {
            get => unlockedBodyParts;
            set
            {
                unlockedBodyParts = value;
                bodyPartsText.SetArguments(unlockedBodyParts, bodyParts.Length);
                TryAdvance();
            }
        }
        public int NumUnlockedPatterns
        {
            get => unlockedPatterns;
            set
            {
                unlockedPatterns = value;
                patternsText.SetArguments(unlockedPatterns, patterns.Length);
                TryAdvance();
            }
        }
        public int NumCompletedQuests
        {
            get => completedQuests;
            set
            {
                completedQuests = value;
                questsText.SetArguments(completedQuests, quests.Length);
                TryAdvance();
            }
        }

        public bool IsTimed
        {
            get => WorldManager.Instance.World.Mode == Mode.Timed;
        }
        public bool CanAdvance
        {
            get => (nextMap != Map.ComingSoon) && (bodyParts.Length + patterns.Length + quests.Length) == (NumUnlockedBodyParts + NumUnlockedPatterns + NumCompletedQuests);
        }
        #endregion

        #region Methods
        private void Start()
        {
            if (!WorldManager.Instance.IsCreative)
            {
                progress.SetActive(true);

                // Body Parts
                NumUnlockedBodyParts = 0;
                foreach (UnlockableBodyPart bodyPart in bodyParts)
                {
                    if (bodyPart.IsUnlocked)
                    {
                        NumUnlockedBodyParts++;
                    }
                    else
                    {
                        bodyPart.onUnlock.AddListener(() => NumUnlockedBodyParts++);
                    }
                }

                // Patterns
                NumUnlockedPatterns = 0;
                foreach (UnlockablePattern pattern in patterns)
                {
                    if (pattern.IsUnlocked)
                    {
                        NumUnlockedPatterns++;
                    }
                    else
                    {
                        pattern.onUnlock.AddListener(() => NumUnlockedPatterns++);
                    }
                }

                // Quests
                NumCompletedQuests = 0;
                foreach (Quest quest in quests)
                {
                    if (quest.HasCompleted)
                    {
                        NumCompletedQuests++;
                    }
                    else
                    {
                        quest.onComplete.AddListener(() => NumCompletedQuests++);
                    }
                }

                isAllowedToAdvance = true;
            }
        }

        private void TryAdvance()
        {
            if (isAllowedToAdvance && CanAdvance)
            {
                if (WorldManager.Instance.World.Mode == Mode.Adventure)
                {
                    InformationDialog.Inform(LocalizationUtility.Localize("cc_ready-to-advance_title"), LocalizationUtility.Localize("cc_ready-to-advance_message"), onOkay: delegate
                    {
                        foreach (Teleport teleport in teleports)
                        {
                            teleport.GetComponent<MinimapIcon>().MinimapIconUI.gameObject.AddComponent<BlinkingGraphic>();
                            Instantiate(worldHintPrefab, teleport.transform, false).transform.localScale *= 3f;
                        }
                    });
                    ProgressManager.Instance.UnlockMap(nextMap);
                }
                else
                if (WorldManager.Instance.World.Mode == Mode.Timed)
                {
                    TimedManager.Instance.Complete();
                }
            }
        }
        #endregion
    }
}