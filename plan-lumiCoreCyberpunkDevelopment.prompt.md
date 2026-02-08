# LUMICORE DEVELOPMENT ROADMAP
## Cyberpunk Survival Game - Chi Tiết Từng Bước

**Project**: LumiCore - Glitch Defense  
**Style**: Vampire Survivors + Cyberpunk Aesthetic  
**Focus**: Shader Graph Learning + Professional Portfolio  
**Estimated Time**: 4-6 tuần (80-120 giờ)

---

## CURRENT STATUS - ĐÃ HOÀN THÀNH

✅ **Core Mechanics** (Tuần 1)
- [x] Player Movement (WASD) với Rigidbody
- [x] Camera Follow System
- [x] Auto-aim Weapon System
- [x] Basic Enemy AI (chase player)
- [x] Enemy Spawner
- [x] Health System (Player + Enemy)
- [x] Level & EXP System
- [x] Basic Upgrade UI
- [x] Game Over Screen
- [x] ScriptableObject cho PlayerStats & UpgradeData

---

## PHASE 1: FOUNDATION & OPTIMIZATION (Tuần 2)
### Mục tiêu: Tối ưu code hiện tại, chuẩn bị cho visual effects

### TASK 1.1: Object Pooling System
**Thời gian**: 2-3 giờ  
**Tại sao cần**: Tránh lag khi có nhiều projectile/enemy/effects

#### Các bước thực hiện:

**Bước 1.1.1**: Tạo ObjectPool Manager
- [ ] Tạo file `ObjectPool.cs` trong `Assets/Scripts/Manager/`
- [ ] Code class ObjectPool với Dictionary<string, Queue<GameObject>>
- [ ] Implement methods: `SpawnFromPool()`, `ReturnToPool()`
- [ ] Tạo interface `IPooledObject` với method `OnObjectSpawn()`

**Bước 1.1.2**: Setup ObjectPool trong Scene
- [ ] Tạo Empty GameObject tên "ObjectPool" trong Hierarchy
- [ ] Attach script ObjectPool vào GameObject này
- [ ] Configure các pools trong Inspector:
  - Pool 1: Tag = "Projectile", Prefab = Projectile_Player, Size = 50
  - Pool 2: Tag = "Enemy", Prefab = Enemy_Dummy, Size = 100
  - Pool 3: Tag = "ExpGem", Prefab = ExpGem, Size = 50

**Bước 1.1.3**: Refactor Projectile.cs để dùng Pool
- [ ] Mở file `Projectile.cs`
- [ ] Implement interface `IPooledObject`
- [ ] Thay thế `Destroy(gameObject)` → `ObjectPool.Instance.ReturnToPool(gameObject)`
- [ ] Thay thế `Destroy(gameObject, lifeTime)` bằng Coroutine + ReturnToPool
- [ ] Add method `OnObjectSpawn()` để reset velocity và state

**Bước 1.1.4**: Refactor WeaponController.cs
- [ ] Mở file `WeaponController.cs`
- [ ] Thay thế `Instantiate(projecttilePrefab, ...)` 
- [ ] → `ObjectPool.Instance.SpawnFromPool("Projectile", ...)`

**Bước 1.1.5**: Refactor EnemySpawner.cs
- [ ] Mở file `EnemySpawner.cs`
- [ ] Thay thế `Instantiate(enemyPrefab, ...)` 
- [ ] → `ObjectPool.Instance.SpawnFromPool("Enemy", ...)`

**Bước 1.1.6**: Refactor EnemyHealth.cs
- [ ] Mở file `EnemyHealth.cs`
- [ ] Implement interface `IPooledObject` cho Enemy
- [ ] Thay thế `Destroy(gameObject)` trong `Die()` 
- [ ] → `ObjectPool.Instance.ReturnToPool(gameObject)`
- [ ] Add method `OnObjectSpawn()` để reset health về maxHealth

**Bước 1.1.7**: Refactor ExperienceGem.cs
- [ ] Thay thế `Instantiate()` trong EnemyHealth 
- [ ] → `ObjectPool.Instance.SpawnFromPool("ExpGem", ...)`
- [ ] Thay thế `Destroy()` trong ExperienceGem 
- [ ] → `ObjectPool.Instance.ReturnToPool()`

**Test**: Chạy game, kiểm tra không còn Instantiate/Destroy spam trong Profiler

---

### TASK 1.2: VFX Preparation - Particle System Setup
**Thời gian**: 2 giờ  
**Tại sao cần**: Chuẩn bị base cho visual effects trước khi học Shader Graph

**Bước 1.2.1**: Tạo cấu trúc thư mục VFX
- [ ] Tạo folder `Assets/VFX/`
- [ ] Tạo subfolder: `Particles/`, `Materials/`, `Textures/`

