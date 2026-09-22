# Architecture Decision Records

## ADR-001: Use Razor MVC as the primary web client

- **Context:** The course requires an advanced web system, and the application has server-rendered workflows with Identity.
- **Options:** Razor MVC; React/SPA with separate API; server-side Blazor.
- **Decision:** Use ASP.NET Core MVC/Razor plus a separate REST API.
- **Rationale:** One deployable application, direct Identity integration, simple form validation, and sufficient separation for API consumers.
- **Benefits:** Lower deployment complexity and fast academic delivery.
- **Costs/risks:** Less interactive than a SPA; client-side experience depends on Razor views.
- **Revisit trigger:** A mobile/SPA client becomes a primary product requirement.

## ADR-002: Use cookie Identity authentication instead of JWT

- **Context:** The primary client is a browser-based Razor MVC application.
- **Options:** Identity cookie; JWT bearer tokens; external identity provider only.
- **Decision:** Use ASP.NET Identity cookies, role claims, CSRF protection, and API 401/403 redirect overrides.
- **Rationale:** Cookies fit server-rendered forms; JWT would add token storage/refresh complexity without a separate frontend requirement.
- **Benefits:** Integrated login, logout, role checks, and secure browser session behavior.
- **Costs/risks:** Cookie-authenticated APIs require CSRF protection; mobile clients would need an additional auth strategy.
- **Revisit trigger:** A public mobile or separate SPA client is introduced.

## ADR-003: Use a modular monolith

- **Context:** Scope is one venue and a six-week academic project.
- **Options:** Modular monolith; microservices; serverless functions.
- **Decision:** Keep MVC, API, services, worker, and persistence in one deployable application.
- **Rationale:** The domain is small and shared transactions are important for booking correctness.
- **Benefits:** Simple deployment, debugging, and database transaction boundaries.
- **Costs/risks:** Scale-out boundaries are less independent.
- **Revisit trigger:** Multiple venues, independent teams, or sustained load requiring separate scaling.

## ADR-004: Use transaction locking plus overlap validation

- **Context:** Two customers must not reserve the same court/time range.
- **Options:** Application check only; transaction with row/range locking; external distributed lock.
- **Decision:** Use a database transaction, `FOR UPDATE` overlap query, and service-level validation.
- **Rationale:** The database is the source of truth and the current scope is a single MySQL database.
- **Benefits:** Centralized consistency and no extra infrastructure.
- **Costs/risks:** Concurrency behavior must be verified with real MySQL and indexes.
- **Revisit trigger:** High contention or multi-database deployment.
