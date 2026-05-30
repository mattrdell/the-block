import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { axe } from 'vitest-axe'
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
  }
]

describe('App accessibility', () => {
  afterEach(() => {
    vi.restoreAllMocks()
  })

  it('has no axe violations on initial render', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: async () => inventory
    })

    const { container } = render(<App />)

    await waitFor(() => {
      expect(screen.getByText('The Block Buyer Console')).toBeInTheDocument()
      expect(screen.getByText('2024 Honda Civic')).toBeInTheDocument()
    })

    const results = await axe(container)
    expect(results).toHaveNoViolations()
  })

  it('has no axe violations after bid validation error message is shown', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: async () => inventory
    })

    render(<App />)

    await waitFor(() => {
      expect(screen.getByText('2024 Honda Civic Sport')).toBeInTheDocument()
    })

    const bidInput = screen.getByLabelText('Your bid (CAD)')
    await userEvent.clear(bidInput)
    await userEvent.type(bidInput, '1000')
    await userEvent.click(screen.getByRole('button', { name: 'Submit Bid' }))

    await waitFor(() => {
      expect(screen.getByText(/Bid must be at least/i)).toBeInTheDocument()
    })

    const results = await axe(document.body)
    expect(results).toHaveNoViolations()
  })
})
