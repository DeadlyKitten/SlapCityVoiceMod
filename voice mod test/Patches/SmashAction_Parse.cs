using HarmonyLib;
using Smash;
using System;
using System.Collections.Generic;
using SlapCityVoiceMod.Actions;

namespace SlapCityVoiceMod.Patches
{
    [HarmonyPatch(typeof(SmashAction), "Parse", new Type[] { typeof(Dictionary<string, object>) })]
    class SmashAction_Parse
    {
        static bool Prefix(Dictionary<string, object> dict, ref SmashAction __result)
        {
            if (dict.ContainsKey("t"))
            {
                var text = dict["t"] as string;
                if (text == "SAPVL")
                {
                    __result = SAPlayVoiceLine.Parse(dict);
                    return false;
                }
            }
            return true;
        }
    }
}
