using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Projectiles;
public class UmbralBolt : ModProjectile
{
    // Cache actual movement history for accurate client-side afterimages.
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults() {
        Projectile.width = 14; Projectile.height = 14;
        Projectile.hostile = true; Projectile.friendly = false;
        Projectile.penetrate = 1; Projectile.timeLeft = 240;
        Projectile.tileCollide = true; Projectile.ignoreWater = true;
    }
    public override void AI() {
        Projectile.rotation += 0.16f;
        Lighting.AddLight(Projectile.Center, 0.32f, 0.06f, 0.5f);
        if (!Main.dedServ && Main.rand.NextBool(3)) {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.25f;
        }
    }
}
