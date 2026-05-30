import { useEffect, useMemo, useState } from 'react'
import './App.css'
import VehicleList from './components/VehicleList'
import VehicleDetail from './components/VehicleDetail'
import { VehicleSchema, VehiclesSchema } from './apiSchemas'

const API_URL = '/api/vehicles'

function sortVehicles(vehicles, sortBy) {
  const data = [...vehicles]

  switch (sortBy) {
    case 'priceHigh':
      return data.sort((a, b) => b.current_bid - a.current_bid)
    case 'priceLow':
      return data.sort((a, b) => a.current_bid - b.current_bid)
    case 'grade':
      return data.sort((a, b) => b.condition_grade - a.condition_grade)
    case 'endingSoon':
      return data.sort((a, b) => new Date(a.auction_start) - new Date(b.auction_start))
    default:
      return data.sort((a, b) => new Date(b.auction_start) - new Date(a.auction_start))
  }
}

function App() {
  const [vehicles, setVehicles] = useState([])
  const [selectedId, setSelectedId] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  const [search, setSearch] = useState('')
  const [makeFilter, setMakeFilter] = useState('all')
  const [provinceFilter, setProvinceFilter] = useState('all')
  const [sortBy, setSortBy] = useState('endingSoon')

  const [bidDraft, setBidDraft] = useState('')
  const [bidBusy, setBidBusy] = useState(false)
  const [bidError, setBidError] = useState(null)
  const [bidSuccess, setBidSuccess] = useState(null)

  useEffect(() => {
    async function loadVehicles() {
      try {
        setLoading(true)
        const response = await fetch(API_URL)
        const json = await response.json()
        const parsed = VehiclesSchema.safeParse(json)

        if (!parsed.success) {
          setError('Vehicle response format is invalid.')
          return
        }

        setVehicles(parsed.data)
        if (parsed.data.length > 0) {
          setSelectedId(parsed.data[0].id)
        }
      } catch (loadError) {
        console.error(loadError)
        setError('Could not reach the API. Check that backend is running.')
      } finally {
        setLoading(false)
      }
    }

    loadVehicles()
  }, [])

  const makes = useMemo(() => [...new Set(vehicles.map((vehicle) => vehicle.make))].sort(), [vehicles])
  const provinces = useMemo(() => [...new Set(vehicles.map((vehicle) => vehicle.province))].sort(), [vehicles])

  const filteredVehicles = useMemo(() => {
    const keyword = search.trim().toLowerCase()

    const filtered = vehicles.filter((vehicle) => {
      if (makeFilter !== 'all' && vehicle.make !== makeFilter) {
        return false
      }

      if (provinceFilter !== 'all' && vehicle.province !== provinceFilter) {
        return false
      }

      if (!keyword) {
        return true
      }

      return [
        vehicle.make,
        vehicle.model,
        vehicle.trim,
        vehicle.vin,
        vehicle.lot,
        vehicle.selling_dealership
      ].some((field) => field.toLowerCase().includes(keyword))
    })

    return sortVehicles(filtered, sortBy)
  }, [vehicles, search, makeFilter, provinceFilter, sortBy])

  const effectiveSelectedId = useMemo(() => {
    if (filteredVehicles.length === 0) {
      return null
    }

    const currentSelectionStillVisible = filteredVehicles.some((vehicle) => vehicle.id === selectedId)
    return currentSelectionStillVisible ? selectedId : filteredVehicles[0].id
  }, [filteredVehicles, selectedId])

  const selectedVehicle = useMemo(
    () => vehicles.find((vehicle) => vehicle.id === effectiveSelectedId) ?? null,
    [vehicles, effectiveSelectedId]
  )

  async function handleSubmitBid(minimumBid) {
    if (!selectedVehicle) {
      return
    }

    const amount = Number(bidDraft)
    if (!Number.isFinite(amount) || amount < minimumBid) {
      setBidError(`Bid must be at least ${minimumBid.toLocaleString('en-CA', { style: 'currency', currency: 'CAD', maximumFractionDigits: 0 })}.`)
      setBidSuccess(null)
      return
    }

    setBidBusy(true)
    setBidError(null)
    setBidSuccess(null)

    try {
      const response = await fetch(`${API_URL}/${selectedVehicle.id}/bids`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount })
      })

      const json = await response.json()

      if (!response.ok) {
        setBidError(json.message ?? 'Bid failed.')
        return
      }

      const parsed = VehicleSchema.safeParse(json)
      if (!parsed.success) {
        setBidError('Bid was accepted, but response could not be validated.')
        return
      }

      setVehicles((current) => current.map((vehicle) => vehicle.id === parsed.data.id ? parsed.data : vehicle))
      setBidDraft('')
      setBidSuccess('Bid placed successfully.')
    } catch (submitError) {
      console.error(submitError)
      setBidError('Bid request failed. Please try again.')
    } finally {
      setBidBusy(false)
    }
  }

  return (
    <div className="app-shell">
      <header>
        <p className="eyebrow">OPENLANE Challenge Prototype</p>
        <h1>The Block Buyer Console</h1>
        <p>Browse inventory, inspect condition details, and place bids in real time.</p>
      </header>

      <section className="toolbar" aria-label="Inventory controls">
        <input
          aria-label="Search inventory"
          placeholder="Search by make, model, VIN, lot, or dealership"
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />

        <select aria-label="Filter by make" value={makeFilter} onChange={(event) => setMakeFilter(event.target.value)}>
          <option value="all">All makes</option>
          {makes.map((make) => <option key={make} value={make}>{make}</option>)}
        </select>

        <select aria-label="Filter by province" value={provinceFilter} onChange={(event) => setProvinceFilter(event.target.value)}>
          <option value="all">All provinces</option>
          {provinces.map((province) => <option key={province} value={province}>{province}</option>)}
        </select>

        <select aria-label="Sort vehicles" value={sortBy} onChange={(event) => setSortBy(event.target.value)}>
          <option value="endingSoon">Auction start (earliest)</option>
          <option value="priceHigh">Current bid (high to low)</option>
          <option value="priceLow">Current bid (low to high)</option>
          <option value="grade">Condition grade (high to low)</option>
        </select>
      </section>

      {loading ? <p className="status">Loading inventory...</p> : null}
      {error ? <p className="status error" role="alert">{error}</p> : null}

      {!loading && !error ? (
        <main className="layout">
          <VehicleList vehicles={filteredVehicles} selectedId={effectiveSelectedId} onSelect={setSelectedId} />
          <VehicleDetail
            vehicle={selectedVehicle}
            bidDraft={bidDraft}
            onBidDraftChange={setBidDraft}
            onSubmitBid={handleSubmitBid}
            busy={bidBusy}
            error={bidError}
            success={bidSuccess}
          />
        </main>
      ) : null}
    </div>
  )
}

export default App

