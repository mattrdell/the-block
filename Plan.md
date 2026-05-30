# Build Plan

This plan reflects how the current project was built and hardened.

## 1. Intake And Constraints
- Confirm challenge requirements from `README.md`, `SUBMISSION.md`, and `WALKTHROUGH.md`.
- Use `taskmanager2025` as the technical blueprint for stack and patterns.
- Target outcome: runnable full-stack prototype with clear setup and tests.

## 2. Technical Baseline (Pattern Reuse)
- Backend: ASP.NET Core Web API, service layer, EF Core InMemory.
- Frontend: React + Vite, componentized UI, API-driven state.
- Validation: Zod schemas for frontend API contract checks.
- Workflow: VS Code tasks for install, launch, and testing.

## 3. Backend Delivery
- Scaffold backend and solution (`TheBlock.sln`).
- Implement domain model and request models:
  - `Vehicle`
  - `PlaceBidRequest`
- Implement persistence and seed flow:
  - `VehiclesContext`
  - `VehicleDataSeeder` loading `data/vehicles.json`
- Implement business logic in `VehiclesService`:
  - Browse and filtering support
  - Bid rules: minimum increment, buy-now cap
- Expose API endpoints via `VehiclesController`:
  - `GET /api/vehicles`
  - `GET /api/vehicles/{id}`
  - `POST /api/vehicles/{id}/bids`

## 4. Frontend Delivery
- Build inventory experience:
  - Search
  - Filter by make/province
  - Sort by timing, price, and grade
- Build details experience:
  - Specs and condition report
  - Damage notes
  - Dealer + lot metadata
- Build bid flow:
  - Client-side minimum bid enforcement
  - Server-side result handling and success/error states
- Add responsive styling and accessible form/controls.

## 5. Testing And Quality Hardening
- Backend unit tests for service rules.
- Backend API integration tests using `WebApplicationFactory`.
- Frontend interaction tests with Testing Library + Vitest.
- Lint, build, and smoke checks for local reliability.

## 6. CI Automation
- Add GitHub Actions workflow:
  - Backend restore + test
  - Frontend install + lint + test + build

## 7. Local Browser Automation Support
- Install Playwright in frontend tooling.
- Enable local browser smoke execution for page-level verification.

## 8. Commit Strategy Used
- Commit 1: full feature implementation and project structure.
- Commit 2: test coverage expansion and validation behavior.
- Commit 3: CI pipeline for repeatable quality checks.

## 9. Current Completion Definition
- Backend and frontend launch locally.
- API returns seeded dataset.
- Bid flow updates visible state and enforces rules.
- Tests and lint pass.
- CI executes the same quality gate automatically.
