using Smash;
using System.Collections.Generic;
using System.Linq;
using SlapCityVoiceMod.Managers;

namespace SlapCityVoiceMod.Actions
{
    class SAPlayVoiceLine : SmashAction
    {
        public override void Happen(SmashCharacter sc)
        {
            var character = SmashLoader.Instance.GetPlayer(sc.playerindex).currentlyCharacter;

            VoicepackManager.Instance.voicepacks.FirstOrDefault(x => x.characterId == character)?.Play(soundID);
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
