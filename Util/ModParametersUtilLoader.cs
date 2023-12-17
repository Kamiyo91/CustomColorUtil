using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using CustomColorUtil.Models;
using Mod;
using Debug = UnityEngine.Debug;

namespace CustomColorUtil.Util
{
    public static class ModParametersUtilLoader
    {
        private static readonly List<string> IgnoreDll = new List<string>
        {
            "0Harmony", "Mono.Cecil", "MonoMod.RuntimeDetour", "MonoMod.Utils", "1BigDLL4221", "1SMotion-Loader",
            "1CustomColorUtil"
        };

        public static void LoadMods()
        {
            foreach (var modContentInfo in Singleton<ModContentManager>.Instance.GetAllMods().Where(modContentInfo =>
                         modContentInfo.activated &&
                         modContentInfo.invInfo.workshopInfo.uniqueId != ModParameters.PackageId))
                try
                {
                    var modId = modContentInfo.invInfo?.workshopInfo?.uniqueId;
                    var path = modContentInfo.dirInfo.FullName + "/Assemblies";
                    if (string.IsNullOrEmpty(modId) || !Directory.Exists(path)) continue;
                    var loadBy =
                        Directory.Exists(modContentInfo.dirInfo.FullName + "/Assemblies/CustomColorUtil")
                            ? "XML"
                            : "";
                    var directoryInfo = new DirectoryInfo(path);
                    var assemblies = (from fileInfo in directoryInfo.GetFiles()
                        where fileInfo.Extension.ToLower() == ".dll" && !IgnoreDll.Contains(fileInfo.FullName)
                        select Assembly.LoadFile(fileInfo.FullName)).ToList();
                    if (string.IsNullOrEmpty(loadBy) &&
                        assemblies.Any(x => x.GetType($"{x.GetName().Name}.CustomColorLoader21341") != null))
                        loadBy = "DLL";
                    if (string.IsNullOrEmpty(loadBy)) continue;
                    var stopwatch = new Stopwatch();
                    Debug.Log($"Custom Color Util Tool : Start loading mod files {modId} at path {path}");
                    stopwatch.Start();
                    var loadByLog = "Parameters loaded by XML";
                    if (loadBy.Equals("DLL"))
                    {
                        loadByLog = "Parameters loaded by DLL";
                        LoadModParametersFromDLL(assemblies);
                    }
                    else
                    {
                        LoadModParameters(path, modId);
                    }

                    ArtUtil.GetArtWorks(new DirectoryInfo(path + "/CustomColorUtil/ArtWork"), modId);
                    stopwatch.Stop();
                    Debug.Log(
                        $"Custom Color Util Tool : Loading mod files {modId} at path {path} finished in {stopwatch.ElapsedMilliseconds} ms - {loadByLog}");
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"Error while loading the mod {modContentInfo.invInfo?.workshopInfo?.uniqueId} - {ex.Message}");
                }
        }

        private static void LoadModParametersFromDLL(List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
                LoadModParametersDLLInternal(assembly);
        }

        private static void LoadModParametersDLLInternal(Assembly assembly)
        {
            var loaderType = assembly.GetType($"{assembly.GetName().Name}.CustomColorLoader21341");
            if (loaderType == null) return;
            ModParameters.CardOptions.AddRange(
                LoadModParametersDLLMethod<List<CardOptionRoot>>(loaderType.GetMethod(nameof(CardOptionRoot))));
            ModParameters.CategoryOptions.AddRange(
                LoadModParametersDLLMethod<List<CategoryOptionRoot>>(loaderType.GetMethod(nameof(CategoryOptionRoot))));
            ModParameters.DropBookOptions.AddRange(
                LoadModParametersDLLMethod<List<DropBookOptionRoot>>(loaderType.GetMethod(nameof(DropBookOptionRoot))));
            ModParameters.EmotionCardOptions.AddRange(
                LoadModParametersDLLMethod<List<EmotionCardOptionRoot>>(
                    loaderType.GetMethod(nameof(EmotionCardOptionRoot))));
            ModParameters.KeypageOptions.AddRange(
                LoadModParametersDLLMethod<List<KeypageOptionRoot>>(loaderType.GetMethod(nameof(KeypageOptionRoot))));
            ModParameters.PassiveOptions.AddRange(
                LoadModParametersDLLMethod<List<PassiveOptionRoot>>(loaderType.GetMethod(nameof(PassiveOptionRoot))));
            ModParameters.StageOptions.AddRange(
                LoadModParametersDLLMethod<List<StageOptionRoot>>(loaderType.GetMethod(nameof(StageOptionRoot))));
        }

        public static T LoadModParametersDLLMethod<T>(MethodInfo method) where T : new()
        {
            if (method == null) return new T();
            return (T)method.Invoke(null, null);
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