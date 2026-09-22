# User Stories and Acceptance Criteria

## US-001 Discover availability

**As a** Guest or Customer, **I want** to filter courts by date and time, **so that** I can find a court that is actually available.

- **AC-001:** Given a valid future date and time range, only Active courts without overlapping Pending/Confirmed bookings are returned.
- **AC-002:** Given an end time not after the start time, the request is rejected with a business validation error.
- **AC-003:** Cancelled and Expired bookings do not block availability.

## US-002 Create a booking

**As a** Customer, **I want** to reserve a court, **so that** I can play at a selected time.

- **AC-004:** A Guest or non-Customer cannot create a booking.
- **AC-005:** A booking for an inactive court is rejected.
- **AC-006:** A successful booking starts in Pending status and has calculated total price.
- **AC-007:** A duplicate time range returns HTTP 409 and creates no new booking.

## US-003 Administer reservations

**As an** Admin, **I want** to view and confirm Pending bookings, **so that** the venue can approve reservations.

- **AC-008:** Admin can view all bookings; Customer can view only their own.
- **AC-009:** Only Admin can confirm a Pending booking.
- **AC-010:** Confirmed bookings become Completed after their end time through the worker.

## US-004 Cancel a reservation

**As a** Customer, **I want** to cancel an eligible booking, **so that** I can release a time slot.

- **AC-011:** A Customer can cancel only their own Pending/Confirmed booking.
- **AC-012:** Cancellation is rejected within two hours of the start time.
- **AC-013:** Admin can cancel a Pending/Confirmed booking subject to the same service state rules.

## US-005 Manage courts

**As an** Admin, **I want** to manage court data and status, **so that** unavailable courts cannot be booked.

- **AC-014:** Court codes are unique and normalized.
- **AC-015:** Maintenance/Inactive courts are hidden from public availability and rejected by booking creation.
- **AC-016:** A court with related bookings cannot be deleted.

## US-006 Manage account

**As a** User, **I want** to update my profile and password, **so that** my account remains accurate and secure.

- **AC-017:** Profile updates validate name and phone number.
- **AC-018:** Password change requires the current password and a matching new password confirmation.
- **AC-019:** Password recovery returns the same generic response for known and unknown emails.
