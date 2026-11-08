using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Projectiles;
public class NullSpark : ModProjectile {
    // Cache actual movement history for accurate client-side afterimages.
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

 public override void SetDefaults() { Projectile.width=12; Projectile.height=12; Projectile.friendly=true;
 Projectile.DamageType=DamageClass.Magic; Projectile.penetrate=1; Projectile.timeLeft=150;
 Projectile.ignoreWater=true; Projectile.tileCollide=true; }
 public override void AI() { Projectile.rotation+=0.12f; Lighting.AddLight(Projectile.Center,0.25f,0.06f,0.4f); }
 public override void OnKill(int timeLeft) {
 if (!Main.dedServ)
  for(int i=0;i<8;i++) Dust.NewDust(Projectile.position,Projectile.width,Projectile.height,DustID.PurpleTorch);
 }
}
