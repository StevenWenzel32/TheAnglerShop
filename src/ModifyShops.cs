using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI;

namespace NoFishingQuests;

public class ModifyShops : GlobalNPC
{
	private static readonly NPCShop anglerShop = new(NPCID.Angler);
	private static readonly NPCShop anglerDecorShop = new(NPCID.Angler, "Decor");
	
	public override void Load()
	{
		AnglerCoin.id = CustomCurrencyManager.RegisterCurrency(new AnglerCoin(ModContent.ItemType<AnglerCoinItem>(), 999));
		
		AddItemToShop(anglerShop, ItemID.SonarPotion, 2);
		AddItemToShop(anglerShop, ItemID.FishingPotion, 2);
		AddItemToShop(anglerShop, ItemID.CratePotion, 2);
		AddItemToShop(anglerShop, ItemID.ApprenticeBait, 3);
		AddItemToShop(anglerShop, ItemID.JourneymanBait, 10);
		AddItemToShop(anglerShop, ItemID.MasterBait, 15);
		AddItemToShop(anglerShop, ItemID.FuzzyCarrot, 10);
		AddItemToShop(anglerShop, ItemID.FishMinecart, 10);
		AddItemToShop(anglerShop, ItemID.AnglerHat, 8);
		AddItemToShop(anglerShop, ItemID.AnglerVest, 8);
		AddItemToShop(anglerShop, ItemID.AnglerPants, 8);
		AddItemToShop(anglerShop, ItemID.FishHook, 30);
		AddItemToShop(anglerShop, ItemID.GoldenFishingRod, 50);
		AddItemToShop(anglerShop, ItemID.GoldenBugNet, 50);
		AddItemToShop(anglerShop, ItemID.HighTestFishingLine, 8);
		AddItemToShop(anglerShop, ItemID.AnglerEarring, 8);
		AddItemToShop(anglerShop, ItemID.TackleBox, 8);
		AddItemToShop(anglerShop, ItemID.FishermansGuide, 15);
		AddItemToShop(anglerShop, ItemID.WeatherRadio, 15);
		AddItemToShop(anglerShop, ItemID.Sextant, 15);
		AddItemToShop(anglerShop, ItemID.FishingBobber, 15);
		AddItemToShop(anglerShop, ItemID.BottomlessHoneyBucket, 40, Condition.DownedQueenBee);
		AddItemToShop(anglerShop, ItemID.HoneyAbsorbantSponge, 40, Condition.DownedQueenBee);
		AddItemToShop(anglerShop, ItemID.BottomlessBucket, 40);
		AddItemToShop(anglerShop, ItemID.SuperAbsorbantSponge, 40);
		AddItemToShop(anglerShop, ItemID.FinWings, 40, Condition.Hardmode);
		AddItemToShop(anglerShop, ItemID.HotlineFishingHook, 40, Condition.Hardmode);
		AddItemToShop(anglerShop, ItemID.ChumBucket, 5, Condition.BloodMoon);
		AddItemToShop(anglerShop, ItemID.TeleportationPylonOcean, 12, Condition.HappyEnoughToSellPylons);
		//anglerShop.Register();

		AddItemToShop(anglerDecorShop, ItemID.SeashellHairpin, 4);
		AddItemToShop(anglerDecorShop, ItemID.MermaidAdornment, 4);
		AddItemToShop(anglerDecorShop, ItemID.MermaidTail, 4);
		AddItemToShop(anglerDecorShop, ItemID.FishCostumeMask, 4);
		AddItemToShop(anglerDecorShop, ItemID.FishCostumeShirt, 4);
		AddItemToShop(anglerDecorShop, ItemID.FishCostumeFinskirt, 4);
		AddItemToShop(anglerDecorShop, ItemID.BunnyfishTrophy, 1);
		AddItemToShop(anglerDecorShop, ItemID.GoldfishTrophy, 1);
		AddItemToShop(anglerDecorShop, ItemID.SharkteethTrophy, 1);
		AddItemToShop(anglerDecorShop, ItemID.SwordfishTrophy, 1);
		AddItemToShop(anglerDecorShop, ItemID.TreasureMap, 1);
		AddItemToShop(anglerDecorShop, ItemID.SeaweedPlanter, 1);
		AddItemToShop(anglerDecorShop, ItemID.PillaginMePixels, 1);
		AddItemToShop(anglerDecorShop, ItemID.CompassRose, 1);
		AddItemToShop(anglerDecorShop, ItemID.ShipsWheel, 1);
		AddItemToShop(anglerDecorShop, ItemID.ShipInABottle, 1);
		AddItemToShop(anglerDecorShop, ItemID.LifePreserver, 1);
		AddItemToShop(anglerDecorShop, ItemID.WallAnchor, 1);
		AddItemToShop(anglerDecorShop, ItemID.NotSoLostInParadise, 1);
		AddItemToShop(anglerDecorShop, ItemID.Crustography, 1);
		AddItemToShop(anglerDecorShop, ItemID.WhatLurksBelow, 1);
		AddItemToShop(anglerDecorShop, ItemID.Fangs, 1);
		AddItemToShop(anglerDecorShop, ItemID.CouchGag, 1);
		AddItemToShop(anglerDecorShop, ItemID.SilentFish, 1);
		AddItemToShop(anglerDecorShop, ItemID.TheDuke, 1);
		//anglerDecorShop.Register();
	}

	public override void ModifyActiveShop(NPC npc, string shopName, Item?[] items)
	{
		if (npc.type != NPCID.Angler) {
			return;
		}
		
		if (shopName == NoFishingQuests.ShopName) {
			anglerShop.FillShop(items, npc, out _);
		}
		else if (shopName == NoFishingQuests.DecorationShopName) {
			anglerDecorShop.FillShop(items, npc, out _);
		}
	}

	private static void AddItemToShop(NPCShop shop, short id, int price, params Condition[] conditions)
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
		
		shop.Add(item, conditions);
	}
}