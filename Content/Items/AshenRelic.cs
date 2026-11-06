using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
namespace UmbraShatteredNight.Content.Items;
public class AshenRelic : ModItem {
 public override void SetDefaults() { Item.width=24; Item.height=28;Item.accessory=true;Item.rare=ItemRarityID.Pink;Item.value=Item.sellPrice(gold:2); }
 public override void UpdateAccessory(Player player,bool hideVisual) {player.GetDamage(DamageClass.Generic)+=0.03f; if(player.InModBiome<AshenWastes>())player.statDefense+=5;}
}
