using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Projectiles;
public class VoidMinion : ModProjectile {
 public override void SetDefaults() { Projectile.width=24; Projectile.height=24; Projectile.friendly=true; Projectile.minion=true; Projectile.minionSlots=1f; Projectile.DamageType=DamageClass.Summon; Projectile.penetrate=-1; Projectile.tileCollide=false; Projectile.ignoreWater=true; Projectile.timeLeft=18000; }
 public override bool? CanCutTiles()=>false;
 public override void AI() {
  Player owner=Main.player[Projectile.owner];
  if(!owner.active || owner.dead) { Projectile.Kill(); return; }
  if(owner.HasBuff(ModContent.BuffType<UmbraShatteredNight.Content.Buffs.VoidMinionBuff>())) Projectile.timeLeft=2;
  NPC target=null; float distance=560f;
  foreach(NPC npc in Main.npc) if(npc.active && npc.CanBeChasedBy()) { float d=Vector2.Distance(Projectile.Center,npc.Center); if(d<distance) {distance=d;target=npc;} }
  Vector2 destination=target!=null?target.Center:owner.Center+new Vector2(owner.direction*48f,-58f);
  Vector2 direction=destination-Projectile.Center;
  float speed=target!=null?10f:8f;
  if(direction.Length()>16f) Projectile.velocity=Vector2.Lerp(Projectile.velocity,direction.SafeNormalize(Vector2.Zero)*speed,.09f);
  else Projectile.velocity*=.9f;
  if(Vector2.DistanceSquared(Projectile.Center,owner.Center)>1600f*1600f) Projectile.Center=owner.Center;
  Projectile.rotation+=.06f;
  Lighting.AddLight(Projectile.Center,.18f,.04f,.28f);
 }
 public override bool MinionContactDamage()=>true;
}
