using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
namespace UmbraShatteredNight.Content.Items;
public class FractureBeacon : ModItem {
 public override void SetDefaults() { Item.width=30; Item.height=30; Item.maxStack=20; Item.consumable=true; Item.useStyle=ItemUseStyleID.HoldUp; Item.useAnimation=40; Item.useTime=40; Item.UseSound=SoundID.Roar; Item.rare=ItemRarityID.Green; }
 public override bool CanUseItem(Player player)=>!NPC.AnyNPCs(ModContent.NPCType<ShardDevourer>()) && player.ZoneRockLayerHeight;
 public override bool? UseItem(Player player) {
  if(player.whoAmI==Main.myPlayer) {
   int npcType=ModContent.NPCType<ShardDevourer>();
   if(Main.netMode!=NetmodeID.MultiplayerClient) NPC.SpawnOnPlayer(player.whoAmI,npcType);
   else NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent,number:player.whoAmI,number2:npcType);
  }
  return true;
 }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<VoidShard>(10).AddIngredient<UmbralBar>(5).AddTile(TileID.Anvils).Register();
}
