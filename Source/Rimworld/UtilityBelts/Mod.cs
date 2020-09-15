using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopment.UtilityBelts
{
    public class Mod : Verse.Mod
    {
        public const string ModName = "Frontier Developments Utility Belts";
        
        public Mod(ModContentPack content) : base(content)
        {
            var harmony = new HarmonyLib.Harmony("FrontierDevelopments.UtilityBelts");
            harmony.PatchAll();
        }
        
        public override string SettingsCategory()
        {
            return ModName;
        }

        private static void AddToggleComps()
        {
            try
            {
                var typeShieldBelt = typeof(ShieldBelt);
                var names = new List<string>();
                    
                foreach(var def in DefDatabase<ThingDef>.AllDefs)
                {
                    try
                    {
                        if (def != null && def.thingClass == typeShieldBelt)
                        {
                            def.comps.Add(new CompPropertiesShieldToggle());
                            if (!def.defName.NullOrEmpty())
                                names.Add(def.defName);
                            else
                                names.Add("<no defname>");
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                Log.Message(ModName + " :: Loaded. Adding shield toggle to: " 
                            + names.Aggregate("", (current, next) => current + " " + next));
            }
            catch (Exception e)
            {
                Log.Warning(
                    ModName + " :: Failed to patch shield belts with: " +
                    e.Message);
            }
        }

        [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
        class Patch_GenerateImpliedDefs_PostResolve
        {
            [HarmonyPostfix]
            static void LoadModContent()
            {
                AddToggleComps();
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