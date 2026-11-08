#!/usr/bin/env python3
"""Source-level regression checks for the UMBRA 0.66 combat VFX hooks.

This is intentionally static: a successful check does not replace in-game tests.
"""
from pathlib import Path

root = Path(__file__).resolve().parents[1]
impact = (root / "Content/Systems/UMBRAImpactVisuals.cs").read_text(encoding="utf-8")
trail = (root / "Content/Systems/UMBRAVisualFX.cs").read_text(encoding="utf-8")

for symbol in (
    "override void OnHitNPC(Projectile projectile",
    "override void OnHitPlayer(Projectile projectile",
    "override void OnKill(Projectile projectile",
    "override void HitEffect(NPC npc",
    "override Color? GetAlpha(NPC npc",
    "Main.dedServ",
    "projectile.owner != Main.myPlayer",
    "_hitTarget",
    "npc.life <= 0",
):
    assert symbol in impact, f"Missing impact VFX guard or hook: {symbol}"
for name in ("CinderEmber", "ColossusCinder", "AstralPulse", "SovereignArcaneOrb",
             "SovereignBladeWave", "UmbralBolt", "NullSpark", "ArchonArcaneShot"):
    assert f"ProjectileType<{name}>" in impact and f"ProjectileType<{name}>" in trail, name
for banned in ("Projectile.NewProjectile", "NPC.NewNPC", "NetMessage.SendData"):
    assert banned not in impact, f"Gameplay-side effect in visual-only code: {banned}"
assert "version = 0.66.0" in (root / "build.txt").read_text(encoding="utf-8")
print("UMBRA 0.66 combat VFX static checks: OK")
