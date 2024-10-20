using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Dance/MaxwellCat")]
    public class MaxwellCat : Dance
    {
        public override bool CanPerform => base.CanPerform && !Player.Instance.Underwater.IsUnderwater && Player.Instance.Grounded.IsGrounded;
    }
}