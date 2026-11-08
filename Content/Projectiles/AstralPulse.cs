using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Projectiles;
public class AstralPulse : ModProjectile
{
    // Cache actual movement history for accurate client-side afterimages.
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults()
    {
        Projectile.width = 14; Projectile.height = 14;
        Projectile.hostile = true; Projectile.friendly = false;
        Projectile.penetrate = 1; Projectile.timeLeft = 170;
        Projectile.tileCollide = true; Projectile.ignoreWater = true;
    }
    public override void AI()
    {
        Projectile.rotation += 0.12f;
        Lighting.AddLight(Projectile.Center, 0.36f, 0.12f, 0.55f);
        if (!Main.dedServ && Main.rand.NextBool(4))
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
    }
}
