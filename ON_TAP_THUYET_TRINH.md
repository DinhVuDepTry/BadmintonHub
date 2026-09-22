# Ôn tập thuyết trình và câu hỏi giảng viên

Tài liệu này dùng để các thành viên ôn nhanh trước demo. Câu trả lời cần ngắn, đúng code và có thể mở file hoặc demo để chứng minh.

## 1. Tổng quan hệ thống

### Hỏi: Đề tài giải quyết vấn đề gì?

**Đáp:** BadmintonHub hỗ trợ một địa điểm cầu lông quản lý sân và booking. Guest tìm sân, Customer đặt/hủy booking và Admin quản lý sân, xác nhận booking, theo dõi dashboard.

### Hỏi: Những actor nào tồn tại?

**Đáp:** Guest, Customer và Admin. Guest chỉ xem/tìm sân; Customer đặt, xem và hủy booking của mình; Admin quản lý sân và toàn bộ booking.

### Hỏi: Vì sao không có CourtOwner?

**Đáp:** MVP phục vụ một địa điểm nên nhiều chủ sân không nằm trong phạm vi. Nhóm đã bỏ role chưa có nghiệp vụ để RBAC nhất quán; khi mở rộng nhiều địa điểm có thể thêm `OwnerId` cho Court và policy theo owner.

## 2. Kiến trúc và công nghệ

### Hỏi: Vì sao dùng Razor MVC thay vì React/SPA?

**Đáp:** Hệ thống chủ yếu là form và dashboard server-rendered. Razor MVC tích hợp tốt với Identity cookie, validation và deployment một ứng dụng. REST API vẫn được giữ để mở rộng client khác sau này.

### Hỏi: Kiến trúc project là gì?

**Đáp:** Modular monolith theo lớp: Controllers/Razor UI và API, Services chứa nghiệp vụ, Data/EF Core chứa persistence, Models/ViewModels tách entity và DTO, Background Worker xử lý lifecycle booking.

### Hỏi: Vì sao không microservices?

**Đáp:** Một venue, phạm vi nhỏ và cần transaction booking nhất quán. Microservices tăng chi phí deploy, tracing và distributed transaction mà chưa mang lại lợi ích tương xứng.

### Hỏi: Vì sao dùng MySQL và EF Core?

**Đáp:** MySQL phù hợp môi trường triển khai phổ biến; EF Core hỗ trợ migrations, mapping entity, query async và kiểm soát quan hệ/constraint.

## 3. Database và nghiệp vụ booking

### Hỏi: Các entity chính là gì?

**Đáp:** `ApplicationUser`, `Court`, `Booking`. Booking có FK đến Customer và Court; CourtCode unique; xóa User/Court bị restrict khi còn booking.

### Hỏi: Làm sao chống đặt trùng sân?

**Đáp:** Khi tạo booking, service bắt đầu transaction, query các booking Pending/Confirmed cùng sân/ngày bằng locking, sau đó kiểm tra overlap. Nếu trùng thì ném business conflict và không insert booking.

### Hỏi: Điều kiện overlap là gì?

**Đáp:** Hai khoảng thời gian overlap khi `requestedStart < existingEnd && requestedEnd > existingStart`. Vì vậy booking 18:00-20:00 và 20:00-21:00 không trùng, nhưng 19:00-21:00 thì trùng.

### Hỏi: Vì sao cần transaction/locking nếu đã query overlap?

**Đáp:** Nếu chỉ query thông thường, hai request đồng thời có thể cùng thấy sân trống rồi cùng insert. Transaction và lock giảm race condition; production cần test MySQL concurrency và index phù hợp.

### Hỏi: Vòng đời booking là gì?

**Đáp:** Customer tạo `Pending`; Admin xác nhận thành `Confirmed`; worker định kỳ chuyển Pending quá hạn thành `Expired`, Confirmed đã qua giờ kết thúc thành `Completed`; Customer/Admin có thể hủy booking hợp lệ thành `Cancelled`.

### Hỏi: Vì sao chỉ hủy trước 2 giờ?

**Đáp:** Đây là business policy để tránh hủy sát giờ làm ảnh hưởng vận hành sân. Rule được kiểm tra ở service, không chỉ ẩn nút ở UI.

## 4. Authentication, RBAC và security

### Hỏi: Authentication khác Authorization thế nào?

**Đáp:** Authentication xác định người dùng là ai, ở đây dùng ASP.NET Identity cookie. Authorization quyết định họ được làm gì, ở đây dùng role Admin/Customer và ownership booking.

