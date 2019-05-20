using System;
using System.Collections.Generic;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopment.UtilityBelts.Harmony
{
    public class Harmony_SmokepopBelt
    {
        private static readonly Type smokepopType = typeof(SmokepopBelt);
        
        [HarmonyPatch(typeof(SmokepopBelt), nameof(SmokepopBelt.GetWornGizmos))]
        public class Harmony_ShieldBelt_GetWornGizmos
        {
            public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, SmokepopBelt __instance)
            {
                foreach (var gizmo in __result)
                {
                    yield return gizmo;
                }

                // check if this is a smokepop belt since smokepop belt does not have this method (it overrides Apparel)
                if (smokepopType.IsInstanceOfType(__instance) && __instance.Wearer.Faction == Faction.OfPlayer)
                {
                    yield return new Command_Action
                    {
                        icon = (Texture2D)__instance.Graphic.MatSouth.mainTexture ?? Resources.TriggerSmokepop,
                        defaultLabel = "FrontierDevelopment.SmokepopBelt.Toggle.Label".Translate(),
                        defaultDesc = "FrontierDevelopment.SmokepopBelt.Toggle.Desc".Translate().Replace("{0}", __instance.LabelShort),
                        activateSound = SoundDef.Named("Click"),
                        action = () =>
                        {
                            __instance.CheckPreAbsorbDamage(
                                new DamageInfo(DamageDefOf.Bullet, 0, weapon: ThingDef.Named("Gun_BoltActionRifle")));
                        }
                    };
                }
            }
        }
    }
}
