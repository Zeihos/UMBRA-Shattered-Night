using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;
public class NullScepter : ModItem {
 public override void SetDefaults() {
 Item.width=34; Item.height=38; Item.damage=23; Item.DamageType=DamageClass.Magic;
 Item.mana=9; Item.useTime=23; Item.useAnimation=23; Item.useStyle=ItemUseStyleID.Shoot;
 Item.noMelee=true; Item.knockBack=2f; Item.shoot=ModContent.ProjectileType<NullSpark>();
 Item.shootSpeed=9f; Item.UseSound=SoundID.Item20; Item.autoReuse=true;
 Item.value=Item.sellPrice(gold:1); Item.rare=ItemRarityID.Green; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(9).AddIngredient<VoidShard>(8).AddTile(TileID.Anvils).Register();
}
