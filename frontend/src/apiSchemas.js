import { z } from 'zod'

export const VehicleSchema = z.object({
  id: z.string().min(1),
  vin: z.string().min(1),
  year: z.number().int(),
  make: z.string().min(1),
  model: z.string().min(1),
  trim: z.string().min(1),
  body_style: z.string().min(1),
  exterior_color: z.string().min(1),
  interior_color: z.string().min(1),
  engine: z.string().min(1),
  transmission: z.string().min(1),
  drivetrain: z.string().min(1),
  odometer_km: z.number().int().nonnegative(),
  fuel_type: z.string().min(1),
  condition_grade: z.number().min(0).max(5),
  condition_report: z.string().min(1),
  damage_notes: z.array(z.string()),
  title_status: z.string().min(1),
  province: z.string().min(1),
  city: z.string().min(1),
  auction_start: z.string().min(1),
  starting_bid: z.number().nonnegative(),
  reserve_price: z.number().nonnegative().nullable(),
  buy_now_price: z.number().nonnegative().nullable(),
  images: z.array(z.string().url()),
  selling_dealership: z.string().min(1),
  lot: z.string().min(1),
  current_bid: z.number().nonnegative(),
  bid_count: z.number().int().nonnegative()
})

export const VehiclesSchema = z.array(VehicleSchema)

