// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class PivotXYTool : PivotTool
    {
        public override bool CanShow => !BodyPartEditor.BodyPartConstructor.CanMirror;
    }
}