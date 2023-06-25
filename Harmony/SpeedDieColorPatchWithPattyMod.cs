using System.Linq;
using CustomColorUtil.Util;
using HarmonyLib;
using LOR_BattleUnit_UI;

namespace CustomColorUtil.Harmony
{
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
            if (keypageOption?.CustomDiceColorOptions != null)
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
}