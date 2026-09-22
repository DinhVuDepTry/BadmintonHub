# Threat Model and RBAC

## Assets

- User credentials and password reset tokens.
- Customer profile and contact data.
- Booking ownership and schedule integrity.
- Court availability and prices.
- Admin operations and revenue metrics.
- SMTP and database credentials.

## Entry points and trust boundaries

- Public Razor pages and public court API.
- Authenticated Razor pages and cookie session.
- Authenticated state-changing API endpoints.
- Identity registration/login/reset endpoints.
- SMTP and MySQL are external infrastructure boundaries.

## Role-permission matrix

| Resource/action | Guest | Customer | Admin |
|---|---:|---:|---:|
| Read active courts/availability | Allow | Allow | Allow |
| Create booking | Deny | Allow | Deny |
| Read own bookings | Deny | Allow | Allow |
| Read all bookings | Deny | Deny | Allow |
| Cancel own booking | Deny | Allow | Allow |
| Confirm booking | Deny | Deny | Allow |
| Create/update/delete court | Deny | Deny | Allow |
| Admin dashboard | Deny | Deny | Allow |
| Update own profile/password | Deny | Allow | Allow |

## Threats and mitigations

| ID | Threat | Risk | Mitigation | Verification |
|---|---|---:|---|---|
| T-001 | IDOR reads another customer's booking | High | Controller and service ownership checks | API request as second Customer -> 403 |
| T-002 | Unauthorized court mutation | High | Admin role attributes and service boundary | Guest/Customer mutation -> 401/403 |
| T-003 | CSRF on cookie-authenticated API | High | Anti-forgery attributes and `/api/security/csrf` | Missing token -> rejected |
| T-004 | Double booking race | High | Transaction, lock query, overlap validation | Concurrent MySQL integration test |
| T-005 | Account enumeration in password reset | Medium | Generic confirmation response | Known/unknown email responses are equivalent |
| T-006 | Brute-force login | Medium | Identity lockout and password policy | Five failed attempts lock account |
| T-007 | SMTP/database secret exposure | High | User Secrets/environment variables; no source secrets | Secret scan and deployment review |
| T-008 | Abuse of public/API endpoints | Medium | Rate limiting and validation | Exceed rate window -> limited response |
| T-009 | Sensitive exception leakage | Medium | Generic 500 response and structured logging target | Trigger unexpected error in production mode |

## Security design principles

- Default deny for protected controllers.
- Least privilege: Customer cannot manage courts or confirm bookings.
- Server-side ownership checks; UI hiding is not authorization.
- CSRF for cookie-authenticated writes.
- Secrets externalized from source control.
