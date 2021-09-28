using SlapCityVoiceMod.Managers;

namespace SlapCityVoiceMod.Classes
{
    [System.Serializable]
    public class Voiceclip
    {
        public string id;
        public string path;
        public float volume = 0.7f;

        public UnityEngine.AudioClip clip;

        public void Play()
        {
            VoicepackManager.Instance.PlayClip(clip, volume);
        }
    }
}
