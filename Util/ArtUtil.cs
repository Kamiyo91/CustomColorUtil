using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomColorUtil.Models;
using EmotionCardUtil;
using LOR_BattleUnit_UI;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CustomColorUtil.Util
{
    public static class ArtUtil
    {
        public static void GetArtWorks(DirectoryInfo dir, string packageId)
        {
            try
            {
                if (dir.GetDirectories().Length != 0)
                {
                    var directories = dir.GetDirectories();
                    foreach (var t in directories) GetArtWorks(t, packageId);
                }

                foreach (var fileInfo in dir.GetFiles())
                {
                    var texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
                    var value = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),
                        new Vector2(0f, 0f));
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                    ModParameters.ArtWorks.Add(new CustomSprite
                        { PackageId = packageId, Name = fileNameWithoutExtension, Sprite = value });
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void ChangeColorToCombatPageList(Color c)
        {
            foreach (var text in Object.FindObjectsOfType<TextMeshProUGUI>()
                         .Where(x => x.GetComponentInParent<UICustomSelectable>() &&
                                     x.name.Contains("[Text]Feedbook_TextMesh")))
                text.color = c;
            foreach (var img in Object.FindObjectsOfType<Image>()
                         .Where(x => x.GetComponentInParent<UICustomSelectable>() &&
                                     x.name.Contains("[Image]buttonImage")))
                img.color = c;
            foreach (var img in Object.FindObjectsOfType<Image>()
                         .Where(x => x.GetComponentInParent<UICustomSelectable>() && x.name.Contains("[Image]Line")))
                img.color = c;
        }

        public static void UICardSetDataPre(UIDetailEgoCardSlot instance)
        {
            var frame = instance.img_Frames.FirstOrDefault(x => x.name.Contains("[Image]NormalFrame"));
            if (frame != null) frame.overrideSprite = null;
            var component = instance.img_Artwork.transform.parent.parent.GetChild(1).GetComponent<Image>();
            if (component != null) component.overrideSprite = null;
        }

        public static void UICardSetDataPre(UIOriginCardSlot instance)
        {
            var frame = instance.img_Frames.FirstOrDefault(x => x.name.Contains("[Image]NormalFrame"));
            if (frame != null) frame.overrideSprite = null;
            var component = instance.img_Artwork.transform.parent.parent.GetChild(1).GetComponent<Image>();
            if (component != null) component.overrideSprite = null;
        }

        public static void UICardSetDataPost(UIDetailEgoCardSlot instance, CardColorOptionRoot cardColorOption,
            string packageId)
        {
            var cardColor = cardColorOption.CardColor.ConvertColor();
            if (cardColor != null)
            {
                foreach (var img in instance.img_Frames)
                    img.color = cardColor.Value;
                foreach (var img in instance.img_linearDodge)
                    img.color = cardColor.Value;
                instance.costNumbers.SetContentColor(cardColor.Value);
                instance.colorFrame = cardColor.Value;
                instance.colorLineardodge = cardColor.Value;
                instance.img_RangeIcon.color = cardColor.Value;
            }

            var frame = instance.img_Frames.FirstOrDefault(x => x.name.Contains("[Image]NormalFrame"));
            if (frame != null)
                if (!string.IsNullOrEmpty(cardColorOption.LeftFrame))
                {
                    var leftFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                        x.PackageId == packageId && x.Name == cardColorOption.LeftFrame);
                    if (leftFrameImg != null)
                    {
                        frame.overrideSprite = leftFrameImg.Sprite;
                        frame.overrideSprite.name = $"{cardColorOption.LeftFrame}_LFrame";
                        if (cardColorOption.ApplySideFrontColors && cardColor != null)
                            frame.color = cardColor.Value;
                        else frame.color = Color.white;
                    }
                }

            var component = instance.img_Artwork.transform.parent.parent.GetChild(1).GetComponent<Image>();
            if (component != null)
                if (!string.IsNullOrEmpty(cardColorOption.FrontFrame))
                {
                    var frontFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                        x.PackageId == packageId && x.Name == cardColorOption.FrontFrame);
                    if (frontFrameImg != null)
                    {
                        component.overrideSprite = frontFrameImg.Sprite;
                        component.overrideSprite.name = $"{cardColorOption.FrontFrame}_FFrame";
                        if (cardColorOption.ApplyFrontColor && cardColor != null)
                            component.color = cardColor.Value;
                    }
                }

            if (string.IsNullOrEmpty(cardColorOption.CustomIcon)) return;
            var icon = ModParameters.ArtWorks.FirstOrDefault(x =>
                x.PackageId == packageId && x.Name == cardColorOption.CustomIcon);
            if (icon == null) return;
            instance.img_RangeIcon.overrideSprite = icon.Sprite;
        }

        public static void UICardSetDataPost(UIOriginCardSlot instance, CardColorOptionRoot cardColorOption,
            string packageId)
        {
            var cardColor = cardColorOption.CardColor.ConvertColor();
            if (cardColor != null)
            {
                foreach (var img in instance.img_Frames)
                    img.color = cardColor.Value;
                foreach (var img in instance.img_linearDodge)
                    img.color = cardColor.Value;
                instance.costNumbers.SetContentColor(cardColor.Value);
                instance.colorFrame = cardColor.Value;
                instance.colorLineardodge = cardColor.Value;
                instance.img_RangeIcon.color = cardColor.Value;
            }

            var frame = instance.img_Frames.FirstOrDefault(x => x.name.Contains("[Image]NormalFrame"));
            if (frame != null)
                if (!string.IsNullOrEmpty(cardColorOption.LeftFrame))
                {
                    var leftFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                        x.PackageId == packageId && x.Name == cardColorOption.LeftFrame);
                    if (leftFrameImg != null)
                    {
                        frame.overrideSprite = leftFrameImg.Sprite;
                        frame.overrideSprite.name = $"{cardColorOption.LeftFrame}_LFrame";
                        if (cardColorOption.ApplySideFrontColors && cardColor != null)
                            frame.color = cardColor.Value;
                        else frame.color = Color.white;
                    }
                }

            var component = instance.img_Artwork.transform.parent.parent.GetChild(1).GetComponent<Image>();
            if (component != null)
                if (!string.IsNullOrEmpty(cardColorOption.FrontFrame))
                {
                    var frontFrameImg = ModParameters.ArtWorks.FirstOrDefault(x =>
                        x.PackageId == packageId && x.Name == cardColorOption.FrontFrame);
                    if (frontFrameImg != null)
                    {
                        component.overrideSprite = frontFrameImg.Sprite;
                        component.overrideSprite.name = $"{cardColorOption.FrontFrame}_FFrame";
                        if (cardColorOption.ApplyFrontColor && cardColor != null)
                            component.color = cardColor.Value;
                    }
                }

            if (string.IsNullOrEmpty(cardColorOption.CustomIcon)) return;
            var icon = ModParameters.ArtWorks.FirstOrDefault(x =>
                x.PackageId == packageId && x.Name == cardColorOption.CustomIcon);
            if (icon == null) return;
            instance.img_RangeIcon.overrideSprite = icon.Sprite;
        }

        public static void UIEquipPageSetDataPre(UIEquipPagePreviewPanel instance)
        {
            if (instance.bookDataModel == null) return;
            instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        public static void UIEquipPageSetDataPre(UIEquipPageModelPreviewPanel instance)
        {
            if (instance.bookDataModel == null) return;
            instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        public static void UIEquipPageSetDataPost(UIEquipPageModelPreviewPanel instance)
        {
            if (instance.bookDataModel == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == instance.bookDataModel.BookId.packageId &&
                x.Ids.Contains(instance.bookDataModel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
                foreach (var t in instance.graphic_Frames)
                    t.color = frameColor.Value;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.setter_bookname.underlayColor = textColor.Value;
            instance.setter_bookname.enabled = false;
            instance.setter_bookname.enabled = true;
            instance.txt_BookName.color = textColor.Value;
        }

        public static void UIEquipPageSetDataPost(UIEquipPagePreviewPanel instance)
        {
            if (instance.bookDataModel == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == instance.bookDataModel.BookId.packageId &&
                x.Ids.Contains(instance.bookDataModel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
                foreach (var t in instance.graphic_Frames)
                    t.color = frameColor.Value;
            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.setter_bookname.underlayColor = textColor.Value;
            instance.setter_bookname.enabled = false;
            instance.setter_bookname.enabled = true;
            instance.txt_BookName.color = textColor.Value;
        }

        public static void UIPassiveSuccessionPanelSetColorByRarityPre(UIPassiveSuccessionPreviewBookPanel instance)
        {
            if (instance._currentbookmodel == null) return;
            instance.txt_name.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        public static void UIPassiveSuccessionPanelSetColorByRarityPre(UIPassiveSuccessionCenterEquipBookSlot instance)
        {
            if (instance._currentbookmodel == null) return;
            instance.txt_name.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        public static void UIPassiveSuccessionPanelSetColorByRarityPost(UIPassiveSuccessionPreviewBookPanel instance)
        {
            if (instance._currentbookmodel == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == instance._currentbookmodel.BookId.packageId &&
                x.Ids.Contains(instance._currentbookmodel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                instance.img_Frame.color = frameColor.Value;
                instance.img_IconGlow.color = frameColor.Value;
            }

            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.setter_name.underlayColor = textColor.Value;
            instance.txt_name.color = textColor.Value;
        }

        public static void UIPassiveSuccessionPanelSetColorByRarityPost(UIPassiveSuccessionCenterEquipBookSlot instance)
        {
            if (instance._currentbookmodel == null) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == instance._currentbookmodel.BookId.packageId &&
                x.Ids.Contains(instance._currentbookmodel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                instance.img_Frame.color = frameColor.Value;
                instance.img_IconGlow.color = frameColor.Value;
            }

            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.setter_name.underlayColor = textColor.Value;
            instance.txt_name.color = textColor.Value;
        }

        public static void UIPassiveSuccessionEquipBookSlotSetRarityColorPre(UIPassiveEquipBookSlot instance)
        {
            if (instance.bookmodel == null) return;
            instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
            instance.img_Frame.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
            instance.img_IconGlow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
            instance.setter_txtbookname.underlayColor = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
            instance.setter_txtbookname.faceColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        public static void UIPassiveSuccessionEquipBookSlotSetRarityColorPre(UIPassiveSuccessionEquipBookSlot instance)
        {
            if (instance.bookmodel == null) return;
            instance.txt_BookName.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
            instance.img_Frame.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
            instance.img_IconGlow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
            instance.setter_txtbookname.underlayColor = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
            instance.setter_txtbookname.faceColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
        }

        public static void UIPassiveSuccessionEquipBookSlotSetRarityColorPost(UIPassiveEquipBookSlot instance, Color c)
        {
            if (instance.bookmodel == null || c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == instance.bookmodel.BookId.packageId && x.Ids.Contains(instance.bookmodel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                instance.img_Frame.color = frameColor.Value;
                instance.img_IconGlow.color = frameColor.Value;
            }

            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.setter_txtbookname.underlayColor = textColor.Value;
            instance.setter_txtbookname.faceColor = textColor.Value;
            instance.txt_BookName.color = textColor.Value;
        }

        public static void UIPassiveSuccessionEquipBookSlotSetRarityColorPost(UIPassiveSuccessionEquipBookSlot instance,
            Color c)
        {
            if (instance.bookmodel == null || c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var keypageItem = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == instance.bookmodel.BookId.packageId && x.Ids.Contains(instance.bookmodel.BookId.id));
            if (keypageItem?.KeypageColorOptions == null) return;
            var frameColor = keypageItem.KeypageColorOptions.FrameColor.ConvertColor();
            if (frameColor != null)
            {
                instance.img_Frame.color = frameColor.Value;
                instance.img_IconGlow.color = frameColor.Value;
            }

            var textColor = keypageItem.KeypageColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.setter_txtbookname.underlayColor = textColor.Value;
            instance.setter_txtbookname.faceColor = textColor.Value;
            instance.txt_BookName.color = textColor.Value;
        }

        public static void UIInvitationSetColorPost(UIAddedFeedBookSlot instance, Color c)
        {
            if (instance.BookId == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted) ||
                c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var dropBookOption = ModParameters.DropBookOptions.FirstOrDefault(x =>
                x.PackageId == instance.BookId.packageId && x.Ids.Contains(instance.BookId.id));
            if (dropBookOption?.DropBookColorOptions == null) return;
            var frameColor = dropBookOption.DropBookColorOptions.FrameColor.ConvertColor();
            if (frameColor != null) instance.bookNumBg.color = frameColor.Value;
            var textColor = dropBookOption.DropBookColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.txt_bookNum.color = textColor.Value;
        }

        public static void UIInvitationSetColorPost(UIInvitationDropBookSlot instance, Color c)
        {
            if (instance.BookId == null || c == UIColorManager.Manager.GetUIColor(UIColor.Highlighted) ||
                c == UIColorManager.Manager.GetUIColor(UIColor.Disabled)) return;
            var dropBookOption = ModParameters.DropBookOptions.FirstOrDefault(x =>
                x.PackageId == instance.BookId.packageId && x.Ids.Contains(instance.BookId.id));
            if (dropBookOption?.DropBookColorOptions == null) return;
            var frameColor = dropBookOption.DropBookColorOptions.FrameColor.ConvertColor();
            if (frameColor != null) instance.bookNumBg.color = frameColor.Value;
            var textColor = dropBookOption.DropBookColorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.txt_bookNum.color = textColor.Value;
        }

        public static void EmotionPassiveCardUISetSpritesPre(UIEmotionPassiveCardInven __instance)
        {
            __instance.img_LeftTotalFrame.gameObject.SafeDestroyComponent<_2dxFX_ColorChange>();
            __instance._leftFrameTitleLineardodge.gameObject.SetActive(true);
            __instance._rightFrame.gameObject.SafeDestroyComponent<_2dxFX_ColorChange>();
        }

        public static void EmotionPassiveCardUISetSpritesPre(EmotionPassiveCardUI __instance)
        {
            __instance.img_LeftTotalFrame.gameObject.SafeDestroyComponent<_2dxFX_ColorChange>();
            __instance._leftFrameTitleLineardodge.gameObject.SetActive(true);
            __instance._rightFrame.gameObject.SafeDestroyComponent<_2dxFX_ColorChange>();
        }

        public static void EmotionPassiveCardUISetSpritesPost(EmotionPassiveCardUI instance,
            EmotionCardXmlExtension cardExtension)
        {
            instance._artwork.sprite =
                Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(cardExtension.LorId.packageId,
                    cardExtension.Artwork);
            var emotionCardOption = ModParameters.EmotionCardOptions.FirstOrDefault(x =>
                x.PackageId == cardExtension.LorId.packageId && x.Id.Contains(cardExtension.LorId.id));
            if (emotionCardOption == null) return;
            instance.img_LeftTotalFrame.sprite = UISpriteDataManager.instance.AbnormalityFrame.ElementAtOrDefault(0);
            var orAddComponent = instance.img_LeftTotalFrame.gameObject.GetOrAddComponent<_2dxFX_ColorChange>();
            ChangeEmotionCardColor(emotionCardOption, ref orAddComponent);
            instance._rightBg.sprite = instance._positiveBgSprite.ElementAtOrDefault(1);
            var orAddComponent2 = instance._rightBg.gameObject.GetOrAddComponent<_2dxFX_ColorChange>();
            ChangeEmotionCardColor(emotionCardOption, ref orAddComponent2);
            instance._rightFrame.sprite = instance._positiveFrameSprite.ElementAtOrDefault(1);
            var orAddComponent3 = instance._rightFrame.gameObject.GetOrAddComponent<_2dxFX_ColorChange>();
            ChangeEmotionCardColor(emotionCardOption, ref orAddComponent3);
            instance._leftFrameTitleLineardodge.gameObject.SetActive(false);
            var textColor = emotionCardOption.ColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                instance._flavorText.fontMaterial.SetColor("_UnderlayColor", textColor.Value);
                instance._abilityDesc.fontMaterial.SetColor("_UnderlayColor", textColor.Value);
            }

            var frameColor = emotionCardOption.ColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            instance._hOverImg.color = frameColor.Value;
            var rootColor = frameColor.Value;
            rootColor.a = 0.25f;
            instance._rootImageBg.color = rootColor;
            var component = instance.txt_Level.GetComponent<TextMeshProMaterialSetter>();
            if (component == null) return;
            component.glowColor = frameColor.Value;
            component.underlayColor = frameColor.Value;
            component.enabled = false;
            component.enabled = true;
        }

        public static void EmotionPassiveCardUISetSpritesPost(UIEmotionPassiveCardInven instance,
            EmotionCardXmlExtension cardExtension)
        {
            instance._artwork.sprite =
                Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(cardExtension.LorId.packageId,
                    cardExtension.Artwork);
            var emotionCardOption = ModParameters.EmotionCardOptions.FirstOrDefault(x =>
                x.PackageId == cardExtension.LorId.packageId && x.Id.Contains(cardExtension.LorId.id));
            if (emotionCardOption == null) return;
            instance.img_LeftTotalFrame.sprite = UISpriteDataManager.instance.AbnormalityFrame.ElementAtOrDefault(0);
            var orAddComponent = instance.img_LeftTotalFrame.gameObject.GetOrAddComponent<_2dxFX_ColorChange>();
            ChangeEmotionCardColor(emotionCardOption, ref orAddComponent);
            instance._rightBg.sprite = instance._positiveBgSprite.ElementAtOrDefault(1);
            var orAddComponent2 = instance._rightBg.gameObject.GetOrAddComponent<_2dxFX_ColorChange>();
            ChangeEmotionCardColor(emotionCardOption, ref orAddComponent2);
            instance._rightFrame.sprite = instance._positiveFrameSprite.ElementAtOrDefault(1);
            var orAddComponent3 = instance._rightFrame.gameObject.GetOrAddComponent<_2dxFX_ColorChange>();
            ChangeEmotionCardColor(emotionCardOption, ref orAddComponent3);
            instance._leftFrameTitleLineardodge.gameObject.SetActive(false);
            var textColor = emotionCardOption.ColorOptions.TextColor.ConvertColor();
            if (textColor != null)
            {
                instance._flavorText.fontMaterial.SetColor("_UnderlayColor", textColor.Value);
                instance._abilityDesc.fontMaterial.SetColor("_UnderlayColor", textColor.Value);
            }

            var frameColor = emotionCardOption.ColorOptions.FrameColor.ConvertColor();
            if (frameColor == null) return;
            instance._hOverImg.color = frameColor.Value;
            var rootColor = frameColor.Value;
            rootColor.a = 0.25f;
            instance._rootImageBg.color = rootColor;
            var component = instance.txt_Level.GetComponent<TextMeshProMaterialSetter>();
            if (component == null) return;
            component.glowColor = frameColor.Value;
            component.underlayColor = frameColor.Value;
            component.enabled = false;
            component.enabled = true;
        }

        public static void ChangeEmotionCardColor(EmotionCardOptionRoot cardOptions, ref _2dxFX_ColorChange component)
        {
            component._Color = cardOptions.ColorOptions.FrameColor.ConvertColor() ?? component._Color;
            component._HueShift = cardOptions.ColorOptions.FrameHSVColor?.H ?? component._HueShift;
            component._Saturation = cardOptions.ColorOptions.FrameHSVColor?.S ?? component._Saturation;
            component._ValueBrightness = cardOptions.ColorOptions.FrameHSVColor?.V ?? component._Saturation;
        }

        public static void ChangeSpeedDiceColor(SpeedDiceUI instance, CustomDiceColorOptionRoot colorOptions,
            string packageId)
        {
            if (instance == null) return;
            if (!string.IsNullOrEmpty(colorOptions.IconId))
            {
                var normalFrame =
                    ModParameters.ArtWorks.FirstOrDefault(
                        x => x.PackageId == packageId && x.Name == colorOptions.IconId);
                var glowFrame = ModParameters.ArtWorks.FirstOrDefault(x =>
                    x.PackageId == packageId && x.Name == colorOptions.IconId + "_Glow");
                var hoveredFrame = ModParameters.ArtWorks.FirstOrDefault(x =>
                    x.PackageId == packageId && x.Name == colorOptions.IconId + "_Hovered");
                instance.img_normalFrame.sprite =
                    normalFrame != null
                        ? normalFrame.Sprite
                        : instance.img_normalFrame.sprite;
                instance.img_lightFrame.sprite =
                    glowFrame != null
                        ? glowFrame.Sprite
                        : instance.img_lightFrame.sprite;
                instance.img_highlightFrame.sprite =
                    hoveredFrame != null
                        ? hoveredFrame.Sprite
                        : instance.img_highlightFrame.sprite;
            }

            var textColor = colorOptions.TextColor.ConvertColor();
            if (textColor == null) return;
            instance._txtSpeedRange.color = textColor.Value;
            instance._rouletteImg.color = textColor.Value;
            instance._txtSpeedMax.color = textColor.Value;
            instance.img_tensNum.color = textColor.Value;
            instance.img_unitsNum.color = textColor.Value;
            var rootColor = textColor.Value;
            rootColor.a -= 0.6f;
            instance.img_breakedFrame.color = rootColor;
            instance.img_breakedLinearDodge.color = rootColor;
            instance.img_lockedFrame.color = rootColor;
            instance.img_lockedIcon.color = rootColor;
        }

        public static void SetBooksData(UISettingInvenEquipPageListSlot instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            var categoryOption =
                ModParameters.CategoryOptions.FirstOrDefault(
                    x => storyKey.workshopId == x.PackageId + "_" + x.AdditionalValue);
            if (categoryOption == null) return;
            if (books.Count < 0) return;
            if (categoryOption.BookDataColor == null) return;
            var frameColor = categoryOption.BookDataColor.FrameColor.ConvertColor();
            if (frameColor != null)
                SetBooksDataFrameColor(frameColor.Value, instance.img_EdgeFrame,
                    instance.img_LineFrame,
                    instance.img_IconGlow, instance.img_Icon);
            var textColor = categoryOption.BookDataColor.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.txt_StoryName.color = textColor.Value;
            var component = instance.txt_StoryName.gameObject.GetComponent<TextMeshProMaterialSetter>();
            component.underlayColor = textColor.Value;
            component.enabled = false;
            component.enabled = true;
        }

        public static void SetBooksData(UIInvenEquipPageListSlot instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            var categoryOption =
                ModParameters.CategoryOptions.FirstOrDefault(
                    x => storyKey.workshopId == x.PackageId + "_" + x.AdditionalValue);
            if (categoryOption == null) return;
            if (books.Count < 0) return;
            if (categoryOption.BookDataColor == null) return;
            var frameColor = categoryOption.BookDataColor.FrameColor.ConvertColor();
            if (frameColor != null)
                SetBooksDataFrameColor(frameColor.Value, instance.img_EdgeFrame,
                    instance.img_LineFrame,
                    instance.img_IconGlow, instance.img_Icon);
            var textColor = categoryOption.BookDataColor.TextColor.ConvertColor();
            if (textColor == null) return;
            instance.txt_StoryName.color = textColor.Value;
            var component = instance.txt_StoryName.gameObject.GetComponent<TextMeshProMaterialSetter>();
            component.underlayColor = textColor.Value;
            component.enabled = false;
            component.enabled = true;
        }

        public static void SetBooksDataFrameColor(Color c, Image img_EdgeFrame, Image img_LineFrame, Image img_IconGlow,
            Image img_Icon)
        {
            img_EdgeFrame.color = c;
            img_LineFrame.color = c;
            img_IconGlow.color = c;
            img_Icon.color = c;
        }

        public static void ResetColorData(UIInvenEquipPageListSlot instance)
        {
            var defaultColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            instance.img_Icon.color = defaultColor;
            if (instance.txt_StoryName == null) return;
            instance.txt_StoryName.color = defaultColor;
            var component = instance.txt_StoryName.gameObject.GetComponent<TextMeshProMaterialSetter>();
            component.underlayColor = defaultColor;
            component.enabled = false;
            component.enabled = true;
        }

        public static void ResetColorData(UISettingInvenEquipPageListSlot instance)
        {
            var defaultColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            instance.img_Icon.color = defaultColor;
            if (instance.txt_StoryName == null) return;
            instance.txt_StoryName.color = defaultColor;
            var component = instance.txt_StoryName.gameObject.GetComponent<TextMeshProMaterialSetter>();
            component.underlayColor = defaultColor;
            component.enabled = false;
            component.enabled = true;
        }
    }
}