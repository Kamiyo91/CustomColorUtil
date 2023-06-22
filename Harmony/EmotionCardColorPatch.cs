using System.Linq;
using CustomColorUtil.Util;
using EmotionCardUtil;
using HarmonyLib;
using UI;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class EmotionCardColorPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmotionPassiveCardUI), "SetSprites")]
        [HarmonyPatch(typeof(UIEmotionPassiveCardInven), "SetSprites")]
        public static void EmotionPassiveCardUI_SetSprites_Pre(object __instance)
        {
            switch (__instance)
            {
                case EmotionPassiveCardUI instance:
                    ArtUtil.EmotionPassiveCardUISetSpritesPre(instance);
                    break;
                case UIEmotionPassiveCardInven instance:
                    ArtUtil.EmotionPassiveCardUISetSpritesPre(instance);
                    break;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIAbnormalityCardPreviewSlot), "Init")]
        public static void UIAbnormalityCardPreviewSlot_Init(UIAbnormalityCardPreviewSlot __instance, object card)
        {
            if (!(card is EmotionCardXmlExtension cardExtension)) return;
            __instance.artwork.sprite =
                Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(cardExtension.LorId.packageId,
                    cardExtension.Artwork);
            var emotionCardOption = ModParameters.EmotionCardOptions.FirstOrDefault(x =>
                x.PackageId == cardExtension.LorId.packageId && x.Id.Contains(cardExtension.LorId.id));
            if (emotionCardOption == null) return;
            var frameColor = emotionCardOption.ColorOptions.FrameColor.ConvertColor();
            if (frameColor != null) __instance.frame.color = frameColor.Value;
            var textColor = emotionCardOption.ColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            __instance.cardName.color = textColor.Value;
            __instance.cardName.GetComponent<TextMeshProMaterialSetter>().underlayColor =
                textColor.Value;
            __instance.cardName.gameObject.SetActive(false);
            __instance.cardName.gameObject.SetActive(true);
            __instance.cardLevel.color = textColor.Value;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BattleDialogUI), "TurnOnAbnormalityDlg")]
        public static void BattleDialogUI_TurnOnAbnormalityDlg(BattleDialogUI __instance, object card)
        {
            if (!(card is EmotionCardXmlExtension cardExtension)) return;
            var emotionCardOption = ModParameters.EmotionCardOptions.FirstOrDefault(x =>
                x.PackageId == cardExtension.LorId.packageId && x.Id.Contains(cardExtension.LorId.id));
            var textColor = emotionCardOption?.ColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            __instance._txtAbnormalityDlg.fontMaterial.SetColor("_GlowColor", textColor.Value);
            __instance._txtAbnormalityDlg.color = textColor.Value;
            __instance._txtAbnormalityDlg.GetComponent<AbnormalityDlgEffect>().Init();
        }
    }
}