**Bước 1.2.2**: Tạo Projectile Hit Effect
- [ ] Trong Hierarchy: Create → Effects → Particle System
- [ ] Tên: "VFX_ProjectileHit"
- [ ] Configure particle:
  - Duration: 0.5s
  - Start Lifetime: 0.3s
  - Start Speed: 5
  - Start Size: 0.2-0.5
  - Start Color: Cyan gradient (cyberpunk)
  - Emission: Burst = 10-15 particles
  - Shape: Sphere, Radius = 0.3
- [ ] Tắt "Looping"
- [ ] Tắt "Play On Awake"
- [ ] Save as Prefab trong `Assets/VFX/Particles/`

**Bước 1.2.3**: Tạo Enemy Death Effect
- [ ] Create Particle System: "VFX_EnemyDeath"
- [ ] Configure:
  - Duration: 1s
  - Particles: 20-30
  - Color: Red → Orange gradient
  - Emission: Burst = 25
  - Add Size over Lifetime (shrink)
  - Add Rotation over Lifetime
- [ ] Save as Prefab

**Bước 1.2.4**: Tạo Level Up Effect
- [ ] Create Particle System: "VFX_LevelUp"
- [ ] Configure:
  - Ring shape particles
  - Color: Yellow/Gold cyberpunk
  - Emission: Burst = 50
  - Add Velocity over Lifetime (expand outward)
- [ ] Save as Prefab

**Bước 1.2.5**: Integrate VFX vào code
- [ ] Thêm VFX vào ObjectPool config (3 pools mới)
- [ ] Trong `Projectile.cs` → OnTriggerEnter: spawn hit VFX
- [ ] Trong `EnemyHealth.cs` → Die(): spawn death VFX
- [ ] Trong `LevelManger.cs` → LevelUp(): spawn levelup VFX

**Test**: Bắn enemy → thấy hit effect, enemy chết → thấy death effect

---

### TASK 1.3: Audio Manager Setup
**Thời gian**: 2-3 giờ

**Bước 1.3.1**: Tạo AudioManager script
- [ ] Tạo file `AudioManager.cs` trong `Assets/Scripts/Manager/`
- [ ] Implement Singleton pattern
- [ ] Tạo 2 AudioSource: musicSource, sfxSource
- [ ] Methods: `PlaySFX(AudioClip clip)`, `PlayMusic(AudioClip clip)`
- [ ] Add volume control variables

**Bước 1.3.2**: Tạo AudioData ScriptableObject
- [ ] Tạo file `AudioData.cs` trong `Assets/Scripts/ScriptableObjects/`
- [ ] Định nghĩa enum: `SFXType { Shoot, Hit, EnemyDeath, LevelUp, ButtonClick }`
- [ ] Class AudioData chứa: Dictionary<SFXType, AudioClip>

**Bước 1.3.3**: Setup AudioManager GameObject
- [ ] Tạo Empty GameObject "AudioManager"
- [ ] Attach AudioManager script
- [ ] Add 2 AudioSource components
- [ ] Assign references trong Inspector

**Bước 1.3.4**: Download/Import Free Audio
- [ ] Tìm free cyberpunk SFX trên:
  - freesound.org (search: "laser", "impact", "electric")
  - mixkit.co (search: "game sounds")
- [ ] Download 5-10 sound effects cơ bản
- [ ] Import vào `Assets/Audio/SFX/`
- [ ] Tạo folder `Assets/Audio/Music/`

**Bước 1.3.5**: Integrate audio vào gameplay
- [ ] WeaponController → Shoot(): `AudioManager.Instance.PlaySFX(shootSound)`
- [ ] Projectile → OnTriggerEnter(): PlaySFX(hitSound)
- [ ] EnemyHealth → Die(): PlaySFX(deathSound)
- [ ] LevelManger → LevelUp(): PlaySFX(levelUpSound)
- [ ] All UI buttons: Add PlaySFX(buttonClick)

**Test**: Bắn súng có tiếng, enemy chết có tiếng, UI có feedback âm thanh

---

## PHASE 2: SHADER GRAPH - CYBERPUNK VISUAL (Tuần 3-4)
### Mục tiêu: Học Shader Graph, tạo cyberpunk aesthetic

### TASK 2.1: Shader Graph Basics - Hologram Enemy
**Thời gian**: 4-6 giờ  
**Học**: Shader nodes cơ bản, UV, Transparency, Emission

**Bước 2.1.1**: Setup URP Shader Graph
- [ ] Mở Window → Package Manager
- [ ] Verify "Shader Graph" package đã installed
- [ ] Tạo folder `Assets/Art/ShaderGraph/` (đã có)
- [ ] Tạo folder `Assets/Art/Textures/` để chứa texture maps

**Bước 2.1.2**: Tạo Hologram Shader - Part 1 (Base)
- [ ] Right-click trong ShaderGraph folder
- [ ] Create → Shader Graph → URP → Lit Shader Graph
- [ ] Tên: "SH_HologramEnemy"
- [ ] Double-click để mở Shader Graph Editor
- [ ] Làm quen với interface: Graph, Blackboard, Node Library

