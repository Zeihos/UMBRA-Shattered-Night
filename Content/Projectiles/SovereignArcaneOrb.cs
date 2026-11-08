using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Projectiles;

// Distinct magic projectile with limited target seeking, separate from hostile boss attacks.
public class SovereignArcaneOrb : ModProjectile
{
    // Cache actual movement history for accurate client-side afterimages.
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults()
    {
        Projectile.width = 18;
        Projectile.height = 18;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = 2;
        Projectile.timeLeft = 150;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 20;
    }

    public override void AI()
    {
        Projectile.rotation += 0.2f;
        Lighting.AddLight(Projectile.Center, 0.4f, 0.18f, 0.75f);
        NPC closest = null;
        float maxDistanceSq = 300f * 300f;
        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (!npc.CanBeChasedBy(Projectile)) continue;
            float distanceSq = Vector2.DistanceSquared(npc.Center, Projectile.Center);
            if (distanceSq < maxDistanceSq && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
            {
                maxDistanceSq = distanceSq;
                closest = npc;
            }
        }
        if (closest != null)
        {
            Vector2 desired = (closest.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 15f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.045f);
        }
        if (!Main.dedServ && Main.rand.NextBool(4))
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;
        }
    }
}
