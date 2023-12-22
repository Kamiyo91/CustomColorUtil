using System;
using System.IO;
using System.Reflection;
using CustomColorUtil.Harmony;
using CustomColorUtil.Util;
using UnityEngine.SceneManagement;

namespace CustomColorUtil
{
    public class ModInit : ModInitializer
    {
        public override void OnInitializeMod()
        {
            ArtUtil.GetArtWorks(
                new DirectoryInfo(
                    Path.GetDirectoryName(
                        Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)) +
                    "/ArtWork"), ModParameters.PackageId);
            GenericUtil.OtherModCheck();
            ModParametersUtilLoader.LoadMods();
            ModParameters.Harmony.CreateClassProcessor(typeof(ColorPatch)).Patch();
            ModParameters.Harmony.CreateClassProcessor(typeof(CombatDiceColorPatch)).Patch();
            if (ModParameters.UtilLoaderDLLFound)
                ModParameters.Harmony.CreateClassProcessor(ModParameters.SpeedDiceColorModFound
                    ? typeof(SpeedDieColorPatchWithPattyModAndUtil)
                    : typeof(SpeedDieColorPatchWithUtil)).Patch();
            else
                ModParameters.Harmony.CreateClassProcessor(ModParameters.SpeedDiceColorModFound
                    ? typeof(SpeedDieColorPatchWithPattyMod)
                    : typeof(SpeedDieColorPatch)).Patch();
            SceneManager.sceneLoaded += GenericUtil.OnLoadingScreen;
            GenericUtil.RemoveError();
        }
    }
}