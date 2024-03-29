using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NoFishingQuests;

public class ModifyShops : GlobalNPC
{
	static readonly (short id, int price)[] ShopItems = {
		(ItemID.SonarPotion, 2),
		(ItemID.FishingPotion, 2),
		(ItemID.CratePotion, 2),
		(ItemID.ApprenticeBait, 3),
		(ItemID.JourneymanBait, 10),
		(ItemID.MasterBait, 15),
		(ItemID.FuzzyCarrot, 10),
		(ItemID.FishMinecart, 10),
		(ItemID.AnglerHat, 8),
		(ItemID.AnglerVest, 8),
		(ItemID.AnglerPants, 8),
		(ItemID.FishHook, 30),
		(ItemID.GoldenFishingRod, 50),
		(ItemID.GoldenBugNet, 50),
		(ItemID.HighTestFishingLine, 8),
		(ItemID.AnglerEarring, 8),
		(ItemID.TackleBox, 8),
		(ItemID.FishermansGuide, 15),
		(ItemID.WeatherRadio, 15),
		(ItemID.Sextant, 15),
		(ItemID.FishingBobber, 15),
		(ItemID.BottomlessHoneyBucket, 40),
		(ItemID.HoneyAbsorbantSponge, 40),
		(ItemID.BottomlessBucket, 40),
		(ItemID.SuperAbsorbantSponge, 40),
		(ItemID.FinWings, 40),
		(ItemID.HotlineFishingHook, 40)
	};
	
	static readonly (short id, int price)[] DecorationItems = {
		(ItemID.SeashellHairpin, 4),
		(ItemID.MermaidAdornment, 4),
		(ItemID.MermaidTail, 4),
		(ItemID.FishCostumeMask, 4),
		(ItemID.FishCostumeShirt, 4),
		(ItemID.FishCostumeFinskirt, 4),
		(ItemID.BunnyfishTrophy, 1),
		(ItemID.GoldfishTrophy, 1),
		(ItemID.SharkteethTrophy, 1),
		(ItemID.SwordfishTrophy, 1),
		(ItemID.TreasureMap, 1),
		(ItemID.SeaweedPlanter, 1),
		(ItemID.PillaginMePixels, 1),
		(ItemID.CompassRose, 1),
		(ItemID.ShipsWheel, 1),
		(ItemID.ShipInABottle, 1),
		(ItemID.LifePreserver, 1),
		(ItemID.WallAnchor, 1),
		(ItemID.NotSoLostInParadise, 1),
		(ItemID.Crustography, 1),
		(ItemID.WhatLurksBelow, 1),
		(ItemID.Fangs, 1),
		(ItemID.CouchGag, 1),
		(ItemID.SilentFish, 1),
		(ItemID.TheDuke, 1),
	};
	
	private static readonly short[] HardModeItems = { ItemID.FinWings, ItemID.HotlineFishingHook };
	
	private static readonly NPCShop shop = new(NPCID.Angler);
	private static readonly NPCShop decorShop = new(NPCID.Angler, Language.GetTextValue("GameUI.PainterDecor"));

	public override void Load()
	{
		AddAnglerShopItems(shop, ShopItems);
		AddAnglerShopItems(decorShop, DecorationItems);
	}

	public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
	{
		if (npc.type != NPCID.Angler) {
			return;
		}
		
		if (shopName == NoFishingQuests.ShopName) {
			shop.FillShop(items, npc, out _);
		}
		else if (shopName == NoFishingQuests.DecorationShopName) {
			decorShop.FillShop(items, npc, out _);
		}
	}
	
	private static void AddAnglerShopItems(NPCShop npcShop, (short id, int price)[] items)
	{
		// add items to the list
		for (int i = 0; i < items.Length; i++) {
			var item = new Item(items[i].id) {
				isAShopItem = true,
				shopCustomPrice = items[i].price
			};

			if (ModContent.GetInstance<Config>().useCustomCurrency) {
				item.shopSpecialCurrency = AnglerCoin.Id;
			}
			else {
				item.shopCustomPrice *= 6700;
			}

			if (HardModeItems.Contains(items[i].id)) {
				npcShop.Add(item, Condition.Hardmode);
			}
			else {
				npcShop.Add(item);
			}
		}

		npcShop.Add(new Item(ItemID.TeleportationPylonOcean), Condition.HappyEnoughToSellPylons);
	}
}