#!/usr/bin/env python3
"""Source-level boss summon concurrency and minimum progression loot checks.

Not a Terraria runtime or multiplayer simulation.
"""
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
def source(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8")

system = source("Content/Systems/BossSummoningSystem.cs")
inv = source("Content/Systems/NightInvasionSystem.cs")
world = source("Content/Systems/UmbraWorld.cs")
fracture = source("Content/Items/FractureBeacon.cs")
inv_beacon = source("Content/Items/NightInvasionBeacon.cs")

for token in ("lastSuccessfulSummonTick = ulong.MaxValue",
              "lastSuccessfulSummonTick != (ulong)Main.GameUpdateCount",
              "lastSuccessfulSummonTick = (ulong)Main.GameUpdateCount",
              "HasFreeNpcSlot()", "NPC.AnyNPCs(bossType)",
              "HasActiveUmbraBoss()", "CanSummonDevourer(Player player)",
              "TrySummonDevourer(Player player)"):
    assert token in system, f"Missing summon protection: {token}"
assert system.count("NPC.SpawnOnPlayer(") == 2

for entry in ("internal static bool TrySummon(Player player, int itemType)",
              "internal static bool TrySummonDevourer(Player player)"):
    body = system.split(entry, 1)[1].split("\n    }", 1)[0]
    assert body.index("CanProcessSummon()") < body.index("BeaconConsumption.TryConsumeHeld(")
    assert body.index("BeaconConsumption.TryConsumeHeld(") < body.index("lastSuccessfulSummonTick = (ulong)Main.GameUpdateCount;")
    assert body.index("lastSuccessfulSummonTick = (ulong)Main.GameUpdateCount;") < body.index("NPC.SpawnOnPlayer(")

assert world.count("BossSummoningSystem.ResetRuntime()") == 2
assert "BossSummoningSystem.TrySummonDevourer(player)" in fracture
assert "BossSummoningSystem.CanSummonDevourer(player)" in fracture
assert "!BossSummoningSystem.HasActiveUmbraBoss()" in inv
assert "!BossSummoningSystem.HasActiveUmbraBoss()" in inv_beacon

for name in ("EclipseSentinel","ShardDevourer","RiftArchon","AstralSovereign","AshenOverlord"):
    assert f"ModContent.NPCType<{name}>()" in system

def drop_min(npc: str, resource: str) -> int:
    text = source(f"Content/NPCs/{npc}.cs")
    match = re.search(rf"ItemDropRule\.Common\(ModContent\.ItemType<{resource}>\(\),\s*1,\s*(\d+),\s*(\d+)\)", text)
    assert match, f"Missing guaranteed loot: {npc}, {resource}"
    low, high = map(int, match.groups())
    assert 0 < low <= high
    return low

def cost(item: str, resource: str) -> int:
    text = source(f"Content/Items/{item}.cs")
    match = re.search(rf"\.AddIngredient<{resource}>\((\d+)\)", text)
    assert match, f"Missing recipe material: {item}, {resource}"
    return int(match[1])

assert drop_min("EclipseSentinel","EclipseCore") >= cost("NightInvasionBeacon","EclipseCore") + cost("ArchonBeacon","EclipseCore")
assert 2 * drop_min("AstralHeart","AstralHeartFragment") >= cost("SovereignBeacon","AstralHeartFragment")
assert 2 * drop_min("AshenColossus","AshenCore") >= cost("OverlordEmberSeal","AshenCore")
print("Boss summon guard and guaranteed progression loot checked (source-level only).")
