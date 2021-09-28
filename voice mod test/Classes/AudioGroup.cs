using SlapCityVoiceMod.Extensions;

namespace SlapCityVoiceMod.Classes
{
    [System.Serializable]
    public class AudioGroup
    {
        public string name;
        public AudioGroupItem[] clips;

        public string GetRandomClipId()
        {
            var result = clips.GetRandomItem(x => x.weight).id;

            return result;
        }

        [System.Serializable]
        public class AudioGroupItem
        {
            public string id;
            public float weight;
        }
    }
}
