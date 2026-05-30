import { currency } from '../utils/formatters'

function VehicleList({ vehicles, selectedId, onSelect }) {
  if (vehicles.length === 0) {
    return <p className="empty-state">No vehicles match this search.</p>
  }

  return (
    <ul className="inventory-grid" aria-label="Vehicle inventory">
      {vehicles.map((vehicle) => {
        const isSelected = selectedId === vehicle.id

        return (
          <li key={vehicle.id}>
            <button
              className={`vehicle-card ${isSelected ? 'selected' : ''}`}
              type="button"
              onClick={() => onSelect(vehicle.id)}
              aria-pressed={isSelected}
            >
              <img
                src={vehicle.images[0]}
                alt={`${vehicle.year} ${vehicle.make} ${vehicle.model}`}
                className="card-image"
                loading="lazy"
              />
              <div className="card-body">
                <p className="card-lot">Lot {vehicle.lot}</p>
                <h2>{vehicle.year} {vehicle.make} {vehicle.model}</h2>
                <p>{vehicle.trim} • {vehicle.city}, {vehicle.province}</p>
                <div className="card-meta">
                  <span>{currency.format(vehicle.current_bid)}</span>
                  <span>{vehicle.bid_count} bids</span>
                </div>
              </div>
            </button>
          </li>
        )
      })}
    </ul>
  )
}

export default VehicleList

