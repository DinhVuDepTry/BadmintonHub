# Operations, Scalability, and Limitations

## Deployment

- Build a Release publish or Docker image.
- Provide `ConnectionStrings__BadmintonDb` and SMTP variables through the deployment secret store.
- Apply EF migrations before first traffic.
- Put the app behind an HTTPS reverse proxy.
- Expose `/health` to the platform health probe.

## Current operational protections

- HSTS and forwarded headers in production.
- Generic exception responses.
- Fixed-window API rate limiter.
- Database-aware health check.
- Background booking lifecycle worker.
- Dockerfile and release publish workflow.

## Scale assumptions

The MVP targets one venue and low-to-moderate traffic. The main bottleneck is availability/overlap queries under concurrent booking traffic.

Recommended next steps when traffic grows:

1. Add a composite booking index for court/date/status/time.
2. Add pagination to booking/admin lists.
3. Add MySQL concurrency and load tests.
4. Add structured audit logging for Admin actions.
5. Add metrics for booking conflicts, latency, worker failures, and email failures.
6. Add database backup/restore verification.

## Known limitations and technical debt

- CourtOwner is intentionally out of scope; current RBAC is Admin/Customer.
- Payment, refund, and notification workflows are out of scope.
- SMTP sender logs a warning when not configured; production must configure an email provider.
- The current automated tests use EF InMemory; MySQL integration testing remains a submission follow-up.
- The worker runs in-process; multi-instance deployments should add a distributed job/locking strategy.

## Revisit triggers

- Add JWT or an external identity provider when a separate SPA/mobile client becomes primary.
- Split services only when independent scaling or team ownership justifies operational complexity.
- Add caching only after availability/read metrics show a database bottleneck.
