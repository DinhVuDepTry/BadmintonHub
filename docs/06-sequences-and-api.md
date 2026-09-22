# Sequences and API Contract

## Booking sequence

```mermaid
sequenceDiagram
    actor Customer
    participant UI as Razor UI/API Client
    participant Controller
    participant BookingService
    participant CourtService
    participant DB as MySQL

    Customer->>UI: Select date and time
    UI->>CourtService: GetAvailableAsync(date, start, end)
    CourtService->>DB: Query active courts and overlaps
    DB-->>CourtService: Available courts
    CourtService-->>UI: Court list
    Customer->>Controller: POST booking
    Controller->>BookingService: CreateAsync(customerId, DTO)
    BookingService->>DB: Load court and begin transaction
    BookingService->>DB: Lock/query active overlaps
    alt overlap or invalid court/time
        BookingService-->>Controller: Business error
        Controller-->>UI: 409/validation error
    else valid
        BookingService->>DB: Insert Pending booking
        DB-->>BookingService: booking id
        BookingService-->>Controller: Booking DTO
        Controller-->>UI: 201/redirect
    end
```

## Core endpoints

| Method | Endpoint | Auth | Success | Main errors |
|---|---|---|---|---|
| GET | `/api/courts` | Public | 200 court list | 400 invalid range |
| GET | `/api/courts?date=...&startTime=...&endTime=...` | Public | 200 available courts | 409 invalid range |
| GET | `/api/courts/{id}` | Public | 200 court | 404 |
| POST | `/api/courts` | Admin + CSRF | 201 | 401/403/409 |
| PUT | `/api/courts/{id}` | Admin + CSRF | 200 | 401/403/404/409 |
| DELETE | `/api/courts/{id}` | Admin + CSRF | 204 | 401/403/404/409 |
| GET | `/api/bookings` | Admin/Customer | 200 scoped list | 401 |
| GET | `/api/bookings/{id}` | Admin/owner | 200 booking | 401/403/404 |
| POST | `/api/bookings` | Customer + CSRF | 201 Pending | 401/403/409 |
| POST | `/api/bookings/{id}/confirm` | Admin + CSRF | 204 | 401/403/404 |
| DELETE | `/api/bookings/{id}` | Admin/owner + CSRF | 204 | 401/403/404 |
| GET | `/api/security/csrf` | Authenticated | 200 token | 401 |
| GET | `/health` | Public | 200/503 | 503 |

## DTO examples

```json
{
  "courtId": 1,
  "bookingDate": "2026-10-01",
  "startTime": "18:00:00",
  "endTime": "20:00:00"
}
```

```json
{
  "bookingId": 10,
  "courtId": 1,
  "courtName": "Court One",
  "customerId": "user-id",
  "bookingDate": "2026-10-01",
  "startTime": "18:00:00",
  "endTime": "20:00:00",
  "status": "Pending",
  "totalPrice": 200000
}
```
