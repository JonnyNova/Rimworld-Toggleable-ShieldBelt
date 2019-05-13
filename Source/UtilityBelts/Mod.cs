using System;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            var harmony = HarmonyInstance.Create("FrontierDevelopments.Toggleable-ShieldBelt");
            harmony.PatchAll();
            
            Log.Message("Frontier Developments Toggleable Shield Belt :: Loading");
        }
        
        public override string SettingsCategory()
        {
            return "Frontier Developments Toggleable Shield Belt";
        }
        
        [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PostResolve")]
        class Patch_GenerateImpliedDefs_PostResolve
        {
            static void Postfix()
            {
                try
                {
                    ThingDefOf.Apparel_ShieldBelt.comps.Add(new CompPropertiesShieldToggle());
                    Log.Message("Frontier Developments Toggleable Shield Belt :: Loaded");
                }
                catch (Exception e)
                {
                    Log.Warning(
                        "Frontier Developments Toggleable Shield Belt :: Failed to patch shield belt with: " +
                        e.Message);
                }
            }
        }
    }
    
    [StaticConstructorOnStartup]
    public static class Resources
    {
        public static readonly Texture2D ToggleShield = ContentFinder<Texture2D>.Get("Things/Pawn/Humanlike/Apparel/ShieldBelt/ShieldBelt");
        public static readonly Texture2D TriggerSmokepop = ContentFinder<Texture2D>.Get("Things/Pawn/Humanlike/Apparel/SmokepopBelt/SmokepopBelt");
    } 
}