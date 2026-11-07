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
    // Prevent the final wave from softlocking if the summoned mini-boss despawns.
    private const int FinalRespawnDelay = 600;
    public static void ResetRuntime() { spawnTimer = 0; noticeWave = 0; }

    public static int RequiredKills => UmbraWorld.NightInvasionWave switch { 1 => 8, 2 => 12, 3 => 16, _ => 0 };

    // The beacon is consumed only after an authoritative start succeeds.
    // This prevents clients from starting the invasion by sending a packet
    // without owning the summon item, and avoids double consumption.
    public static bool TryStartWithBeacon(Player player)
    {
        if (!CanStart(player))
            return false;

        // Consume exactly the selected beacon and synchronize that inventory slot.
        if (!BeaconConsumption.TryConsumeHeld(player, ModContent.ItemType<NightInvasionBeacon>()))
            return false;

        BeginInvasion();
        return true;
    }

    private static bool CanStart(Player player) =>
        Main.netMode != NetmodeID.MultiplayerClient && player != null && player.active && !player.dead &&
        !Main.dayTime && !UmbraWorld.NightInvasionActive && UmbraWorld.DownedSentinel;

    private static void BeginInvasion()
    {
        UmbraWorld.NightInvasionActive = true;
        UmbraWorld.NightInvasionWave = 1;
        UmbraWorld.NightInvasionKills = 0;
        spawnTimer = 0;
        noticeWave = 0;
        Sync();
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
            ResetRuntime();
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
            if (HasTaggedFinalBoss())
            {
                // Keep the cooldown armed while the boss is alive.
                spawnTimer = FinalRespawnDelay;
                return;
            }
            // If the boss leaves the world without dying, spawn another after a delay.
            // A legitimate defeat completes the invasion in RecordKill before this runs.
            if (spawnTimer > 0) { spawnTimer--; return; }
            Player target = FindPlayer();
            if (target == null) return;
            // Spawn a tagged invasion boss so independently summoned Devourers
            // cannot accidentally complete the event.
            int direction = Main.rand.NextBool() ? 1 : -1;
            int bossX = System.Math.Clamp((int)target.Center.X + direction * 650, 200, Main.maxTilesX * 16 - 200);
            int bossY = System.Math.Clamp((int)target.Center.Y - 180, 200, Main.maxTilesY * 16 - 200);
            SpawnInvasionNPC(bossX, bossY, ModContent.NPCType<ShardDevourer>(), 4);
            spawnTimer = FinalRespawnDelay;
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
        {
            NPC npc = Main.npc[i];
            // Enforce the cap for the CURRENT wave's NPCs, not free-roaming
            // biome monsters or survivors from the previous event wave.
            if (npc.active && npc.GetGlobalNPC<NightInvasionKillTracker>().SpawnedWave == wave &&
                npc.type == (wave == 1 ? wisp : wave == 2 ? mite : crawler) &&
                Vector2.Distance(npc.Center, player.Center) < 1100f)
                nearby++;
        }
        if (nearby >= 7) return;
        int type = wave == 1 ? wisp : wave == 2 ? mite : crawler;
        int direction = Main.rand.NextBool() ? -1 : 1;
        Vector2 spot = player.Center + new Vector2(direction * Main.rand.Next(500, 700), -100);
        SpawnInvasionNPC((int)spot.X, (int)spot.Y, type, wave);
    }

    private static void SpawnInvasionNPC(int x, int y, int type, int sourceWave)
    {
        int index = NPC.NewNPC(new EntitySource_WorldEvent(), x, y, type);
        if (index < 0 || index >= Main.maxNPCs || !Main.npc[index].active)
            return;

        // Mark on the authority. Clients do not need this private progress flag.
        Main.npc[index].GetGlobalNPC<NightInvasionKillTracker>().SpawnedWave = sourceWave;
        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.SyncNPC, number: index);
    }

    private static bool HasTaggedFinalBoss()
    {
        int bossType = ModContent.NPCType<ShardDevourer>();
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC npc = Main.npc[i];
            if (npc.active && npc.type == bossType &&
                npc.GetGlobalNPC<NightInvasionKillTracker>().SpawnedWave == 4)
                return true;
        }
        return false;
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
        // Never award event progress for natural spawns, manual summons,
        // or leftovers from an earlier wave.
        if (npc.GetGlobalNPC<NightInvasionKillTracker>().SpawnedWave != wave)
            return;
        if (wave == 4 && npc.type == ModContent.NPCType<ShardDevourer>())
        {
            UmbraWorld.NightInvasionActive = false;
            UmbraWorld.DownedNightInvasion = true;
            // One crest per completed invasion; dropped in-world by the authority.
            Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<InvasionCrest>(), 1);
            Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<VoidShard>(), Main.rand.Next(15, 26));
            UmbraWorld.NightInvasionWave = 0;
            UmbraWorld.NightInvasionKills = 0;
            ResetRuntime();
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
    // Each spawned NPC must have its own progress marker.
    public override bool InstancePerEntity => true;
    public int SpawnedWave;

    // NPC slots are reused. Reset the marker for any non-event spawn before
    // SpawnInvasionNPC tags the specially spawned enemies on the authority.
    public override void OnSpawn(NPC npc, IEntitySource source) => SpawnedWave = 0;

    public override void OnKill(NPC npc) => NightInvasionSystem.RecordKill(npc);
}