### Hỏi: RBAC được áp dụng ở đâu?

**Đáp:** Controller/API dùng `[Authorize(Roles = ...)]`. Service vẫn kiểm tra CustomerId khi đọc/hủy booking để chống IDOR. UI chỉ ẩn nút để cải thiện trải nghiệm, không thay thế authorization.

### Hỏi: 401 và 403 khác nhau thế nào?

**Đáp:** `401 Unauthorized` nghĩa là chưa đăng nhập hoặc session không hợp lệ. `403 Forbidden` nghĩa là đã đăng nhập nhưng không có role/quyền cần thiết.

### Hỏi: IDOR là gì và dự án phòng chống ra sao?

**Đáp:** IDOR là đổi ID trên URL để truy cập tài nguyên người khác. Khi xem/hủy booking, hệ thống kiểm tra booking.CustomerId có bằng user hiện tại không, trừ Admin.

### Hỏi: Vì sao dùng cookie thay vì JWT dù rubric có JWT?

**Đáp:** Client chính là Razor MVC cùng domain nên Identity cookie phù hợp hơn và giảm token-refresh complexity. API vẫn trả 401/403; nếu sau này có SPA/mobile tách riêng, JWT là trigger nâng cấp đã ghi trong ADR.

### Hỏi: CSRF được xử lý thế nào?

**Đáp:** Form MVC dùng anti-forgery token. API write operation yêu cầu token và client lấy token qua `/api/security/csrf` khi dùng cookie authentication.

### Hỏi: Password reset có lộ email tồn tại không?

**Đáp:** Không. Submit email luôn redirect về cùng trang confirmation. Chỉ khi email đã xác nhận thì hệ thống mới tạo token và gửi SMTP.

### Hỏi: Bí mật database/SMTP được lưu ở đâu?

**Đáp:** User Secrets khi local và environment variables/secret store khi production; không commit mật khẩu vào source.

## 5. API, test và vận hành

### Hỏi: Endpoint API quan trọng nào?

**Đáp:** `GET /api/courts` và availability query; `POST /api/bookings`; `POST /api/bookings/{id}/confirm`; `DELETE /api/bookings/{id}`; `GET /api/security/csrf`; `/health`.

### Hỏi: Những status code nào hệ thống dùng?

**Đáp:** `200` đọc thành công, `201` tạo booking/court, `204` confirm/hủy/xóa thành công, `401` chưa đăng nhập, `403` sai quyền, `404` không tìm thấy, `409` conflict nghiệp vụ như trùng lịch.

### Hỏi: Dự án đã test gì?

**Đáp:** Test service availability loại sân Maintenance và booking overlap, test khoảng không overlap, và test Pending chuyển Confirmed. Chạy bằng `dotnet test .\Tests\BadmintonHub.Tests.csproj`.

### Hỏi: Hạn chế của test hiện tại?

**Đáp:** Test dùng EF InMemory, chưa thay thế được MySQL concurrency/migration test thật. Đây là technical debt đã nêu trong operations document.

### Hỏi: Health check hoạt động ra sao?

**Đáp:** `/health` gọi `Database.CanConnectAsync`. Trả 200 khi database reachable, 503 khi không kết nối được.

### Hỏi: Hệ thống scale thế nào nếu nhiều user?

**Đáp:** Trước hết thêm composite index cho availability query, pagination cho admin list, metrics/logging, MySQL load/concurrency test. Chỉ cache hoặc tách service khi metrics chứng minh có bottleneck.

## 6. Kịch bản demo ngắn

1. Guest lọc sân trống tại `/Courts`.
2. Customer đăng nhập, tạo booking Pending.
3. Admin vào `/Bookings` hoặc `/Admin`, xác nhận booking.
4. Customer thấy booking Confirmed.
5. Thử tạo booking overlap để thấy lỗi conflict.
6. Customer thử đọc booking người khác để chứng minh 403.
7. Admin đổi sân sang Maintenance, Customer không còn đặt được sân đó.
8. Mở `/Identity/Account/Manage` để demo profile và password.
9. Mở `/health` và Swagger để kết thúc.

## 7. Câu hỏi phân công

- **Đình Văn Vũ:** ôn kiến trúc, database, locking, lifecycle, RBAC/security, deployment.
- **Nguyễn Duy Anh:** ôn API, status code, availability, test và ownership.
- **Nguyễn Minh Hạnh:** ôn UI flow, validation, documentation, acceptance criteria và demo evidence.
