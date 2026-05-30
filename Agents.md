# Agents

This document describes active "agents" (execution roles) in the project and how they cooperate.

## 1. UI Agent (Frontend Runtime)

Location:
- `frontend/src/App.jsx`
- `frontend/src/components/*`

Responsibilities:
- Render buyer-facing auction experience
- Manage search/filter/sort and selected vehicle state
- Submit bids to backend API
- Validate incoming API payloads with Zod
- Show success/error feedback for bid actions

Inputs:
- `GET /api/vehicles`
- `POST /api/vehicles/{id}/bids`

Outputs:
- User-visible inventory, details, and bid feedback

## 2. Domain Agent (Backend Service Layer)

Location:
- `backend/Services/VehiclesService.cs`

Responsibilities:
- Execute browse/filter/sort behavior
- Enforce bid business rules
- Update current bid and bid count

Inputs:
- Controller requests and query parameters

Outputs:
- Validated domain results to controllers

## 3. API Agent (HTTP Boundary)

Location:
- `backend/Controllers/VehiclesController.cs`

Responsibilities:
- Expose HTTP endpoints
- Validate request model state
- Return status codes and response payloads
- Translate service outcomes into API contracts

## 4. Data Agent (Seeder + Context)

Location:
- `backend/Data/VehicleDataSeeder.cs`
- `backend/Data/VehiclesContext.cs`

Responsibilities:
- Load dataset from `data/vehicles.json`
- Normalize seed anomalies (null `current_bid`)
- Manage in-memory persistence model conversion details

## 5. Verification Agents (Test Suites)

### Backend Test Agent
Location:
- `backend.Tests/VehiclesServiceTests.cs`
- `backend.Tests/VehiclesApiTests.cs`

Responsibilities:
- Validate service rules and API behavior
- Prevent regressions in bid logic and endpoint responses

### Frontend Test Agent
Location:
- `frontend/src/App.test.jsx`

Responsibilities:
- Validate render, filtering, and bid UX behavior
- Verify client-side bid validation path

## 6. CI Agent (GitHub Actions)

Location:
- `.github/workflows/ci.yml`

Responsibilities:
- Run backend and frontend quality gates on push/PR
- Keep lint/test/build health visible and repeatable

## 7. Optional Browser Automation Agent

Location:
- Local development dependency: `playwright`

Responsibilities:
- Perform local page-level smoke checks against running frontend
- Support interactive validation when diagnosing runtime issues

## Coordination Model

- UI Agent consumes API Agent contracts.
- API Agent delegates business decisions to Domain Agent.
- Domain Agent reads/writes through Data Agent.
- Verification Agents test each boundary.
- CI Agent enforces the verification workflow continuously.
