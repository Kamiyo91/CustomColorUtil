using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CustomColorUtil.Models;
using Mod;
using Debug = UnityEngine.Debug;

namespace CustomColorUtil.Util
{
    public static class ModParametersUtilLoader
    {
        public static void LoadMods()
        {
            foreach (var modContentInfo in Singleton<ModContentManager>.Instance.GetAllMods().Where(modContentInfo =>
                         modContentInfo.activated &&
                         modContentInfo.invInfo.workshopInfo.uniqueId != ModParameters.PackageId &&
                         Directory.Exists(modContentInfo.dirInfo.FullName + "/Assemblies/CustomColorUtil")))
                try
                {
                    var modId = modContentInfo.invInfo.workshopInfo.uniqueId;
                    var path = modContentInfo.dirInfo.FullName + "/Assemblies";
                    var stopwatch = new Stopwatch();
                    Debug.Log($"Custom Color Util Tool : Start loading mod files {modId} at path {path}");
                    stopwatch.Start();
                    LoadModParameters(path, modId);
                    ArtUtil.GetArtWorks(new DirectoryInfo(path + "/CustomColorUtil/ArtWork"), modId);
                    stopwatch.Stop();
                    Debug.Log(
                        $"Custom Color Util Tool : Loading mod files {modId} at path {path} finished in {stopwatch.ElapsedMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"Error while loading the mod {modContentInfo.invInfo.workshopInfo.uniqueId} - {ex.Message}");
                }
        }

        private static void LoadModParameters(string path, string modId)
        {
            var cardOptions = LoadParameters<CardOptionsRoot>(path + "/CustomColorUtil/CardOptions", modId);
            ModParameters.CardOptions.AddRange(cardOptions.Key
                ? cardOptions.Value.CardOption
                : new List<CardOptionRoot>());
            var categoryOptions = LoadParameters<CategoryOptionsRoot>(path + "/CustomColorUtil/CategoryOptions", modId);
            ModParameters.CategoryOptions.AddRange(categoryOptions.Key
                ? categoryOptions.Value.CategoryOption
                : new List<CategoryOptionRoot>());
            var dropBookOptions = LoadParameters<DropBookOptionsRoot>(path + "/CustomColorUtil/DropBookOptions", modId);
            ModParameters.DropBookOptions.AddRange(dropBookOptions.Key
                ? dropBookOptions.Value.DropBookOption
                : new List<DropBookOptionRoot>());
            var emotionCardOptions =
                LoadParameters<EmotionCardOptionsRoot>(path + "/CustomColorUtil/EmotionCardOptions", modId);
            ModParameters.EmotionCardOptions.AddRange(emotionCardOptions.Key
                ? emotionCardOptions.Value.EmotionCardOption
                : new List<EmotionCardOptionRoot>());
            var keypageOptions = LoadParameters<KeypageOptionsRoot>(path + "/CustomColorUtil/KeypageOptions", modId);
            ModParameters.KeypageOptions.AddRange(keypageOptions.Key
                ? keypageOptions.Value.KeypageOption
                : new List<KeypageOptionRoot>());
            var passiveOptions = LoadParameters<PassiveOptionsRoot>(path + "/CustomColorUtil/PassiveOptions", modId);
            ModParameters.PassiveOptions.AddRange(passiveOptions.Key
                ? passiveOptions.Value.PassiveColorOptions
                : new List<PassiveOptionRoot>());
            var stageOptions = LoadParameters<StageOptionsRoot>(path + "/CustomColorUtil/StageOptions", modId);
            ModParameters.StageOptions.AddRange(stageOptions.Key
                ? stageOptions.Value.StageOption
                : new List<StageOptionRoot>());
        }

        private static KeyValuePair<bool, T> LoadParameters<T>(string path, string packageId)
        {
            var error = false;
            try
            {
                var file = new DirectoryInfo(path).GetFiles().FirstOrDefault();
                error = true;
                if (file == null) return new KeyValuePair<bool, T>(false, default);
                using (var stringReader = new StringReader(File.ReadAllText(file.FullName)))
                {
                    var root =
                        (T)new XmlSerializer(typeof(T))
                            .Deserialize(stringReader);
                    return new KeyValuePair<bool, T>(true, root);
                }
            }
            catch (Exception ex)
            {
                if (error)
                    Debug.LogError($"Error loading {nameof(T)} packageId : " + packageId + " Error : " + ex.Message);
                return new KeyValuePair<bool, T>(false, default);
            }
        }
    }
}