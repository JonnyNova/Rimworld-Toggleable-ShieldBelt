using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopment.UtilityBelts
{
    public class CompPropertiesShieldToggle : CompProperties
    {
        public CompPropertiesShieldToggle()
        {
            compClass = typeof(CompShieldToggle);
        }
    }

    public class CompShieldToggle : ThingComp
    {
        public static bool IsEnabled(ThingWithComps shieldBelt)
        {
            var comp = shieldBelt.GetComp<CompShieldToggle>();
            return comp == null || comp._enabled;
        }

        private bool _enabled = true;

        public bool Enabled => _enabled;

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (var gizmo in base.CompGetWornGizmosExtra())
            {
                yield return gizmo;
            }
            
            if (ShouldShowGizmo())
            {
                yield return new Command_Toggle
                {
                    icon = (Texture2D)parent.Graphic.MatSouth.mainTexture ?? Resources.ToggleShield,
                    defaultLabel = "FrontierDevelopment.ShieldBelt.Toggle.Label".Translate(),
                    defaultDesc = "FrontierDevelopment.ShieldBelt.Toggle.Desc".Translate().Replace("{0}", parent.LabelShort),
                    isActive = () => _enabled,
                    toggleAction = () => _enabled = !_enabled
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref _enabled, "enabled", true);
        }

        private bool ShouldShowGizmo()
        {
            switch (parent)
            {
                case Apparel apparel:
                    return apparel.Wearer?.Faction == Faction.OfPlayer;
            }
            return false;
        }
    }
}