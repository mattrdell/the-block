# Architecture

## Overview

The system is a two-tier web application:
- React/Vite frontend for buyer workflows
- ASP.NET Core API backend for inventory and bidding logic

The backend loads a static dataset (`data/vehicles.json`) into an in-memory EF Core store at startup.

## Component Map

### Frontend (`frontend/`)
- `src/App.jsx`
  - App-level state management
  - Data loading from `/api/vehicles`
  - Search/filter/sort orchestration
  - Bid submit flow and result handling
- `src/components/VehicleList.jsx`
  - Card grid for inventory navigation
- `src/components/VehicleDetail.jsx`
  - Detailed specs and bidding UI
- `src/apiSchemas.js`
  - Zod schemas for API response validation
- `src/utils/formatters.js`
  - Currency/date/number formatting

### Backend (`backend/`)
- `Program.cs`
  - Dependency injection setup
  - In-memory DB registration
  - CORS and controller mapping
  - Startup seed execution
- `Controllers/VehiclesController.cs`
  - HTTP API contract for browse/detail/bid
- `Services/VehiclesService.cs`
  - Query/filter/sort behavior
  - Bid rule enforcement and updates
- `Data/VehiclesContext.cs`
  - EF Core context and list conversion/value comparison
- `Data/VehicleDataSeeder.cs`
  - JSON data load + normalization into DB
- `Models/Vehicle.cs`, `Models/PlaceBidRequest.cs`
  - Domain and request DTOs

### Tests (`backend.Tests/`, `frontend/src/App.test.jsx`)
- Service-level rule checks
- API integration checks
- UI interaction checks (render/filter/bid success/bid validation)

## Runtime Flow

1. Backend boots and seeds vehicles into InMemory DB.
2. Frontend fetches `/api/vehicles`.
3. User filters and selects a vehicle.
4. User submits bid via `POST /api/vehicles/{id}/bids`.
5. Backend validates and mutates vehicle state.
6. Frontend validates response (Zod) and re-renders with updated bid values.

## Data Model Notes

`Vehicle` includes:
- Identity and listing metadata (id, vin, lot, dealer)
- Specs and condition details
- Auction pricing (`starting_bid`, `current_bid`, `reserve_price`, `buy_now_price`)
- Bidding counters (`bid_count`)

Seed normalization:
- Records with `current_bid: null` are normalized to `starting_bid` at load time.

## Non-Goals In Current Version

- Auth, user accounts, or bidder identity lifecycle
- Persistent storage beyond process lifetime
- Bid history timeline or outbid notification system
- Realtime auction broadcast channels
