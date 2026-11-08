#!/usr/bin/env python3
"""Reproducible UMBRA alpha 0.63 pixel-art polish. Requires Pillow.

Run once on the original v0.62 assets. A version marker prevents recoloring twice.
Never resize sprites: NPC frames and equipment sheets must retain their geometry.
"""
from pathlib import Path
from PIL import Image, ImageDraw
import hashlib
import math

ROOT = Path(__file__).resolve().parents[1]
STAMP = ROOT / 'scripts' / '.visual-063-applied'
PALETTES = {
    'void': [(10,10,23),(24,19,46),(45,30,83),(70,46,122),(103,61,164),(149,96,205),(194,138,245),(234,197,255),(250,231,255)],
    'star': [(13,16,43),(32,30,82),(59,53,123),(92,76,173),(130,113,210),(171,159,238),(199,215,250),(230,233,255),(252,248,255)],
    'rift': [(12,10,36),(31,20,68),(59,34,114),(99,54,169),(143,79,218),(180,121,241),(214,178,255),(237,215,255),(255,243,255)],
    'ember':[(30,13,19),(58,22,23),(94,31,25),(137,45,25),(186,58,26),(232,89,35),(255,141,55),(255,194,98),(255,238,176)],
    'night':[(21,17,40),(51,28,66),(94,45,107),(135,64,144),(184,95,182),(222,133,219),(245,189,230),(255,222,241),(255,242,251)],
}

def theme(name):
    n = name.lower()
    if any(s in n for s in ('ashen','cinder','ember','overlord','colossus','inferno')): return 'ember'
    if any(s in n for s in ('astral','sovereign','heart','stellar')): return 'star'
    if any(s in n for s in ('rift','archon')): return 'rift'
    if any(s in n for s in ('eclipse','night','sentinel','invasion','convergence')): return 'night'
    return 'void'

def shade(image, name):
    """Nine-step shading with preserved alpha and a deterministic pixel facet pattern."""
    image = image.convert('RGBA'); w,h = image.size
    old = image.load(); result = Image.new('RGBA',(w,h)); pix = result.load()
    vals = [.2126*r+.7152*g+.0722*b for r,g,b,a in image.getdata() if a]
    if not vals: return result
    lo,hi = min(vals),max(vals); pal = PALETTES[theme(name)]
    for y in range(h):
        for x in range(w):
            r,g,b,a = old[x,y]
            if not a: continue
            lumin = .2126*r+.7152*g+.0722*b
            normal = (lumin-lo)/(hi-lo) if hi>lo else .6
            lit = .5*(1-x/max(1,w-1)) + .5*(1-y/max(1,h-1))
            facets = ((x*19+y*13+(x*y)%11)%17)/16
            level = .72*normal + .19*lit + .09*facets
            if hi>lo and normal<.13: level=min(.13,level)
            idx = min(8,max(0,round(8*level)))
            rr,gg,bb=pal[idx]
            if theme(name)=='star' and b>r*1.22 and normal>.55:
                rr=max(0,rr-15);gg=min(255,gg+15)
            pix[x,y]=(rr,gg,bb,a)
    return result

def armor_icon(name,size):
    w,h=size; c=w//2; y=h//2; p=PALETTES[theme(name)]
    out=Image.new('RGBA',size);d=ImageDraw.Draw(out)
    if 'Helm' in name or 'Hood' in name:
        d.polygon([(c-10,y-5),(c-7,y-11),(c+7,y-11),(c+10,y-5),(c+8,y+8),(c-8,y+8)],fill=p[0])
        d.polygon([(c-8,y-4),(c-5,y-9),(c+5,y-9),(c+8,y-4),(c+6,y+6),(c-6,y+6)],fill=p[3])
        d.line([(c-7,y-2),(c+7,y-2)],fill=p[6],width=2)
        d.line([(c-5,y+2),(c+5,y+2)],fill=p[1],width=3)
        d.polygon([(c,y-10),(c+4,y-6),(c,y-3),(c-4,y-6)],fill=p[7])
    elif 'Greaves' in name or 'Boots' in name:
        for xx in (c-10,c+1):
            d.polygon([(xx,y-9),(xx+9,y-9),(xx+8,y+6),(xx+10,y+9),(xx-1,y+9)],fill=p[0])
            d.polygon([(xx+2,y-7),(xx+7,y-7),(xx+6,y+7),(xx+1,y+7)],fill=p[3])
            d.line([(xx+3,y-5),(xx+6,y-5)],fill=p[7],width=2)
    else:
        d.polygon([(c-10,y-9),(c-4,y-12),(c+4,y-12),(c+10,y-9),(c+9,y+9),(c-9,y+9)],fill=p[0])
        d.polygon([(c-8,y-8),(c-3,y-10),(c+3,y-10),(c+8,y-8),(c+7,y+7),(c-7,y+7)],fill=p[3])
        for side in (-1,1):
            d.polygon([(c+side*7,y-9),(c+side*12,y-7),(c+side*12,y),(c+side*5,y-2)],fill=p[5],outline=p[7])
        d.polygon([(c,y-7),(c+5,y-1),(c,y+6),(c-5,y-1)],fill=p[5],outline=p[7])
        d.line([(c,y-5),(c,y+3)],fill=p[8],width=1)
    return out

