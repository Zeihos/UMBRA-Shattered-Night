using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Projectiles;

// Player-only projectile. Never reuse the hostile Archon/UmbralBolt projectile for player weapons.
public class ArchonArcaneShot : ModProjectile
{
    // Cache actual movement history for accurate client-side afterimages.
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = 2;
        Projectile.timeLeft = 180;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
    }

    public override void AI()
    {
        Projectile.rotation += 0.17f;
        Lighting.AddLight(Projectile.Center, 0.5f, 0.12f, 0.72f);
        if (!Main.dedServ && Main.rand.NextBool(3))
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;
        }
    }
}
