using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SlapCityVoiceMod
{
    [BepInPlugin("com.steven.slapcity.voicemod", "Voice Test Mod", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            var harmony = new Harmony("com.steven.slapcity.voicemod");
            harmony.PatchAll();

            new Managers.VoicepackManager();
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
