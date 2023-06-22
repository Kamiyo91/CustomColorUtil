using System.Linq;
using CustomColorUtil.Util;
using HarmonyLib;
using UI;
using UtilLoader21341.Extensions;
using Workshop;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class SkinProjectionColorPatch
    {
        [HarmonyPatch(typeof(UIEquipPageCustomizeSlot), "SetData", typeof(WorkshopSkinData))]
        [HarmonyPostfix]
        public static void UIEquipPageCustomizeSlot_SetData(object __instance, object data)
        {
            if (!(data is WorkshopSkinDataExtension workshopData) ||
                !(__instance is UIOriginEquipPageSlot instance)) return;
            if (!workshopData.RealKeypageId.HasValue) return;
            instance._bookDataModel =
                new BookModel(Singleton<BookXmlList>.Instance.GetData(new LorId(workshopData.PackageId,
                    workshopData.RealKeypageId.Value)));
            var sprite = UISpriteDataManager.instance.GetStoryIcon(instance._bookDataModel.ClassInfo.BookIcon);
            instance.Icon.sprite = sprite.icon;
            instance.IconGlow.sprite = sprite.iconGlow;
            var keypageOption = ModParameters.KeypageOptions.FirstOrDefault(x =>
                x.PackageId == workshopData.PackageId && x.Ids.Contains(workshopData.RealKeypageId.Value));
            var frameColor = keypageOption?.KeypageColorOptions?.FrameColor.ConvertColor();
            if (frameColor == null) return;
            instance.SetGlowColor(frameColor.Value);
        }
    }
}