using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Buffs;

namespace UmbraShatteredNight.Content.Projectiles;

public class OverlordInfernoMinion : ModProjectile {
    public override void SetDefaults() {
        Projectile.width = 30; Projectile.height = 30;
        Projectile.friendly = true; Projectile.minion = true; Projectile.minionSlots = 1f;
        Projectile.DamageType = DamageClass.Summon; Projectile.penetrate = -1;
        Projectile.tileCollide = false; Projectile.ignoreWater = true; Projectile.timeLeft = 2;
        Projectile.usesLocalNPCImmunity = true; Projectile.localNPCHitCooldown = 18;
    }
    public override bool? CanCutTiles() => false;
    public override bool MinionContactDamage() => true;
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(BuffID.OnFire3, 240);
    }
    public override void AI() {
        Player owner = Main.player[Projectile.owner];
        if (!owner.active || owner.dead || !owner.HasBuff(ModContent.BuffType<OverlordInfernoBuff>())) {
            Projectile.Kill(); return;
        }
        Projectile.timeLeft = 2;
        NPC target = null;
        if (owner.HasMinionAttackTargetNPC && owner.MinionAttackTargetNPC >= 0 && owner.MinionAttackTargetNPC < Main.maxNPCs) {
            NPC marked = Main.npc[owner.MinionAttackTargetNPC];
            if (marked.CanBeChasedBy() && Vector2.DistanceSquared(owner.Center, marked.Center) < 1000f * 1000f)
                target = marked;
        }
        if (target == null) {
            float closest = 700f * 700f;
            foreach (NPC npc in Main.npc) {
                if (!npc.CanBeChasedBy()) continue;
                float distance = Vector2.DistanceSquared(Projectile.Center, npc.Center);
                if (distance < closest) { closest = distance; target = npc; }
            }
        }
        Vector2 destination = target != null ? target.Center : owner.Center + new Vector2(-56f * owner.direction, -70f);
        Vector2 direction = destination - Projectile.Center;
        float speed = target != null ? 17f : 10f;
        if (direction.LengthSquared() > 18f * 18f)
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction.SafeNormalize(Vector2.Zero) * speed, .13f);
        else Projectile.velocity *= .83f;
        if (Vector2.DistanceSquared(Projectile.Center, owner.Center) > 1450f * 1450f && Projectile.owner == Main.myPlayer) {
            Projectile.Center = owner.Center;
            Projectile.netUpdate = true;
        }
        Projectile.rotation += .065f;
        Lighting.AddLight(Projectile.Center, 1f, .36f, .06f);
        if (Main.rand.NextBool(7)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
    }
}
