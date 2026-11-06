using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Projectiles;

// A piercing, short-lived wave fired from the Sovereign Greatblade.
public class SovereignBladeWave : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = 2;
        Projectile.timeLeft = 35;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 15;
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        Lighting.AddLight(Projectile.Center, 0.45f, 0.16f, 0.8f);
        if (Main.rand.NextBool(3))
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Main.dust[dust].noGravity = true;
        }
    }
}
