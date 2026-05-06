# Complete Order to Load Workflow Example

Ky dokument eshte nje shembull praktik per ta testuar komplet flow-in e nje order dhe nje load ne logistics platform. Te dhenat jane te menduara si rast real pune: customer, carrier, pickup, delivery, commodity, costs, documents dhe status progression.

## Scenario

Customer `FreshMart Distribution Inc.` porosit transport per 18 pallets me produce temperature-controlled nga Chicago, IL ne Columbus, OH.

Broker/dispatcher krijon order-in, e konfirmon me customer-in, krijon load-in, cakton carrier-in, dispatch-on driver-in, ekzekuton stops, ngarkon POD, rishikon costs dhe e kompleton load-in per billing.

## Master Data

### Customer

Use existing customer or create:

- Customer name: `FreshMart Distribution Inc.`
- MC/DOT: optional
- Main email: `ap@freshmart.example`
- Phone: `+1 312 555 0140`
- Billing email: `invoices@freshmart.example`
- Billing terms: `Net 30`
- Payment method: `ACH`
- Auto invoice: `true`
- Billing address:
  - Address 1: `4100 S Packers Ave`
  - City: `Chicago`
  - State: `IL`
  - Postal: `60609`
  - Country: `USA`

### Carrier

Use existing carrier or create:

- Carrier name: `Midwest Express Carriers LLC`
- MC number: `MC-845219`
- DOT number: `DOT-2938471`
- SCAC: `MWEX`
- Dispatch email: `dispatch@midwestexpress.example`
- Phone: `+1 317 555 0188`
- Equipment: `Reefer`
- Insurance status: active
- Preferred lane: Midwest regional

## Step 1: Create Order

Create a new order with:

- Customer: `FreshMart Distribution Inc.`
- Preferred carrier: optional, `Midwest Express Carriers LLC`
- Order type: `Transportation`
- Direction: `Outbound`
- Order window start: `2026-05-06 08:00`
- Order window end: `2026-05-07 18:00`
- Planned pickup: `2026-05-06 09:00`
- Planned delivery: `2026-05-07 14:00`

Business details:

- Commodity: `Fresh Produce - Mixed Greens`
- PO number: `FM-PO-105884`
- BOL number: `BOL-774920`
- PRO number: `PRO-20260506-01`
- Total pallets: `18`
- Total weight: `24,500 lb`
- Total volume: `1,240 cu ft`
- Dispatch notes: `Reefer must be pre-cooled to 36F before arrival.`
- Delivery notes: `Receiver requires appointment number at guard shack.`

Expected initial status:

- Order status: `Draft`
- Order phase: `Open`

## Step 2: Add Order Routes

Routes are the customer/order planning stops. They become load stops when creating a load.

### Route 1 - Pickup

- Sequence: `1`
- Stop type: `Pickup`
- Location: `FreshMart Chicago DC`
- Address 1: `4100 S Packers Ave`
- City: `Chicago`
- State: `IL`
- Postal code: `60609`
- Country: `USA`
- Planned arrival from: `2026-05-06 09:00`
- Planned arrival to: `2026-05-06 11:00`
- Appointment type: `Appointment`
- Appointment number: `PU-99821`
- Stop reference: `FM-PICK-001`
- Has time window: `true`
- Copy to load: `true`
- Notes: `Check in at shipping office. Product must stay at 34-38F.`

### Route 2 - Delivery

- Sequence: `2`
- Stop type: `Delivery`
- Location: `FreshMart Columbus Store Replenishment`
- Address 1: `1550 Distribution Pkwy`
- City: `Columbus`
- State: `OH`
- Postal code: `43228`
- Country: `USA`
- Planned arrival from: `2026-05-07 13:00`
- Planned arrival to: `2026-05-07 15:00`
- Appointment type: `Appointment`
- Appointment number: `DEL-44291`
- Stop reference: `FM-DEL-001`
- Has time window: `true`
- Copy to load: `true`
- Notes: `Use receiving dock 6. Lumper may be required.`

Important rule:

- The platform should always copy the first pickup and last delivery to a load.
- `Copy to load` is mainly for extra stops, but keep it checked for clarity.

## Step 3: Add Order Items

Order items describe the freight from the customer side.

### Item 1

- Name: `Mixed Greens Cases`
- Customer reference: `FM-SKU-MIX-2026`
- Quantity: `18`
- Quantity unit: `Pallet`
- Handling quantity: `18`
- Handling unit: `Pallet`
- Unit gross weight: `1361.11`
- Weight unit: `Lb`
- Total approximate weight: `24,500 lb`
- Length: `48`
- Width: `40`
- Height: `72`
- Dimension unit: `In`
- Volume unit: `CuFt`
- Temperature min: `34`
- Temperature max: `38`
- Temperature unit: `F`
- Hazmat: `false`
- Stackable: `false`
- Freight class: `70`
- Declared value: `18500`
- Currency: `USD`
- Copy to load: `true`
- Notes: `Keep refrigerated. Do not freeze.`

