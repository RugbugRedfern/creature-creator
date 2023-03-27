using DanielLochner.Assets.CreatureCreator.Abilities;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class KeybindingsManager : DataManager<KeybindingsManager, Keybindings>
    {
        #region Fields
        [Header("Keybindings")]
        [SerializeField] private NetworkPlayersMenu networkPlayersMenu;
        [Space]
        [SerializeField] private Drop dropAbility;
        [SerializeField] private Eat eatAbility;
        [SerializeField] private Abilities.Ping pingAbility;
        [SerializeField] private Shoot shootAbility;
        [SerializeField] private Spit spitAbility;
        [SerializeField] private Growl growlAbility;
        [SerializeField] private NightVision nightVisionAbility;
        [SerializeField] private Bite[] biteAbilities;
        [SerializeField] private Jump[] jumpAbilities;
        [SerializeField] private Flap[] flapAbilities;
        [SerializeField] private Sprint[] sprintAbilities;
        [SerializeField] private Strike[] strikeAbilities;
        #endregion

        #region Methods
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Save();
        }

        public void RebindViewPlayers(Keybind key)
        {
            Data.ViewPlayers = networkPlayersMenu.Keybind = key;
        }
        public void RebindBite(Keybind key)
        {
            foreach (Bite biteAbility in biteAbilities)
            {
                biteAbility.PerformKeybind = key;
            }
            Data.Bite = key;
        }
        public void RebindDrop(Keybind key)
        {
            dropAbility.PerformKeybind = key;
            Data.Drop = key;
        }
        public void RebindShoot(Keybind key)
        {
            shootAbility.PerformKeybind = key;
            Data.Shoot = key;
        }
        public void RebindFlap(Keybind key)
        {
            foreach (Flap flapAbility in flapAbilities)
            {
                flapAbility.PerformKeybind = key;
            }
            Data.Flap = key;
        }
        public void RebindJump(Keybind key)
        {
            foreach (Jump jumpAbility in jumpAbilities)
            {
                jumpAbility.PerformKeybind = key;
            }
            Data.Jump = key;
        }
        public void RebindSpit(Keybind key)
        {
            spitAbility.PerformKeybind = key;
            Data.Spit = key;
        }
        public void RebindGrowl(Keybind key)
        {
            growlAbility.PerformKeybind = key;
            Data.Growl = key;
        }
        public void RebindSprint(Keybind key)
        {
            foreach (Sprint sprintAbility in sprintAbilities)
            {
                sprintAbility.PerformKeybind = key;
            }
            Data.Sprint = key;
        }
        public void RebindNightVision(Keybind key)
        {
            nightVisionAbility.PerformKeybind = key;
            Data.NightVision = key;
        }
        #endregion
    }
}