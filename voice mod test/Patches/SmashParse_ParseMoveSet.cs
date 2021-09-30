using HarmonyLib;
using Newtonsoft.Json;
using SlapCityVoiceMod.Managers;
using Smash;
using System.Security.Cryptography;
using System.Text;

namespace SlapCityVoiceMod.Patches
{
    [HarmonyPatch(typeof(SmashParse), "ParseMoveSet")]
    class SmashParse_ParseMoveSet
    {
        static void Prefix(ref string moveset)
        {
            var hash = CreateMD5(moveset);

            if (VoicepackManager.Instance.TryGetVoicepackFromMovesetHash(hash, out var result))
            {
                result.voicepack.LoadClips();

                var obj = JsonConvert.DeserializeObject(moveset);
                result.patch.ApplyTo(obj);
                moveset = JsonConvert.SerializeObject(obj);
            }
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
