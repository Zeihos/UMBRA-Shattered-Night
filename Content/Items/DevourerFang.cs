using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class DevourerFang : ModItem {
 public override void SetDefaults() { Item.width=24; Item.height=30; Item.maxStack=9999; Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(silver:30); }
}
