# Walkthrough Prep Guide

This doc is a practical script for the 45-60 minute OPENLANE walkthrough described in `WALKTHROUGH.md`.

---

## 1) Demo (~5 min)

### Suggested demo flow
1. Open the app at `http://localhost:5173`.
2. Show inventory cards and explain data comes from the seeded backend dataset.
3. Use search (make/model/VIN/lot/dealership).
4. Apply make/province filters and sort options.
5. Click a vehicle and walk through detail data:
   - condition report
   - damage notes
   - dealership + lot
   - spec grid
6. Place a valid bid and show live UI state update.
7. Place an invalid bid (below minimum) and show validation feedback.

### 30-second summary line
“I built a buyer-side auction prototype with browse, detail, and bidding flows on a React frontend backed by an ASP.NET Core API with in-memory seeded data, plus automated tests and CI.”

---

## 2) Decisions (~15 min)

### Why this stack
- Reused patterns from `taskmanager2025` to move quickly with consistency:
  - ASP.NET Core service-layer API
  - React + Vite frontend
  - Zod contract validation
  - test-first hardening

### What I built first
1. End-to-end skeleton (API + frontend running together)
2. Core user flow (browse -> inspect -> bid)
3. Data validation and edge-case handling
4. Tests + CI + docs polish

### What I intentionally cut
- auth/accounts
- persistent database
- bid history timeline
- realtime websocket updates

### Tradeoffs
- In-memory DB gives speed for prototype, not persistence.
- Prioritized complete core flow and reliability over extra features.
- Focused on clear architecture and testability over broad surface area.

---

## 3) Code Deep Dive (~15 min)

### Backend highlights
- `backend/Program.cs`
  - DI, CORS, in-memory DB, startup seeding.
- `backend/Data/VehicleDataSeeder.cs`
  - Loads `data/vehicles.json`.
  - Normalizes null `current_bid` values.
- `backend/Services/VehiclesService.cs`
  - Search/filter/sort logic.
  - Bid rule enforcement:
    - minimum bid = `max(current_bid + 100, starting_bid)`
    - buy-now cap behavior.
- `backend/Controllers/VehiclesController.cs`
  - clean HTTP boundary and status handling.

### Frontend highlights
- `frontend/src/App.jsx`
  - orchestrates inventory state, filters, and bid submit behavior.
- `frontend/src/components/VehicleList.jsx`
  - inventory navigation and selection.
- `frontend/src/components/VehicleDetail.jsx`
  - detailed listing info + bid form.
- `frontend/src/apiSchemas.js`
  - Zod schemas to validate backend responses.

### Testing highlights
- Backend unit + integration:
  - `backend.Tests/VehiclesServiceTests.cs`
  - `backend.Tests/VehiclesApiTests.cs`
- Frontend interaction:
  - `frontend/src/App.test.jsx`
  - render/filter/bid success/bid validation scenarios.

---

## 4) Workflow Discussion (~15 min)

### How I worked
- Started from requirement clarity.
- Implemented a vertical slice fast.
- Repeated validation loop: lint, test, build, API smoke.
- Added CI to keep checks automatic.

### Tooling/process
- VS Code tasks for install/launch/tests.
- Git commits chunked by concern (feature -> tests -> CI).
- Local browser validation with Playwright for runtime confidence.

### Quality gate used
- `dotnet test TheBlock.sln`
- `npm run lint`
- `npm run test:run`
- `npm run build`
- live API smoke at `http://localhost:5117/api/vehicles`

---

## 5) Likely Questions + Strong Answers

### Q: Why service layer for a prototype?
A: It keeps business rules isolated and testable. Even in a prototype, bid logic is core domain behavior, so separating it from controllers reduces coupling and makes iteration safer.

### Q: What was the hardest issue?
A: Data realism issues (null current bids) and ensuring consistent behavior across frontend and backend validation. I fixed this at the seed layer and added tests for low-bid rejection paths.

### Q: What would you do next with more time?
A:
1. Persistent storage and bid history timeline.
2. Realtime outbid updates.
3. Auth + buyer identity.
4. Pagination/virtualization for larger inventories.
5. Observability and metrics.

### Q: How did you ensure reliability?
A: Contract validation with Zod, backend integration tests for endpoint behavior, frontend interaction tests, and CI gates for lint/test/build.

---

## 6) 90-Second Closing Pitch

“This prototype focuses on the buyer’s critical journey: discover inventory, assess listing confidence, and place bids with immediate feedback. I reused a proven architecture pattern for speed and maintainability, then invested in test coverage and CI so the core experience is reliable. Given more time, I’d extend into persistence, bid history, and realtime auction dynamics.”

---

## 7) Pre-Call Checklist

- Backend running: `http://localhost:5117`
- Frontend running: `http://localhost:5173`
- One vehicle selected for bid demo
- A valid and invalid bid amount ready
- Tests pass locally before call
- Have `README.md`, `Architecture.md`, and this prep doc open
