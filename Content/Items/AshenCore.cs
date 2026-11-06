using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AshenCore : ModItem {
 public override void SetDefaults() { Item.width=20;Item.height=20;Item.maxStack=9999;Item.rare=ItemRarityID.LightRed;Item.value=Item.sellPrice(silver:45); }
}
