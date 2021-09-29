using BepInEx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace SlapCityVoiceMod.Classes
{
    [System.Serializable]
    public class Voicepack
    {
        public string characterId;
        public AudioGroup[] audioGroups;
        public Voiceclip[] voiceClips;
        public MovesetPatch[] movesetPatches;

        public string zipPath;

        private bool loaded = false;

        public string TempFolder => Path.Combine(Paths.CachePath, characterId);

        public void LoadClips()
        {
            if (loaded) return;

            SharedCoroutineStarter.StartCoroutine(LoadAllClips());
        }

        private IEnumerator LoadAllClips()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var coroutines = new List<Coroutine>();

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                Directory.CreateDirectory(TempFolder);

                for (int i = 0; i < voiceClips.Count(); i++)
                {
                    var clip = voiceClips[i];

                    var entry = archive.GetEntry(clip.path);

                    if (entry != null)
                    {
                        var tempLocation = ExtractToTempFile(clip.path, entry);

                        coroutines.Add(SharedCoroutineStarter.StartCoroutine(LoadClip(tempLocation, clip)));
                    }
                }
            }

            foreach (var coroutine in coroutines)
                yield return coroutine;

            try { Directory.Delete(TempFolder, true); }
            catch (System.Exception) { }

            stopwatch.Stop();
            Plugin.LogInfo($"Loaded {voiceClips.Length} audio clip{(voiceClips.Length == 1 ? "" : "s")} in {stopwatch.ElapsedMilliseconds} ms.");

            loaded = true;
        }

        private string ExtractToTempFile(string path, ZipArchiveEntry entry)
        {
            var tempLocation = Path.Combine(TempFolder, Path.GetFileName(path));

            if (File.Exists(tempLocation)) File.Delete(tempLocation);
            entry.ExtractToFile(tempLocation);
            return tempLocation;
        }

        private IEnumerator LoadClip(string path, Voiceclip voiceclip)
        {
            var audioType = (Path.GetExtension(path) == ".wav") ? AudioType.WAV : AudioType.OGGVORBIS;
            var loader = UnityWebRequestMultimedia.GetAudioClip(path, audioType);
            yield return loader.SendWebRequest();

            if (loader.error != null)
            {
                Debug.LogError($"Error loading song from path: {path}\n{loader.error}");
                voiceClips = voiceClips.Where(x => x != voiceclip).ToArray();
                yield break;
            }

            voiceclip.clip = DownloadHandlerAudioClip.GetContent(loader);
        }

        public void Play(string id)
        {
            AudioGroup group;
            if ((group = audioGroups.FirstOrDefault(x => x.name == id)) != null)
                id = group.GetRandomClipId();

            var clip = voiceClips.FirstOrDefault(x => x.id == id);

            clip?.Play();
        }
    }
}
