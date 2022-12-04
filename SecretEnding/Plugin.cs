using BepInEx;
using HarmonyLib;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SecretEnding
{
    [BepInPlugin("com.kuborro.plugins.fp2.secretending", "Secret(er) Ending", "1.0.1")]
    [BepInProcess("FP2.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle moddedBundle;
        public static AssetBundle moddedScene;
        private void Awake()
        {
            string assetPath = Path.Combine(Path.GetFullPath("."), "mod_overrides");
            moddedBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "secretending.assets"));
            moddedScene = AssetBundle.LoadFromFile(Path.Combine(assetPath, "secretending.scene"));
            if (moddedBundle == null)
            {
                Logger.LogError("Failed to load AssetBundle! Mod cannot work without it, exiting. Please reinstall it.");
                return;
            }

            var harmony = new Harmony("com.kuborro.plugins.fp2.secretending");
            harmony.PatchAll(typeof(PatchCutsceneMenu));
        }
    }
    class PatchCutsceneMenu {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MenuCutsceneViewer), "Start", MethodType.Normal)]
        [HarmonyPatch(typeof(MenuCutsceneViewer), "Update", MethodType.Normal)]
        static void Prefix(ref MenuCutsceneViewerEntry[] ___cutscenes)
        {
            Plugin.moddedBundle.LoadAllAssets();
            Plugin.moddedScene.GetAllScenePaths();
            if (___cutscenes!= null && ___cutscenes.Length == 69) {
                ___cutscenes = ___cutscenes.AddItem(new MenuCutsceneViewerEntry() {
                    name = "Intercepted Transmission",
                    carol = true,
                    lilac = true,
                    neera = true,
                    milla = true,
                    sceneName = "Cutscene_Secret",
                    playerPosition = new Vector2(0, 0),
                    flagsToChange = new int[0],
                    newValues = new byte[0]
                }).ToArray();
            }
        }
    }
}
