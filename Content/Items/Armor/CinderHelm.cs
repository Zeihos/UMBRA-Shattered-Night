using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Head)]
public class CinderHelm : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=28; Item.defense=15; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:5); }
 public override void UpdateEquip(Player player) { player.GetCritChance(DamageClass.Generic) += 5f; }
  public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == Type && body.type == ModContent.ItemType<CinderPlate>() && legs.type == ModContent.ItemType<CinderGreaves>();
 public override void UpdateArmorSet(Player player) { player.setBonus = "Ashen fury: +9% damage and 5% damage reduction"; player.GetDamage(DamageClass.Generic) += 0.09f; player.endurance += 0.05f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderBar>(15).AddTile(TileID.MythrilAnvil).Register();
}
