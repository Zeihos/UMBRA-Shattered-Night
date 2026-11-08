#!/usr/bin/env python3
"""Check animation metadata and exact per-frame texture geometry. No Pillow needed."""
from pathlib import Path
import re, struct, sys
root=Path(__file__).resolve().parents[1]
expected={
    "AshenColossus":(54,56,6), "AshenOverlord":(96,96,6),
    "AstralHeart":(50,52,6), "AstralSovereign":(96,96,6),
    "EclipseSentinel":(76,76,6), "RiftArchon":(82,82,6),
    "ShardDevourer":(48,48,6), "NightWisp":(32,32,4),
}
errors=[]
for name,(w,h,frames) in expected.items():
    cs=(root/'Content/NPCs'/f'{name}.cs').read_text(encoding='utf8')
    img=(root/'Content/NPCs'/f'{name}.png').read_bytes()
    if img[:8]!=b'\x89PNG\r\n\x1a\n': errors.append(f'{name}: broken PNG');continue
    rw,rh=struct.unpack('>II', img[16:24])
    if (rw,rh)!=(w,h*frames):errors.append(f'{name}: expected {(w,h*frames)}, found {(rw,rh)}')
    if not re.search(r'Main\.npcFrameCount\[Type\]\s*=\s*'+str(frames)+r'\b',cs):
        errors.append(f'{name}: incorrect Main.npcFrameCount')
    if 'FindFrame' not in cs:errors.append(f'{name}: FindFrame missing')
    if rh%frames:errors.append(f'{name}: frame sheet does not divide evenly')
if errors:
    print('\n'.join(errors),file=sys.stderr);sys.exit(1)
print(f'Animation geometry validated for {len(expected)} NPCs.')
