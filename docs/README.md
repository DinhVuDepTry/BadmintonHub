# BadmintonHub Analysis and Design Dossier

This dossier is the submission companion for the Advanced Web Programming project and maps the implementation to the analysis/design rubric.

## Reading order

0. [Submission Cover](00-submission-cover.md)
1. [Requirements](01-requirements.md)
2. [Use Cases](02-use-cases.md)
3. [User Stories and Acceptance Criteria](03-user-stories.md)
4. [Domain and ERD](04-domain-and-erd.md)
5. [System Design](05-system-design.md)
6. [Sequences and API Contract](06-sequences-and-api.md)
7. [Architecture Decisions](07-adr.md)
8. [Threat Model and RBAC](08-threat-model-rbac.md)
9. [Traceability Matrix](09-traceability-matrix.md)
10. [Operations and Limitations](10-operations.md)
11. [Submission Checklist](11-submission-checklist.md)
12. [Member Contributions](12-member-contributions.md)

## Scope statement

BadmintonHub is a modular ASP.NET Core MVC/Razor application for one badminton venue. It supports public court discovery, customer reservations, Admin approval/management, Identity authentication, role authorization, REST APIs, and an automatic booking lifecycle.

## Source and tools

- Source of truth: the BadmintonHub codebase in this repository.
- Framework/runtime: ASP.NET Core/.NET 8.
- Persistence: EF Core and MySQL.
- Diagrams: Mermaid syntax rendered by compatible Markdown viewers.
- Verification: `dotnet build` and `dotnet test .\Tests\BadmintonHub.Tests.csproj`.
- AI assistance: used for code review, documentation drafting, and consistency checking; final decisions were checked against the source code and tests.

## Rubric coverage

- Analysis/design: requirements, use cases, stories, domain model, architecture, sequences.
- Trade-offs: four ADRs with options, rationale, consequences, and revisit triggers.
- Security: assets, boundaries, threats, mitigations, RBAC matrix, and negative-test plan.
- Traceability: goal -> use case -> requirement -> implementation -> test/evidence.
- Operations: deployment, health, rate limiting, lifecycle worker, scalability assumptions, and limitations.
