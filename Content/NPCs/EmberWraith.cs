using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.NPCs;
public class EmberWraith : ModNPC {
 public override void SetStaticDefaults() { Main.npcFrameCount[Type] = 1; }
 public override void SetDefaults() {
  NPC.width=30; NPC.height=38; NPC.lifeMax=420; NPC.damage=61; NPC.defense=16;
  NPC.knockBackResist=0.35f; NPC.noGravity=true; NPC.noTileCollide=true;
  NPC.aiStyle=14; AIType=NPCID.CursedSkull;
  NPC.value=Item.buyPrice(silver:35);
  NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
  UmbraWorld.DownedAstralSovereign && !spawnInfo.PlayerSafe && spawnInfo.Player.InModBiome<AshenWastes>() ? 0.08f : 0f;
 public override void AI() { Lighting.AddLight(NPC.Center, 0.65f, 0.19f, 0.04f); }
 public override void ModifyNPCLoot(NPCLoot npcLoot) { npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CinderOreItem>(),3,1,2)); }
}
