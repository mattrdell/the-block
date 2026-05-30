# The Block Buyer Console

A full-stack prototype for the OPENLANE "The Block" challenge.

This implementation follows the same technical decisions used in `mattrdell/taskmanager2025`:
- ASP.NET Core backend with a clear controller + service layer
- EF Core InMemory data store for a fast prototype loop
- React + Vite frontend
- Zod response validation in the UI
- Accessibility-minded controls and semantic HTML
- Basic automated tests for backend and frontend
- VS Code tasks for setup, launch, and test workflows

## What I Built

- Inventory browsing with card-based listing
- Search by make, model, VIN, lot, and dealership
- Filtering by make and province
- Sorting by auction timing, price, and condition grade
- Vehicle detail experience with specs, condition, damage notes, and dealership info
- Bid placement flow with server-side bid validation and live UI updates

## Architecture

### Backend (`backend/`)
- `Controllers/VehiclesController.cs`: browse, detail, and bid endpoints
- `Services/VehiclesService.cs`: query logic and bidding rules
- `Data/VehiclesContext.cs`: EF Core context (in-memory)
- `Data/VehicleDataSeeder.cs`: loads `data/vehicles.json` at startup
- `Models/`: request and vehicle domain models

### Frontend (`frontend/`)
- `src/App.jsx`: orchestration, state, filters, and API interaction
- `src/components/VehicleList.jsx`: inventory listing UI
- `src/components/VehicleDetail.jsx`: detail + bid UI
- `src/apiSchemas.js`: Zod schemas for API contracts
- `src/utils/formatters.js`: display formatting helpers

## API

- `GET /api/vehicles`
- `GET /api/vehicles/{id}`
- `POST /api/vehicles/{id}/bids`

Bid rules:
- New bid must be at least `max(current_bid + 100, starting_bid)`
- If bid is at/above buy-now price, bid is capped to buy-now amount

## Local Setup

### Prereqs
- .NET SDK 9+
- Node.js 22+

### 1. Install frontend packages
```bash
cd frontend
npm install
```

### 2. Run backend
```bash
cd backend
dotnet run --launch-profile "http"
```
Backend listens on `http://localhost:5117`.

### 3. Run frontend
In a second terminal:
```bash
cd frontend
npm run dev
```
Frontend runs at `http://localhost:5173` and proxies `/api` to the backend.

## Testing

From repo root:
```bash
dotnet test
```

From `frontend/`:
```bash
npm run test:run
```

## VS Code Tasks

Open `theblock.code-workspace`, then run:
- `Install All`
- `Launch All`
- `Run All Tests`

## Assumptions and Tradeoffs

- Prototype scope only, no authentication or user accounts
- In-memory backend (data resets on restart)
- Static synthetic dataset used as source of truth
- Bid history persistence/audit trail intentionally omitted for timebox

## What I'd Improve Next

1. Add bid history timelines and outbid notifications.
2. Add pagination/virtualization for larger inventory sets.
3. Add backend integration tests for controller-level behavior.
4. Add websocket updates for concurrent bidding visibility.
5. Add auth, role-based permissions, and persistent DB.

