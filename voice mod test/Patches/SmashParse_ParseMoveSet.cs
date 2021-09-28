using HarmonyLib;
using Newtonsoft.Json;
using Smash;
using System.Security.Cryptography;
using System.Text;
using SlapCityVoiceMod.Classes;
using SlapCityVoiceMod.Managers;

namespace SlapCityVoiceMod.Patches
{
    [HarmonyPatch(typeof(SmashParse), "ParseMoveSet")]
    class SmashParse_ParseMoveSet
    {
        static void Prefix(ref string moveset)
        {
            var hash = CreateMD5(moveset);
            MovesetPatch patch = null;

            for (int i = 0; i < VoicepackManager.Instance.voicepacks.Count; i++)
            {
                var voicepack = VoicepackManager.Instance.voicepacks[i];
                foreach (var movesetPatch in voicepack.movesetPatches)
                {
                    if (movesetPatch.hash == hash)
                    {
                        patch = movesetPatch;
                        break;
                    }
                }
                if (patch != null)
                {
                    voicepack.LoadClips();
                    break;
                }
            }

            if (patch == null) return;

            var obj = JsonConvert.DeserializeObject(moveset);
            patch.patch.ApplyTo(obj);
            moveset = JsonConvert.SerializeObject(obj);
        }

        static string CreateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
