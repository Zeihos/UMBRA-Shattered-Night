using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
using UmbraShatteredNight.Content.Projectiles;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Client-side, deterministic glow overlays and short projectile afterimages.
/// No additional network packets, allocations per frame, or server-side dust.
/// </summary>
public sealed class UMBRABossVisuals : GlobalNPC
{
    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (Main.dedServ || npc.ModNPC?.Mod?.Name != "UmbraShatteredNight")
            return true;

        bool highlight = npc.boss || npc.type == ModContent.NPCType<ShardDevourer>() ||
            npc.type == ModContent.NPCType<AstralHeart>() ||
            npc.type == ModContent.NPCType<AshenColossus>();
        if (!highlight) return true;

        Color tint;
        if (npc.type == ModContent.NPCType<AshenOverlord>() || npc.type == ModContent.NPCType<AshenColossus>())
            tint = new Color(245, 100, 36, 0);
        else if (npc.type == ModContent.NPCType<AstralSovereign>() || npc.type == ModContent.NPCType<AstralHeart>())
            tint = new Color(136, 167, 255, 0);
        else tint = new Color(176, 92, 240, 0);

        Texture2D texture = TextureAssets.Npc[npc.type].Value;
        if (npc.frame.Width == 0 || npc.frame.Height == 0) return true;
        float pulse = 0.5f + 0.5f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 5f + npc.whoAmI);
        float glow = 0.12f + 0.11f * pulse;
        Vector2 center = npc.Center - screenPos + new Vector2(0, npc.gfxOffY);
        Vector2 origin = npc.frame.Size() * 0.5f;
        SpriteEffects flip = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        // Sprite draw order: halo then vanilla sprite. Alpha=0 gives soft additive light.
        // Deliberate attack cue: warning ring appears before the actual radial attack.
        // Uses only synchronized boss timers; no damaging visual projectiles are spawned.
        float charge = TelegraphCharge(npc);
        if (charge >= 0f)
            DrawTelegraph(center, npc, charge, tint);

        Main.EntitySpriteDraw(texture, center, npc.frame, tint * glow, npc.rotation, origin, npc.scale * 1.08f, flip, 0);
        return true;
    }

    private static float TelegraphCharge(NPC npc)
    {
        if (npc.type == ModContent.NPCType<RiftArchon>() && npc.ai[2] > 0f)
            return 1f - MathHelper.Clamp(npc.ai[2] / 45f, 0f, 1f);
        if ((npc.type == ModContent.NPCType<AstralSovereign>() ||
             npc.type == ModContent.NPCType<AshenOverlord>()) && npc.ai[2] > 0f)
            return 1f - MathHelper.Clamp(npc.ai[2] / 55f, 0f, 1f);
        if (npc.type == ModContent.NPCType<AstralHeart>() && npc.ai[1] > 0f)
            return 1f - MathHelper.Clamp(npc.ai[1] / 45f, 0f, 1f);
        if (npc.type == ModContent.NPCType<AshenColossus>())
        {
            int cycle = npc.life <= npc.lifeMax / 2 ? 210 : 270;
            int tick = (int)npc.ai[0] % cycle;
            int warningStart = cycle - 65;
            int warningEnd = cycle - 20;
            if (tick >= warningStart && tick < warningEnd)
                return (tick - warningStart) / (float)(warningEnd - warningStart);
        }
        return -1f;
    }

    private static void DrawTelegraph(Vector2 center, NPC npc, float charge, Color tint)
    {
        Texture2D pixel = TextureAssets.MagicPixel.Value;
        // 18 short segments, no particle allocations and no per-frame texture creation.
        const int segments = 18;
        float radius = System.Math.Max(npc.width, npc.height) * 0.66f + 18f + charge * 9f;
        float thickness = 1.5f + charge * 1.5f;
        Color warning = new Color(tint.R, tint.G, tint.B) * (0.38f + 0.45f * charge);
        float rotation = Main.GlobalTimeWrappedHourly * 0.28f;
        for (int i = 0; i < segments; i++)
        {
            float angle = MathHelper.TwoPi * i / segments + rotation;
            float next = MathHelper.TwoPi * (i + 0.75f) / segments + rotation;
            Vector2 from = center + angle.ToRotationVector2() * radius;
            Vector2 to = center + next.ToRotationVector2() * radius;
            Vector2 direction = to - from;
            Main.EntitySpriteDraw(pixel, (from + to) * 0.5f, null, warning,
                direction.ToRotation(), new Vector2(0.5f, 0.5f),
                new Vector2(direction.Length(), thickness), SpriteEffects.None, 0);
        }
    }
}

public sealed class UMBRAProjectileVisuals : GlobalProjectile
{
    public override void PostDraw(Projectile projectile, Color lightColor)
    {
        if (Main.dedServ || projectile.ModProjectile?.Mod?.Name != "UmbraShatteredNight" ||
            projectile.velocity.LengthSquared() < 4f) return;

        int id = projectile.type;
        bool ember = id == ModContent.ProjectileType<CinderEmber>() || id == ModContent.ProjectileType<ColossusCinder>();
        bool astral = id == ModContent.ProjectileType<AstralPulse>() || id == ModContent.ProjectileType<SovereignArcaneOrb>() ||
                      id == ModContent.ProjectileType<SovereignBladeWave>();
        bool voidBolt = id == ModContent.ProjectileType<UmbralBolt>() || id == ModContent.ProjectileType<NullSpark>() ||
                        id == ModContent.ProjectileType<ArchonArcaneShot>();
        if (!ember && !astral && !voidBolt) return;

        Texture2D texture = TextureAssets.Projectile[id].Value;
        Color glow = ember ? new Color(255, 112, 36, 0) :
            astral ? new Color(135, 170, 255, 0) : new Color(166, 82, 245, 0);
        // Real projectile history makes curves, bounces and sudden changes read
        // accurately; velocity extrapolation used to leave detached fake trails.
        if (projectile.oldPos == null || projectile.oldPos.Length < 2) return;
        Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
        int limit = System.Math.Min(projectile.oldPos.Length - 1, 6);
        for (int step = limit; step >= 1; step--)
        {
            Vector2 previous = projectile.oldPos[step];
            if (previous == Vector2.Zero) continue; // history not filled after spawn
            float alpha = 0.31f * (1f - step / (limit + 1f));
            Vector2 position = previous + projectile.Size * 0.5f - Main.screenPosition;
            float rotation = step < projectile.oldRot.Length ? projectile.oldRot[step] : projectile.rotation;
            Main.EntitySpriteDraw(texture, position, null, glow * alpha, rotation,
                origin, projectile.scale * (1f - step * 0.065f), SpriteEffects.None, 0);
        }
    }
}
