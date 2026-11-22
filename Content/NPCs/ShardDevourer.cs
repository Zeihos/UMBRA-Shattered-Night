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
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 6;

 
    // Six hand-finished animation frames. The sprite sheet preserves the original
    // per-frame bounds so collision and attack positioning do not change.
    public override void FindFrame(int frameHeight)
    {
        float pace = NPC.life < NPC.lifeMax * 0.4f ? 0.17f : 0.11f;
        if (NPC.ai[2] > 0f) pace += 0.045f; // make charged attacks more legible
        NPC.frameCounter += pace;
        if (NPC.frameCounter >= 6d) NPC.frameCounter -= 6d;
        NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
    }

    public override void SetDefaults() {
  NPC.width=48; NPC.height=48; NPC.lifeMax=900; NPC.damage=28; NPC.defense=9;
  NPC.knockBackResist=0f; NPC.noGravity=true; NPC.noTileCollide=true; NPC.aiStyle=-1;
  NPC.value=Item.buyPrice(gold:1); NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
    public override void AI()
    {
        if (!UMBRAEnemyTargeting.TryGetTarget(NPC, out Player player, 1800f))
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.EncourageDespawn(10);
            NPC.velocity.Y -= 0.15f;
            return;
        }

        NPC.ai[0]++;
        bool enraged = NPC.life < NPC.lifeMax / 2;
        float timer = NPC.ai[0];
        NPC.rotation = NPC.velocity.X * 0.025f;
        Lighting.AddLight(NPC.Center, NPC.ai[2] > 0f ? 0.62f : 0.4f, 0.12f, 0.6f);

        // ai[1] is a synchronized dash duration, ai[2] a visible windup.
        // Keep attacks on the server; clients only animate particles.
        if (NPC.ai[2] > 0f)
        {
            NPC.ai[2]--;
            NPC.velocity *= 0.85f;
            if (!Main.dedServ && Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.25f;
            }
            if (NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) *
                    (enraged ? 12f : 9f);
                NPC.ai[1] = 16f;
                NPC.netUpdate = true;
            }
            return;
        }

        if (NPC.ai[1] > 0f)
        {
            NPC.ai[1]--;
            NPC.velocity *= 0.99f;
            if (NPC.ai[1] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
                NPC.netUpdate = true;
            return;
        }

        Vector2 hovering = player.Center +
            new Vector2((float)System.Math.Sin(timer * 0.025f) * 160f, -130f);
        NPC.velocity = Vector2.Lerp(NPC.velocity,
            (hovering - NPC.Center).SafeNormalize(Vector2.UnitY) *
            (enraged ? 8f : 5.5f), 0.055f);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        if ((int)timer % (enraged ? 170 : 220) == 40)
        {
            NPC.ai[2] = 26f;
            NPC.netUpdate = true;
            return;
        }

        int cycle = enraged ? 90 : 130;
        if ((int)timer % cycle == 0)
        {
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            int shots = enraged ? 3 : 1;
            for (int i = 0; i < shots; i++)
            {
                Vector2 velocity = aim.RotatedBy((i - (shots - 1) / 2f) * 0.26f) *
                    (enraged ? 8f : 6f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<UmbralBolt>(), enraged ? 18 : 12,
                    1f, Main.myPlayer);
            }
        }
    }
 public override void OnKill() { if (Main.netMode!=NetmodeID.MultiplayerClient) { UmbraWorld.DownedDevourer=true; if(Main.netMode==NetmodeID.Server) NetMessage.SendData(MessageID.WorldData); } }
 public override void ModifyNPCLoot(NPCLoot loot) {
  loot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),1,12,20));
  loot.Add(ItemDropRule.Common(ModContent.ItemType<DevourerFang>(),1,3,5));
 }
}
