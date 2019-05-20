using System;
using System.Collections.Generic;
using Harmony;
using RimWorld;
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
                        icon = Resources.TriggerSmokepop,
                        defaultDesc = "FrontierDevelopment.SmokepopBelt.Toggle.Desc".Translate(),
                        defaultLabel = "FrontierDevelopment.SmokepopBelt.Toggle.Label".Translate(),
                        activateSound = SoundDef.Named("Click"),
                        action = () =>
                        {
                            GenExplosion.DoExplosion(
                                __instance.Wearer.Position, 
                                __instance.Wearer.Map, 
                                __instance.GetStatValue(StatDefOf.SmokepopBeltRadius, true), 
                                DamageDefOf.Smoke, 
                                null, 
                                -1, 
                                -1f, 
                                null, 
                                null, 
                                null, 
                                null, 
                                ThingDefOf.Gas_Smoke, 
                                1f);
                            __instance.Destroy();
                        }
                    };
                }
            }
        }
    }
}
