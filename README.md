Môn Thực Tập Cơ Sở - Thầy Nguyễn Xuân Đức
Sinh Viên: Phạm Văn Vỹ
MSV: B23DCCN954
Dự án: LUMI-CORE (3D Arena Survival Game)

BÁO CÁO TIẾN ĐỘ TUẦN 1
(Thời gian: 29/01/2026 - 02/02/2026)

1. Quản lý dự án & Thiết lập môi trường (Project Setup)

Khởi tạo dự án Unity (Version 6000.3.6f1).

Thiết lập kho lưu trữ mã nguồn (Source Control) trên GitHub.

Cấu hình file .gitignore chuẩn cho Unity để tối ưu dung lượng upload.

Xây dựng cấu trúc thư mục dự án (Scripts, Prefabs, Materials, Scenes...) theo chuẩn.

2. Gameplay cốt lõi (Core Mechanics)

Player Controller: Cài đặt logic di chuyển cơ bản (Movement) sử dụng Rigidbody.

Camera System: Lập trình Camera đi theo người chơi (Camera Follow) với độ mượt.

Combat System:

Tạo Prefab đạn (Projectile).

Xây dựng thuật toán Auto-aim: Tự động tìm mục tiêu gần nhất trong bán kính tấn công.

Enemy AI: Tạo Enemy cơ bản, thiết lập AI di chuyển đuổi theo người chơi.

3. Hệ thống & Giao diện

Level System: Xây dựng logic quản lý kinh nghiệm (EXP) và lên cấp (Level Up).

User Interface (UI): Thiết kế và lập trình thanh máu (Health Bar), thanh kinh nghiệm (EXP Bar) cơ bản.

Assets: Import và xử lý các assets đồ họa (Sprites) cho nhân vật và môi trường.

BÁO CÁO TIẾN ĐỘ TUẦN 2 (02/02/2026 - 08/02/2026)
- Cơ chế LevelUp:
    Tạo levelup panel và card cho các loại nâng cấp:
        Tăng speed, Tăng damage, tăng hp
    Gép UI cho các card và level up panel
- Implement ObjectPooling.cs với Generic Dictionary-Queue architecture
- Tạo interface IPooledObject với method OnObjectSpawn()
- Refactor 4 class để dùng pooling:
    Projectile.cs - Viên đạn player
    EnemyHealth.cs - Enemy respawn system
    WeaponController.cs - Weapon spawn logic
    EnemySpawner.cs - Enemy spawn management
- Tạo VFX prefab cho EnemyDeath, LevelUp, ProjectileHit

