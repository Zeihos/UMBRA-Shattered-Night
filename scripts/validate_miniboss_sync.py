#!/usr/bin/env python3
"""UMBRA mini-boss targeting and radial warnings: source-level regression only.

This does not simulate Terraria, network timing or actual multiplayer play.
"""
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
def text(path):
    return (ROOT / path).read_text(encoding="utf-8")

target = text("Content/Systems/UMBRAEnemyTargeting.cs")
assert "Main.netMode == NetmodeID.MultiplayerClient" in target
assert "npc.TargetClosest(false)" in target
assert target.index("Main.netMode == NetmodeID.MultiplayerClient") < target.index("npc.TargetClosest(false)")
assert "npc.netUpdate = true;" in target
assert "Vector2.DistanceSquared(candidate.Center, npc.Center) <= maxDistance * maxDistance" in target
assert "candidate.active && !candidate.dead" in target

for name in ("AstralHeart", "AshenColossus", "ShardDevourer"):
    code = text(f"Content/NPCs/{name}.cs")
    assert "UMBRAEnemyTargeting.TryGetTarget(NPC, out Player" in code, name
    assert "if (Main.netMode != NetmodeID.MultiplayerClient)" in code, name
    assert "NPC.EncourageDespawn(10);" in code, name
    assert "NPC.TargetClosest(false)" not in code, name

astral = text("Content/NPCs/AstralHeart.cs")
assert "Main.netMode != NetmodeID.MultiplayerClient && secondPhase" in astral
assert "NPC.ai[1] = 45f;" in astral
assert "if (NPC.ai[1] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)" in astral
assert "if (NPC.ai[1] > 0f) pace +=" in astral
assert astral.index("NPC.ai[1] = 45f;") < astral.index("NPC.ai[1]--;")
assert astral.index("NPC.ai[1]--;") < astral.index("Projectile.NewProjectile(")

ashen = text("Content/NPCs/AshenColossus.cs")
fx = text("Content/Systems/UMBRAVisualFX.cs")
assert "NPC.ai[2] = 45f;" in ashen
assert "NPC.ai[2]--;" in ashen
assert "NPC.ai[1] = 17f;" in ashen
assert ashen.index("if (NPC.ai[2] > 0f)") < ashen.index("if (NPC.ai[1] > 0f)")
assert ashen.index("if (NPC.ai[1] > 0f)") < ashen.index("if (Main.netMode == NetmodeID.MultiplayerClient)")
assert ashen.index("if (Main.netMode == NetmodeID.MultiplayerClient)") < ashen.index("NPC.ai[2] = 45f;")
assert "NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient" in ashen
assert "npc.ai[2] / 45f" in fx
assert "if (npc.type == ModContent.NPCType<AshenColossus>() && npc.ai[2] > 0f)" in fx
assert "warningStart = cycle - 65" not in fx
print("Mini-boss targeting, server-only radial charges and warning visuals: source-level checks passed.")
