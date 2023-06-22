using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CustomColorUtil.Harmony;
using CustomColorUtil.Models;
using Mod;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CustomColorUtil.Util
{
    public static class GenericUtil
    {
        public static Color? ConvertColor(this ColorRoot color)
        {
            if (color == null) return null;
            return new Color(color.R != 0 ? color.R / 255 : 0, color.G != 0 ? color.G / 255 : 0,
                color.B != 0 ? color.B / 255 : 0, color.A != 0 ? color.A / 255 : 0);
        }

        public static HSVColor ConvertHsvColor(this HsvColorRoot color)
        {
            return color == null ? null : new HSVColor(color.H, color.S, color.V);
        }

        public static async Task PutTaskDelay(int delay)
        {
            await Task.Delay(delay);
        }

        public static void SafeDestroyComponent<T>(this GameObject self) where T : MonoBehaviour
        {
            if (self == null) return;
            var component = self.GetComponent<T>();
            if (component != null)
                Object.DestroyImmediate(component);
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : MonoBehaviour
        {
            if (self == null) return default;
            var t = self.GetComponent<T>();
            if (t == null)
                t = self.AddComponent<T>();
            return t;
        }

        public static void RemoveError()
        {
            Singleton<ModContentManager>.Instance.GetErrorLogs().RemoveAll(x => new List<string>
            {
                "0Harmony",
                "Mono.Cecil",
                "MonoMod.RuntimeDetour",
                "MonoMod.Utils",
                "MonoMod.Common",
                "CustomMapUtility",
                "NAudio"
            }.Exists(y => x.Contains("The same assembly name already exists. : " + y)));
            Singleton<ModContentManager>.Instance.GetErrorLogs().RemoveAll(x => new List<string>
            {
                "CustomColorUtil"
            }.Exists(x.Contains));
        }

        public static void OtherModCheck()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            ModParameters.SpeedDiceColorModFound = assemblies.Any(x => x.GetName().Name == "Patty_SpeedDiceColor_MOD");
            ModParameters.EmotionCardLoaderFound = assemblies.Any(x => x.GetName().Name == "1EmotionCardUtil");
            ModParameters.TiphEgoModFound = assemblies.Any(x => x.GetName().Name == "Luca1125_EgoTiphereth");
            ModParameters.UtilLoaderDLLFound = assemblies.Any(x => x.GetName().Name == "1UtilLoader21341");
        }

        public static void OnLoadingScreen(Scene scene, LoadSceneMode _)
        {
            if (scene.name != "Stage_Hod_New" || ModParameters.ModLoaded) return;
            ModParameters.ModLoaded = true;
            if (ModParameters.EmotionCardLoaderFound) PatchEmotionCards();
            if (ModParameters.UtilLoaderDLLFound) PatchSkinProjection();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void PatchEmotionCards()
        {
            ModParameters.Harmony.CreateClassProcessor(typeof(EmotionCardColorPatch)).Patch();
            ModParameters.Harmony.CreateClassProcessor(ModParameters.TiphEgoModFound
                ? typeof(EmotionCardColorPatchWithTiphMod)
                : typeof(EmotionCardColorPatchWithoutTiphMod)).Patch();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void PatchSkinProjection()
        {
            ModParameters.Harmony.CreateClassProcessor(typeof(SkinProjectionColorPatch)).Patch();
        }
    }
}