## Step 4: Add Order Equipment

Equipment requirement:

- Equipment type: `Reefer`
- Quantity: `1`
- Equipment size: `53 ft`
- Max weight: `44000`
- Weight unit: `Lb`
- Temperature min: `34`
- Temperature max: `38`
- Temperature unit: `F`
- Mandatory: `true`
- Preferred: `true`
- Copy to load: `true`
- Notes: `Air chute required. Unit must be running at check-in.`

## Step 5: Add Order Costs

Order costs are the customer quote/sell-side charges.

Recommended order cost lines:

| Type | Qty | Price | Bill Customer | Pay Carrier | Notes |
|---|---:|---:|---|---|---|
| Linehaul | 1 | 1850.00 | Yes | No | Customer linehaul quote |
| FuelSurcharge | 1 | 275.00 | Yes | No | Fuel surcharge |
| Other | 1 | 75.00 | Yes | No | Temperature-controlled service fee |

Expected customer quote:

- Base freight: `1850.00`
- Accessorials: `350.00`
- Quoted total before tax: `2200.00`

If tax is configured, quoted total may include tax depending backend cost logic.

## Step 6: Submit and Confirm Order

Submit order.

Expected:

- Order status: `Submitted`
- Order phase: `Open`

Confirm or schedule order, depending available UI action.

Expected before load creation:

- Order status: `Confirmed` or `Scheduled`
- Order phase: `Plan`

## Step 7: Create Load From Order

Click `Create Load` from order.

Expected load snapshot:

- Load number: auto-generated, example `L-20260506103045`
- Customer: `FreshMart Distribution Inc.`
- Carrier: preferred carrier if selected, otherwise empty
- Mode: `TL`
- Origin: `FreshMart Chicago DC`
- Destination: `FreshMart Columbus Store Replenishment`
- Customer rate: from order/customer sell rate
- Carrier rate: initially `0` unless provided
- Stops:
  - Pickup copied from Route 1
  - Delivery copied from Route 2
- Items:
  - Mixed Greens Cases copied from order item
- Equipment:
  - Reefer requirement copied from order equipment

Expected statuses:

- Load status: `Draft` or planning status depending backend sync
- Order status: usually `Confirmed` or `Scheduled`
- Order phase: `Plan`

## Step 8: Dispatcher Setup on Load

The dispatcher prepares the execution load.

Fill Dispatcher panel:

- Carrier: `Midwest Express Carriers LLC`
- Carrier rate: `1450.00`
- Rate confirmation number: `RC-20260506-MWEX`
- Tracking number: `TRK-FM-774920`
- BOL: `BOL-774920`
- PRO: `PRO-20260506-01`
- Driver name: `Carlos Ramirez`
- Driver phone: `+1 317 555 0199`
- Driver email: `carlos.ramirez@midwestexpress.example`
- Truck number: `MWX-4821`
- Trailer number: `RFR-7704`
- Carrier SCAC: `MWEX`

Do not manually set POD uploaded fields.

POD received/uploaded should be generated from document upload:

- Upload document type: `POD`
- Backend records:
  - `PodReceivedAt = current timestamp`
  - `PodUploadedBy = current user`

## Step 9: Accept and Dispatch Load

Minimal current UI:

1. Save dispatcher panel.
2. Click `Mark Accepted`.
3. Click `Dispatch`.

Expected:

- Load status: `Dispatched`
- Order status: `Dispatched`
- Order phase: `Ship`

Backend dispatch rule:

- Load must have carrier.
- Load must be `Accepted`.
- Driver name is required.
- Truck number is required.

## Step 10: Execute Pickup Stop

Pickup workflow:

1. Click `EnRoute`
   - Stop status: `EnRoute`
   - Load status: `Dispatched`
2. Click `Arrive`
   - Stop status: `Arrived`
   - Load status: `AtPickup`
   - Order status: `AtPickup`
3. Click `Loaded`
   - Stop status: `Loaded`
   - Load status: `InTransit`
   - Order status: `InTransit`
   - Order phase: `Ship`

Do not click `Unloaded` on pickup. Unloaded is only for delivery stops.

## Step 11: Execute Delivery Stop

Delivery workflow:

1. Click `EnRoute`
   - Stop status: `EnRoute`
   - Load remains in execution
2. Click `Arrive`
   - Stop status: `Arrived`
   - Load status: `AtDelivery`
   - Order status: `AtDelivery`
3. Click `Unloaded`
   - Stop status becomes `Completed`
   - Load status becomes `Delivered`
   - Order status becomes `Delivered`
   - Order phase remains `Ship`

Load becomes `Delivered` only when all delivery stops are completed.

