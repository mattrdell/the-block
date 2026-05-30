import { currency, formatAuctionDate, integer } from '../utils/formatters'

function VehicleDetail({ vehicle, bidDraft, onBidDraftChange, onSubmitBid, busy, error, success }) {
  if (!vehicle) {
    return (
      <aside className="detail-panel empty" aria-live="polite">
        <p>Select a vehicle to view details and place a bid.</p>
      </aside>
    )
  }

  const minimumBid = Math.max(vehicle.current_bid + 100, vehicle.starting_bid)

  return (
    <aside className="detail-panel" aria-live="polite">
      <img
        src={vehicle.images[0]}
        alt={`${vehicle.year} ${vehicle.make} ${vehicle.model} featured photo`}
        className="hero-photo"
      />
      <div className="detail-content">
        <p className="lot-badge">Lot {vehicle.lot}</p>
        <h2>{vehicle.year} {vehicle.make} {vehicle.model} {vehicle.trim}</h2>
        <p className="dealership">Sold by {vehicle.selling_dealership}</p>

        <dl className="spec-grid">
          <div><dt>Current Bid</dt><dd>{currency.format(vehicle.current_bid)}</dd></div>
          <div><dt>Starting Bid</dt><dd>{currency.format(vehicle.starting_bid)}</dd></div>
          <div><dt>Condition</dt><dd>{vehicle.condition_grade.toFixed(1)} / 5</dd></div>
          <div><dt>Odometer</dt><dd>{integer.format(vehicle.odometer_km)} km</dd></div>
          <div><dt>Engine</dt><dd>{vehicle.engine}</dd></div>
          <div><dt>Drivetrain</dt><dd>{vehicle.drivetrain}</dd></div>
          <div><dt>Fuel</dt><dd>{vehicle.fuel_type}</dd></div>
          <div><dt>Auction Start</dt><dd>{formatAuctionDate(vehicle.auction_start)}</dd></div>
        </dl>

        <section>
          <h3>Condition Report</h3>
          <p>{vehicle.condition_report}</p>
        </section>

        <section>
          <h3>Damage Notes</h3>
          {vehicle.damage_notes.length === 0 ? (
            <p>No damage notes listed.</p>
          ) : (
            <ul className="damage-list">
              {vehicle.damage_notes.map((note) => <li key={note}>{note}</li>)}
            </ul>
          )}
        </section>

        <form
          className="bid-form"
          noValidate
          onSubmit={(event) => {
            event.preventDefault()
            onSubmitBid(minimumBid)
          }}
        >
          <h3>Place Bid</h3>
          <p>Minimum next bid: {currency.format(minimumBid)}</p>
          <label htmlFor="bidAmount">Your bid (CAD)</label>
          <input
            id="bidAmount"
            type="number"
            step="100"
            min={minimumBid}
            value={bidDraft}
            onChange={(event) => onBidDraftChange(event.target.value)}
            required
          />
          <button type="submit" disabled={busy}>
            {busy ? 'Submitting...' : 'Submit Bid'}
          </button>
          {error ? <p className="form-error" role="alert">{error}</p> : null}
          {success ? <p className="form-success" role="status">{success}</p> : null}
        </form>
      </div>
    </aside>
  )
}

export default VehicleDetail

