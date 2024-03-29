using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoFishingQuests;

[Autoload(Side = ModSide.Client)]
internal class UISystem : ModSystem
{
	private UserInterface? userInterface;
	private static bool dialogueTweakLoaded;

	public override void Load()
	{
		if (!Main.dedServ) {
			UIState anglerShopUI = new AnglerShopUI();
			anglerShopUI.Activate();
			userInterface = new UserInterface();
			userInterface.SetState(anglerShopUI);
		}
	}

	public override void PostSetupContent()
	{
		if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak)) {
			dialogueTweakLoaded = true;
			dialogueTweak.Call("AddButton",
				NPCID.Angler, // NPC ID
				() => Language.GetTextValue("LegacyInterface.28"),
				"DialogueTweak/Interfaces/Assets/Icon_Default", // The texture's path
				() =>
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
						AnglerShopUI.OpenShop(NoFishingQuests.ShopName);
				}
			);
			dialogueTweak.Call("AddButton",
				NPCID.Angler, // NPC ID
				() => Language.GetTextValue("GameUI.PainterDecor"),
				"DialogueTweak/Interfaces/Assets/Icon_Default", // The texture's path
				() =>
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
						AnglerShopUI.OpenShop(NoFishingQuests.DecorationShopName);
				}
			);
		}
	}

	private GameTime? lastUpdateUiGameTime;
	public override void UpdateUI(GameTime gameTime)
	{
		lastUpdateUiGameTime = gameTime;

		// if the player is talking to the Angler and the new shop isn't opened
		if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Angler && Main.npcShop != 99) {
			userInterface?.Update(gameTime);
		}
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		if (mouseTextIndex != -1) {
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"AnglerShop: UI",
				delegate
				{
					// if the player is talking to the Angler, the new shop isn't opened and dialogue tweak mod isn't active.
					if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Angler && Main.npcShop != 99 && !dialogueTweakLoaded) {
						userInterface?.Draw(Main.spriteBatch, lastUpdateUiGameTime);
					}
					return true;
				}, InterfaceScaleType.UI));
		}
	}
}
