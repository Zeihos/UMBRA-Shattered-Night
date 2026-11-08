using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.NPCs;
public class AstralHeart : ModNPC
{
    public override void SetStaticDefaults() { Main.npcFrameCount[Type] = 6; }
    
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

    public override void SetDefaults()
    {
        NPC.width = 50; NPC.height = 52; NPC.lifeMax = 1800;
        NPC.damage = 65; NPC.defense = 18; NPC.knockBackResist = 0f;
        NPC.value = Item.buyPrice(gold: 3);
        NPC.noGravity = true; NPC.noTileCollide = true;
        NPC.HitSound = SoundID.NPCHit5; NPC.DeathSound = SoundID.NPCDeath6;
        NPC.aiStyle = -1;
    }
    public override float SpawnChance(NPCSpawnInfo info)
    {
        if (!UmbraWorld.DownedArchon || info.PlayerSafe || !info.Player.InModBiome<AstralRift>()) return 0f;
        return NPC.AnyNPCs(Type) ? 0f : 0.004f;
    }
    public override void AI()
    {
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers || !Main.player[NPC.target].active || Main.player[NPC.target].dead)
            NPC.TargetClosest(false);
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers)
        {
            NPC.EncourageDespawn(10);
            return;
        }
        Player target = Main.player[NPC.target];
        if (!target.active || target.dead)
        {
            NPC.EncourageDespawn(10);
            return;
        }
        NPC.ai[0]++;
        // A clearly signalled radial attack at low health. Only the server creates damaging projectiles.
        bool secondPhase = NPC.life < NPC.lifeMax / 2;
        if (secondPhase && (int)NPC.ai[0] % 240 == 180)
        {
            NPC.ai[1] = 45;
            NPC.netUpdate = true;
        }
        if (NPC.ai[1] > 0)
        {
            NPC.ai[1]--;
            NPC.velocity *= 0.88f;
            if (!Main.dedServ && Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch);
                dust.noGravity = true;
                dust.velocity *= 1.7f;
            }
            if (NPC.ai[1] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 10f) * 6.5f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<global::UmbraShatteredNight.Content.Projectiles.AstralPulse>(),
                        23, 1f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            Lighting.AddLight(NPC.Center, 0.9f, 0.35f, 0.95f);
            return;
        }
        float phaseSpeed = NPC.life < NPC.lifeMax / 2 ? 7f : 4.5f;
        Vector2 desired = target.Center + new Vector2((float)System.Math.Sin(NPC.ai[0] / 75f) * 190f, -145f);
        Vector2 offset = desired - NPC.Center;
        if (offset.Length() > 20f)
        {
            offset.Normalize();
            NPC.velocity = Vector2.Lerp(NPC.velocity, offset * phaseSpeed, 0.055f);
        }
        else NPC.velocity *= 0.93f;
        NPC.rotation += 0.012f;
        Lighting.AddLight(NPC.Center, 0.52f, 0.23f, 0.75f);
        if (Main.netMode != NetmodeID.MultiplayerClient && (int)NPC.ai[0] % (NPC.life < NPC.lifeMax / 2 ? 65 : 100) == 0)
        {
            Vector2 direction = target.Center - NPC.Center;
            if (direction.LengthSquared() < 0.01f) direction = Vector2.UnitY;
            direction.Normalize();
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 8f,
                ModContent.ProjectileType<global::UmbraShatteredNight.Content.Projectiles.AstralPulse>(), 30, 1f, Main.myPlayer);
            if (NPC.life < NPC.lifeMax / 2)
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    Vector2 side = direction.RotatedBy(MathHelper.ToRadians(18 * i));
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, side * 8f,
                        ModContent.ProjectileType<global::UmbraShatteredNight.Content.Projectiles.AstralPulse>(), 24, 1f, Main.myPlayer);
                }
            }
        }
    }
    public override void OnKill()
    {
        // Executed in single-player or on the server, then broadcast as world data.
        if (Main.netMode == NetmodeID.MultiplayerClient || UmbraWorld.DownedAstralHeart)
            return;
        UmbraWorld.DownedAstralHeart = true;
        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.WorldData);
    }

    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<AstralHeartFragment>(), 1, 3, 6));
    }
}
