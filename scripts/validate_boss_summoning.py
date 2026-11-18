#!/usr/bin/env python3
"""Source-level regression checks for authoritative multiplayer boss summoning.

These tests do not simulate a Terraria server; GitHub Actions also compiles
the full mod against its pinned tModLoader release.
"""
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
def read(name: str) -> str:
    return (ROOT / name).read_text(encoding="utf-8")

system = read("Content/Systems/BossSummoningSystem.cs")
packets = read("UmbraShatteredNight.cs")
consume = read("Content/Systems/BeaconConsumption.cs")
bosses = [
    ("NightSigil", "RequestSentinelSummon", "EclipseSentinel", None),
    ("ArchonBeacon", "RequestArchonSummon", "RiftArchon", "DownedNightInvasion"),
    ("SovereignBeacon", "RequestSovereignSummon", "AstralSovereign", "DownedAstralHeart"),
    ("OverlordEmberSeal", "RequestOverlordSummon", "AshenOverlord", "DownedAshenColossus"),
]
assert "Main.netMode == NetmodeID.MultiplayerClient || !CanSummon(player, itemType)" in system
assert "Main.dayTime || UmbraWorld.NightInvasionActive" in system
assert "NPC.AnyNPCs(bossType)" in system
assert "BeaconConsumption.IsHolding(player, itemType)" in system
assert "BeaconConsumption.TryConsumeHeld(player, itemType)" in system
assert system.index("BeaconConsumption.TryConsumeHeld(player, itemType)") < system.index("NPC.SpawnOnPlayer(player.whoAmI, bossType)")
assert "itemType == ModContent.ItemType<NightSigil>()" in system
assert "Main.netMode != NetmodeID.Server || whoAmI < 0" in packets
assert "player.selectedItem" in consume and "MessageID.SyncEquipment" in consume

for item, packet, npc, gate in bosses:
    code = read(f"Content/Items/{item}.cs")
    assert re.search(r"Item\.consumable\s*=\s*false\s*;", code), item
    assert "BossSummoningSystem.CanSummon(player, Type)" in code, item
    assert "BossSummoningSystem.TrySummon(player, Type)" in code, item
    assert f"PacketType.{packet}" in code, item
    assert f"case PacketType.{packet}:" in packets, item
    assert f"BossSummoningSystem.TrySummon(player, ModContent.ItemType<{item}>())" in packets, item
    assert f"ModContent.NPCType<{npc}>()" in system, npc
    assert "MessageID.SpawnBossUseLicenseStartEvent" not in code, item
    assert "NPC.SpawnOnPlayer" not in code, item
    if gate:
        assert f"UmbraWorld.{gate}" in system, gate

print("Four boss beacons: authority, progression, inventory consumption and packet mapping passed (source-level).")
