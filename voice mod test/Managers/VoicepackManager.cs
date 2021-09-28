using BepInEx;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SlapCityVoiceMod.Classes;

namespace SlapCityVoiceMod.Managers
{
    class VoicepackManager
    {
        private readonly AudioSource audioPlayer;
        public static VoicepackManager Instance
        {
            get;
            private set;
        }

        public List<Voicepack> voicepacks = new List<Voicepack>();

        public VoicepackManager()
        {
            Instance = this;

            audioPlayer = new GameObject("VoiceMod Audio Player").AddComponent<AudioSource>();
            GameObject.DontDestroyOnLoad(audioPlayer.gameObject);

            Task.Run(() => LoadAllVoicepacks());
        }

        private void LoadAllVoicepacks()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var folder = Path.Combine(Paths.BepInExRootPath, "Voicepacks");
            var files = Directory.GetFiles(folder, "*.zip");

            foreach (var file in files)
            {
                using (ZipArchive archive = ZipFile.OpenRead(file))
                {
                    var jsonEntry = archive.Entries.First(i => i.Name == "package.json");
                    Voicepack json = null;
                    if (jsonEntry != null)
                    {
                        var stream = new StreamReader(jsonEntry.Open(), Encoding.Default);
                        var jsonString = stream.ReadToEnd();
                        json = JsonConvert.DeserializeObject<Voicepack>(jsonString);
                        json.zipPath = file;
                    }

                    for (int i = 0; i < json.movesetPatches.Length; i++)
                    {
                        var entry = archive.GetEntry(json.movesetPatches[i].path);

                        if (entry != null)
                        {
                            using (var stream = new StreamReader(entry.Open()))
                            {
                                if (!json.movesetPatches[i].TryLoadMovesetPatch(stream.ReadToEnd()))
                                    Plugin.LogError($"Unable to load moveset from: {json.movesetPatches[i].path}");
                            }
                        }
                    }

                    if (json.movesetPatches.Any() && json.voiceClips.Any())
                        voicepacks.Add(json);
                }
            }

            stopwatch.Stop();
            Plugin.LogInfo($"Loaded {voicepacks.Count()} voice pack{(voicepacks.Count() == 1 ? "" : "s")} in {stopwatch.ElapsedMilliseconds} ms.");
        }

        public void PlayClip(AudioClip clip, float volume)
        {
            audioPlayer.PlayOneShot(clip, volume);
        }
    }
}