def buff(name,size):
    w,h=size;c=w//2;y=h//2;p=PALETTES[theme(name)]
    o=Image.new('RGBA',size);d=ImageDraw.Draw(o)
    d.ellipse((3,3,w-4,h-4),fill=p[1],outline=p[6],width=2)
    d.ellipse((7,7,w-8,h-8),outline=p[4])
    if 'Inferno' in name:
        d.polygon([(c-6,y+7),(c-5,y-5),(c-2,y-3),(c,y-10),(c+5,y-4),(c+7,y+7)],fill=p[5],outline=p[0])
        d.polygon([(c-2,y+5),(c,y-3),(c+3,y+1),(c+2,y+6)],fill=p[8])
    elif 'Colossus' in name:
        d.polygon([(c-10,y-2),(c-7,y-8),(c+7,y-8),(c+10,y-2),(c+7,y+8),(c-7,y+8)],fill=p[4],outline=p[7])
        d.polygon([(c-3,y-3),(c+3,y-3),(c,y+6)],fill=p[8])
    elif 'Void' in name:
        d.ellipse((c-9,y-5,c+9,y+5),fill=p[0],outline=p[7])
        d.ellipse((c-4,y-4,c+4,y+4),fill=p[8])
    else:
        d.polygon([(c,y-10),(c+8,y),(c,y+10),(c-8,y)],fill=p[4],outline=p[8])
    return o

def weapon(name,size):
    w,h=size;p=PALETTES[theme(name)];o=Image.new('RGBA',size);d=ImageDraw.Draw(o)
    Q=lambda x,y:(round(x*w/40),round(y*h/40))
    if any(k in name for k in ('Greatblade','Greatsword')):
        d.polygon([Q(x,y) for x,y in [(13,29),(10,25),(26,7),(34,3),(31,13),(17,30)]],fill=p[1],outline=p[7])
        d.line([Q(15,27),Q(30,9)],fill=p[8],width=2)
        d.line([Q(7,35),Q(15,27)],fill=p[3],width=3)
        d.line([Q(8,24),Q(17,33)],fill=p[7],width=3)
        d.ellipse([Q(5,34),Q(9,38)],fill=p[8])
    elif any(k in name for k in ('Staff','Scepter')):
        d.line([Q(5,34),Q(27,7)],fill=p[1],width=4)
        d.line([Q(6,32),Q(26,8)],fill=p[5],width=2)
        kind=int(hashlib.sha256(name.encode()).hexdigest()[:2],16)%3
        if kind==0:
            d.polygon([Q(x,y) for x,y in [(25,10),(23,5),(28,1),(34,6),(30,13)]],fill=p[7],outline=p[1])
            d.polygon([Q(x,y) for x,y in [(27,9),(28,4),(31,7)]],fill=p[8])
        elif kind==1:
            d.ellipse([Q(23,1),Q(36,14)],fill=p[4],outline=p[8],width=2)
            d.ellipse([Q(27,4),Q(32,10)],fill=p[8])
        else:
            d.polygon([Q(x,y) for x,y in [(24,10),(27,2),(34,3),(37,11),(29,14)]],fill=p[5],outline=p[7])
            d.line([Q(29,4),Q(33,9)],fill=p[8],width=2)
    else:
        pts=[Q(10+5*math.sin((y-4)*math.pi/31),y) for y in range(4,36)]
        d.line(pts,fill=p[7],width=3);d.line(pts[0:1]+pts[-1:],fill=p[8],width=1)
        if 'Repeater' in name:
            d.line([Q(6,20),Q(31,17)],fill=p[7],width=3)
            d.line([Q(18,19),Q(22,33)],fill=p[1],width=3)
        else:
            d.polygon([Q(x,y) for x,y in [(10,17),(14,14),(17,19),(13,22)]],fill=p[5],outline=p[8])
    return o

