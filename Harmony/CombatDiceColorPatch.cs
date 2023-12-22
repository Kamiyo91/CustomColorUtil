using System.Collections.Generic;
using System.Linq;
using CustomColorUtil.Util;
using HarmonyLib;
using LOR_DiceSystem;
using UI;

namespace CustomColorUtil.Harmony
{
    [HarmonyPatch]
    public class CombatDiceColorPatch
    {
        [HarmonyPatch(typeof(BattleDiceCard_BehaviourDescUI), nameof(BattleDiceCard_BehaviourDescUI.SetBehaviourInfo))]
        [HarmonyPatch(typeof(UIDetailCardDescSlot), nameof(UIDetailCardDescSlot.SetBehaviourInfo))]
        [HarmonyPostfix]
        public static void UIDetailCardDescSlot_SetBehaviourInfo_Post(object __instance, DiceBehaviour behaviour,
            LorId cardId, List<DiceBehaviour> behaviourList)
        {
            var cardOption =
                ModParameters.CardOptions.FirstOrDefault(x =>
                    x.PackageId == cardId.packageId && x.Ids.Contains(cardId.id));
            if (cardOption?.CardColorOptions == null || !cardOption.CardColorOptions.CustomDiceIcon.Any()) return;
            var diceIndex = behaviourList.IndexOf(behaviour);
            var customDice = cardOption.CardColorOptions.CustomDiceIcon.FirstOrDefault(x => x.DiceNumber == diceIndex);
            var color = customDice?.TextColor.ConvertColor();
            if (!color.HasValue) return;
            switch (__instance)
            {
                case UIDetailCardDescSlot instance:
                    instance.SetBehaviourInfoCustom(color.Value, customDice.PackageId, customDice.KeywordIconId);
                    break;
                case BattleDiceCard_BehaviourDescUI instance:
                    instance.SetBehaviourInfoCustom(color.Value, customDice.PackageId, customDice.KeywordIconId);
                    break;
            }
        }

        [HarmonyPatch(typeof(BattleUnitKeepedDiceUI), nameof(BattleUnitKeepedDiceUI.Init))]
        [HarmonyPostfix]
        public static void BattleUnitKeepedDiceUI_Init_Post(BattleUnitKeepedDiceUI __instance)
        {
            if (__instance._view.model.IsDead()) return;
            var card = __instance._view.model.cardSlotDetail?.keepCard;
            if (card?.card == null) return;
            var cardOption = ModParameters.CardOptions.FirstOrDefault(x =>
                x.PackageId == card.card.GetID()?.packageId && x.Ids.Contains(card.card.GetID().id));
            if (cardOption?.CardColorOptions == null || !cardOption.CardColorOptions.CustomDiceIcon.Any()) return;
            if (__instance._canvas == null || !__instance._canvas.enabled) return;
            var diceBehaviorList = __instance._view.model.cardSlotDetail?.keepCard.GetDiceBehaviorList();
            var num = 0;
            while (num < diceBehaviorList?.Count && num < __instance.dices.Count)
            {
                var dice = cardOption.CardColorOptions.CustomDiceIcon.FirstOrDefault(x => x.DiceNumber == num);
                if (dice == null)
                {
                    num++;
                    continue;
                }

                var diceColor = dice.DiceColor.ConvertColor();
                if (diceColor.HasValue) __instance.dices[num].SetColorsCustom(diceColor.Value, diceColor.Value);
                __instance.dices[num].SetCenterIconSpriteCustom(dice.PackageId, dice.KeywordIconIdClash);
                var textColor = dice.TextColor.ConvertColor();
                if (textColor.HasValue) __instance.dices[num].SetRangeTextCustom(textColor.Value);
                num++;
            }
        }

        [HarmonyPatch(typeof(BattleSimpleActionUI_Dice), nameof(BattleSimpleActionUI_Dice.PrepareDice),
            typeof(BattleDiceBehaviourUI))]
        [HarmonyPatch(typeof(BattleSimpleActionUI_Dice), nameof(BattleSimpleActionUI_Dice.PrepareDice),
            typeof(BattleDiceBehavior))]
        [HarmonyPostfix]
        public static void BattleSimpleActionUI_Dice_PrepareDice_Post(BattleSimpleActionUI_Dice __instance,
            object b)
        {
            switch (b)
            {
                case BattleDiceBehavior dice:
                    __instance.PrepareDiceCustom(dice);
                    break;
                case BattleDiceBehaviourUI dice:
                    __instance.PrepareDiceCustom(dice);
                    break;
            }
        }

        [HarmonyPatch(typeof(BattleSimpleActionUI_Dice), nameof(BattleSimpleActionUI_Dice.PrepareDice),
            typeof(List<BattleCardBehaviourResult>))]
        [HarmonyPostfix]
        public static void BattleSimpleActionUI_Dice_PrepareDice_Post2(BattleSimpleActionUI_Dice __instance,
            List<BattleCardBehaviourResult> battleDiceBehaviorResults)
        {
            __instance.PrepareDiceCustom(battleDiceBehaviorResults);
        }
    }
}