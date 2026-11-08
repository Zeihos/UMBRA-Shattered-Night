using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Projectiles;
public class CinderEmber : ModProjectile {
    // Cache actual movement history for accurate client-side afterimages.
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

 public override void SetDefaults() {
  Projectile.width=14; Projectile.height=14; Projectile.friendly=true;
  Projectile.hostile=false; Projectile.DamageType=DamageClass.Magic;
  Projectile.penetrate=2; Projectile.timeLeft=180; Projectile.tileCollide=true;
  Projectile.ignoreWater=true;
 }
 public override void AI() {
  Projectile.rotation += 0.19f;
  Lighting.AddLight(Projectile.Center,0.8f,0.3f,0.06f);
  if (!Main.dedServ && Main.rand.NextBool(3)) {
   int d=Dust.NewDust(Projectile.position,Projectile.width,Projectile.height,DustID.Torch);
   Main.dust[d].noGravity=true; Main.dust[d].velocity *= 0.3f;
  }
 }
 public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
  target.AddBuff(BuffID.OnFire3,180);
 }
}
