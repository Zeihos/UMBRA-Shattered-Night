using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
using UmbraShatteredNight.Content.Items;

namespace UmbraShatteredNight.Content.Systems;

public class NightInvasionSystem : ModSystem
{
    private static int spawnTimer;
    private static int noticeWave;
    public static void ResetRuntime() { spawnTimer = 0; noticeWave = 0; }

    public static int RequiredKills => UmbraWorld.NightInvasionWave switch { 1 => 8, 2 => 12, 3 => 16, _ => 0 };

    // The beacon is consumed only after an authoritative start succeeds.
    // This prevents clients from starting the invasion by sending a packet
    // without owning the summon item, and avoids double consumption.
    public static bool TryStartWithBeacon(Player player)
    {
        if (player == null || !player.active || player.dead ||
            player.HeldItem.type != ModContent.ItemType<NightInvasionBeacon>() ||
            player.HeldItem.stack <= 0)
            return false;

        if (!TryStart(player))
            return false;

        player.ConsumeItem(ModContent.ItemType<NightInvasionBeacon>());
        return true;
    }

    public static bool TryStart(Player player)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient || Main.dayTime || UmbraWorld.NightInvasionActive || !UmbraWorld.DownedSentinel || !player.active || player.dead)
            return false;
        UmbraWorld.NightInvasionActive = true;
        UmbraWorld.NightInvasionWave = 1;
        UmbraWorld.NightInvasionKills = 0;
        spawnTimer = 0;
        noticeWave = 0;
        Sync();
        return true;
    }

    public static void Sync()
    {
        if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
    }

    public override void PostUpdateWorld()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient || !UmbraWorld.NightInvasionActive) return;
        if (Main.dayTime)
        {
            UmbraWorld.NightInvasionActive = false;
            UmbraWorld.NightInvasionWave = 0;
            UmbraWorld.NightInvasionKills = 0;
            Sync();
            return;
        }
        int wave = UmbraWorld.NightInvasionWave;
        if (wave != noticeWave)
        {
            noticeWave = wave;
            if (Main.netMode == NetmodeID.Server)
                Terraria.Chat.ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral(wave == 4 ? "UMBRA: Final wave - Shard Devourer!" : $"UMBRA: Night Invasion - wave {wave}/3"), new Color(187, 112, 240));
            else Main.NewText(wave == 4 ? "UMBRA: Final wave - Shard Devourer!" : $"UMBRA: Night Invasion - wave {wave}/3", 187, 112, 240);
        }
        if (wave == 4)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<ShardDevourer>())) return;
            // The boss is spawned only once. A final-wave boss kill completes the event through GlobalNPC.
            if (spawnTimer == -1) return;
            Player target = FindPlayer();
            if (target == null) return;
            NPC.SpawnOnPlayer(target.whoAmI, ModContent.NPCType<ShardDevourer>());
            spawnTimer = -1;
            return;
        }
        if (wave < 1 || wave > 3) return;
        if (++spawnTimer < 100) return;
        spawnTimer = 0;
        Player player = FindPlayer();
        if (player == null) return;
        int nearby = 0;
        int wisp = ModContent.NPCType<NightWisp>();
        int mite = ModContent.NPCType<RiftMite>();
        int crawler = ModContent.NPCType<AbyssCrawler>();
        for (int i = 0; i < Main.maxNPCs; i++)
            if (Main.npc[i].active && (Main.npc[i].type == wisp || Main.npc[i].type == mite || Main.npc[i].type == crawler) && Vector2.Distance(Main.npc[i].Center, player.Center) < 1100f) nearby++;
        if (nearby >= 7) return;
        int type = wave == 1 ? wisp : wave == 2 ? mite : crawler;
        int direction = Main.rand.NextBool() ? -1 : 1;
        Vector2 spot = player.Center + new Vector2(direction * Main.rand.Next(500, 700), -100);
        NPC.NewNPC(new EntitySource_WorldEvent(), (int)spot.X, (int)spot.Y, type);
    }

    private static Player FindPlayer()
    {
        for (int i = 0; i < Main.maxPlayers; i++)
            if (Main.player[i].active && !Main.player[i].dead && Main.player[i].ZoneOverworldHeight) return Main.player[i];
        return null;
    }

    public static void RecordKill(NPC npc)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient || !UmbraWorld.NightInvasionActive) return;
        int wave = UmbraWorld.NightInvasionWave;
        if (wave == 4 && npc.type == ModContent.NPCType<ShardDevourer>())
        {
            UmbraWorld.NightInvasionActive = false;
            UmbraWorld.DownedNightInvasion = true;
            // One crest per completed invasion; dropped in-world by the authority.
            Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<InvasionCrest>(), 1);
            Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<VoidShard>(), Main.rand.Next(15, 26));
            UmbraWorld.NightInvasionWave = 0;
            UmbraWorld.NightInvasionKills = 0;
            Sync();
            return;
        }
        int correct = wave == 1 ? ModContent.NPCType<NightWisp>() : wave == 2 ? ModContent.NPCType<RiftMite>() : wave == 3 ? ModContent.NPCType<AbyssCrawler>() : -1;
        if (npc.type != correct) return;
        UmbraWorld.NightInvasionKills++;
        if (UmbraWorld.NightInvasionKills >= RequiredKills)
        {
            UmbraWorld.NightInvasionKills = 0;
            UmbraWorld.NightInvasionWave++;
            spawnTimer = 0;
        }
        Sync();
    }
}

public class NightInvasionKillTracker : GlobalNPC
{
    public override void OnKill(NPC npc) => NightInvasionSystem.RecordKill(npc);
}
