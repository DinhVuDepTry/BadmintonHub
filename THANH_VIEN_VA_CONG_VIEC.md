# Thành viên và công việc

> Bảng này tổng hợp phân công và nội dung mỗi thành viên phụ trách khi thuyết trình. Nhóm cần xác nhận lại phần đóng góp thực tế trước khi nộp chính thức.

| Thành viên | Vai trò | Phần phụ trách chính |
|---|---|---|
| Đình Văn Vũ | Trưởng nhóm / Main developer | Kiến trúc, database, nghiệp vụ booking khó, RBAC/security, dashboard và tích hợp project |
| Nguyễn Duy Anh | Backend/API developer | Court/Booking API, availability search, status code, authorization và service tests |
| Nguyễn Minh Hạnh | UI/documentation support | Razor UI, kiểm tra user flow, tài liệu rubric, checklist demo và evidence |

## 1. Đình Văn Vũ

### Công việc đã phụ trách

- Thiết lập kiến trúc ASP.NET Core MVC/Razor, EF Core và MySQL.
- Thiết kế entity `Court`, `Booking`, `ApplicationUser`, enum trạng thái và migrations.
- Xây dựng business rule booking:
  - Chỉ đặt sân `Active`.
  - Kiểm tra ngày/giờ tương lai.
  - Chống trùng sân/khung giờ bằng transaction và locking.
  - Tính tổng tiền theo thời lượng và giá sân.
  - Hủy booking trước tối thiểu 2 giờ.
- Xây dựng lifecycle `Pending -> Confirmed -> Completed/Expired` và background worker.
- Tích hợp ASP.NET Identity, Admin/Customer RBAC, CSRF, rate limiting, health check và error handling.
- Xây dựng Admin dashboard, quản lý sân và luồng xác nhận booking.
- Tích hợp Docker, cấu hình deploy, README và review/tích hợp toàn bộ project.

### Khi thuyết trình nên trình bày

- Kiến trúc modular monolith và lý do dùng Razor MVC.
- Quan hệ User-Court-Booking, transaction và chống double booking.
- RBAC, IDOR, CSRF và password recovery.
- Background worker và trạng thái booking.

## 2. Nguyễn Duy Anh

### Công việc đã phụ trách

- Thiết kế API contract cho Court và Booking.
- Hoàn thiện API danh sách/chi tiết sân, tìm sân trống, tạo/xem/hủy/xác nhận booking.
- Kiểm tra HTTP status code: `200`, `201`, `204`, `401`, `403`, `404`, `409`.
- Kiểm tra phân quyền API theo Guest, Customer, Admin.
- Kiểm tra ownership booking để tránh truy cập chéo dữ liệu.
- Bổ sung test service cho availability, overlap và confirm booking.
- Hỗ trợ Swagger, sequence/API contract và traceability matrix.

### Khi thuyết trình nên trình bày

- API availability nhận `date`, `startTime`, `endTime` hoạt động thế nào.
- Vì sao overlap condition là `requestedStart < end && requestedEnd > start`.
- Khác nhau giữa `401`, `403`, `404`, `409`.
- Vì sao Customer chỉ thấy booking của mình còn Admin thấy toàn bộ.

## 3. Nguyễn Minh Hạnh

### Công việc đã phụ trách

- Kiểm tra và hoàn thiện các trang Razor: Home, Courts, Booking, Login, Register, Forgot/Reset Password, Manage Account.
- Chuẩn hóa giao diện tiếng Việt, validation, empty state, badge trạng thái và responsive layout.
- Kiểm tra luồng Guest/Customer/Admin trên giao diện.
- Tổng hợp User Story, Acceptance Criteria, Use Case, checklist rubric và checklist demo.
- Chuẩn bị evidence cho booking trùng, không có quyền, Admin xác nhận booking và health check.
- Kiểm tra README, tài liệu nộp bài và kịch bản trình bày.

### Khi thuyết trình nên trình bày

- Luồng người dùng từ tìm sân đến đặt sân.
- Khác biệt giao diện và quyền giữa Customer/Admin.
- Validation form và hiển thị lỗi thân thiện.
- Cách dùng checklist/evidence để kiểm chứng yêu cầu.

## Tỷ lệ đóng góp đề xuất

| Thành viên | Tỷ lệ |
|---|---:|
| Đình Văn Vũ | 45% |
| Nguyễn Duy Anh | 35% |
| Nguyễn Minh Hạnh | 20% |

## Checklist trước khi nộp

- [ ] Mỗi thành viên xác nhận phần mô tả là đúng với đóng góp thực tế.
- [ ] Điền lớp, nhóm, MSSV vào `docs/00-submission-cover.md`.
- [ ] Chuẩn bị ảnh/video evidence theo `docs/11-submission-checklist.md`.
- [ ] Mỗi thành viên ôn phần tương ứng trong `ON_TAP_THUYET_TRINH.md`.
