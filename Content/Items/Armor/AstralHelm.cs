using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Head)]
public class AstralHelm : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=26; Item.defense=12; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:3); }
 public override void UpdateEquip(Player player) { player.GetCritChance(DamageClass.Generic) += 4f; }
 public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == Type && body.type == ModContent.ItemType<AstralPlate>() && legs.type == ModContent.ItemType<AstralGreaves>();
 public override void UpdateArmorSet(Player player) { player.setBonus = "Astral resonance: +8% damage and +8% movement speed"; player.GetDamage(DamageClass.Generic) += 0.08f; player.moveSpeed += 0.08f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<AstralBar>(12).AddTile(TileID.MythrilAnvil).Register();
}
