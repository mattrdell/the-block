import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import App from './App'

const inventory = [
  {
    id: '1',
    vin: 'ABCDEFG1234567890',
    year: 2024,
    make: 'Honda',
    model: 'Civic',
    trim: 'Sport',
    body_style: 'Sedan',
    exterior_color: 'Blue',
    interior_color: 'Black',
    engine: '2.0L I4',
    transmission: 'automatic',
    drivetrain: 'FWD',
    odometer_km: 12000,
    fuel_type: 'gasoline',
    condition_grade: 4.2,
    condition_report: 'Great condition',
    damage_notes: [],
    title_status: 'clean',
    province: 'Ontario',
    city: 'Toronto',
    auction_start: '2026-04-05T14:00:00',
    starting_bid: 15000,
    reserve_price: 19000,
    buy_now_price: null,
    images: ['https://placehold.co/800x600?text=Honda'],
    selling_dealership: 'Demo Motors',
    lot: 'A-001',
    current_bid: 16200,
    bid_count: 5
  },
  {
    id: '2',
    vin: '1234567890ABCDEFG',
    year: 2023,
    make: 'Ford',
    model: 'Bronco',
    trim: 'Big Bend',
    body_style: 'SUV',
    exterior_color: 'Red',
    interior_color: 'Black',
    engine: '2.7L V6',
    transmission: 'automatic',
    drivetrain: '4WD',
    odometer_km: 38000,
    fuel_type: 'gasoline',
    condition_grade: 3.8,
    condition_report: 'Average condition',
    damage_notes: ['Scratch on door'],
    title_status: 'clean',
    province: 'Ontario',
    city: 'Hamilton',
    auction_start: '2026-04-05T18:00:00',
    starting_bid: 14000,
    reserve_price: null,
    buy_now_price: null,
    images: ['https://placehold.co/800x600?text=Ford'],
    selling_dealership: 'North Auto',
    lot: 'A-002',
    current_bid: 15100,
    bid_count: 3
  }
]

describe('App', () => {
  afterEach(() => {
    vi.restoreAllMocks()
  })

  it('renders inventory data from API', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: async () => inventory
    })

    render(<App />)

    await waitFor(() => {
      expect(screen.getByText('The Block Buyer Console')).toBeInTheDocument()
      expect(screen.getByText('2024 Honda Civic')).toBeInTheDocument()
      expect(screen.getByText('2023 Ford Bronco')).toBeInTheDocument()
    })
  })

  it('filters vehicles by make', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: async () => inventory
    })

    render(<App />)

    await screen.findByText('2024 Honda Civic')
    await screen.findByText('2023 Ford Bronco')

    await userEvent.selectOptions(screen.getByLabelText('Filter by make'), 'Ford')

    await waitFor(() => {
      expect(screen.queryByText('2024 Honda Civic')).not.toBeInTheDocument()
      expect(screen.getByText('2023 Ford Bronco')).toBeInTheDocument()
    })
  })

  it('submits a bid and refreshes visible state', async () => {
    const updatedVehicle = {
      ...inventory[0],
      current_bid: 17000,
      bid_count: inventory[0].bid_count + 1
    }

    const fetchMock = vi.spyOn(globalThis, 'fetch').mockImplementation(async (input, init) => {
      const url = String(input)

      if (url === '/api/vehicles' && !init) {
        return { ok: true, json: async () => inventory }
      }

      if (url === '/api/vehicles/1/bids' && init?.method === 'POST') {
        return { ok: true, json: async () => updatedVehicle }
      }

      throw new Error(`Unhandled fetch call: ${url}`)
    })

    render(<App />)

    await screen.findByText('2024 Honda Civic Sport')

    const bidInput = screen.getByLabelText('Your bid (CAD)')
    await userEvent.clear(bidInput)
    await userEvent.type(bidInput, '17000')
    await userEvent.click(screen.getByRole('button', { name: 'Submit Bid' }))

    await waitFor(() => {
      expect(screen.getByText('Bid placed successfully.')).toBeInTheDocument()
      expect(screen.getAllByText(/17,000/).length).toBeGreaterThan(0)
    })

    expect(fetchMock).toHaveBeenCalledTimes(2)
  })

  it('shows validation message when bid is below minimum', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: async () => inventory
    })

    render(<App />)

    await screen.findByText('2024 Honda Civic Sport')

    const bidInput = screen.getByLabelText('Your bid (CAD)')
    await userEvent.clear(bidInput)
    await userEvent.type(bidInput, '1000')
    await userEvent.click(screen.getByRole('button', { name: 'Submit Bid' }))

    await waitFor(() => {
      expect(screen.getByText(/Bid must be at least/i)).toBeInTheDocument()
    })

    expect(fetchMock).toHaveBeenCalledTimes(1)
  })
})
