# Traceability Matrix

| Goal | Use case | User story/AC | Requirement | Implementation | Test/evidence |
|---|---|---|---|---|---|
| Find a usable court | UC-01 | US-001/AC-001..003 | FR-001, FR-002, NFR-004 | CourtService.GetAvailableAsync; CourtsController | CourtServiceTests availability tests |
| Prevent schedule conflicts | UC-01 | US-002/AC-006..007 | FR-003, FR-004 | BookingService.CreateAsync | overlap test; 409 behavior |
| Protect customer data | UC-03 | US-003/AC-008 | FR-005, NFR-002 | BookingsApiController; BookingService | ownership review; 403 negative case |
| Operate reservations | UC-02 | US-003/AC-009..010 | FR-008, FR-009 | ConfirmAsync; BookingLifecycleWorker | ConfirmAsync test; worker implementation |
| Release a slot safely | UC-03 | US-004/AC-011..013 | FR-006 | CancelAsync; BookingsController | service policy review |
| Manage courts | UC-04 | US-005/AC-014..016 | FR-007, NFR-003 | CourtService; CourtsController/API | build and controller authorization review |
| Protect accounts | UC-05/UC-06 | US-006/AC-017..019 | FR-010, NFR-001 | Identity pages; SmtpEmailSender | build; password/reset flow review |
| Operate the system | UC-04 | US-003 | FR-011, NFR-005..007 | AdminController; health; Dockerfile | build, release publish, health endpoint |

## Test inventory

- `CourtServiceTests.GetAvailableAsync_ExcludesInactiveAndOverlappingCourts`
- `CourtServiceTests.GetAvailableAsync_IncludesCourtWhenBookingDoesNotOverlap`
- `CourtServiceTests.ConfirmAsync_ChangesPendingBookingToConfirmed`

## Known verification gaps

- Real MySQL concurrency and migration test.
- Browser end-to-end test for login/reset/booking.
- SMTP delivery test with a non-production mailbox.
