using System.Linq;
using CustomColorUtil.Util;
using HarmonyLib;
using LOR_BattleUnit_UI;
using UnityEngine;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class SpeedDieColorPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpeedDiceUI), "Init")]
        public static void Init(SpeedDiceUI __instance, Faction faction)
        {
            var keypageOption =
                ModParameters.KeypageOptions.FirstOrDefault(x =>
                    x.PackageId == __instance._view.model.Book.BookId.packageId &&
                    x.Ids.Contains(__instance._view.model.Book.BookId.id));
            if (keypageOption?.CustomDiceColorOptions != null &&
                !keypageOption.CustomDiceColorOptions.ChangeAllTeamDice)
            {
                ArtUtil.ChangeSpeedDiceColor(__instance, keypageOption.CustomDiceColorOptions,
                    __instance._view.model.Book.BookId.packageId);
                return;
            }

            if (faction == Faction.Player)
            {
                var playerUnits = BattleObjectManager.instance.GetList(Faction.Player);
                var changeAllTeamUnit = ModParameters.KeypageOptions.FirstOrDefault(x =>
                    playerUnits.Any(y =>
                        x.PackageId == y.Book.BookId.packageId && x.Ids.Contains(y.Book.BookId.id) &&
                        x.CustomDiceColorOptions != null && x.CustomDiceColorOptions.ChangeAllTeamDice));
                if (changeAllTeamUnit?.CustomDiceColorOptions != null &&
                    changeAllTeamUnit.CustomDiceColorOptions.ChangeAllTeamDice)
                    ArtUtil.ChangeSpeedDiceColor(__instance, changeAllTeamUnit.CustomDiceColorOptions,
                        changeAllTeamUnit.PackageId);
                return;
            }

            var stageId = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
            var stageItem =
                ModParameters.StageOptions.FirstOrDefault(x =>
                    x.PackageId == stageId.packageId && x.Ids.Contains(stageId.id));
            if (stageItem?.CustomDiceColorOptions != null)
                ArtUtil.ChangeSpeedDiceColor(__instance, stageItem.CustomDiceColorOptions, stageId.packageId);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpeedDiceUI), "ChangeSprite")]
        public static void ChangeSprite(SpeedDiceUI __instance, int value)
        {
            if (value < 999) return;
            var infinite =
                ModParameters.ArtWorks.FirstOrDefault(x =>
                    x.PackageId == ModParameters.PackageId && x.Name == "Infinite");
            if (infinite == null) return;
            __instance._txtSpeedMax.gameObject.SetActive(false);
            __instance.img_tensNum.sprite = infinite.Sprite;
            __instance.img_tensNum.gameObject.transform.localPosition += new Vector3(13f, 0f, 0f);
            __instance.img_tensNum.color = __instance.img_unitsNum.color;
            __instance.img_tensNum.gameObject.transform.localScale = new Vector3(0.95f, 0.85f, 0f);
            __instance.img_tensNum.gameObject.SetActive(true);
        }
    }

    [HarmonyPatch]
    public class SpeedDieColorPatchWithUtil
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpeedDiceUI), "Init")]
        public static void Init(SpeedDiceUI __instance, Faction faction)
        {
            var keypageOption =
                ModParameters.KeypageOptions.FirstOrDefault(x =>
                    x.PackageId == __instance._view.model.Book.BookId.packageId &&
                    x.Ids.Contains(__instance._view.model.Book.BookId.id));
            if (keypageOption?.CustomDiceColorOptions != null &&
                !keypageOption.CustomDiceColorOptions.ChangeAllTeamDice)
            {
                ArtUtil.ChangeSpeedDiceColor(__instance, keypageOption.CustomDiceColorOptions,
                    __instance._view.model.Book.BookId.packageId);
                return;
            }

            if (faction == Faction.Player)
            {
                var playerUnits = BattleObjectManager.instance.GetList(Faction.Player);
                var changeAllTeamUnit = ModParameters.KeypageOptions.FirstOrDefault(x =>
                    playerUnits.Any(y =>
                        x.PackageId == y.Book.BookId.packageId && x.Ids.Contains(y.Book.BookId.id) &&
                        x.CustomDiceColorOptions != null && x.CustomDiceColorOptions.ChangeAllTeamDice));
                if (changeAllTeamUnit?.CustomDiceColorOptions != null &&
                    changeAllTeamUnit.CustomDiceColorOptions.ChangeAllTeamDice)
                {
                    ArtUtil.ChangeSpeedDiceColor(__instance, changeAllTeamUnit.CustomDiceColorOptions,
                        changeAllTeamUnit.PackageId);
                }
                else if (UtilLoader21341.ModParameters.EgoAndEmotionCardChanged.TryGetValue(
                             Singleton<StageController>.Instance.CurrentFloor, out var savedOptions))
                {
                    if (!savedOptions.IsActive || savedOptions.KeypageId == null) return;
                    var floorKeypageOption = ModParameters.KeypageOptions.FirstOrDefault(x =>
                        x.PackageId == savedOptions.FloorOptions?.PackageId &&
                        x.Ids.Contains(savedOptions.KeypageId.Value));
                    if (floorKeypageOption?.CustomDiceColorOptions == null) return;
                    ArtUtil.ChangeSpeedDiceColor(__instance, floorKeypageOption.CustomDiceColorOptions,
                        floorKeypageOption.PackageId);
                }

                return;
            }

            var stageId = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
            var stageItem =
                ModParameters.StageOptions.FirstOrDefault(x =>
                    x.PackageId == stageId.packageId && x.Ids.Contains(stageId.id));
            if (stageItem?.CustomDiceColorOptions != null)
                ArtUtil.ChangeSpeedDiceColor(__instance, stageItem.CustomDiceColorOptions, stageId.packageId);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpeedDiceUI), "ChangeSprite")]
        public static void ChangeSprite(SpeedDiceUI __instance, int value)
        {
            if (value < 999) return;
            var infinite =
                ModParameters.ArtWorks.FirstOrDefault(x =>
                    x.PackageId == ModParameters.PackageId && x.Name == "Infinite");
            if (infinite == null) return;
            __instance._txtSpeedMax.gameObject.SetActive(false);
            __instance.img_tensNum.sprite = infinite.Sprite;
            __instance.img_tensNum.gameObject.transform.localPosition += new Vector3(13f, 0f, 0f);
            __instance.img_tensNum.color = __instance.img_unitsNum.color;
            __instance.img_tensNum.gameObject.transform.localScale = new Vector3(0.95f, 0.85f, 0f);
            __instance.img_tensNum.gameObject.SetActive(true);
        }
    }
}