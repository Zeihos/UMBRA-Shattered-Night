using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.NPCs;
public class AstralDrifter : ModNPC {
 public override void SetDefaults() {
  NPC.width=30;NPC.height=32;NPC.lifeMax=340;NPC.damage=54;NPC.defense=18;
  NPC.knockBackResist=.3f;NPC.value=Item.buyPrice(silver:28);
  NPC.aiStyle=2;AIType=NPCID.DemonEye;
  NPC.noGravity=true;NPC.noTileCollide=true;
  NPC.HitSound=SoundID.NPCHit3;NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
  UmbraWorld.DownedArchon && spawnInfo.Player.InModBiome<AstralRift>() && !spawnInfo.PlayerSafe ? .16f : 0f;
 public override void AI() {Lighting.AddLight(NPC.Center,.17f,.13f,.42f);NPC.rotation=NPC.velocity.X*.035f;}
 public override void ModifyNPCLoot(NPCLoot loot) {
  loot.Add(ItemDropRule.Common(ModContent.ItemType<AstralGlassItem>(),1,2,5));
  loot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),2,1,3));
 }
}
