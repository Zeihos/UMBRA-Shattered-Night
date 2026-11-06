using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Head)]
public class RiftguardHelm : ModItem {
 public override void SetDefaults() { Item.width=26; Item.height=24; Item.defense=9; Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1); }
 public override void UpdateEquip(Player player) { player.GetDamage(DamageClass.Melee) += 0.04f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(10).AddIngredient<DevourerFang>(5).AddTile(TileID.Anvils).Register();
 public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == Type && body.type == ModContent.ItemType<RiftguardPlate>() && legs.type == ModContent.ItemType<RiftguardBoots>();
 public override void UpdateArmorSet(Player player) { player.setBonus = "Riftguard: +8% melee damage and +5% damage reduction"; player.GetDamage(DamageClass.Melee) += .08f; player.endurance += .05f; }
}
