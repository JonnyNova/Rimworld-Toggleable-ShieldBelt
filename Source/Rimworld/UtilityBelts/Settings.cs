using HugsLib.Settings;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public static class Settings
    {
        private static SettingHandle<bool> shieldRechargeFromFlickOn;

        public static bool ShieldRechargeFromFlickOn => shieldRechargeFromFlickOn.Value;

        public static void Init(ModSettingsPack settings)
        {
            shieldRechargeFromFlickOn = settings.GetHandle(
                "shieldRechargeFromFlickOn",
                Translator.Translate("FrontierDevelopments.UtilityBelts.Settings.ShieldRechargeFromFlickOn.Label"),
                Translator.Translate(
                    "FrontierDevelopments.UtilityBelts.Settings.ShieldRechargeFromFlickOn.Description"), false);
        }
    }
}