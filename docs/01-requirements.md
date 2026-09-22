# Requirements

## Scope

BadmintonHub is a web system for managing badminton courts and reservations for one venue. The current scope includes public court discovery, customer reservations, administrator operations, authentication, and a REST API.

Out of scope: online payment, multi-venue ownership, chat, reviews, mobile applications, and social features.

## Actors

- **Guest**: browses active courts and availability; can register or log in.
- **Customer**: manages a profile, searches availability, creates reservations, views own reservations, and cancels within policy.
- **Admin**: manages courts, views all reservations, confirms reservations, cancels reservations, and monitors the dashboard.

## Functional Requirements

| ID | Requirement | Priority | Acceptance evidence |
|---|---|---:|---|
| FR-001 | Guest can view active courts and prices. | Must | `GET /api/courts`, `/Courts` |
| FR-002 | User can search courts by date and time range. | Must | Availability filter and `GET /api/courts?date=...` |
| FR-003 | Customer can create a reservation for an active court. | Must | `POST /api/bookings`, `/Bookings/Create` |
| FR-004 | The system rejects overlapping reservations for the same court/date. | Must | Conflict response and service test |
| FR-005 | Customer can view only their own reservations. | Must | Ownership filter in booking service/controller |
| FR-006 | Customer can cancel an eligible reservation at least two hours before start. | Must | Cancellation policy in service |
| FR-007 | Admin can create, edit, deactivate, and delete courts when no bookings exist. | Must | Admin court UI/API |
| FR-008 | Admin can confirm a pending reservation. | Must | `/api/bookings/{id}/confirm`, Admin UI |
| FR-009 | The worker changes overdue pending/confirmed reservations to Expired/Completed. | Must | `BookingLifecycleWorker` |
| FR-010 | User can register, log in, recover password, update profile, and change password. | Must | Identity pages |
| FR-011 | Admin can view court, pending booking, daily booking, and daily revenue metrics. | Should | `/Admin` dashboard |
| FR-012 | API write operations require authentication, role authorization, and CSRF protection. | Must | API attributes and security endpoint |

## Non-functional Requirements

| ID | Requirement | Measure |
|---|---|---|
| NFR-001 | Password security | Minimum 8 characters, uppercase, digit, lockout after 5 failures. |
| NFR-002 | Authorization | Unauthorized API requests return 401/403; ownership is checked server-side. |
| NFR-003 | Data integrity | Court code is unique; court deletion is restricted when bookings exist. |
| NFR-004 | Availability correctness | Overlap rule treats time ranges as half-open: `[start, end)`. |
| NFR-005 | Operability | `/health` returns 200 with database access and 503 otherwise. |
| NFR-006 | Abuse resistance | API rate limit is 120 requests per minute per fixed limiter policy. |
| NFR-007 | Deployment | Release build and Dockerfile are provided; secrets come from configuration. |
| NFR-008 | Maintainability | Business rules are isolated in services and covered by automated tests. |

## Assumptions and Constraints

- The system serves one venue and uses local venue time for booking rules.
- MySQL is the production database.
- Identity cookie authentication is used because the primary client is server-rendered Razor MVC.
- Payment is outside the current academic MVP scope.
