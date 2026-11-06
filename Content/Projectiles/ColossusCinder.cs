using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Projectiles;

// Hostile projectile used only by the Ashen Colossus.
public class ColossusCinder : ModProjectile {
 public override string Texture => "Terraria/Images/Projectile_85";
 public override void SetDefaults() {
  Projectile.width = 14; Projectile.height = 14;
  Projectile.hostile = true; Projectile.friendly = false;
  Projectile.penetrate = 1; Projectile.timeLeft = 180;
  Projectile.tileCollide = true; Projectile.ignoreWater = true;
 }
 public override void AI() {
  Projectile.rotation += 0.12f;
  Lighting.AddLight(Projectile.Center, 0.9f, 0.3f, 0.03f);
  if (Main.rand.NextBool(3)) {
   int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
   Main.dust[dust].noGravity = true;
  }
 }
 public override void OnHitPlayer(Player target, Player.HurtInfo info) {
  target.AddBuff(BuffID.OnFire3, 120);
 }
}
