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
        Main.EntitySpriteDraw(texture, center, npc.frame, tint * glow, npc.rotation, origin, npc.scale * 1.08f, flip, 0);
        return true;
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
        Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
        for (int step = 1; step <= 3; step++)
        {
            Vector2 position = projectile.Center - projectile.velocity * (step * 0.9f) - Main.screenPosition;
            float alpha = 0.22f / step;
            Main.EntitySpriteDraw(texture, position, null, glow * alpha, projectile.rotation,
                origin, projectile.scale * (1.0f - step * 0.17f), SpriteEffects.None, 0);
        }
    }
}