If load has no delivery stop, it cannot properly become delivered. Add a delivery stop or recreate load from order after fixing route copy behavior.

## Step 12: Upload Documents

Recommended documents:

| Document Type | Example File | Internal | Notes |
|---|---|---|---|
| BOL | `BOL-774920.pdf` | No | Can be uploaded before pickup or after |
| RateConfirmation | `RC-20260506-MWEX.pdf` | Yes | Carrier rate confirmation |
| LumperReceipt | `Lumper-Columbus-4481.pdf` | No | If lumper was paid |
| POD | `POD-BOL-774920.pdf` | No | Required before completing load |

When uploading POD:

- Use document type `POD`.
- Backend should update POD received metadata automatically.

## Step 13: Add Load Costs

Load costs are execution financials. They are different from order quote.

Recommended load cost setup:

### Carrier pay

Set carrier rate in dispatcher:

- Carrier rate: `1450.00`

Add payable line items only for extra carrier charges:

| Type | Qty | Price | Bill Customer | Pay Carrier | Notes |
|---|---:|---:|---|---|---|
| Lumper | 1 | 180.00 | No | Yes | Lumper fee at Columbus |
| Detention | 1 | 75.00 | No | Yes | 1 hour detention approved |

### Customer billable extras

Add bill customer lines only for extra charges to customer:

| Type | Qty | Price | Bill Customer | Pay Carrier | Notes |
|---|---:|---:|---|---|---|
| Lumper | 1 | 180.00 | Yes | No | Pass-through lumper billed to customer |
| Detention | 1 | 125.00 | Yes | No | Customer-approved detention |

Expected financial summary:

- Customer rate from order: `2200.00`
- Extra billable: `305.00`
- Total billable: `2505.00`
- Carrier rate: `1450.00`
- Extra payable: `255.00`
- Total payable: `1705.00`
- Margin: `800.00`

Avoid double counting:

- Do not add carrier linehaul as a payable line if carrier rate already contains linehaul.
- Do not add customer linehaul as billable line if customer rate/order quote already contains it.

## Step 14: Complete Load

Complete Load should be available only after:

- Load status is `Delivered`.
- Delivery stop is completed.
- POD is uploaded if business requires POD.
- Costs are reviewed.

Click `Complete Load`.

Expected backend behavior:

- Load status: `Completed`
- Customer invoice generated
- Carrier settlement generated if carrier exists
- Order status: `ReadyForBilling`
- Order phase: `Bill`

Order should not immediately become `Completed`.

Normal billing lifecycle:

1. Load completed
2. Order `ReadyForBilling / Bill`
3. Invoice sent or marked billed
4. Order `Billed / Bill`
5. Settlement/invoice closed
6. Order `Completed / Complete`

## Full Status Reference

### Load statuses

| Step | Load Status | Meaning |
|---|---|---|
| Created | Draft | Load exists, not accepted/ready |
| Carrier accepted | Accepted | Carrier assigned and accepted |
| Dispatch | Dispatched | Driver/truck assigned and dispatched |
| Pickup arrived | AtPickup | Driver arrived pickup |
| Pickup loaded | InTransit | Freight loaded and moving |
| Delivery arrived | AtDelivery | Driver arrived delivery |
| Delivery unloaded | Delivered | Delivery complete |
| Financial close trigger | Completed | Generates invoice/settlement |

### Order statuses and phases

| Load Event | Order Status | Order Phase |
|---|---|---|
| Order created | Draft | Open |
| Order submitted | Submitted | Open |
| Order confirmed/scheduled | Confirmed/Scheduled | Plan |
| Load dispatched | Dispatched | Ship |
| Pickup arrived | AtPickup | Ship |
| Pickup loaded | InTransit | Ship |
| Delivery arrived | AtDelivery | Ship |
| Delivery unloaded | Delivered | Ship |
| Load completed | ReadyForBilling | Bill |
| Invoice billed | Billed | Bill |
| Financially closed | Completed | Complete |

## What To Validate In The App

Use this checklist while testing:

- Order has both pickup and delivery routes.
- Order route table shows `Copy = Yes/No`.
- Load created from order has pickup and delivery stops.
- Load overview shows `Delivery stops = 1`.
- Load dispatcher panel can save carrier and driver/truck data.
- `Mark Accepted` appears after carrier is saved.
- `Dispatch` appears after accepted.
- Pickup stop does not show `Unloaded`.
- Delivery stop does not show `Loaded`.
- After pickup loaded, load/order become `InTransit`.
- After delivery unloaded, load becomes `Delivered`.
- `Complete Load` appears only when load is `Delivered`.
- After complete load, order becomes `ReadyForBilling / Bill`.
- POD upload records POD metadata automatically.
- Costs distinguish `Bill Customer` from `Pay Carrier`.

