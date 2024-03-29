using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace NoFishingQuests;

public class AnglerCoin : CustomCurrencySingleCoin
{
	public static int id;
	
	public AnglerCoin(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
	{
		CurrencyTextKey = Language.GetTextValue("Mods.NoFishingQuests.Currencies.AnglerCoin");
		CurrencyTextColor = Color.Orange;
	}
}