# Phân công và đóng góp thành viên

> Tài liệu này là bảng phân công/đóng góp đề xuất dựa trên các phần đã có trong project. Các thành viên cần đọc và xác nhận lại trước khi nộp chính thức, đặc biệt nếu trường yêu cầu khai báo trung thực người thực hiện từng phần.

## Thành viên

| Thành viên | Vai trò | Mức phụ trách |
|---|---|---|
| Đình Văn Vũ | Trưởng nhóm / Main developer | Phần lõi và các hạng mục kỹ thuật khó |
| Nguyễn Duy Anh | Backend/API developer | Nghiệp vụ booking, API và kiểm thử service |
| Nguyễn Minh Hạnh | Documentation/UI support | Tài liệu, kiểm tra giao diện và checklist demo |

## Phân công chi tiết

### 1. Đình Văn Vũ - Trưởng nhóm và lập trình chính

**Các phần phụ trách:**

- Phân tích phạm vi và thiết kế kiến trúc tổng thể.
- Thiết lập project ASP.NET Core MVC/Razor .NET 8.
- Thiết kế cấu trúc module: Controllers, Services, Models, ViewModels và Data.
- Thiết kế database với EF Core, MySQL và migrations.
- Tích hợp ASP.NET Identity và cơ chế phân quyền Admin/Customer.
- Xử lý các business rule khó:
  - Kiểm tra sân đang Active.
  - Kiểm tra ngày/giờ hợp lệ.
  - Chống đặt trùng sân và khung giờ.
  - Transaction và locking khi tạo booking.
  - Booking lifecycle `Pending`, `Confirmed`, `Expired`, `Completed`.
- Xây dựng background worker xử lý booking quá hạn.
- Thiết kế và hoàn thiện Admin dashboard.
- Xử lý bảo mật nền tảng: CSRF, IDOR, rate limiting, HTTPS, health check và exception handling.
- Review code, tích hợp các phần của nhóm và chuẩn bị bản build/deploy.

**Sản phẩm bàn giao:**

- Kiến trúc và module chính của hệ thống.
- `Program.cs`, `AppDbContext`, Models, Services và Background Worker.
- Database migrations.
- Admin dashboard và các luồng nghiệp vụ lõi.
- Bản build chạy được và bản release trên Git.

### 2. Nguyễn Duy Anh - Backend và API

**Các phần phụ trách:**

- Phân tích API contract cho Court và Booking.
- Xây dựng/hoàn thiện các API:
  - Xem danh sách và chi tiết sân.
  - Tìm sân trống theo ngày/giờ.
  - Tạo, xem, hủy booking.
  - Admin xác nhận booking.
- Kiểm tra status code và error response: `200`, `201`, `204`, `401`, `403`, `404`, `409`.
- Kiểm tra phân quyền API giữa Guest, Customer và Admin.
- Kiểm tra ownership booking để tránh truy cập chéo dữ liệu.
- Bổ sung Swagger endpoint description và ví dụ request/response.
- Viết và chạy test service cho:
  - Sân bảo trì không xuất hiện trong availability.
  - Booking bị trùng thời gian.
  - Booking không trùng thời gian vẫn được chấp nhận.
  - Pending booking chuyển sang Confirmed.
- Ghi nhận các edge case của API trong tài liệu thiết kế.

**Sản phẩm bàn giao:**

- API controllers và DTO contract.
- API authorization checklist.
- Service test cases.
- Phần API trong tài liệu sequence, contract và traceability.

### 3. Nguyễn Minh Hạnh - Tài liệu và hỗ trợ giao diện

**Các phần phụ trách:**

- Kiểm tra nội dung và bố cục các trang Razor:
  - Trang chủ.
  - Danh sách sân.
  - Form đặt sân.
  - Lịch đặt sân.
  - Login/Register.
  - Forgot password/Reset password.
  - Manage account.
- Kiểm tra label, thông báo lỗi, empty state và trạng thái booking.
- Kiểm tra responsive cơ bản trên desktop/mobile.
- Chuẩn hóa nội dung tiếng Việt hiển thị trên giao diện.
- Soạn checklist demo cho các vai trò Guest, Customer và Admin.
- Tổng hợp ảnh/chứng cứ khi demo:
  - Customer đặt sân.
  - Admin xác nhận booking.
  - Booking trùng bị từ chối.
  - User không có quyền nhận `403`.
- Hỗ trợ hoàn thiện các tài liệu:
  - Use Case.
  - User Story và Acceptance Criteria.
  - Requirement Matrix.
  - Traceability Matrix.
  - Submission Checklist.
- Kiểm tra README và hướng dẫn chạy project.

**Sản phẩm bàn giao:**

- Nội dung và checklist giao diện.
- Kịch bản demo.
- Bộ evidence trước khi nộp.
- Các phần tài liệu yêu cầu và truy vết được nhóm phân công.

## Bảng tỷ lệ đóng góp đề xuất

| Thành viên | Code/kiến trúc | Backend/API | UI/tài liệu/test | Tỷ lệ đề xuất |
|---|---:|---:|---:|---:|
| Đình Văn Vũ | Cao | Trung bình | Trung bình | 45% |
| Nguyễn Duy Anh | Trung bình | Cao | Trung bình | 35% |
| Nguyễn Minh Hạnh | Thấp | Thấp | Cao | 20% |

Tỷ lệ trên là đề xuất để phân công và thuyết trình. Nhóm nên điều chỉnh theo commit history, nội dung thực tế đã làm và yêu cầu của giảng viên trước khi nộp.

## Kịch bản thuyết trình theo vai trò

- **Đình Văn Vũ:** Trình bày kiến trúc, database, Identity/RBAC, booking transaction, worker và deployment.
- **Nguyễn Duy Anh:** Trình bày API, availability search, booking flow, status code và test case.
- **Nguyễn Minh Hạnh:** Trình bày giao diện, user flow, validation, tài liệu yêu cầu và demo checklist.

## Xác nhận thành viên

| Thành viên | Xác nhận nội dung đóng góp | Chữ ký/ngày |
|---|---|---|
| Đình Văn Vũ | [ ] | |
| Nguyễn Duy Anh | [ ] | |
| Nguyễn Minh Hạnh | [ ] | |
