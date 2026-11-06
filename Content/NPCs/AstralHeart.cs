using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.NPCs;
public class AstralHeart : ModNPC
{
    public override void SetStaticDefaults() { Main.npcFrameCount[Type] = 1; }
    public override void SetDefaults()
    {
        NPC.width = 50; NPC.height = 52; NPC.lifeMax = 1800;
        NPC.damage = 65; NPC.defense = 18; NPC.knockBackResist = 0f;
        NPC.value = Item.buyPrice(gold: 3);
        NPC.noGravity = true; NPC.noTileCollide = true;
        NPC.HitSound = SoundID.NPCHit5; NPC.DeathSound = SoundID.NPCDeath6;
        NPC.aiStyle = -1;
    }
    public override float SpawnChance(NPCSpawnInfo info)
    {
        if (!UmbraWorld.DownedArchon || info.PlayerSafe || !info.Player.InModBiome<AstralRift>()) return 0f;
        return NPC.AnyNPCs(Type) ? 0f : 0.004f;
    }
    public override void AI()
    {
        Player target = Main.player[NPC.target];
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers || !target.active || target.dead)
        {
            NPC.TargetClosest(false);
            target = Main.player[NPC.target];
            if (!target.active || target.dead) { NPC.velocity.Y -= 0.15f; NPC.EncourageDespawn(10); return; }
        }
        NPC.ai[0]++;
        float phaseSpeed = NPC.life < NPC.lifeMax / 2 ? 7f : 4.5f;
        Vector2 desired = target.Center + new Vector2((float)System.Math.Sin(NPC.ai[0] / 75f) * 190f, -145f);
        Vector2 offset = desired - NPC.Center;
        if (offset.Length() > 20f)
        {
            offset.Normalize();
            NPC.velocity = Vector2.Lerp(NPC.velocity, offset * phaseSpeed, 0.055f);
        }
        else NPC.velocity *= 0.93f;
        NPC.rotation += 0.012f;
        Lighting.AddLight(NPC.Center, 0.52f, 0.23f, 0.75f);
        if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] % (NPC.life < NPC.lifeMax / 2 ? 65 : 100) == 0)
        {
            Vector2 direction = target.Center - NPC.Center;
            if (direction.LengthSquared() < 0.01f) direction = Vector2.UnitY;
            direction.Normalize();
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 8f,
                ProjectileID.PurpleLaser, 30, 1f, Main.myPlayer);
        }
    }
    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<AstralHeartFragment>(), 1, 2, 4));
    }
}
