#!/usr/bin/env python3
"""Public documentation and repository hygiene check, not an in-game test."""
from pathlib import Path
import re, subprocess
root=Path(__file__).resolve().parents[1]
metadata=(root/'build.txt').read_text(encoding='utf-8')
match=re.search(r'^version\s*=\s*(\d+\.\d+\.\d+)\s*$',metadata,re.M)
assert match, 'Missing build.txt version'
version=match.group(1)
for name in ('README.md','CHANGELOG.md','docs/INSTALL.md','docs/DEVELOPMENT.md',
             'docs/ROADMAP.md','docs/TESTING.md'):
    assert (root/name).is_file(), f'Missing documentation: {name}'
assert version in (root/'README.md').read_text(encoding='utf-8')
assert f'## {version} ' in (root/'CHANGELOG.md').read_text(encoding='utf-8')
assert version in (root/'docs/ROADMAP.md').read_text(encoding='utf-8')
for old in ('scripts/visual_polish.py','scripts/.visual-063-applied',
            '.github/workflows/visual-polish.yml'):
    assert not (root/old).exists(), f'Obsolete asset: {old}'
if (root/'.git').exists():
    tracked=subprocess.run(['git','ls-files','-z'],cwd=root,check=True,capture_output=True).stdout
    for raw in tracked.split(b'\0'):
        if not raw: continue
        path=Path(raw.decode('utf-8'))
        assert path.suffix not in ('.tmod','.pyc') and '__pycache__' not in path.parts, f'Generated file committed: {path}'
print(f'Repository metadata and documentation validated (v{version})')
