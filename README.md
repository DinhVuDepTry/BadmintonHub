# BadmintonHub — Hệ thống đặt sân cầu lông

Project môn Lập trình Web Nâng Cao (30INF067). Xây dựng bằng ASP.NET Core MVC, EF Core, MySQL, có RBAC (Admin / CourtOwner / Customer) và API riêng cho các thao tác chính.

## Công nghệ

- .NET 8, ASP.NET Core MVC (Razor Views)
- EF Core + Pomelo.EntityFrameworkCore.MySql 8.0.3
- MySQL 8.x
- ASP.NET Core Identity (RBAC: Admin / CourtOwner / Customer)
- Swagger (Swashbuckle.AspNetCore)

## Cách chạy project

### 1. Yêu cầu
- .NET SDK 8.0.x
- MySQL Server 8.x đang chạy

### 2. Tạo database và user MySQL

```sql
CREATE DATABASE badmintonhub_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_0900_ai_ci;

CREATE USER 'badminton_api'@'localhost'
IDENTIFIED BY '<mật khẩu của bạn>';

GRANT ALL PRIVILEGES ON badmintonhub_db.*
TO 'badminton_api'@'localhost';
```

### 3. Cấu hình connection string qua User Secrets

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:BadmintonDb" "Server=localhost;Port=3306;Database=badmintonhub_db;User=badminton_api;Password=<mật khẩu của bạn>;"
```

### 4. Cài EF Core tool và chạy Migration

```powershell
dotnet tool restore
dotnet ef database update
```

### 5. Chạy project

```powershell
dotnet run
```

Mở `http://localhost:5176` (hoặc port hiển thị trong terminal khi chạy).

- Swagger (test API): `http://localhost:5176/swagger`
- Đăng ký tài khoản: `http://localhost:5176/Identity/Account/Register`

## Phân quyền (RBAC)

Có 3 role: **Admin**, **CourtOwner**, **Customer** — được tự động tạo (seed) khi ứng dụng khởi động lần đầu.

Vì hiện chưa có giao diện gán role, cần gán thủ công qua MySQL sau khi đăng ký tài khoản:

```sql
SELECT Id FROM aspnetusers WHERE Email = '<email tài khoản>';
SELECT Id, Name FROM aspnetroles;

INSERT INTO aspnetuserroles (UserId, RoleId)
VALUES ('<user-id>', '<role-id>');
```

| Role | Quyền |
|---|---|
| Admin | Toàn quyền quản lý sân (tạo/sửa/xóa Court), xem tất cả booking |
| CourtOwner | (dự phòng mở rộng — hiện dùng chung quyền với Admin ở mức Court) |
| Customer | Đặt sân, xem/hủy booking của chính mình |

## API endpoints chính

| Method | Endpoint | Quyền | Mô tả |
|---|---|---|---|
| GET | `/api/courts` | Public | Danh sách sân |
| GET | `/api/courts/{id}` | Public | Chi tiết 1 sân |
| POST | `/api/courts` | Admin | Tạo sân mới |
| PUT | `/api/courts/{id}` | Admin | Cập nhật sân |
| DELETE | `/api/courts/{id}` | Admin | Xóa sân (chặn nếu còn booking) |
| GET | `/api/bookings` | Đã đăng nhập | Admin xem tất cả, Customer xem của mình |
| GET | `/api/bookings/{id}` | Đã đăng nhập | Chi tiết 1 booking |
| POST | `/api/bookings` | Customer | Đặt sân (tự check trùng lịch, trả `409` nếu trùng) |
| DELETE | `/api/bookings/{id}` | Chủ booking | Hủy booking |

## Giao diện Razor

| Trang | Đường dẫn | Mô tả |
|---|---|---|
| Danh sách sân | `/Courts` | Xem tất cả sân đang hoạt động |
| Lịch đặt sân | `/Bookings` | Xem lịch sử đặt sân (theo quyền) |
| Đặt sân mới | `/Bookings/Create` | Form đặt sân |

## Business rules đã áp dụng

- Không cho đặt trùng sân + ngày + khung giờ giao nhau với booking đang `Pending`/`Confirmed` (trả `409 Conflict`)
- Không xóa được sân nếu vẫn còn booking liên quan
- Giá booking tự tính = số giờ đặt × giá/giờ của sân
- Mọi thao tác ghi (create/update/delete) qua API yêu cầu đăng nhập và đúng role tương ứng (đã kiểm thử `403 Forbidden` khi sai role)

## Cấu trúc project

```
BadmintonHub/
├── Controllers/          # MVC Controllers (Home, Courts, Bookings)
│   └── Api/               # API Controllers (CourtsApi, BookingsApi)
├── Views/                 # Razor Views
├── Areas/Identity/         # Identity UI (Login/Register)
├── Data/                  # AppDbContext
├── Models/                # Entities (Court, Booking, ApplicationUser)
│   └── Enums/              # BookingStatus
├── ViewModels/             # DTOs cho API và Form
├── Services/               # Business logic (CourtService, BookingService)
├── Migrations/             # EF Core Migrations
└── Program.cs
```