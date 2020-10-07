using System;
using System.Collections.Generic;
using System.Reflection;
using FrontierDevelopments.UtilityBelts;
using HarmonyLib;
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
        private static MethodInfo ShieldBeltBreak = AccessTools.Method(typeof(ShieldBelt), "Break");
        
        public static bool IsEnabled(ThingWithComps shieldBelt)
        {
            var comp = shieldBelt.GetComp<CompShieldToggle>();
            return comp == null || comp._enabled;
        }

        private bool _enabled = true;

        public bool Enabled => _enabled;

        private ShieldBelt Parent => (ShieldBelt) parent;

        private void Break()
        {
            if(Parent.ShieldState == ShieldState.Active) 
                ShieldBeltBreak.Invoke(Parent, new object[]{});
        }

        private void HandleToggle()
        {
            _enabled = !_enabled;
            if (!_enabled && Settings.ShieldRechargeFromFlickOn)
            {
                Break();
            }
        }

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
                    toggleAction = HandleToggle
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref _enabled, "shieldToggleEnabled", true);
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
