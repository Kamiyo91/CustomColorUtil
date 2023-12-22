using System.Collections.Generic;
using System.Linq;
using CustomColorUtil.Util;
using HarmonyLib;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class ColorPatch
    {
        [HarmonyPatch(typeof(BattleUnitInformationUI_PassiveList.BattleUnitInformationPassiveSlot), "SetData")]
        [HarmonyPostfix]
        public static void BattleUnitInformationPassiveSlot_SetData_Post(
            BattleUnitInformationUI_PassiveList.BattleUnitInformationPassiveSlot __instance, PassiveAbilityBase passive)
        {
            var passiveItem = ModParameters.PassiveOptions.FirstOrDefault(x =>
                x.PackageId == passive.id.packageId && x.Ids.Contains(passive.id.id));
            var frameColor = passiveItem?.PassiveColorOptions?.FrameColor.ConvertColor();
            if (frameColor == null) return;
            if (__instance.img_Icon != null)
                __instance.img_Icon.color = frameColor.Value;
            if (__instance.img_IconGlow != null)
                __instance.img_IconGlow.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UILibrarianEquipInfoSlot), "SetData")]
        [HarmonyPostfix]
        public static void UILibrarianEquipInfoSlot_SetData_Post(UILibrarianEquipInfoSlot __instance,
            BookPassiveInfo passive)
        {
            var passiveItem = ModParameters.PassiveOptions.FirstOrDefault(x =>
                x.PackageId == passive.passive.id.packageId && x.Ids.Contains(passive.passive.id.id));
            if (passiveItem == null) return;
            var frameColor = passiveItem.PassiveColorOptions?.FrameColor.ConvertColor();
            if (frameColor != null) __instance.Frame.color = frameColor.Value;
            var textColor = passiveItem.PassiveColorOptions?.TextColor.ConvertColor();
            if (textColor != null) __instance.txt_cost.color = textColor.Value;
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionSlot), "SetColorByRarity")]
        [HarmonyPostfix]
        public static void UIPassiveColor_SetColorByRarity(UIPassiveSuccessionSlot __instance, Color c)
        {
            if (__instance.passivemodel == null || c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var passiveItem = ModParameters.PassiveOptions.FirstOrDefault(x =>
                x.PackageId == __instance.passivemodel.reservedData.currentpassive.id.packageId &&
                x.Ids.Contains(__instance.passivemodel.reservedData.currentpassive.id.id));
            var frameColor = passiveItem?.PassiveColorOptions?.FrameColor.ConvertColor();
            if (frameColor == null) return;
            foreach (var graphic in __instance.rarityGraphics)
                graphic.CrossFadeColor(frameColor.Value, 0f, true, true);
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionCenterPassiveSlot), "SetData")]
        [HarmonyPatch(typeof(UIPassiveSuccessionPreviewPassiveSlot), "SetData")]
        [HarmonyPostfix]
        public static void UIPassive_SetData_Post(object __instance, PassiveModel passive)
        {
            var passiveItem = ModParameters.PassiveOptions.FirstOrDefault(x =>
                x.PackageId == passive.originData.currentpassive.id.packageId &&
                x.Ids.Contains(passive.originData.currentpassive.id.id));
            var frameColor = passiveItem?.PassiveColorOptions?.FrameColor.ConvertColor();
            if (frameColor == null) return;
            switch (__instance)
            {
                case UIPassiveSuccessionCenterPassiveSlot instance:
                    foreach (var graphic in instance.graphics_Rarity)
                        graphic.color = frameColor.Value;
                    break;
                case UIPassiveSuccessionPreviewPassiveSlot instance:
                    foreach (var graphic in instance.graphics_Rarity)
                        graphic.color = frameColor.Value;
                    break;
            }
        }

        [HarmonyPatch(typeof(UIDetailEgoCardSlot), "SetData")]
        [HarmonyPatch(typeof(UIOriginCardSlot), "SetData")]
        [HarmonyPrefix]
        public static void UICard_SetData_Pre(object __instance, DiceCardItemModel cardmodel)
        {
            if (cardmodel == null) return;
            switch (__instance)
            {
                case UIDetailEgoCardSlot instance:
                    ArtUtil.UICardSetDataPre(instance);
                    break;
                case UIOriginCardSlot instance:
                    ArtUtil.UICardSetDataPre(instance);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIDetailEgoCardSlot), "SetData")]
        [HarmonyPatch(typeof(UIOriginCardSlot), "SetData")]
        [HarmonyPostfix]
        public static void UICard_SetData_Post(object __instance, DiceCardItemModel cardmodel)
        {
            if (cardmodel == null) return;
            var cardItem = ModParameters.CardOptions.FirstOrDefault(x =>
                x.PackageId == cardmodel.GetID().packageId && x.Ids.Contains(cardmodel.GetID().id));
            if (cardItem == null) return;
            switch (__instance)
            {
                case UIDetailEgoCardSlot instance:
                    ArtUtil.UICardSetDataPost(instance, cardItem.CardColorOptions, cardmodel.GetID().packageId);
                    break;
                case UIOriginCardSlot instance:
                    ArtUtil.UICardSetDataPost(instance, cardItem.CardColorOptions, cardmodel.GetID().packageId);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIDetailCardSlot), "SetData")]
        [HarmonyPrefix]
        public static void UIDetailCardSlot_SetData_Pre(UIDetailCardSlot __instance, DiceCardItemModel cardmodel)
        {
            if (cardmodel == null) return;
            var gameObject = __instance.ob_selfAbility.transform.parent.parent.parent.gameObject;
            var rightFrame = gameObject.GetComponentsInChildren<Image>()
                .FirstOrDefault(x => x.name.Contains("[Image]BgFrame"));
            if (rightFrame == null) return;
            rightFrame.overrideSprite = null;
        }

        [HarmonyPatch(typeof(UIDetailCardSlot), "SetData")]
        [HarmonyPostfix]
        public static void UIDetailCardSlot_SetData(UIDetailCardSlot __instance, DiceCardItemModel cardmodel)
        {
            if (cardmodel == null) return;
            var gameObject = __instance.ob_selfAbility.transform.parent.parent.parent.gameObject;
            var rightFrame = gameObject.GetComponentsInChildren<Image>()
                .FirstOrDefault(x => x.name.Contains("[Image]BgFrame"));
            if (rightFrame == null) return;
            var cardItem = ModParameters.CardOptions.FirstOrDefault(x =>
                x.PackageId == cardmodel.GetID().packageId && x.Ids.Contains(cardmodel.GetID().id));
            if (cardItem == null ||
                string.IsNullOrEmpty(cardItem.CardColorOptions.RightFrame)) return;
            var rightFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                x.PackageId == cardmodel.GetID().packageId && x.Name == cardItem.CardColorOptions.RightFrame);
            if (rightFrameImg == null) return;
            rightFrame.overrideSprite = rightFrameImg.Sprite;
            rightFrame.overrideSprite.name = $"{cardItem.CardColorOptions.RightFrame}_RFrame";
            var cardColor = cardItem.CardColorOptions.CardColor.ConvertColor();
            if (cardItem.CardColorOptions.ApplySideFrontColors && cardColor != null)
                rightFrame.color = cardColor.Value;
            else rightFrame.color = Color.white;
        }

        [HarmonyPatch(typeof(UIOriginCardSlot), "SetRangeIconHsv")]
        [HarmonyPostfix]
        public static void UICard_SetRangeIconHsv_Post(UIOriginCardSlot __instance, Vector3 hsvvalue)
        {
            if (__instance.CardModel == null || hsvvalue == UIColorManager.Manager.CardRangeHsvValue[6]) return;
            var cardItem = ModParameters.CardOptions.FirstOrDefault(x =>
                x.PackageId == __instance.CardModel.GetID().packageId &&
                x.Ids.Contains(__instance.CardModel.GetID().id));
            if (cardItem == null) return;
            if (__instance.hsv_rangeIcon == null) return;
            var iconColor = cardItem.CardColorOptions.CustomIconColor.ConvertColor();
            if (iconColor != null)
                __instance.img_RangeIcon.color = iconColor.Value;
            if (!cardItem.CardColorOptions.UseHSVFilter)
            {
                __instance.hsv_rangeIcon.enabled = false;
                return;
            }

            var iconColorHSV = cardItem.CardColorOptions.IconColor;
            if (iconColorHSV == null ||
                hsvvalue == UIColorManager.Manager.CardRangeHsvValue[6]) return;
            __instance.hsv_rangeIcon._HueShift = iconColorHSV.H;
            __instance.hsv_rangeIcon._Saturation = iconColorHSV.S;
            __instance.hsv_rangeIcon._ValueBrightness = iconColorHSV.V;
            __instance.hsv_rangeIcon.CallUpdate();
            __instance.hsv_rangeIcon.enabled = false;
            __instance.hsv_rangeIcon.enabled = true;
        }

        [HarmonyPatch(typeof(BattleDiceCardUI), "SetRangeIconHsv")]
        [HarmonyPostfix]
        public static void BattleDiceCardUI_SetRangeIconHsv(BattleDiceCardUI __instance, Vector3 hsvvalue)
        {
            if (__instance.CardModel == null) return;
            var cardItem = ModParameters.CardOptions.FirstOrDefault(x =>
                x.PackageId == __instance.CardModel.GetID().packageId &&
                x.Ids.Contains(__instance.CardModel.GetID().id));
            if (cardItem == null) return;
            var iconColor = cardItem.CardColorOptions.CustomIconColor.ConvertColor();
            if (iconColor != null)
                __instance.img_icon.color = iconColor.Value;
            if (__instance.hsv_rangeIcon == null) return;
            if (!cardItem.CardColorOptions.UseHSVFilter)
            {
                __instance.hsv_rangeIcon.enabled = false;
                return;
            }

            var iconColorHSV = cardItem.CardColorOptions.IconColor;
            if (iconColorHSV == null ||
                hsvvalue == UIColorManager.Manager.CardRangeHsvValue[6]) return;
            __instance.hsv_rangeIcon._HueShift = iconColorHSV.H;
            __instance.hsv_rangeIcon._Saturation = iconColorHSV.S;
            __instance.hsv_rangeIcon._ValueBrightness = iconColorHSV.V;
            __instance.hsv_rangeIcon.CallUpdate();
            __instance.hsv_rangeIcon.enabled = false;
            __instance.hsv_rangeIcon.enabled = true;
        }

        [HarmonyPatch(typeof(BattleDiceCardUI), "SetRangeIconHsv")]
        [HarmonyPrefix]
        public static void BattleDiceCardUI_SetRangeIconHsv_Pre(BattleDiceCardUI __instance, Vector3 hsvvalue)
        {
            if (__instance.CardModel == null || __instance.hsv_rangeIcon == null) return;
            __instance.img_icon.color = Color.white;
            __instance.hsv_rangeIcon.enabled = true;
            __instance.hsv_rangeIcon._HueShift = hsvvalue.x;
            __instance.hsv_rangeIcon._Saturation = hsvvalue.y;
            __instance.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
            __instance.hsv_rangeIcon.CallUpdate();
        }

        [HarmonyPatch(typeof(BattleDiceCardUI), "SetCard")]
        [HarmonyPrefix]
        public static void UIBattleCard_SetCard_Pre(BattleDiceCardUI __instance, BattleDiceCardModel cardModel)
        {
            if (cardModel == null) return;
            var frame = __instance.img_Frames.ElementAtOrDefault(0);
            if (frame != null)
                frame.overrideSprite = null;
            var rightFrame = __instance.img_Frames.ElementAtOrDefault(4);
            if (rightFrame != null)
                rightFrame.overrideSprite = null;
            var component = __instance.img_artwork.transform.parent.parent.GetChild(1).GetComponent<Image>();
            if (component != null) component.overrideSprite = null;
            if (__instance.img_icon.overrideSprite != null) __instance.img_icon.overrideSprite = null;
        }

        [HarmonyPatch(typeof(BattleDiceCardUI), "SetCard")]
        [HarmonyPostfix]
        public static void UIBattleCard_SetCard_Post(BattleDiceCardUI __instance, BattleDiceCardModel cardModel)
        {
            var cardItem = ModParameters.CardOptions.FirstOrDefault(x =>
                x.PackageId == cardModel.GetID().packageId && x.Ids.Contains(cardModel.GetID().id));
            if (cardItem == null) return;
            if (cardItem.CardColorOptions.CustomDiceIcon.Any())
            {
                var behaviourList = __instance._cardModel.GetBehaviourList();
                for (var i = 0; i < behaviourList.Count; i++)
                {
                    var customIconData =
                        cardItem.CardColorOptions.CustomDiceIcon.FirstOrDefault(x => x.DiceNumber == i);
                    if (customIconData == null) continue;
                    var sprite = ModParameters.ArtWorks
                        .FirstOrDefault(x =>
                            x.PackageId == customIconData.PackageId && x.Name == customIconData.KeywordIconId)
                        ?.Sprite;
                    if (sprite == null) continue;
                    __instance.img_behaviourDetatilList[i].sprite = sprite;
                    __instance.img_behaviourDetatilList[i].gameObject.SetActive(true);
                }
            }

            var cardColor = cardItem.CardColorOptions.CardColor.ConvertColor();
            if (cardColor != null)
            {
                foreach (var img in __instance.img_Frames)
                    img.color = cardColor.Value;
                foreach (var img in __instance.img_linearDodges)
                    img.color = cardColor.Value;
                __instance.costNumbers.SetContentColor(cardColor.Value);
                __instance.colorFrame = cardColor.Value;
                __instance.colorLineardodge = cardColor.Value;
                __instance.colorLineardodge_deactive = cardColor.Value;
                __instance.img_icon.color = cardColor.Value;
            }

            var frame = __instance.img_Frames.ElementAtOrDefault(0);
            if (frame != null && !string.IsNullOrEmpty(cardItem.CardColorOptions.LeftFrame))
            {
                var leftFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                    x.PackageId == cardModel.GetID().packageId && x.Name == cardItem.CardColorOptions.LeftFrame);
                if (leftFrameImg != null)
                {
                    frame.overrideSprite = leftFrameImg.Sprite;
                    frame.overrideSprite.name = $"{cardItem.CardColorOptions.LeftFrame}_LFrame";
                    if (cardItem.CardColorOptions.ApplySideFrontColors && cardColor != null)
                        frame.color = cardColor.Value;
                    else frame.color = Color.white;
                }
            }

            var rightFrame = __instance.img_Frames.ElementAtOrDefault(4);
            if (rightFrame != null && !string.IsNullOrEmpty(cardItem.CardColorOptions.RightFrame))
            {
                var rightFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                    x.PackageId == cardModel.GetID().packageId && x.Name == cardItem.CardColorOptions.RightFrame);
                if (rightFrameImg != null)
                {
                    rightFrame.overrideSprite = rightFrameImg.Sprite;
                    rightFrame.overrideSprite.name = $"{cardItem.CardColorOptions.RightFrame}_LFrame";
                    if (cardItem.CardColorOptions.ApplySideFrontColors && cardColor != null)
                        rightFrame.color = cardColor.Value;
                    else rightFrame.color = Color.white;
                }
            }

            var component = __instance.img_artwork.transform.parent.parent.GetChild(1).GetComponent<Image>();
            if (component != null)
                if (!string.IsNullOrEmpty(cardItem.CardColorOptions.FrontFrame))
                {
                    var frontFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                        x.PackageId == cardModel.GetID().packageId && x.Name == cardItem.CardColorOptions.FrontFrame);
                    if (frontFrameImg != null)
                    {
                        component.overrideSprite = frontFrameImg.Sprite;
                        component.overrideSprite.name = $"{cardItem.CardColorOptions.FrontFrame}_FFrame";
                        if (cardItem.CardColorOptions.ApplyFrontColor && cardColor != null)
                            component.color = cardColor.Value;
                    }
                }

            if (string.IsNullOrEmpty(cardItem.CardColorOptions.CustomIcon)) return;
            var icon = ModParameters.ArtWorks.FirstOrDefault(x =>
                x.PackageId == cardModel.GetID().packageId && x.Name == cardItem.CardColorOptions.CustomIcon);
            if (icon == null) return;
            __instance.img_icon.overrideSprite = icon.Sprite;
        }

        [HarmonyPatch(typeof(UICharacterBookSlot), "SetHighlighted")]
        [HarmonyPrefix]
        public static void UICharacterBookSlot_SetHighlighted_Post_Pre(UICharacterBookSlot __instance)
        {
            if (__instance.BookModel == null) return;
            __instance.BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        [HarmonyPatch(typeof(UICharacterBookSlot), "SetHighlighted")]
        [HarmonyPostfix]
        public static void UICharacterBookSlot_SetHighlighted_Post(UICharacterBookSlot __instance, bool on)
        {
            if (__instance.BookModel == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.BookModel.BookId.packageId && x.Ids.Contains(__instance.BookModel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null || on) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                foreach (var graphic in __instance._targetGraphics.Where(x => x != null))
                    graphic.CrossFadeColor(frameColor.Value, 0.1f, true, true);
                foreach (var graphic in __instance._defaultGraphics.Where(x => x != null))
                    graphic.CrossFadeColor(frameColor.Value, 0.1f, true, true);
            }

            var component = __instance.BookName.GetComponent<TextMeshProMaterialSetter>();
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                if (component != null)
                {
                    component.independentSetting = true;
                    component.underlayColor = textColor.Value;
                    component.enabled = false;
                    component.enabled = true;
                }
                else
                {
                    __instance.BookName.fontMaterial.SetColor("_UnderlayColor",
                        textColor.Value);
                }

                __instance.BookName.color = textColor.Value;
            }

            __instance.BookName.gameObject.SetActive(false);
            __instance.BookName.gameObject.SetActive(true);
        }

        [HarmonyPatch(typeof(UIOriginEquipPageSlot), "SetGlowColor")]
        [HarmonyPrefix]
        public static void UIEquip_SetGlowColor_Pre(UIOriginEquipPageSlot __instance, Color gc)
        {
            if (__instance.BookDataModel == null ||
                gc == UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) return;
            __instance.BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        [HarmonyPatch(typeof(UIOriginEquipPageSlot), "SetGlowColor")]
        [HarmonyPostfix]
        public static void UIEquip_SetGlowColor(UIOriginEquipPageSlot __instance, Color gc)
        {
            if (__instance.BookDataModel == null ||
                gc == UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.BookDataModel.BookId.packageId &&
                x.Ids.Contains(__instance.BookDataModel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                __instance.setter_BookName.underlayColor = textColor.Value;
                __instance.setter_BookName.InitMaterialProperty();
                __instance.BookName.color = textColor.Value;
            }

            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            __instance.Frame.color = frameColor.Value;
            __instance.IconGlow.color = frameColor.Value;
            __instance.FrameGlow.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UILibrarianEquipDeckPanel), "SetDefaultColorPanel")]
        [HarmonyPrefix]
        public static void UILibrarianEquipDeckPanel_SetDefaultColorPanel_Pre(UILibrarianEquipDeckPanel __instance)
        {
            if (__instance._unitdata == null) return;
            __instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        [HarmonyPatch(typeof(UILibrarianEquipDeckPanel), "SetDefaultColorPanel")]
        [HarmonyPostfix]
        public static void UILibrarianEquipDeckPanel_SetDefaultColorPanel(UILibrarianEquipDeckPanel __instance)
        {
            if (__instance._unitdata == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance._unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance._unitdata.bookItem.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                __instance.txt_BookName.color = textColor.Value;
                __instance.setter_bookname.underlayColor = textColor.Value;
            }

            __instance.setter_bookname.enabled = false;
            __instance.setter_bookname.enabled = true;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            __instance.img_BookIcon.color = frameColor.Value;
            __instance.img_BookIconGlow.color = frameColor.Value;
            __instance.Frames.SetColor(frameColor.Value);
            __instance.img_Frame.color = frameColor.Value;
            __instance.img_LineFrame.color = frameColor.Value;
            __instance.DeckListPanel.img_DeckFrame.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UIEquipPageModelPreviewPanel), "SetData")]
        [HarmonyPatch(typeof(UIEquipPagePreviewPanel), "SetData")]
        [HarmonyPrefix]
        public static void UIEquipPage_SetData_Pre(object __instance)
        {
            switch (__instance)
            {
                case UIEquipPageModelPreviewPanel instance:
                    ArtUtil.UIEquipPageSetDataPre(instance);
                    break;
                case UIEquipPagePreviewPanel instance:
                    ArtUtil.UIEquipPageSetDataPre(instance);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIEquipPageModelPreviewPanel), "SetData")]
        [HarmonyPatch(typeof(UIEquipPagePreviewPanel), "SetData")]
        [HarmonyPostfix]
        public static void UIEquipPage_SetData(object __instance)
        {
            switch (__instance)
            {
                case UIEquipPageModelPreviewPanel instance:
                    ArtUtil.UIEquipPageSetDataPost(instance);
                    break;
                case UIEquipPagePreviewPanel instance:
                    ArtUtil.UIEquipPageSetDataPost(instance);
                    break;
            }
        }

        [HarmonyPatch(typeof(UILibrarianInfoPanel), "SetFrameColor")]
        [HarmonyPostfix]
        public static void UILibrarianInfoPanel_SetFrameColor(UILibrarianInfoPanel __instance)
        {
            if (__instance._selectedUnit == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance._selectedUnit.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance._selectedUnit.bookItem.BookId.id));
            var frameColor = keypageItem?.KeypageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            __instance.Frames.SetColor(frameColor.Value);
        }

        [HarmonyPatch(typeof(UILibrarianEquipBookInfoPanel), "SetUnitData")]
        [HarmonyPrefix]
        public static void UILibrarianEquipBookInfoPanel_SetUnitData_Pre(UILibrarianEquipBookInfoPanel __instance,
            UnitDataModel data)
        {
            if (data == null) return;
            __instance.bookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
            ArtUtil.ChangeColorToCombatPageList(UIColorManager.Manager.GetUIColor(UIColor.Default));
        }

        [HarmonyPatch(typeof(UILibrarianEquipBookInfoPanel), "SetUnitData")]
        [HarmonyPostfix]
        public static void UILibrarianEquipBookInfoPanel_SetUnitData(UILibrarianEquipBookInfoPanel __instance,
            UnitDataModel data)
        {
            if (data == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == data.bookItem.BookId.packageId && x.Ids.Contains(data.bookItem.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var component = __instance.bookName.GetComponent<TextMeshProMaterialSetter>();
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                __instance.bookName.color = textColor.Value;
                component.underlayColor = textColor.Value;
            }

            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            if (__instance.icon != null)
                __instance.icon.color = frameColor.Value;
            foreach (var graphic in __instance.targetGraphics)
                graphic.CrossFadeColor(frameColor.Value, 0.1f, true, true);
            component.InitMaterialProperty();
            ArtUtil.ChangeColorToCombatPageList(frameColor.Value);
        }

        [HarmonyPatch(typeof(UILibrarianEquipBookInfoPanel), "SetPassiveSlotColor")]
        [HarmonyPostfix]
        public static void UILibrarianEquipBookInfoPanel_SetPassiveSlotColor(UILibrarianEquipBookInfoPanel __instance,
            Color c)
        {
            if (__instance.unitData == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitData.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitData.bookItem.BookId.id));
            var frameColor = keypageItem?.KeypageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            foreach (var graphic in __instance.graphic_passivesSlot.Where(x => x != null))
                graphic.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionPreviewBookPanel), "SetColorByRarity")]
        [HarmonyPatch(typeof(UIPassiveSuccessionCenterEquipBookSlot), "SetColorByRarity")]
        [HarmonyPrefix]
        public static void UIPassiveSuccessionPanel_SetColorByRarity_Pre(object __instance)
        {
            switch (__instance)
            {
                case UIPassiveSuccessionPreviewBookPanel instance:
                    ArtUtil.UIPassiveSuccessionPanelSetColorByRarityPre(instance);
                    break;
                case UIPassiveSuccessionCenterEquipBookSlot instance:
                    ArtUtil.UIPassiveSuccessionPanelSetColorByRarityPre(instance);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionPreviewBookPanel), "SetColorByRarity")]
        [HarmonyPatch(typeof(UIPassiveSuccessionCenterEquipBookSlot), "SetColorByRarity")]
        [HarmonyPostfix]
        public static void UIPassiveSuccessionPanel_SetColorByRarity(object __instance)
        {
            switch (__instance)
            {
                case UIPassiveSuccessionPreviewBookPanel instance:
                    ArtUtil.UIPassiveSuccessionPanelSetColorByRarityPost(instance);
                    break;
                case UIPassiveSuccessionCenterEquipBookSlot instance:
                    ArtUtil.UIPassiveSuccessionPanelSetColorByRarityPost(instance);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionEquipBookSlot), "SetRarityColor")]
        [HarmonyPatch(typeof(UIPassiveEquipBookSlot), "SetRarityColor")]
        [HarmonyPrefix]
        public static void UIPassiveSuccessionEquipBookSlot_SetRarityColor_Pre(object __instance)
        {
            switch (__instance)
            {
                case UIPassiveEquipBookSlot instance:
                    ArtUtil.UIPassiveSuccessionEquipBookSlotSetRarityColorPre(instance);
                    break;
                case UIPassiveSuccessionEquipBookSlot instance:
                    ArtUtil.UIPassiveSuccessionEquipBookSlotSetRarityColorPre(instance);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionEquipBookSlot), "SetRarityColor")]
        [HarmonyPatch(typeof(UIPassiveEquipBookSlot), "SetRarityColor")]
        [HarmonyPostfix]
        public static void UIPassiveSuccessionEquipBookSlot_SetRarityColor(object __instance, Color c)
        {
            switch (__instance)
            {
                case UIPassiveEquipBookSlot instance:
                    ArtUtil.UIPassiveSuccessionEquipBookSlotSetRarityColorPost(instance, c);
                    break;
                case UIPassiveSuccessionEquipBookSlot instance:
                    ArtUtil.UIPassiveSuccessionEquipBookSlotSetRarityColorPost(instance, c);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionBookSlot), "SetDefaultColor")]
        [HarmonyPrefix]
        public static void UIPassiveSuccessionBookSlot_SetDefaultColor_Pre(UIPassiveSuccessionBookSlot __instance)
        {
            if (__instance.currentbookmodel == null) return;
            __instance.txt_bookname.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        [HarmonyPatch(typeof(UIPassiveSuccessionBookSlot), "SetDefaultColor")]
        [HarmonyPostfix]
        public static void UIPassiveSuccessionBookSlot_SetDefaultColor(UIPassiveSuccessionBookSlot __instance)
        {
            if (__instance.currentbookmodel == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.currentbookmodel.BookId.packageId &&
                x.Ids.Contains(__instance.currentbookmodel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                __instance.txt_bookname.color = textColor.Value;
                __instance.txt_bookname.enabled = false;
                __instance.txt_bookname.enabled = true;
                var component = __instance.txt_bookname.GetComponent<TextMeshProMaterialSetter>();
                if (component != null)
                    component.underlayColor = textColor.Value;
                if (__instance.txt_bookname.isActiveAndEnabled)
                    __instance.txt_bookname.fontMaterial.SetColor("_UnderlayColor",
                        textColor.Value);
            }

            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            __instance.equipSet.Frame.color = frameColor.Value;
            if (__instance.equipSet.FrameGlow != null)
                __instance.equipSet.FrameGlow.color = frameColor.Value;
            if (__instance.img_IconGlow != null)
                __instance.img_IconGlow.color = frameColor.Value;
            if (__instance.img_levelFrame != null)
                __instance.img_levelFrame.color = frameColor.Value;
            if (__instance.txt_booklevel != null)
                __instance.txt_booklevel.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UILibrarianInfoInCardPhase), "SetData")]
        [HarmonyPrefix]
        public static void UILibrarianInfoInCardPhase_SetData_Pre(UILibrarianInfoInCardPhase __instance)
        {
            if (__instance.unitdata == null) return;
            __instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        [HarmonyPatch(typeof(UILibrarianInfoInCardPhase), "SetData")]
        [HarmonyPostfix]
        public static void UILibrarianInfoInCardPhase_SetData(UILibrarianInfoInCardPhase __instance)
        {
            if (__instance.unitdata == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitdata.bookItem.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                if (UI.UIController.Instance.CurrentUIPhase != UIPhase.Main_ItemList)
                    foreach (var t in __instance.graphic_Frames)
                        t.color = frameColor.Value;
                __instance.img_BookIconGlow.color = frameColor.Value;
            }

            __instance.setter_bookname.Awake();
            __instance.setter_bookname.Start();
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            __instance.setter_bookname.underlayColor = textColor.Value;
            __instance.setter_bookname.enabled = false;
            __instance.setter_bookname.enabled = true;
            __instance.txt_BookName.color = textColor.Value;
        }

        [HarmonyPatch(typeof(UILibrarianInfoInCardPhase), "SetPassiveSlotColor")]
        [HarmonyPostfix]
        public static void UILibrarianInfoInCardPhase_SetPassiveSlotColor(UILibrarianInfoInCardPhase __instance,
            Color c)
        {
            if (__instance.unitdata == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitdata.bookItem.BookId.id));
            var frameColor = keypageItem?.KeypageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            foreach (var graphic in __instance.graphic_passivesSlot.Where(x => x != null))
                graphic.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UILibrarianInfoInCardPhase), "SetBattlePageSlotColor")]
        [HarmonyPostfix]
        public static void UILibrarianInfoInCardPhase_SetBattlePageSlotColor(UILibrarianInfoInCardPhase __instance,
            Color c)
        {
            if (__instance.unitdata == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitdata.bookItem.BookId.id));
            var frameColor = keypageItem?.KeypageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            foreach (var graphic in __instance.graphic_battlepageSlot.Where(x => x != null))
                graphic.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UILibrarianInfoInCardPhase), "OnPointerExitEquipPage")]
        [HarmonyPostfix]
        public static void UILibrarianInfoInCardPhase_OnPointerExitEquipPage(UILibrarianInfoInCardPhase __instance)
        {
            if (__instance.unitdata == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitdata.bookItem.BookId.id));
            var frameColor = keypageItem?.KeypageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            foreach (var t in __instance.graphic_Frames) t.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UIGachaEquipSlot), "SetDefaultColor")]
        [HarmonyPrefix]
        public static void UIGachaEquipSlot_SetDefaultColor_Pre(UIGachaEquipSlot __instance)
        {
            if (__instance._book == null) return;
            __instance.BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        [HarmonyPatch(typeof(UIGachaEquipSlot), "SetDefaultColor")]
        [HarmonyPostfix]
        public static void UIGachaEquipSlot_SetDefaultColor(UIGachaEquipSlot __instance)
        {
            if (__instance._book == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance._book.BookId.packageId && x.Ids.Contains(__instance._book.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                __instance.BookName.color = textColor.Value;
                var component = __instance.BookName.GetComponent<TextMeshProMaterialSetter>();
                if (component != null) component.underlayColor = textColor.Value;
                if (__instance.BookName.isActiveAndEnabled)
                    __instance.BookName.fontMaterial.SetColor("_UnderlayColor", textColor.Value);
            }

            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            __instance.Frame.color = frameColor.Value;
            __instance.FrameGlow.color = frameColor.Value;
            __instance.Icon.color = frameColor.Value;
            __instance.IconGlow.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UICustomSelectable), "OnPointerExit")]
        [HarmonyPostfix]
        public static async void UICustomSelectable_OnPointerExit(UICustomSelectable __instance,
            PointerEventData eventData)
        {
            if (!__instance.gameObject.name.Contains("[Button]CustomSelectableGraphic")) return;
            if (UI.UIController.Instance.CurrentUIPhase != UIPhase.Librarian) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == UI.UIController.Instance.CurrentUnit.bookItem.BookId.packageId &&
                x.Ids.Contains(UI.UIController.Instance.CurrentUnit.bookItem.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            var color = frameColor ??
                        UIColorManager.Manager.GetUIColor(UIColor.Default);
            if (eventData.pointerCurrentRaycast.gameObject != null &&
                eventData.pointerCurrentRaycast.gameObject.name.Contains("[Xbox]SelectableTarget")) return;
            await GenericUtil.PutTaskDelay(30);
            ArtUtil.ChangeColorToCombatPageList(color);
        }

        [HarmonyPatch(typeof(UIBattleSettingLibrarianInfoPanel), "SetEquipPageSlotColor")]
        [HarmonyPrefix]
        public static void UIBattleSettingLibrarianInfoPanel_SetEquipPageSlotColor_Pre(
            UIBattleSettingLibrarianInfoPanel __instance)
        {
            if (__instance.unitdata == null) return;
            __instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
            foreach (var img in __instance.GetComponentsInChildren<Image>()
                         .Where(x => x.name.Contains("[Image]CenterFrame")))
                img.color = __instance.isSephirahPanel
                    ? UIColorManager.Manager.GetUIColor(UIColor.Default)
                    : UIColorManager.Manager.GetUIColor(UIColor.Disabled);
        }

        [HarmonyPatch(typeof(UIBattleSettingLibrarianInfoPanel), "SetEquipPageSlotColor")]
        [HarmonyPostfix]
        public static void UIBattleSettingLibrarianInfoPanel_SetEquipPageSlotColor(
            UIBattleSettingLibrarianInfoPanel __instance)
        {
            if (__instance.unitdata == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitdata.bookItem.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                __instance.setter_bookname.underlayColor = textColor.Value;
                __instance.txt_BookName.color = textColor.Value;
            }

            __instance.setter_bookname.InitMaterialProperty();
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            __instance.img_BookIconGlow.color = frameColor.Value;
            foreach (var graphic in __instance.graphic_Frames.Where(x => x != null))
                graphic.color = frameColor.Value;
            foreach (var img in __instance.GetComponentsInChildren<Image>()
                         .Where(x => x.name.Contains("[Image]CenterFrame")))
                img.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UIBattleSettingLibrarianInfoPanel), "SetBattlePageSlotColor")]
        [HarmonyPostfix]
        public static void UIBattleSettingLibrarianInfoPanel_SetBattlePageSlotColor_Post(
            UIBattleSettingLibrarianInfoPanel __instance, Color c)
        {
            if (__instance.unitdata == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == __instance.unitdata.bookItem.BookId.packageId &&
                x.Ids.Contains(__instance.unitdata.bookItem.BookId.id));
            var frameColor = keypageItem?.KeypageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            foreach (var graphic in __instance.graphic_battlepageSlot.Where(x => x != null))
                graphic.color = frameColor.Value;
        }

        [HarmonyPatch(typeof(UIBookSlot), "SetGlowColor")]
        [HarmonyPostfix]
        public static void UIBookSlot_SetGlowColor(UIBookSlot __instance, Color c)
        {
            if (__instance.BookId == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted) ||
                c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var dropBookOption = ModParameters.DropBookOptions.FirstOrDefault(x =>
                x.PackageId == __instance.BookId.packageId && x.Ids.Contains(__instance.BookId.id));
            if (dropBookOption?.DropBookColorOptions == null) return;
            var frameColor = dropBookOption.DropBookColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                __instance.Frame.color = frameColor.Value;
                __instance.FrameGlow.color = frameColor.Value;
                __instance.Icon.color = frameColor.Value;
                if (__instance.IconGlow != null)
                    __instance.IconGlow.color = frameColor.Value;
            }

            var textColor = dropBookOption.DropBookColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            __instance.BookName.color = textColor.Value;
            var component = __instance.BookName.GetComponent<TextMeshProMaterialSetter>();
            component.underlayColor = textColor.Value;
            component.enabled = false;
            component.enabled = true;
        }

        [HarmonyPatch(typeof(UIInvitationDropBookSlot), "SetColor")]
        [HarmonyPatch(typeof(UIAddedFeedBookSlot), "SetColor")]
        [HarmonyPostfix]
        public static void UIInvitation_SetColor(object __instance, Color c)
        {
            switch (__instance)
            {
                case UIAddedFeedBookSlot instance:
                    ArtUtil.UIInvitationSetColorPost(instance, c);
                    break;
                case UIInvitationDropBookSlot instance:
                    ArtUtil.UIInvitationSetColorPost(instance, c);
                    break;
            }
        }

        [HarmonyPatch(typeof(UIInvenFeedBookSlot), "SetColor")]
        [HarmonyPostfix]
        public static void UIInvenFeedBookSlot_SetColor(UIInvenFeedBookSlot __instance, Color c)
        {
            if (__instance.BookId == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted) ||
                c == UIColorManager.Manager.GetUIColor(UIColor.Disabled) ||
                __instance.bookNumRoot == null) return;
            var dropBookOption = ModParameters.DropBookOptions.FirstOrDefault(x =>
                x.PackageId == __instance.BookId.packageId && x.Ids.Contains(__instance.BookId.id));
            if (dropBookOption?.DropBookColorOptions == null) return;
            var frameColor = dropBookOption.DropBookColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                __instance.bookNumBg.color = frameColor.Value;
                __instance.plusButton.specialColor = frameColor.Value;
                __instance.plusButton.SetDefault();
                __instance.minusButton.specialColor = frameColor.Value;
                __instance.minusButton.SetDefault();
            }

            var textColor = dropBookOption.DropBookColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            __instance.txt_bookNum.color = textColor.Value;
        }

        [HarmonyPatch(typeof(UIInvitationBookSlot), "SetColor", typeof(Color))]
        [HarmonyPostfix]
        public static void UIInvitationBookSlot_SetColor(UIInvitationBookSlot __instance, Color c)
        {
            if (__instance.Appliedbookid == null) return;
            if (c == UIColorManager.Manager.GetUIColor(UIColor.Default))
            {
                var dropBookOption = ModParameters.DropBookOptions.FirstOrDefault(x =>
                    x.PackageId == __instance.Appliedbookid.packageId && x.Ids.Contains(__instance.Appliedbookid.id));
                if (dropBookOption?.DropBookColorOptions == null) return;
                var frameColor = dropBookOption.DropBookColorOptions.FrameColor.ConvertColor();
                if (frameColor != null)
                {
                    __instance.Frame.color = frameColor.Value;
                    __instance.FrameGlow.color = frameColor.Value;
                    __instance.Icon.color = frameColor.Value;
                    if (__instance.IconGlow != null)
                        __instance.IconGlow.color = frameColor.Value;
                }

                var textColor = dropBookOption.DropBookColorOptions.TextColor.ConvertColor();
                if (textColor == null) return;
                __instance.BookName.color = textColor.Value;
                __instance.BookName.fontMaterial.SetColor("_UnderlayColor",
                    textColor.Value);
            }
            else
            {
                var recipe = __instance.rootPanel.GetBookRecipe();
                if (recipe == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted) ||
                    c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
                var stageOption = ModParameters.StageOptions.FirstOrDefault(x =>
                    x.PackageId == recipe.id.packageId && x.Ids.Contains(recipe.id.id));
                if (stageOption?.StageColorOptions == null) return;
                var frameColor = stageOption.StageColorOptions.FrameColor.ConvertColor();
                if (frameColor != null)
                {
                    __instance.Frame.color = frameColor.Value;
                    __instance.FrameGlow.color = frameColor.Value;
                    __instance.Icon.color = frameColor.Value;
                    if (__instance.IconGlow != null)
                        __instance.IconGlow.color = frameColor.Value;
                }

                var textColor = stageOption.StageColorOptions.TextColor.ConvertColor();
                if (textColor == null) return;
                __instance.BookName.color = textColor.Value;
                __instance.BookName.fontMaterial.SetColor("_UnderlayColor", textColor.Value);
            }
        }

        [HarmonyPatch(typeof(UIInvitationRightMainPanel), "SetColorAllFrames")]
        [HarmonyPostfix]
        public static void UIInvitationRightMainPanel_SetColorAllFrames(UIInvitationRightMainPanel __instance, Color c)
        {
            var recipe = __instance.GetBookRecipe();
            if (recipe == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted) ||
                c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var stageOption = ModParameters.StageOptions.FirstOrDefault(x =>
                x.PackageId == recipe.id.packageId && x.Ids.Contains(recipe.id.id));
            if (stageOption?.StageColorOptions == null) return;
            var frameColor = stageOption.StageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                foreach (var t in __instance.AllFrames)
                    t.CrossFadeColor(frameColor.Value, 0f, false, false);
                var color = frameColor.Value;
                color.a = 0.5f;
                __instance.ButtonFrameHighlight.GetComponent<Image>().color = color;
                if (__instance.button_SendButton.interactable)
                    __instance.button_SendButton.SetColor(frameColor.Value);
            }

            var textColor = stageOption.StageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            foreach (var textMeshProMaterialSetter in __instance.setter_changetxts)
            {
                if (textMeshProMaterialSetter.isActiveAndEnabled)
                    textMeshProMaterialSetter.underlayColor = textColor.Value;
                textMeshProMaterialSetter.enabled = false;
                textMeshProMaterialSetter.enabled = true;
            }
        }

        [HarmonyPatch(typeof(UIInvitationRightMainPanel), "OnPointerExit_SendButton")]
        [HarmonyPostfix]
        public static void UIInvitationRightMainPanel_OnPointerExit_SendButton(UIInvitationRightMainPanel __instance)
        {
            var recipe = __instance.GetBookRecipe();
            if (recipe == null || __instance.currentColor == UIColorManager.Manager.GetUIColor(UIColor.Default) ||
                __instance.currentColor == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var stageOption = ModParameters.StageOptions.FirstOrDefault(x => x.PackageId == recipe.id.packageId);
            var frameColor = stageOption?.StageColorOptions?.FrameColor?.ConvertColor();
            if (frameColor == null) return;
            if (__instance.button_SendButton.interactable)
                __instance.button_SendButton.SetColor(frameColor.Value);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UISettingInvenEquipPageListSlot), "SetBooksData")]
        [HarmonyPatch(typeof(UIInvenEquipPageListSlot), "SetBooksData")]
        public static void General_SetBooksData_Pre(object __instance)
        {
            switch (__instance)
            {
                case UISettingInvenEquipPageListSlot instance:
                    ArtUtil.ResetColorData(instance);
                    break;
                case UIInvenEquipPageListSlot instance:
                    ArtUtil.ResetColorData(instance);
                    break;
            }
        }

        [HarmonyPostfix]
        [HarmonyPriority(0)]
        [HarmonyPatch(typeof(UISettingInvenEquipPageListSlot), "SetBooksData")]
        [HarmonyPatch(typeof(UIInvenEquipPageListSlot), "SetBooksData")]
        public static void General_SetBooksData(object __instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            switch (__instance)
            {
                case UISettingInvenEquipPageListSlot instance:
                    ArtUtil.SetBooksData(instance, books, storyKey);
                    break;
                case UIInvenEquipPageListSlot instance:
                    ArtUtil.SetBooksData(instance, books, storyKey);
                    break;
            }
        }
    }
}