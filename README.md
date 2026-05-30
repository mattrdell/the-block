# The Block Buyer Console

A full-stack prototype for the OPENLANE "The Block" challenge.

This implementation intentionally mirrors technical decisions from `mattrdell/taskmanager2025`:
- ASP.NET Core API with controller + service separation
- EF Core InMemory storage for a fast prototype loop
- React + Vite frontend
- Zod validation of API responses in UI code
- Automated tests and repeatable local/CI workflows

## What It Does

- Browse inventory from the provided 200-vehicle dataset
- Search by make, model, VIN, lot, and dealership
- Filter by make and province
- Sort by auction timing, current bid, and condition grade
- Inspect detailed vehicle info (specs, condition, damage, dealership)
- Place bids with server-side rules and immediate UI updates

## Project Documents

- Build plan: `Plan.md`
- System design: `Architecture.md`
- Runtime/automation roles: `Agents.md`

## Tech Stack

- Backend: .NET 9, ASP.NET Core Web API, EF Core InMemory
- Frontend: React 19, Vite 8, Zod
- Testing:
  - Backend: xUnit + ASP.NET Core integration testing
  - Frontend: Vitest + Testing Library
- Automation: GitHub Actions CI, optional Playwright local browser checks

## API

- `GET /api/vehicles`
- `GET /api/vehicles/{id}`
- `POST /api/vehicles/{id}/bids`

Bid rules:
- Bid must be at least `max(current_bid + 100, starting_bid)`
- If bid meets/exceeds `buy_now_price`, bid is capped to `buy_now_price`

## Local Setup

### Prereqs
- .NET SDK 9+
- Node.js 22+

### Install
```bash
cd frontend
npm install
```

### Run Backend
```bash
cd backend
dotnet run --launch-profile "http"
```
Backend URL: `http://localhost:5117`

### Run Frontend
In a second terminal:
```bash
cd frontend
npm run dev
```
Frontend URL: `http://localhost:5173`

## Testing

From repo root:
```bash
dotnet test TheBlock.sln
```

From `frontend/`:
```bash
npm run lint
npm run test:run
npm run test:ax
npm run build
```

Playwright AX scan (requires running frontend app):
```bash
npm run test:ax:playwright
```

## CI

GitHub Actions workflow (`.github/workflows/ci.yml`) runs:
- Backend restore + tests
- Frontend install + lint + tests (including AX vitest suite) + build

## Playwright MCP Recommendations

For local browser automation/debug loops, use Playwright through Docker MCP:

```bash
docker mcp gateway run --profile playwright
```

Recommended usage:
- Keep backend and frontend running before browser checks.
- Prefer `http://127.0.0.1:5173` over `localhost` if you see intermittent connection issues.
- Run a smoke check after major UI changes:
  - page loads
  - heading renders (`The Block Buyer Console`)
  - inventory cards are visible
  - bid form is interactive
- Keep Playwright as a local validation tool; CI remains lint/test/build focused.

## VS Code Workflow

Open `theblock.code-workspace` and run tasks from `.vscode/tasks.json`:
- `Install All`
- `Launch All`
- `Run All Tests`

## Notes And Tradeoffs

- Prototype scope only; no authentication/authorization
- In-memory backend resets on restart
- No persistent bid history/audit timeline yet
- Dataset normalization handles null `current_bid` entries during seed

## Next Improvements

1. Persist bid history and expose timeline API.
2. Add realtime auction updates (websockets/SSE).
3. Add pagination/virtualized inventory rendering.
4. Add auth and role-based access.
5. Add observability and structured error telemetry.
