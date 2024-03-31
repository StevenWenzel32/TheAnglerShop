using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoFishingQuests;

public class ModifyShops : GlobalNPC
{
	public override void ModifyActiveShop(NPC npc, string shopName, Item?[] items)
	{
		if (npc.type != NPCID.Angler) {
			return;
		}
		
		if (shopName == NoFishingQuests.ShopName) {
			AnglerShopSystem.GetAnglerShop().FillShop(items, npc, out _);
		}
		else if (shopName == NoFishingQuests.DecorationShopName) {
			AnglerShopSystem.GetAnglerDecorShop().FillShop(items, npc, out _);
		}
	}
}