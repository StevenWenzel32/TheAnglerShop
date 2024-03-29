using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent.UI;

namespace NoFishingQuests;

public class ModifyShops : GlobalNPC
{
	private static readonly NPCShop shop = new(NPCID.Angler);
	private static readonly NPCShop decorShop = new(NPCID.Angler, Language.GetTextValue("GameUI.PainterDecor"));
	
	public override void Load()
	{
		AnglerCoin.id = CustomCurrencyManager.RegisterCurrency(new AnglerCoin(ModContent.ItemType<AnglerCoinItem>(), 999));
		
		shop.Add(GetItem(ItemID.SonarPotion, 2));
		shop.Add(GetItem(ItemID.FishingPotion, 2));
		shop.Add(GetItem(ItemID.CratePotion, 2));
		shop.Add(GetItem(ItemID.ApprenticeBait, 3));
		shop.Add(GetItem(ItemID.JourneymanBait, 10));
		shop.Add(GetItem(ItemID.MasterBait, 15));
		shop.Add(GetItem(ItemID.FuzzyCarrot, 10));
		shop.Add(GetItem(ItemID.FishMinecart, 10));
		shop.Add(GetItem(ItemID.AnglerHat, 8));
		shop.Add(GetItem(ItemID.AnglerVest, 8));
		shop.Add(GetItem(ItemID.AnglerPants, 8));
		shop.Add(GetItem(ItemID.FishHook, 30));
		shop.Add(GetItem(ItemID.GoldenFishingRod, 50));
		shop.Add(GetItem(ItemID.GoldenBugNet, 50));
		shop.Add(GetItem(ItemID.HighTestFishingLine, 8));
		shop.Add(GetItem(ItemID.AnglerEarring, 8));
		shop.Add(GetItem(ItemID.TackleBox, 8));
		shop.Add(GetItem(ItemID.FishermansGuide, 15));
		shop.Add(GetItem(ItemID.WeatherRadio, 15));
		shop.Add(GetItem(ItemID.Sextant, 15));
		shop.Add(GetItem(ItemID.FishingBobber, 15));
		shop.Add(GetItem(ItemID.BottomlessHoneyBucket, 40), Condition.DownedQueenBee);
		shop.Add(GetItem(ItemID.HoneyAbsorbantSponge, 40), Condition.DownedQueenBee);
		shop.Add(GetItem(ItemID.BottomlessBucket, 40));
		shop.Add(GetItem(ItemID.SuperAbsorbantSponge, 40));
		shop.Add(GetItem(ItemID.FinWings, 40), Condition.Hardmode);
		shop.Add(GetItem(ItemID.HotlineFishingHook, 40), Condition.Hardmode);
		shop.Add(GetItem(ItemID.ChumBucket, 5), Condition.BloodMoon);
		shop.Add(GetItem(ItemID.TeleportationPylonOcean, 12), Condition.HappyEnoughToSellPylons);

		decorShop.Add(GetItem(ItemID.SeashellHairpin, 4));
		decorShop.Add(GetItem(ItemID.MermaidAdornment, 4));
		decorShop.Add(GetItem(ItemID.MermaidTail, 4));
		decorShop.Add(GetItem(ItemID.FishCostumeMask, 4));
		decorShop.Add(GetItem(ItemID.FishCostumeShirt, 4));
		decorShop.Add(GetItem(ItemID.FishCostumeFinskirt, 4));
		decorShop.Add(GetItem(ItemID.BunnyfishTrophy, 1));
		decorShop.Add(GetItem(ItemID.GoldfishTrophy, 1));
		decorShop.Add(GetItem(ItemID.SharkteethTrophy, 1));
		decorShop.Add(GetItem(ItemID.SwordfishTrophy, 1));
		decorShop.Add(GetItem(ItemID.TreasureMap, 1));
		decorShop.Add(GetItem(ItemID.SeaweedPlanter, 1));
		decorShop.Add(GetItem(ItemID.PillaginMePixels, 1));
		decorShop.Add(GetItem(ItemID.CompassRose, 1));
		decorShop.Add(GetItem(ItemID.ShipsWheel, 1));
		decorShop.Add(GetItem(ItemID.ShipInABottle, 1));
		decorShop.Add(GetItem(ItemID.LifePreserver, 1));
		decorShop.Add(GetItem(ItemID.WallAnchor, 1));
		decorShop.Add(GetItem(ItemID.NotSoLostInParadise, 1));
		decorShop.Add(GetItem(ItemID.Crustography, 1));
		decorShop.Add(GetItem(ItemID.WhatLurksBelow, 1));
		decorShop.Add(GetItem(ItemID.Fangs, 1));
		decorShop.Add(GetItem(ItemID.CouchGag, 1));
		decorShop.Add(GetItem(ItemID.SilentFish, 1));
		decorShop.Add(GetItem(ItemID.TheDuke, 1));
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

	private static Item GetItem(short id, int price)
	{
		var item = new Item(id) {
			isAShopItem = true,
			shopCustomPrice = price
		};

		if (ModContent.GetInstance<Config>().useCustomCurrency) {
			item.shopSpecialCurrency = AnglerCoin.id;
		}
		else {
			item.shopCustomPrice *= (int)(4000 * ModContent.GetInstance<Config>().goldMultiplier);
		}

		return item;
	}
}