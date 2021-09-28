using SlapCityVoiceMod.Classes;
using SlapCityVoiceMod.Managers;
using Smash;
using System.Collections.Generic;
using System.Linq;

namespace SlapCityVoiceMod.Actions
{
    class SAPlayVoiceLine : SmashAction
    {
        private string character;
        private Voicepack voicepack;

        public override void Happen(SmashCharacter sc)
        {
            character ??= SmashLoader.Instance.GetPlayer(sc.playerindex).currentlyCharacter;
            voicepack ??= VoicepackManager.Instance.voicepacks.FirstOrDefault(x => x.characterId == character);

            voicepack?.Play(soundID);
        }

        public override Dictionary<string, object> ToDict()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                ["t"] = "SAPVL",
                ["s"] = soundID
            };

            return dictionary;
        }

        public new static SAPlayVoiceLine Parse(Dictionary<string, object> dict)
        {
            var result = new SAPlayVoiceLine();

            if (dict.ContainsKey("s"))
            {
                result.soundID = (dict["s"] as string);
            }

            return result;
        }

        public string soundID = string.Empty;
    }
}