def boss(name,image):
    if not any(k in name for k in ('Sentinel','Archon','Sovereign','Overlord','Colossus','Heart')): return image
    w,h=image.size;c=w//2;y=h//2;p=PALETTES[theme(name)]
    overlay=Image.new('RGBA',image.size);d=ImageDraw.Draw(overlay)
    if 'Sovereign' in name or 'Heart' in name:
        d.polygon([(c,y-24),(c+12,y),(c,y+24),(c-12,y)],fill=p[1],outline=p[7],width=2)
        d.polygon([(c,y-18),(c+7,y),(c,y+18),(c-7,y)],fill=p[4],outline=p[8])
        for xx,yy in [(c-19,y-9),(c+19,y-13),(c-17,y+13),(c+21,y+17)]:
            d.line([(xx,yy-3),(xx,yy+3)],fill=p[7]);d.line([(xx-3,yy),(xx+3,yy)],fill=p[7])
    elif 'Colossus' in name or 'Overlord' in name:
        d.polygon([(c-21,y-16),(c-14,y-22),(c-6,y-13),(c,y-25),(c+8,y-12),(c+16,y-22),(c+22,y-16),(c+17,y+13),(c-17,y+13)],fill=p[1],outline=p[4],width=2)
        for xx in (c-12,c+12):d.polygon([(xx-5,y-4),(xx,y-10),(xx+5,y-4),(xx+2,y+1)],fill=p[7])
        d.polygon([(c-5,y+3),(c,y-8),(c+5,y+3),(c,y+13)],fill=p[5],outline=p[8])
    elif 'Archon' in name:
        for dx in (-12,12):d.polygon([(c+dx,y-12),(c+dx+4,y-5),(c+dx+2,y+13),(c+dx-4,y+7)],fill=p[4],outline=p[7])
        d.polygon([(c,y-18),(c+9,y),(c,y+18),(c-9,y)],fill=p[1],outline=p[7])
        d.ellipse((c-4,y-4,c+4,y+4),fill=p[8])
    else:
        d.ellipse((c-15,y-11,c+15,y+11),fill=p[1],outline=p[7],width=2)
        d.ellipse((c-7,y-7,c+7,y+7),fill=p[8]);d.ellipse((c-3,y-3,c+3,y+3),fill=p[0])
    mask=Image.frombytes('L',image.size,bytes(min(a,b) for a,b in zip(image.getchannel('A').tobytes(),overlay.getchannel('A').tobytes())))
    overlay.putalpha(mask);image.alpha_composite(overlay)
    return image

def icon():
    o=Image.new('RGBA',(80,80),(13,11,27,255));d=ImageDraw.Draw(o)
    for i in range(150):
        x=(i*37+13)%80;y=(i*23+i*i)%80
        d.point((x,y),fill=(23,20,47))
    d.ellipse((6,6,73,73),fill=(22,16,43),outline=(130,69,181),width=3)
    d.ellipse((12,12,67,67),outline=(75,48,117),width=2)
    d.polygon([(40,8),(49,25),(59,17),(56,37),(68,44),(53,49),(50,65),(40,57),(30,65),(27,49),(12,44),(24,37),(21,17),(31,25)],fill=(50,31,84),outline=(162,98,223))
    d.polygon([(40,19),(53,39),(40,62),(27,39)],fill=(18,15,40),outline=(204,142,255),width=2)
    d.polygon([(40,26),(48,40),(40,54),(32,40)],fill=(107,62,174),outline=(235,194,255))
    d.polygon([(40,30),(43,39),(40,50),(37,39)],fill=(244,215,255))
    d.arc((19,18,61,62),10,166,fill=(253,153,84),width=2)
    return o

def main():
    if STAMP.exists():
        print('Visual polish 0.63.0 already applied.');return
    total=0
    for path in sorted((ROOT/'Content').rglob('*.png')):
        name=path.stem;relative=path.relative_to(ROOT).as_posix()
        old=Image.open(path).convert('RGBA');new=shade(old,relative)
        if '/Armor/' in relative and not name.endswith(('_Head','_Body','_Arms','_Legs')):
            new=armor_icon(name,old.size)
        elif '/Buffs/' in relative:new=buff(name,old.size)
        elif relative.startswith('Content/Items/') and any(k in name for k in ('ColossusGreatsword','OverlordGreatblade','SovereignGreatblade','AstralScepter','AstralSummonerStaff','SovereignSummonerStaff','CinderScepter','ColossusScepter','OverlordScepter','ColossusSummonerStaff','OverlordSummonerStaff','AstralHeartRepeater','AstralLongbow','OverlordLongbow','ColossusLongbow')):
            new=weapon(name,old.size)
        elif relative.startswith('Content/NPCs/'):new=boss(name,new)
        elif relative.startswith('Content/Projectiles/') and name in ('UmbralBolt','ArchonArcaneShot'):
            p=PALETTES[theme(name)];d=ImageDraw.Draw(new);w,h=old.size
            if name=='UmbralBolt':d.polygon([(2,h-4),(w-5,1),(w-2,2),(7,h-2)],fill=p[5],outline=p[7])
            else:d.ellipse((2,2,w-3,h-3),outline=p[7],width=2);d.point((w//2,h//2),fill=p[8])
        elif relative.startswith('Content/Tiles/'):
            p=PALETTES[theme(name)];d=ImageDraw.Draw(new)
            for x,y in ((2,3),(7,11),(11,4),(13,13)):d.point((x,y),fill=p[7])
        assert new.size==old.size, relative
        new.save(path,optimize=True)
        total+=1
    icon().save(ROOT/'icon.png',optimize=True)
    build=ROOT/'build.txt'
    data=build.read_text(encoding='utf-8').replace('version = 0.62.0','version = 0.63.0')
    build.write_text(data,encoding='utf-8')
    STAMP.write_text('v0.63.0 — UMBRA visual polish, 145 sprites, original dimensions preserved\n',encoding='utf-8')
    print(f'Visually polished {total} textures + mod icon; no sprite dimensions changed.')

if __name__=='__main__':main()
