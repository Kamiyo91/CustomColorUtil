﻿using CustomColorUtil.Util;
using EmotionCardUtil;
using HarmonyLib;
using UI;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class EmotionCardColorPatchWithoutTiphMod
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EmotionPassiveCardUI), "SetSprites")]
        [HarmonyPatch(typeof(UIEmotionPassiveCardInven), "SetSprites")]
        public static void EmotionPassiveCardUI_SetSprites(object __instance)
        {
            switch (__instance)
            {
                case EmotionPassiveCardUI instance:
                    if (!(instance.Card is EmotionCardXmlExtension cardExtension)) return;
                    ArtUtil.EmotionPassiveCardUISetSpritesPost(instance, cardExtension);
                    break;
                case UIEmotionPassiveCardInven instance:
                    if (!(instance.Card is EmotionCardXmlExtension cardExtension2)) return;
                    ArtUtil.EmotionPassiveCardUISetSpritesPost(instance, cardExtension2);
                    break;
            }
        }
    }
}