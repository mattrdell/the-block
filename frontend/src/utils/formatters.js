export const currency = new Intl.NumberFormat('en-CA', {
  style: 'currency',
  currency: 'CAD',
  maximumFractionDigits: 0
})

export const integer = new Intl.NumberFormat('en-CA')

export function formatAuctionDate(timestamp) {
  const parsed = new Date(timestamp)
  if (Number.isNaN(parsed.valueOf())) {
    return 'Unknown'
  }

  return new Intl.DateTimeFormat('en-CA', {
    dateStyle: 'medium',
    timeStyle: 'short'
  }).format(parsed)
}

