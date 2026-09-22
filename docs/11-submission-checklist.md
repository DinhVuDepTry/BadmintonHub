# Submission Checklist

## Thông tin bài nộp

- [ ] Đổi tên hồ sơ theo mẫu `TenLop_TenNhom_TenDeTai`.
- [ ] Điền tên lớp, nhóm, thành viên và giảng viên vào file cover.
- [ ] Nộp source code cùng thư mục `docs/`.
- [ ] Không commit password database, SMTP password hoặc User Secret.

## Rubric evidence

### Phân tích và thiết kế - 25 điểm

- [x] Scope và actors: `01-requirements.md`.
- [x] FR/NFR có ID: `01-requirements.md`.
- [x] Ít nhất 3 Use Case: `02-use-cases.md`.
- [x] User Story và Acceptance Criteria: `03-user-stories.md`.
- [x] Domain/ERD/cardinality/constraints: `04-domain-and-erd.md`.
- [x] Context/module/sequence/API: `05-system-design.md`, `06-sequences-and-api.md`.

### Trade-off - 20 điểm

- [x] Razor MVC vs SPA: ADR-001.
- [x] Cookie Identity vs JWT: ADR-002.
- [x] Modular monolith vs microservices: ADR-003.
- [x] Booking locking strategy: ADR-004.

### Security/RBAC - 20 điểm

- [x] Assets, entry points, trust boundaries: `08-threat-model-rbac.md`.
- [x] Role-permission matrix: `08-threat-model-rbac.md`.
- [x] IDOR, CSRF, brute force, double booking threats.
- [x] Mitigation and verification mapping.
- [ ] Capture live HTTP evidence for every 401/403 case before presentation.
- [ ] Run a real MySQL concurrency check before production deployment.

### Documentation/traceability - 20 điểm

- [x] Requirement -> Use Case -> Implementation -> Test matrix.
- [x] Source/tool/AI disclosure.
- [x] Mermaid diagrams with labels and boundaries.
- [ ] Export Mermaid diagrams to PNG/PDF if the lecturer does not render Markdown.

### Scalability/operations - 15 điểm

- [x] Dockerfile and Release publish.
- [x] Health check.
- [x] Rate limiting.
- [x] Background lifecycle worker.
- [x] Scale assumptions, limitations, technical debt and revisit triggers.
- [ ] Configure production SMTP and database backup.
- [ ] Add structured audit logging if time remains.

## Demo script

1. Open `/Courts` as Guest and filter available courts.
2. Register a new Customer account.
3. Log in and open `/Identity/Account/Manage`.
4. Update profile and change password.
5. Create a booking from `/Bookings/Create`.
6. Log in as Admin and open `/Admin`.
7. Confirm the Pending booking from `/Bookings`.
8. Verify the Customer sees `Confirmed` and cannot access another customer's booking.
9. Set a court to Maintenance and show it is unavailable.
10. Try an overlapping booking and show the business conflict.
11. Open Swagger and demonstrate public court API and protected booking API.
12. Open `/health` and show database-aware health status.

## Final verification commands

```powershell
dotnet build .\BadmintonHub.csproj
dotnet test .\Tests\BadmintonHub.Tests.csproj
dotnet ef database update
dotnet run
```

## Evidence table to complete during demo

| Evidence ID | Scenario | Expected result | Captured |
|---|---|---|---|
| E-001 | Guest calls booking API | 401 | [ ] |
| E-002 | Customer creates court | 403 | [ ] |
| E-003 | Customer reads another booking | 403 | [ ] |
| E-004 | Customer confirms booking | 403 | [ ] |
| E-005 | Admin confirms booking | 204/UI status Confirmed | [ ] |
| E-006 | Overlapping booking | 409/business error | [ ] |
| E-007 | Missing CSRF token | Rejected | [ ] |
| E-008 | Health with database | 200 healthy | [ ] |
