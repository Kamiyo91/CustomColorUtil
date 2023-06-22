using System.Collections.Generic;
using CustomColorUtil.Models;
using UnityEngine;

namespace CustomColorUtil
{
    public static class ModParameters
    {
        public static string PackageId = "CustomColorUtil21341";
        public static HarmonyLib.Harmony Harmony = new HarmonyLib.Harmony("LOR.CustomColorUtil_MOD");
        public static List<CustomSprite> ArtWorks = new List<CustomSprite>();
        public static List<CardOptionRoot> CardOptions { get; set; } = new List<CardOptionRoot>();
        public static List<CategoryOptionRoot> CategoryOptions { get; set; } = new List<CategoryOptionRoot>();
        public static List<DropBookOptionRoot> DropBookOptions { get; set; } = new List<DropBookOptionRoot>();
        public static List<EmotionCardOptionRoot> EmotionCardOptions { get; set; } = new List<EmotionCardOptionRoot>();
        public static List<KeypageOptionRoot> KeypageOptions { get; set; } = new List<KeypageOptionRoot>();
        public static List<PassiveOptionRoot> PassiveOptions { get; set; } = new List<PassiveOptionRoot>();
        public static List<StageOptionRoot> StageOptions { get; set; } = new List<StageOptionRoot>();
        public static bool UtilLoaderDLLFound { get; set; } = false;
        public static bool EmotionCardLoaderFound { get; set; } = false;
        public static bool SpeedDiceColorModFound { get; set; } = false;
        public static bool TiphEgoModFound { get; set; } = false;
        public static bool ModLoaded { get; set; } = false;
    }

    public class CustomSprite
    {
        public string PackageId { get; set; }
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
    }
}