**Bước 2.1.3**: Hologram Shader - Add Emission
- [ ] Trong Graph Inspector → Surface Type = Transparent
- [ ] Add node: Color property "HologramColor" (cyan: #00FFFF)
- [ ] Add node: Float property "EmissionStrength" = 2.0
- [ ] Connect: HologramColor × EmissionStrength → Emission
- [ ] Connect: HologramColor → Base Color
- [ ] Set Alpha = 0.5 trong HologramColor
- [ ] Save shader (Ctrl+S)

**Bước 2.1.4**: Test Hologram Shader lần 1
- [ ] Tạo Material mới: "MAT_HologramTest"
- [ ] Assign shader "SH_HologramEnemy" vào material
- [ ] Tạo Sphere test object trong scene
- [ ] Assign material vào sphere
- [ ] Xem kết quả: object trong suốt, phát sáng cyan

**Bước 2.1.5**: Hologram Shader - Add Scanline Effect
- [ ] Mở lại SH_HologramEnemy
- [ ] Add nodes:
  - Time node
  - Position node (set Space = Object)
  - Add node (Position.Y + Time)
  - Fraction node (tạo repeating pattern)
  - Step node (tạo hard edge)
- [ ] Add Float property: "ScanlineSpeed" = 1.0
- [ ] Add Float property: "ScanlineScale" = 5.0
- [ ] Connect chain: (Position.Y × ScanlineScale + Time × ScanlineSpeed) → Fraction → Step
- [ ] Multiply result với EmissionStrength
- [ ] Connect vào Emission

**Bước 2.1.6**: Hologram Shader - Add Fresnel Rim Light
- [ ] Add node: Fresnel Effect
- [ ] Add Float property: "RimPower" = 3.0
- [ ] Connect: Fresnel → Multiply với HologramColor → Add vào Emission
- [ ] Điều chỉnh cho rim sáng hơn ở edges

**Bước 2.1.7**: Apply Hologram Shader lên Enemy
- [ ] Duplicate Enemy_Dummy prefab → "Enemy_Hologram"
- [ ] Tạo Material mới: "MAT_EnemyHologram"
- [ ] Assign SH_HologramEnemy shader
- [ ] Assign material cho Enemy_Hologram model
- [ ] Test trong game: enemy trong suốt, có scanline, có rim light

**Learning Check**: Screenshot shader graph, note lại cách các node hoạt động

---

### TASK 2.2: Neon Projectile Trail Shader
**Thời gian**: 3-4 giờ  
**Học**: Gradient, Texture Sampling, UV Animation

**Bước 2.2.1**: Tạo Neon Trail Shader
- [ ] Create → Shader Graph → URP → Unlit Shader Graph
- [ ] Tên: "SH_NeonTrail"
- [ ] Surface Type = Transparent
- [ ] Blend Mode = Additive (cho glow effect)

**Bước 2.2.2**: Setup Base Gradient
- [ ] Add Gradient property: "TrailGradient"
- [ ] Configure gradient: Blue → Cyan → White (center bright)
- [ ] Add Sample Gradient node
- [ ] Add UV node
- [ ] Connect: UV.x → Sample Gradient → Base Color

**Bước 2.2.3**: Add Fade on Edges (vertical)
- [ ] Add Smoothstep node
- [ ] Input: UV.y
- [ ] Edge1 = 0, Edge2 = 0.3 (fade bottom)
- [ ] Add another Smoothstep: Edge1 = 0.7, Edge2 = 1.0 (fade top)
- [ ] Multiply both results
- [ ] Connect to Alpha

**Bước 2.2.4**: Add Trail Fade (horizontal)
- [ ] Add Power node
- [ ] Input: UV.x
- [ ] Add Float property: "TrailFade" = 2.0
- [ ] Connect: UV.x^TrailFade → Multiply với Alpha
- [ ] Kết quả: trail tối dần về đuôi

**Bước 2.2.5**: Add Animated Noise
- [ ] Add Simple Noise node
- [ ] Add Time node
- [ ] Add Float property: "NoiseSpeed" = 0.5
- [ ] Connect: UV + Time × NoiseSpeed → Simple Noise
- [ ] Multiply với Emission
- [ ] Add Float property: "NoiseStrength" = 0.3

**Bước 2.2.6**: Create Trail GameObject
- [ ] Tạo Empty GameObject: "ProjectileTrail"
- [ ] Add Component: Trail Renderer
- [ ] Configure Trail Renderer:
  - Time: 0.3s
  - Width: 0.2 → 0.0 (curve)
  - Min Vertex Distance: 0.1
  - Create Material → Assign SH_NeonTrail
- [ ] Save as Prefab

**Bước 2.2.7**: Integrate Trail vào Projectile
- [ ] Mở Projectile_Player prefab
- [ ] Add ProjectileTrail as child
- [ ] Position offset về phía sau một chút
- [ ] Test: bắn thấy trail sáng theo sau đạn

**Test**: Projectile bay có trail sáng, fade đẹp, có noise effect

---

### TASK 2.3: Glitch Damage Flash Shader
**Thời gian**: 3-4 giờ  
**Học**: Shader Animation, Properties Control từ Script

**Bước 2.3.1**: Tạo Glitch Flash Shader
- [ ] Create Lit Shader Graph: "SH_GlitchFlash"
- [ ] Add Texture2D property: "MainTexture"
- [ ] Add Color property: "GlitchColor" = Red (#FF0000)
- [ ] Add Float property: "FlashAmount" = 0.0 (range 0-1)

**Bước 2.3.2**: Implement Flash Effect
- [ ] Sample MainTexture
- [ ] Add Lerp node
- [ ] A = MainTexture, B = GlitchColor, T = FlashAmount
- [ ] Connect to Base Color
- [ ] Test: FlashAmount = 1 → object đỏ hoàn toàn

**Bước 2.3.3**: Add Glitch Displacement
- [ ] Add Simple Noise node
- [ ] Add Time node
- [ ] Add Float property: "GlitchIntensity" = 0.0
- [ ] Multiply noise với GlitchIntensity
- [ ] Add to Position offset trong Vertex stage

**Bước 2.3.4**: Tạo Script điều khiển Flash
- [ ] Tạo file `FlashEffect.cs` trong `Assets/Scripts/VFX/`
- [ ] Tạo folder nếu chưa có: `Assets/Scripts/VFX/`
- [ ] Code: Method TriggerFlash(float duration, Color color)
- [ ] Sử dụng Coroutine để animate FlashAmount: 0 → 1 → 0
- [ ] Dùng MaterialPropertyBlock để tối ưu

**Bước 2.3.5**: Apply lên Player
- [ ] Add FlashEffect script vào Player GameObject
- [ ] Assign material reference
- [ ] Trong PlayerHealth.cs → TakeDamage():
  - Call FlashEffect.TriggerFlash(0.2f, Color.red)

**Bước 2.3.6**: Apply lên Enemy
- [ ] Add FlashEffect vào Enemy prefab
- [ ] Trong EnemyHealth → TakeDamage():
  - Call FlashEffect.TriggerFlash(0.15f, Color.cyan)

**Test**: Player/Enemy bị đánh → flash đỏ/cyan, có glitch nhẹ

---

### TASK 2.4: Dissolve Effect cho Enemy Death
**Thời gian**: 4-5 giờ  
**Học**: Texture Masking, Cutout, Advanced Effects

**Bước 2.4.1**: Tìm/Tạo Dissolve Texture
- [ ] Google search: "dissolve texture noise"
- [ ] Download noise texture (512x512, grayscale)
- [ ] Hoặc dùng Photoshop/GIMP tạo cloud noise
- [ ] Import vào `Assets/Art/Textures/`
- [ ] Tên: "T_DissolveNoise"
- [ ] Import Settings: 
  - Texture Type = Default
  - sRGB = OFF (vì là data, không phải color)
  - Wrap Mode = Repeat

**Bước 2.4.2**: Tạo Dissolve Shader
- [ ] Create Lit Shader Graph: "SH_DissolveEnemy"
- [ ] Add Texture2D: "MainTexture"
- [ ] Add Texture2D: "DissolveTexture"
- [ ] Add Float: "DissolveAmount" = 0.0 (range 0-1)
- [ ] Add Color: "EdgeColor" = Orange (#FF6600)
- [ ] Add Float: "EdgeWidth" = 0.1

**Bước 2.4.3**: Implement Dissolve Logic
- [ ] Sample MainTexture → Base Color
- [ ] Sample DissolveTexture with UV
- [ ] Add Step node: Step(DissolveTexture, DissolveAmount)
- [ ] Connect to Alpha Clip Threshold
- [ ] Graph Inspector → Alpha Clipping = ON

**Bước 2.4.4**: Add Glowing Edge
- [ ] Compare DissolveTexture với (DissolveAmount + EdgeWidth)
- [ ] Subtract previous step result
- [ ] Multiply với EdgeColor
- [ ] Add to Emission
- [ ] Multiply với high strength (5-10)

**Bước 2.4.5**: Tạo DissolveEffect Script
- [ ] Tạo `DissolveEffect.cs` trong `Assets/Scripts/VFX/`
- [ ] Method: `StartDissolve(float duration)`
- [ ] Animate DissolveAmount: 0 → 1 over duration
- [ ] OnComplete: ReturnToPool object
- [ ] Use MaterialPropertyBlock

**Bước 2.4.6**: Integrate vào Enemy Death
- [ ] Add DissolveEffect component vào Enemy prefab
- [ ] Tạo Material "MAT_EnemyDissolve" với SH_DissolveEnemy
- [ ] Assign material cho enemy model
- [ ] Trong EnemyHealth.cs → Die():
  - Call DissolveEffect.StartDissolve(1.0f)
  - ReturnToPool sau 1 giây thay vì ngay lập tức

**Test**: Enemy chết → tan dần với edge sáng cam, đẹp mắt

---

### TASK 2.5: UI Shader - Cyberpunk Holographic Panels
**Thời gian**: 3-4 giờ  
**Học**: UI Shader, Screen Space effects

**Bước 2.5.1**: Tạo UI Hologram Shader
- [ ] Create Unlit Shader Graph: "SH_UI_HologramPanel"
- [ ] Target: URP
- [ ] Add Color: "PanelColor" = Cyan với alpha 0.3
- [ ] Add Float: "ScanlineSpeed" = 1.0
- [ ] Add Float: "ScanlineWidth" = 0.05

**Bước 2.5.2**: Implement Scanline cho UI
- [ ] UV node
- [ ] Time node
- [ ] (UV.y + Time × ScanlineSpeed) % 1.0
- [ ] Create repeating pattern
- [ ] Multiply với low alpha (0.2)
- [ ] Add to Base Color

**Bước 2.5.3**: Add Border Glow
- [ ] Check distance from UV to edges
- [ ] If < 0.05 from edge → brighten
- [ ] Add to Emission
- [ ] Create animated pulse với Time

**Bước 2.5.4**: Apply lên UI Elements
- [ ] Open LevelUpPanel UI trong scene
- [ ] Create Image cho background
- [ ] Create Material "MAT_UI_Hologram"
- [ ] Assign SH_UI_HologramPanel
- [ ] Set material trên Image component

**Bước 2.5.5**: Polish UI với shader
- [ ] Apply cho Upgrade Cards background
- [ ] Health bar có border glow
- [ ] XP bar có scanline effect
- [ ] Test: UI look cyberpunk & futuristic

**Test**: Open upgrade panel → UI có hologram effect, scanline, look professional

---

## PHASE 3: WEAPON SYSTEM EXPANSION (Tuần 5)
### Mục tiêu: Đa dạng weapon types, ScriptableObject architecture

### TASK 3.1: Weapon System Refactor
**Thời gian**: 4-5 giờ

**Bước 3.1.1**: Tạo WeaponData ScriptableObject
- [ ] Tạo `WeaponData.cs` trong `Assets/Scripts/ScriptableObjects/`
- [ ] Define enum WeaponType { Projectile, Beam, Orbital, Area }
- [ ] Create ScriptableObject class với properties:
  - weaponName, type, damage, fireRate, range, prefab, icon

**Bước 3.1.2**: Refactor WeaponController → BaseWeapon
- [ ] Rename `WeaponController.cs` → `BaseWeapon.cs`
- [ ] Convert to abstract class
- [ ] Abstract method: `protected abstract void Attack(Transform target)`
- [ ] Keep FindNearestEnemy() logic
- [ ] Add `public WeaponData weaponData` field

**Bước 3.1.3**: Tạo ProjectileWeapon class
- [ ] Tạo `ProjectileWeapon.cs`
- [ ] Extends BaseWeapon
- [ ] Override Attack() method
- [ ] Move Shoot() logic vào đây
- [ ] Use weaponData for stats

**Bước 3.1.4**: Tạo WeaponManager
- [ ] Tạo `WeaponManager.cs` trong Player
- [ ] List<BaseWeapon> equippedWeapons
- [ ] Method: AddWeapon(WeaponData data)
- [ ] Method: UpgradeWeapon(int index, float multiplier)
- [ ] Attach to Player GameObject

**Bước 3.1.5**: Test Refactor
- [ ] Assign ProjectileWeapon script vào Player weapon slot
- [ ] Create WeaponData asset: "WD_BasicLaser"
- [ ] Assign data vào ProjectileWeapon
- [ ] Test: weapon vẫn hoạt động như cũ

---

### TASK 3.2: Laser Beam Weapon
**Thời gian**: 5-6 giờ

**Bước 3.2.1**: Tạo BeamWeapon script
- [ ] Tạo `BeamWeapon.cs` extends BaseWeapon
- [ ] Override Attack(): Use LineRenderer
- [ ] Raycast từ firePoint đến target
- [ ] Deal continuous damage

**Bước 3.2.2**: Tạo Laser Beam VFX
- [ ] Create LineRenderer GameObject: "LaserBeam"
- [ ] Configure: Width 0.1→0.05, Positions: 2 points
- [ ] Create Material: "MAT_LaserBeam"
- [ ] Color: Bright cyan

**Bước 3.2.3**: Tạo Laser Beam Shader
- [ ] Create Unlit Shader Graph: "SH_LaserBeam"
- [ ] Add scrolling UV animation
- [ ] Add pulsing emission
- [ ] Add Fresnel edge glow
- [ ] Assign shader to MAT_LaserBeam

**Bước 3.2.4**: Implement Beam Logic
- [ ] FireBeam() method in BeamWeapon
- [ ] Enable LineRenderer when firing
- [ ] Update positions each frame
- [ ] Deal damage over time with coroutine

**Bước 3.2.5**: Tạo WeaponData cho Laser
- [ ] Create WeaponData asset: "WD_LaserBeam"
- [ ] Configure: type = Beam, damage = 5 (per tick)
- [ ] fireRate = 0.1 (tick rate)

**Bước 3.2.6**: Add Beam Hit Effect
- [ ] Create Particle System: "VFX_BeamHit"
- [ ] Attach to target position
- [ ] Continuous sparks effect
- [ ] Pool management

**Test**: Add laser weapon → liên tục bắn tia laser vào enemy, có VFX

---

### TASK 3.3: Orbital Drone Weapon
**Thời gian**: 5-6 giờ

**Bước 3.3.1**: Tạo OrbitalWeapon script
- [ ] `OrbitalWeapon.cs` extends BaseWeapon
- [ ] Spawn drone objects around player
- [ ] Drones rotate around player
- [ ] Auto-attack nearby enemies

**Bước 3.3.2**: Tạo Drone Prefab
- [ ] Create 3D model: small capsule/sphere
- [ ] Add Drone material với emission
- [ ] Add rotating animation
- [ ] Scale: 0.3-0.5

**Bước 3.3.3**: Implement Orbital Logic
- [ ] Calculate orbit position using trigonometry
- [ ] Update position each frame
- [ ] Multiple drones with angle offset

**Bước 3.3.4**: Drone Attack Logic
- [ ] Each drone tìm enemy trong range nhỏ
- [ ] Shoot projectile tương tự player
- [ ] Cooldown independent

**Bước 3.3.5**: Tạo WeaponData
- [ ] Create: "WD_OrbitalDrone"
- [ ] Type = Orbital
- [ ] Configure orbit radius, speed

**Test**: Add drone weapon → 1-3 drones bay quanh player, tự động bắn

---

## PHASE 4: ENEMY VARIETY (Tuần 5-6)
### Mục tiêu: Diverse enemy types, difficulty scaling

### TASK 4.1: Enemy Data System
**Thời gian**: 3 giờ

**Bước 4.1.1**: Tạo EnemyData ScriptableObject
- [ ] Tạo `EnemyData.cs` trong ScriptableObjects
- [ ] Enum EnemyType { Melee, Ranged, Tank, Elite }
- [ ] Properties: enemyName, type, maxHealth, moveSpeed, damage, expDrop, modelPrefab, primaryColor

**Bước 4.1.2**: Refactor EnemyHealth & EnemyAI
- [ ] Add `public EnemyData data` field
- [ ] Load stats from data trong Start()
- [ ] Apply primaryColor to material

**Bước 4.1.3**: Create Enemy Data Assets
- [ ] "ED_MeleeBasic": 30 HP, 3 speed, 10 dmg
- [ ] "ED_RangedShooter": 20 HP, 2 speed, 5 dmg
- [ ] "ED_TankHeavy": 100 HP, 1.5 speed, 20 dmg
- [ ] "ED_EliteFast": 50 HP, 5 speed, 15 dmg

---

### TASK 4.2: Ranged Enemy Implementation
**Thời gian**: 4 giờ

**Bước 4.2.1**: Tạo RangedEnemyAI script
- [ ] Extends EnemyAI
- [ ] Keep distance from player (min/max range)
- [ ] Shoot projectiles at player

**Bước 4.2.2**: Enemy Projectile
- [ ] Duplicate player projectile
- [ ] Change color to red
- [ ] Slower speed
- [ ] Deal damage to player

**Bước 4.2.3**: AI Logic Implementation
- [ ] Move toward if too far
- [ ] Move away if too close
- [ ] Shoot when in range and cooldown ready

**Bước 4.2.4**: Create Ranged Enemy Prefab
- [ ] Different model/color than melee
- [ ] Assign RangedEnemyAI
- [ ] Assign ED_RangedShooter data
- [ ] Test spawning

**Test**: Spawn ranged enemy → giữ khoảng cách và bắn player

---

### TASK 4.3: Tank & Elite Enemies
**Thời gian**: 6-8 giờ (3-4h mỗi loại)

**Bước 4.3.1**: Tank Enemy
- [ ] Larger scale (1.5x)
- [ ] Slow movement but high HP
- [ ] Heavy damage on hit
- [ ] Optional: Charge attack ability

**Bước 4.3.2**: Elite Enemy
- [ ] Fast movement
- [ ] Medium HP and damage
- [ ] Higher exp drop (2x-3x)
- [ ] Special shader effect (pulsing glow)

**Bước 4.3.3**: Test All Enemy Types
- [ ] Spawn mix of all types
- [ ] Balance stats based on gameplay
- [ ] Adjust difficulty curve

---

### TASK 4.4: Wave System & Difficulty Scaling
**Thời gian**: 4-5 giờ

**Bước 4.4.1**: Tạo WaveManager
- [ ] Tạo `WaveManager.cs`
- [ ] Track currentWave number
- [ ] Calculate difficultyMultiplier
- [ ] Increase spawn rate over time

**Bước 4.4.2**: Enemy Stat Scaling
- [ ] Health *= difficultyMultiplier
- [ ] Damage *= difficultyMultiplier
- [ ] Speed += (wave × 0.1f)
- [ ] Spawn rate increases

**Bước 4.4.3**: Mix Enemy Types per Wave
- [ ] Wave 1-3: Only melee
- [ ] Wave 4+: Add ranged (20%)
- [ ] Wave 7+: Add tanks (10%)
- [ ] Wave 10+: Elites spawn (5%)

**Bước 4.4.4**: UI Wave Display
- [ ] Add TextMeshPro: "Wave {number}"
- [ ] Display current wave number
- [ ] Wave transition animation/notification

**Test**: Play multiple waves, verify difficulty increases appropriately

---

## PHASE 5: POLISH & POST-PROCESSING (Tuần 6)
### Mục tiêu: Professional look, game feel

### TASK 5.1: URP Post-Processing
**Thời gian**: 2-3 giờ

**Bước 5.1.1**: Setup Post-Processing Volume
- [ ] Create Global Volume GameObject
- [ ] Add Volume component
- [ ] Create new Profile asset

**Bước 5.1.2**: Configure Effects
- [ ] Bloom: Intensity = 0.3, Threshold = 1.0
- [ ] Chromatic Aberration: Intensity = 0.2
- [ ] Vignette: Intensity = 0.3, Smoothness = 0.4
- [ ] Color Grading: Temperature shift to cool (cyan tint)
- [ ] Film Grain: Intensity = 0.2 (cyberpunk grit)

**Bước 5.1.3**: Camera Settings
- [ ] Main Camera → Enable Post Processing
- [ ] Configure Tonemapping: ACES
- [ ] Anti-aliasing: SMAA

**Test**: Game look much better, cyberpunk atmosphere established

---

### TASK 5.2: Screen Shake & Juice
**Thời gian**: 3-4 giờ

**Bước 5.2.1**: Tạo CameraShake script
- [ ] Tạo `CameraShake.cs`
- [ ] Method: Shake(float duration, float magnitude)
- [ ] Implement coroutine với random offset
- [ ] Smooth return to original position

**Bước 5.2.2**: Integrate Screen Shake
- [ ] Player hit: light shake (0.1s, 0.1 magnitude)
- [ ] Enemy death: medium shake (0.15s, 0.15 magnitude)
- [ ] Level up: strong shake (0.3s, 0.2 magnitude)

**Bước 5.2.3**: Hit-Stop Effect
- [ ] Freeze time briefly on hit (0.05s)
- [ ] Use Time.timeScale manipulation
- [ ] Restore timeScale after delay

**Bước 5.2.4**: Add More Juice
- [ ] Projectile spawn: scale from 0 with animation
- [ ] Enemy spawn: burst particles + scale animation
- [ ] Collect EXP: gems lerp/move toward player
- [ ] UI animations: scale bounce on show

**Test**: Game feels much more satisfying, responsive feedback

---

### TASK 5.3: Final Audio Polish
**Thời gian**: 2-3 giờ

**Bước 5.3.1**: Find/Create Background Music
- [ ] Search: "cyberpunk background music free"
- [ ] Download 2-3 looping tracks
- [ ] Import to Assets/Audio/Music/
- [ ] Assign to AudioManager

**Bước 5.3.2**: Mix Audio Levels
- [ ] Music volume: 0.6
- [ ] SFX volume: 0.8
- [ ] Balance all sounds
- [ ] No sound overpowering others

**Bước 5.3.3**: Add Audio Transitions
- [ ] Fade in music on game start
- [ ] Fade out on game over
- [ ] Lower music volume during upgrade panel
- [ ] Add ambient cyberpunk sounds

**Test**: Audio experience professional, not distracting

---

## PHASE 6: FINAL TOUCHES & BUILD
### Build & Portfolio Prep

### TASK 6.1: Build & Optimize
**Thời gian**: 2-3 giờ

- [ ] Build Settings → Platform: Windows
- [ ] Player Settings → Company name, Product name
- [ ] Optimize assets (texture compression)
- [ ] Test build performance
- [ ] Create installer/zip package

### TASK 6.2: Portfolio Materials
**Thời gian**: 2-3 giờ

- [ ] Record gameplay video (1-2 minutes)
- [ ] Take high-quality screenshots
- [ ] Create shader graph documentation with images
- [ ] Write technical breakdown of shader implementations
- [ ] Document architecture decisions

### TASK 6.3: Code Cleanup & Documentation
**Thời gian**: 2 giờ

- [ ] Add XML comments to public methods
- [ ] Remove all Debug.Log statements
- [ ] Organize all folders properly
- [ ] Create comprehensive README.md
- [ ] Git commit with proper messages
- [ ] Tag final release version

---

## LEARNING RESOURCES

### Shader Graph
- Unity Learn: "Introduction to Shader Graph" course
- YouTube: Brackeys - Shader Graph Tutorial series
- Website: catlikecoding.com - Rendering tutorials
- Unity Manual: Shader Graph documentation

### VFX
- Gabriel Aguiar Prod (YouTube) - Professional VFX
- Unity VFX Graph tutorials
- Freya Holmér - Shader math explanations

### Game Feel
- Book: "The Art of Screenshake" by Jan Willem Nijman (GDC talk)
- Video: "Juice it or lose it" by Martin Jonasson
- YouTube: Game Maker's Toolkit - Game Feel videos

### Audio
- freesound.org - Free SFX library
- mixkit.co - Free game sounds
- YouTube: Mix and Jam - Audio implementation tutorials

---

## PROGRESS TRACKING TIPS

1. **Checkbox System**: Đánh dấu [x] khi hoàn thành mỗi bước
2. **Daily Goals**: Set 2-3 tasks per coding session
3. **Git Commits**: Commit sau mỗi task hoàn thành với message rõ ràng
4. **Screenshots**: Chụp shader graphs để reference và portfolio
5. **Time Tracking**: Note thời gian thực tế vs estimate để improve planning
6. **Blocker Log**: Ghi lại vấn đề gặp phải và solution để học

### Git Commit Message Format
```
[Phase X.Y] Task name - Brief description

- Detail 1
- Detail 2
```

Example:
```
[Phase 2.1] Hologram Shader - Implemented scanline effect

- Added Time and Position nodes
- Created repeating pattern with Fraction
- Connected to Emission for animated effect
```

---

## ESTIMATED TOTAL TIME

| Phase | Tasks | Time Estimate |
|-------|-------|---------------|
| Phase 1 | Object Pooling, VFX, Audio | 15-20 hours |
| Phase 2 | Shader Graph (5 shaders) | 20-30 hours |
| Phase 3 | Weapon System | 15-20 hours |
| Phase 4 | Enemy Variety | 15-20 hours |
| Phase 5 | Polish & Post-Processing | 10-15 hours |
| Phase 6 | Build & Portfolio | 5-8 hours |
| **TOTAL** | | **80-120 hours** |

**Realistic Timeline**: 4-6 tuần với 15-20 giờ/tuần

---

## SUCCESS CRITERIA

By the end of this roadmap, you will have:

✅ **Technical Skills**:
- Deep understanding of Shader Graph
- Object-oriented game architecture
- Performance optimization techniques
- VFX and particle systems
- Audio implementation

✅ **Portfolio Piece**:
- Polished, playable game
- Professional-looking cyberpunk aesthetic
- 5+ custom shaders demonstrating various techniques
- Clean, documented codebase
- Video showcase and screenshots

✅ **Game Features**:
- Multiple weapon types
- Enemy variety with AI
- Progressive difficulty
- Upgrade system
- Juice and polish

---

## IMPORTANT REMINDERS

⚠️ **DO**:
- Làm tuần tự theo thứ tự tasks
- Test sau mỗi 30 phút coding
- Git commit thường xuyên (mỗi 1-2 giờ)
- Screenshot shader graphs cho học tập
- Comment code phức tạp
- Hỏi khi stuck > 30 phút

⚠️ **DON'T**:
- Nhảy qua các bước foundation
- Code quá nhiều mà không test
- Skip documentation
- Perfectionism trap (ship first, polish later)
- Ignore performance từ đầu

---

## FINAL NOTES

Roadmap này được thiết kế để:
1. **Học Shader Graph** một cách có hệ thống từ basic → advanced
2. **Xây dựng portfolio piece** professional cho đi làm
3. **Thực hành software architecture** với ScriptableObject pattern
4. **Tối ưu performance** từ đầu với Object Pooling

**Mỗi task được chia nhỏ thành các bước cụ thể** để bạn không bị overwhelm.

**Bắt đầu từ TASK 1.1** và làm từng bước một. Đừng vội, chất lượng quan trọng hơn tốc độ!

Good luck với dự án! 🎮✨

---

**Next Step**: Start with Phase 1, Task 1.1, Step 1.1.1 - Tạo ObjectPool.cs file!
