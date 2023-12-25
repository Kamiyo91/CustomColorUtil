using System.Linq;
using CustomColorUtil.Util;
using HarmonyLib;
using LOR_BattleUnit_UI;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class SpeedDieColorPatchWithPattyMod
    {
        [HarmonyPostfix]
        [HarmonyAfter("Patty_SpeedDiceColorChange_MOD")]
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
                var sephirahUnit = BattleObjectManager.instance.GetList(Faction.Player)
                    .FirstOrDefault(x => x.UnitData.unitData.isSephirah);
                if (sephirahUnit == null) return;
                var sephirahKeypageOption = ModParameters.KeypageOptions.FirstOrDefault(x =>
                    x.PackageId == sephirahUnit.Book.BookId.packageId && x.Ids.Contains(sephirahUnit.Book.BookId.id));
                if (sephirahKeypageOption?.CustomDiceColorOptions != null &&
                    sephirahKeypageOption.CustomDiceColorOptions.ChangeAllTeamDice)
                    ArtUtil.ChangeSpeedDiceColor(__instance, sephirahKeypageOption.CustomDiceColorOptions,
                        sephirahUnit.Book.BookId.packageId);
                return;
            }

            var stageId = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
            var stageItem =
                ModParameters.StageOptions.FirstOrDefault(x =>
                    x.PackageId == stageId.packageId && x.Ids.Contains(stageId.id));
            if (stageItem?.CustomDiceColorOptions != null)
                ArtUtil.ChangeSpeedDiceColor(__instance, stageItem.CustomDiceColorOptions, stageId.packageId);
        }
    }

    [HarmonyPatch]
    public class SpeedDieColorPatchWithPattyModAndUtil
    {
        [HarmonyPostfix]
        [HarmonyAfter("Patty_SpeedDiceColorChange_MOD")]
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
                    if (floorKeypageOption?.CustomDiceColorOptions != null)
                    {
                        ArtUtil.ChangeSpeedDiceColor(__instance, floorKeypageOption.CustomDiceColorOptions,
                            floorKeypageOption.PackageId);
                        return;
                    }

                    if (!savedOptions.PassiveId.HasValue) return;
                    var passiveKeypageOption = ModParameters.PassiveOptions.FirstOrDefault(x =>
                        x.PackageId == savedOptions.FloorOptions?.PackageId &&
                        x.Ids.Contains(savedOptions.PassiveId.Value));
                    if (passiveKeypageOption?.CustomDiceColorOptions != null)
                        ArtUtil.ChangeSpeedDiceColor(__instance, passiveKeypageOption.CustomDiceColorOptions,
                            passiveKeypageOption.PackageId);
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
    }
}