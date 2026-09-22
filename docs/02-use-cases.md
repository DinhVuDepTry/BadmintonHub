# Use Cases

## UC-01 Search and reserve a court

**Primary actor:** Customer  
**Supporting actors:** Database  
**Goal:** Reserve an active court for a future time range.

**Preconditions**

- Customer is authenticated with role `Customer`.
- Requested date/time is in the future.
- Selected court is active.

**Main flow**

1. Customer opens the court list.
2. Customer enters date, start time, and end time.
3. System returns active courts with no overlapping Pending or Confirmed reservation.
4. Customer selects a court and opens the reservation form.
5. Customer submits the date and time.
6. System validates the court, time range, date, status, and overlap.
7. System calculates total price from duration and hourly price.
8. System creates a Pending reservation.
9. System redirects the customer to their reservation list.

**Alternative and exception flows**

- A1: End time is not after start time -> return validation error.
- A2: Date/time is in the past -> reject the request.
- A3: Court is inactive -> reject the request.
- A4: Another reservation overlaps -> return HTTP 409.
- A5: Database save fails -> do not expose internal details; transaction is rolled back.

**Postconditions**

- A Pending booking exists with CustomerId, CourtId, date, time, status, and calculated price.

## UC-02 Admin confirms a reservation

**Primary actor:** Admin

**Preconditions:** Admin is authenticated; reservation exists and is Pending; reservation has not ended.

**Main flow:** Admin opens booking management, selects a Pending reservation, and submits Confirm. The system changes status to Confirmed and returns to the booking list.

**Exceptions:** Missing booking or non-Pending/ended booking -> no state change and not found/invalid result.

## UC-03 Customer cancels a reservation

**Primary actor:** Customer

**Preconditions:** Customer owns the reservation; status is Pending or Confirmed; start is more than two hours away.

**Main flow:** Customer submits Cancel. The service verifies ownership and policy, changes status to Cancelled, and returns to the reservation list.

**Exceptions:** Cross-user cancellation, completed/expired/cancelled status, or less than two hours before start -> operation is rejected.

## UC-04 Admin manages courts

**Primary actor:** Admin

**Main flow:** Admin creates, edits status/price, or deletes a court. The service normalizes court code/type, validates duplicate code, and prevents deletion when bookings exist.

## UC-05 Recover account password

**Primary actor:** Guest/User

**Main flow:** User submits an email, receives a generic confirmation response, follows the SMTP reset link, submits a new password, and is redirected to login.

**Security behavior:** The response does not reveal whether the email exists. SMTP credentials are configuration secrets, never source code.
