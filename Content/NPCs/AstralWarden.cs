using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.NPCs;

public class AstralWarden : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = 34;
        NPC.height = 32;
        NPC.lifeMax = 420;
        NPC.damage = 58;
        NPC.defense = 20;
        NPC.knockBackResist = 0.2f;
        NPC.value = Item.buyPrice(silver: 35);
        NPC.aiStyle = 2;
        AIType = NPCID.DemonEye;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.HitSound = SoundID.NPCHit3;
        NPC.DeathSound = SoundID.NPCDeath6;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return UmbraWorld.DownedArchon && !spawnInfo.PlayerSafe &&
            spawnInfo.Player.InModBiome<AstralRift>() ? 0.09f : 0f;
    }

    public override void AI()
    {
        Lighting.AddLight(NPC.Center, 0.22f, 0.12f, 0.43f);
        NPC.rotation = NPC.velocity.X * 0.025f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AstralOreItem>(), 2, 1, 3));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AstralGlassItem>(), 1, 2, 5));
    }
}
