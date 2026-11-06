using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Projectiles;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.NPCs;
public class ShardDevourer : ModNPC {
 public override void SetDefaults() {
  NPC.width=48; NPC.height=48; NPC.lifeMax=900; NPC.damage=28; NPC.defense=9;
  NPC.knockBackResist=0f; NPC.noGravity=true; NPC.noTileCollide=true; NPC.aiStyle=-1;
  NPC.value=Item.buyPrice(gold:1); NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override void AI() {
  NPC.TargetClosest(false);
  Player player=Main.player[NPC.target];
  if (!player.active || player.dead || Vector2.Distance(player.Center,NPC.Center)>1800f) { NPC.EncourageDespawn(10); NPC.velocity.Y-=0.15f; return; }
  NPC.ai[0]++;
  bool enraged=NPC.life < NPC.lifeMax/2;
  Vector2 hovering=player.Center + new Vector2((float)System.Math.Sin(NPC.ai[0]*0.025f)*160f,-130f);
  NPC.velocity=Vector2.Lerp(NPC.velocity,(hovering-NPC.Center).SafeNormalize(Vector2.UnitY)*(enraged?8f:5.5f),0.055f);
  NPC.rotation=NPC.velocity.X*0.025f;
  Lighting.AddLight(NPC.Center,0.4f,0.12f,0.6f);
  if (Main.netMode==NetmodeID.MultiplayerClient) return;
  int cycle=enraged?90:130;
  if ((int)NPC.ai[0]%cycle==0) {
   Vector2 direction=(player.Center-NPC.Center).SafeNormalize(Vector2.UnitY);
   for (int i=0;i<(enraged?3:1);i++) {
    Vector2 speed=direction.RotatedBy((i-(enraged?1:0))*0.26f)*(enraged?8f:6f);
    Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,speed,ModContent.ProjectileType<UmbralBolt>(),enraged?18:12,1f,Main.myPlayer);
   }
  }
  if ((int)NPC.ai[0]%(enraged?170:220)==40) { NPC.velocity=(player.Center-NPC.Center).SafeNormalize(Vector2.UnitY)*(enraged?12f:9f); NPC.netUpdate=true; }
 }
 public override void OnKill() { if (Main.netMode!=NetmodeID.MultiplayerClient) { UmbraWorld.DownedDevourer=true; if(Main.netMode==NetmodeID.Server) NetMessage.SendData(MessageID.WorldData); } }
 public override void ModifyNPCLoot(NPCLoot loot) {
  loot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),1,12,20));
  loot.Add(ItemDropRule.Common(ModContent.ItemType<DevourerFang>(),1,3,5));
 }
}
