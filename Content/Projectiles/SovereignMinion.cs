using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Buffs;
namespace UmbraShatteredNight.Content.Projectiles;
public class SovereignMinion : ModProjectile {
    public override void SetDefaults() {
        Projectile.width = 28; Projectile.height = 28;
        Projectile.friendly = true; Projectile.minion = true; Projectile.minionSlots = 1f;
        Projectile.DamageType = DamageClass.Summon; Projectile.penetrate = -1;
        Projectile.tileCollide = false; Projectile.ignoreWater = true; Projectile.timeLeft = 18000;
        Projectile.usesLocalNPCImmunity = true; Projectile.localNPCHitCooldown = 22;
    }
    public override bool? CanCutTiles() => false;
    public override bool MinionContactDamage() => true;
    public override void AI() {
        Player owner = Main.player[Projectile.owner];
        if (!owner.active || owner.dead) { Projectile.Kill(); return; }
        if (!owner.HasBuff(ModContent.BuffType<SovereignMinionBuff>())) {
            Projectile.Kill();
            return;
        }
        Projectile.timeLeft = 2;
        NPC target = null; float maxDistance = 650f;
        if (owner.HasMinionAttackTargetNPC && owner.MinionAttackTargetNPC >= 0 && owner.MinionAttackTargetNPC < Main.maxNPCs) {
            NPC marked = Main.npc[owner.MinionAttackTargetNPC];
            if (marked.CanBeChasedBy() && Vector2.Distance(owner.Center, marked.Center) < 1000f) target = marked;
        }
        if (target == null) foreach (NPC candidate in Main.npc) {
            if (!candidate.CanBeChasedBy()) continue;
            float distance = Vector2.Distance(candidate.Center, Projectile.Center);
            if (distance < maxDistance) { maxDistance = distance; target = candidate; }
        }
        Vector2 destination = target != null ? target.Center : owner.Center + new Vector2(-48f * owner.direction, -65f);
        Vector2 delta = destination - Projectile.Center;
        float speed = target != null ? 14f : 9f;
        if (delta.LengthSquared() > 20f * 20f) Projectile.velocity = Vector2.Lerp(Projectile.velocity, delta.SafeNormalize(Vector2.Zero) * speed, .10f);
        else Projectile.velocity *= .89f;
        if (Vector2.DistanceSquared(Projectile.Center, owner.Center) > 1500f * 1500f) {
            Projectile.Center = owner.Center; Projectile.netUpdate = true;
        }
        Projectile.rotation += .045f;
        Lighting.AddLight(Projectile.Center, .25f, .10f, .37f);
        if (!Main.dedServ && Main.rand.NextBool(9)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
    }
}
