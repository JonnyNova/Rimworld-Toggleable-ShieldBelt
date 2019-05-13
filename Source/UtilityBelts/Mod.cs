using System;
using System.Collections.Generic;
using System.Linq;
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
                                    names.Add("unknown_defName");
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Warning("Frontier Developments Toggleable Shield Belt :: Failed to process def " + def?.defName + ", " + e.Message);
                        }
                    }
                    Log.Message("Frontier Developments Toggleable Shield Belt :: Loaded. Adding shield toggle to: " 
                                + names.Aggregate("", (current, next) => current + " " + next));
                }
                catch (Exception e)
                {
                    Log.Warning(
                        "Frontier Developments Toggleable Shield Belt :: Failed to patch shield belts with: " +
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