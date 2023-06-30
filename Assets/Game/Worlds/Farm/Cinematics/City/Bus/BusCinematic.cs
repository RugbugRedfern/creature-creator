using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Farm
{
    public class BusCinematic : TeleportCinematic
    {
        #region Methods
        public override void Show()
        {
            base.Show();
            SpawnCreature(spawnPoint);
        }
        #endregion
    }
}