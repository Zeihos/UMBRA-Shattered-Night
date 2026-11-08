using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
using UmbraShatteredNight.Content.Projectiles;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Visual feedback only. No damage, buffs, spawned gameplay projectiles, or network packets.
/// Impact particles are deliberately short and bounded, including in multiplayer.
/// </summary>
internal static class UMBRAImpactPalette
{
    internal static bool TryProjectile(Projectile projectile, out int dustType)
    {
        int id = projectile.type;
        if (id == ModContent.ProjectileType<CinderEmber>() ||
            id == ModContent.ProjectileType<ColossusCinder>())
        {
            dustType = DustID.Torch;
            return true;
        }
        if (id == ModContent.ProjectileType<AstralPulse>() ||
            id == ModContent.ProjectileType<SovereignArcaneOrb>() ||
            id == ModContent.ProjectileType<SovereignBladeWave>())
        {
            dustType = DustID.BlueTorch;
            return true;
        }
        if (id == ModContent.ProjectileType<UmbralBolt>() ||
            id == ModContent.ProjectileType<NullSpark>() ||
            id == ModContent.ProjectileType<ArchonArcaneShot>())
        {
            dustType = DustID.PurpleTorch;
            return true;
        }
        dustType = 0;
        return false;
    }

    internal static int NPCDust(NPC npc)
    {
        if (npc.type == ModContent.NPCType<AshenOverlord>() ||
            npc.type == ModContent.NPCType<AshenColossus>() ||
            npc.type == ModContent.NPCType<AshenCrawler>() ||
            npc.type == ModContent.NPCType<EmberWraith>())
            return DustID.Torch;

        if (npc.type == ModContent.NPCType<AstralSovereign>() ||
            npc.type == ModContent.NPCType<AstralHeart>() ||
            npc.type == ModContent.NPCType<AstralDrifter>() ||
            npc.type == ModContent.NPCType<AstralWarden>())
            return DustID.BlueTorch;

        return DustID.PurpleTorch;
    }

    internal static void Burst(Vector2 center, int dustType, int amount, float speed)
    {
        if (Main.dedServ) return;

        // Offscreen impacts should not spend the whole particle budget.
        if (center.X < Main.screenPosition.X - 160f ||
            center.Y < Main.screenPosition.Y - 160f ||
            center.X > Main.screenPosition.X + Main.screenWidth + 160f ||
            center.Y > Main.screenPosition.Y + Main.screenHeight + 160f)
            return;

        for (int index = 0; index < amount; index++)
        {
            // Symmetric starburst: each spoke has a subtle random variance.
            float angle = MathHelper.TwoPi * index / amount + Main.rand.NextFloat(-0.18f, 0.18f);
            Vector2 velocity = angle.ToRotationVector2() * Main.rand.NextFloat(speed * 0.5f, speed);
            int dustIndex = Dust.NewDust(center - new Vector2(3f), 6, 6, dustType);
            Dust dust = Main.dust[dustIndex];
            dust.position = center;
            dust.velocity = velocity;
            dust.noGravity = true;
            dust.scale = Main.rand.NextFloat(0.75f, 1.18f);
            dust.fadeIn = 0.2f;
        }
    }
}

public sealed class UMBRAProjectileImpactVisuals : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    private bool _hitTarget;

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!UMBRAImpactPalette.TryProjectile(projectile, out int dustType)) return;
        _hitTarget = true;
        // Friendly projectile hit hooks execute on their owning client.
        if (Main.dedServ || projectile.owner != Main.myPlayer) return;
        UMBRAImpactPalette.Burst(projectile.Center, dustType, hit.Crit ? 9 : 6, 2.8f);
    }

    public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
    {
        if (!UMBRAImpactPalette.TryProjectile(projectile, out int dustType)) return;
        _hitTarget = true;
        // A hostile hit callback runs on the damaged player's client.
        if (Main.dedServ) return;
        UMBRAImpactPalette.Burst(projectile.Center, dustType, 7, 2.7f);
    }

    public override void OnKill(Projectile projectile, int timeLeft)
    {
        if (Main.dedServ || _hitTarget || timeLeft <= 0 ||
            projectile.type == ModContent.ProjectileType<NullSpark>() ||
            !UMBRAImpactPalette.TryProjectile(projectile, out int dustType))
            return;

        // Tile collision / early destruction only. Normal expiry in midair has
        // no large flash and NullSpark already implements its own death burst.
        UMBRAImpactPalette.Burst(projectile.Center, dustType, 4, 1.7f);
    }
}

/// <summary>
/// Clear hit confirmation for existing UMBRA enemies and bosses. The white
/// flash never changes stats, hitboxes, or NPC synchronization.
/// </summary>
public sealed class UMBRANPCHitVisuals : GlobalNPC
{
    public override bool InstancePerEntity => true;
    private byte _hitFlash;

    public override void HitEffect(NPC npc, NPC.HitInfo hit)
    {
        if (Main.dedServ || npc.ModNPC?.Mod?.Name != "UmbraShatteredNight")
            return;

        _hitFlash = 8;
        bool defeated = npc.life <= 0;
        int count = defeated ? (npc.boss ? 20 : 10) : (npc.boss ? 5 : 3);
        float speed = defeated ? 4.7f : 2.1f;
        UMBRAImpactPalette.Burst(npc.Center, UMBRAImpactPalette.NPCDust(npc), count, speed);
    }

    public override void PostAI(NPC npc)
    {
        if (_hitFlash > 0) _hitFlash--;
    }

    public override Color? GetAlpha(NPC npc, Color drawColor)
    {
        if (_hitFlash == 0 || npc.ModNPC?.Mod?.Name != "UmbraShatteredNight")
            return null;

        float strength = 0.56f * (_hitFlash / 8f);
        return Color.Lerp(drawColor, Color.White, strength);
    }
}
