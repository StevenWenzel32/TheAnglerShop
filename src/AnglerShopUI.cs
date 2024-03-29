using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NoFishingQuests;

internal class AnglerShopUI : UIState
{
	// Main._textDisplayCache
	private static readonly object TextDisplayCache = typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(Main.instance)!;
	private static readonly PropertyInfo? AmountOfLines = TextDisplayCache.GetType().GetProperty("AmountOfLines", BindingFlags.Instance | BindingFlags.Public);
	private bool focused; // true if the shop button is hovered
	private bool decorationShopFocused; // true if the shop button is hovered

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Main.npcChatText == string.Empty)
			return;
		
		base.Draw(spriteBatch);

		// Source: Main.DrawNPCChatButtons
		
		// How long the npc text is (varies with language)
		int numLines = (int)AmountOfLines!.GetValue(TextDisplayCache)!;

		Vector2 scale = new(0.9f); // scale of the button
		string text = Language.GetTextValue("LegacyInterface.28"); // "Shop"
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale); // size of "Shop" with scale 0.9

		// vanilla did that, idk
		Vector2 value2 = new(1f);
		if (stringSize.X > 260f)
			value2.X *= 260f / stringSize.X;

		// button positions
		float posButton1 = 180 + (Main.screenWidth - 800) / 2f; // position of the first button (Quest)
		float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), scale).X + 30f; // Position of the second button (Close)
		float posButton3 = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f; // Position of the third button (Happiness)
		float posButton4 = posButton3 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("UI.NPCCheckHappiness"), scale).X + 30f; // Position of the new button
		Vector2 position = new(posButton4, 130 + numLines * 30);

		// if the player is hovering over the button
		if (Main.MouseScreen.Between(position, position + stringSize * scale * value2.X) && !PlayerInput.IgnoreMouseInterface) {
			Main.LocalPlayer.mouseInterface = true;
			Main.LocalPlayer.releaseUseItem = false;
			scale *= 1.2f; // make button bigger

			if (!focused) {
				SoundEngine.PlaySound(SoundID.MenuTick);
			}

			focused = true;
		}
		else {
			if (focused) {
				SoundEngine.PlaySound(SoundID.MenuTick);
			}

			focused = false;
		}

		// draw button shadow
		ChatManager.DrawColorCodedStringShadow(spriteBatch: spriteBatch,
			font: FontAssets.MouseText.Value,
			text: text,
			position: position + stringSize * value2 * 0.5f,
			baseColor: !focused ? Color.Black : Color.Brown,
			rotation: 0f,
			origin: stringSize * 0.5f,
			baseScale: scale * value2
		);

		// draw button text
		ChatManager.DrawColorCodedString(spriteBatch: spriteBatch,
			font: FontAssets.MouseText.Value,
			text: text,
			position: position + stringSize * value2 * 0.5f,
			baseColor: !focused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69), // color of the button text
			rotation: 0f,
			origin: stringSize * 0.5f,
			baseScale: scale
		);
		
		Vector2 decorationScale = new(0.9f); // scale of the button
		string decorationText = Language.GetTextValue("GameUI.PainterDecor");
		int offset = 30;
		if (FontAssets.MouseText.Value.MeasureString(decorationText).X > 50) {
			decorationScale = new(0.8f);
			offset = 10;
		}
		Vector2 decorationStringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, decorationText, decorationScale); // size of "GameUI.PainterDecor" with scale 0.9
		Vector2 decorationPosition = new(posButton4 + stringSize.X + offset, 130 + numLines * 30);
		
		// vanilla did that, idk
		Vector2 value3 = new(1f);
		if (decorationStringSize.X > 260f)
			value3.X *= 260f / decorationStringSize.X;
		
		// if the player is hovering over the decoration button
		if (Main.MouseScreen.Between(decorationPosition, decorationPosition + decorationStringSize * value3.X) && !PlayerInput.IgnoreMouseInterface) {
			Main.LocalPlayer.mouseInterface = true;
			Main.LocalPlayer.releaseUseItem = false;
			decorationScale *= 1.2f; // make button bigger

			if (!decorationShopFocused) {
				SoundEngine.PlaySound(SoundID.MenuTick);
			}

			decorationShopFocused = true;
		}
		else {
			if (decorationShopFocused) {
				SoundEngine.PlaySound(SoundID.MenuTick);
			}

			decorationShopFocused = false;
		}
		
		// draw decoration button shadow
		ChatManager.DrawColorCodedStringShadow(spriteBatch: spriteBatch,
			font: FontAssets.MouseText.Value,
			text: decorationText,
			position: decorationPosition + decorationStringSize * value3 * 0.5f,
			baseColor: (!decorationShopFocused) ? Color.Black : Color.Brown,
			rotation: 0f,
			origin: decorationStringSize * 0.5f,
			baseScale: decorationScale * value3
		);

		// draw decoration button text
		ChatManager.DrawColorCodedString(spriteBatch: spriteBatch,
			font: FontAssets.MouseText.Value,
			text: decorationText,
			position: decorationPosition + decorationStringSize * value3 * 0.5f,
			baseColor: !decorationShopFocused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69), // color of the button text
			rotation: 0f,
			origin: decorationStringSize * 0.5f,
			baseScale: decorationScale
		);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		// if the shop button is clicked
		if (focused && Main.mouseLeft) {
			OpenShop(NoFishingQuests.ShopName);
		}
		else if (decorationShopFocused && Main.mouseLeft) {
			OpenShop(NoFishingQuests.DecorationShopName);
		}
	}

	// copied from Main.OpenShop (except the line with the comment)
	internal static void OpenShop(string name)
	{
		Main.playerInventory = true;
		Main.stackSplit = 9999;
		Main.npcChatText = "";
		Main.SetNPCShopIndex(1);
		//Main.instance.shop[Main.npcShop].SetupShop(shopIndex);
		Main.instance.shop[Main.npcShop].SetupShop(name, Main.LocalPlayer.TalkNPC); // Setup Angler shop (gets filled in ModifyShops.cs) 
		SoundEngine.PlaySound(SoundID.MenuTick);
	}
}
