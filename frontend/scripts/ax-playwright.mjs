import { chromium } from 'playwright'
import AxeBuilder from '@axe-core/playwright'

const targetUrl = process.env.AX_URL ?? 'http://127.0.0.1:5173'

const browser = await chromium.launch({ headless: true })
const context = await browser.newContext()
const page = await context.newPage()

try {
  await page.goto(targetUrl, { waitUntil: 'domcontentloaded', timeout: 30000 })

  const heading = await page.locator('h1').first().textContent()
  if (!heading || !heading.includes('The Block Buyer Console')) {
    throw new Error('Expected app heading was not found. Ensure the frontend is running.')
  }

  const axeResults = await new AxeBuilder({ page }).analyze()

  if (axeResults.violations.length > 0) {
    console.error(`AX violations found: ${axeResults.violations.length}`)
    for (const violation of axeResults.violations) {
      console.error(`- ${violation.id}: ${violation.help}`)
      for (const node of violation.nodes) {
        console.error(`  target: ${node.target.join(', ')}`)
      }
    }
    process.exitCode = 1
  } else {
    console.log('AX check passed: no violations found.')
  }
} finally {
  await context.close()
  await browser.close()
}
