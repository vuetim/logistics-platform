# Order to Load Process - Full Business Documentation

Ky dokument e pershkruan procesin real `Order -> Load` ne platforme, duke u bazuar ne fushat ekzistuese te domain/DTO/service deri tani. Qellimi eshte qe secili rol ta dije cka ploteson, pse ekziston fusha, kur perdoret, dhe kush eshte pergjegjes.

## 1. Parimi Kryesor

`Order` eshte kerkesa komerciale/planuese nga customer-i. Aty ruhen premtimi ndaj customer-it, lane, pickup/delivery plan, commodity, quote dhe dokumentet fillestare.

`Load` eshte ekzekutimi operacional i transportit. Aty ruhen carrier-i real, driver/truck/trailer, statusat e stops, POD, tracking, exceptions, carrier pay, invoice/settlement trigger dhe aktivitetet operative.

Kur krijohet load nga order, sistemi ben snapshot:

- `OrderRoute` kopjohet ne `LoadStop`.
- `OrderItem` kopjohet ne `LoadItem`.
- `OrderEquipmentRequirement` kopjohet ne `LoadEquipment`.
- order cost lines jo-linehaul mund te kopjohen si load cost extras.
- `LoadOrder` lidh order-in me load-in per status sync.

Kjo eshte qasje standarde ne TMS: order mban kontraten/customer intent; load mban execution state. Nuk duhet qe load execution ta ndryshoje automatikisht customer order planning history, pervec status/phase sync.

## 2. Rolet Dhe Pergjegjesite

| Rol | Pergjegjesi kryesore | Mund te ndryshoje | Nuk duhet te ndryshoje |
|---|---|---|---|
| Admin | Konfigurim, security, correction, full access | Te gjitha | N/A |
| Broker | Customer order, quote, carrier/customer commercial decisions | Order core, routes, items, customer rate, costs, load create, carrier planning | User/role admin pa permission |
| Dispatcher | Carrier execution dhe dispatch | Carrier, driver, truck, trailer, stops, docs operative, notes, exceptions | Customer quote/billing cost nese nuk ka permission |
| Operator | Tracking dhe daily execution | Statusat e stops, arrived/loaded/unloaded, tracking notes, activity, exceptions | Broker data, dispatcher assignment data, customer/carrier/user master data, billing/cost values |
| Accounting | Billing dhe settlement | Invoice/settlement status, payment, financial docs, cost review sipas permission | Execution/status operational nese nuk ka permission |

## 3. Complete Example Data

Scenario: customer `FreshMart Distribution Inc.` kerkon reefer shipment nga Chicago, IL ne Columbus, OH.

Customer:

- Name: `FreshMart Distribution Inc.`
- Billing email: `invoices@freshmart.example`
- Billing terms: `Net 30`
- Billing address: `4100 S Packers Ave, Chicago, IL 60609, USA`

Carrier:

- Name: `Midwest Express Carriers LLC`
- MC: `MC-845219`
- DOT: `DOT-2938471`
- SCAC: `MWEX`
- Dispatch email: `dispatch@midwestexpress.example`
- Equipment: `Reefer`

Order:

- Customer: FreshMart
- Order type: `Transportation`
- Direction: `Outbound`
- StartDate: `2026-05-06`
- EndDate: `2026-05-07`
- PlannedPickup: `2026-05-06 09:00`
- PlannedDelivery: `2026-05-07 14:00`
- Commodity: `Fresh Produce - Mixed Greens`
- PO: `FM-PO-105884`
- BOL: `BOL-774920`
- PRO: `PRO-20260506-01`
- Total pallets: `18`
- Total weight: `24500`
- CustomerRate: `1850.00`

Routes:

- Pickup sequence 1: `FreshMart Chicago DC`, `4100 S Packers Ave`, appointment `PU-99821`, window `09:00-11:00`.
- Delivery sequence 2: `FreshMart Columbus Store Replenishment`, appointment `DEL-44291`, window `13:00-15:00`.

Item:

- Name: `Mixed Greens Cases`
- Quantity: `18`
- Unit: `Pallets`
- Gross weight: `24500 lb`
- Temperature: `34-38 F`
- Stackable: `false`
- Hazmat: `false`

Equipment:

- Type: `Reefer`
- Size: `53 ft`
- Quantity: `1`
- MaxWeight: `44000 lb`
- Temperature: `34-38 F`
- Mandatory: `true`

Load execution:

- Carrier: Midwest Express
- CarrierRate: `1450.00`
- Rate confirmation: `RC-20260506-MWEX`
- Driver: `Carlos Ramirez`
- Truck: `MWX-4821`
- Trailer: `RFR-7704`
- POD document uploaded after delivery.

## 4. Order Fields

### Core

| Field | Pse duhet | Kur plotesohet | Pergjegjes |
|---|---|---|---|
| `OrderNumber` | Identifikues intern unik per order. Perdoret ne search, audit, linking. | Auto nga backend `O-yyyyMMddHHmmss`. | System |
| `CustomerId` | Tregon kush e ka porositur transportin dhe kush faturohet. | Ne krijim te order-it. | Broker/Admin |
| `PreferredCarrierId` | Carrier i preferuar para dispatch. Nuk eshte carrier final derisa load-i ta perdore. | Opsional gjate planning. | Broker/Dispatcher |
| `OrderType` | Ndan tipin e punes: Transportation, Warehouse, Storage, CustomerOrder. | Ne krijim/update. | Broker |
| `Direction` | Inbound/Outbound/Transfer per raportim dhe workflow. | Ne krijim/update. | Broker |
| `Status` | Gjendja biznesore e order-it. | Auto/manual sipas workflow. | Broker/System |
| `Phase` | Grupim i statusit: Open, Plan, Ship, Bill, Complete, Cancelled. | Auto nga status. | System |
| `CreatedByUserId` | Audit kush e krijoi. | Auto ne krijim. | System |

### Planning Dates

| Field | Pse duhet | Kur plotesohet | Pergjegjes |
|---|---|---|---|
| `StartDate` | Fillimi i dritares se order-it/customer commitment. | Required ne krijim. | Broker |
| `EndDate` | Fundi i dritares se order-it. Nuk guxon te jete para StartDate. | Required ne krijim. | Broker |
| `PlannedPickupDate` | Pickup target per planning dhe load creation override. | Opsional ne order, mund te detajohet ne routes. | Broker/Dispatcher |
| `PlannedDeliveryDate` | Delivery target per planning. Nuk guxon para pickup. | Opsional ne order. | Broker/Dispatcher |

### Business References

| Field | Pse duhet | Kur plotesohet | Pergjegjes |
|---|---|---|---|
| `PrimaryPONumber` | Customer PO, lidh invoice/order me customer system. | Sa here customer e jep. | Broker |
| `PrimaryBolNumber` | BOL reference per shipping docs. | Para pickup ose nga customer docs. | Broker/Dispatcher |
| `PrimaryProNumber` | PRO/tracking reference. | Kur ekziston reference nga carrier/system. | Dispatcher |
| `Commodity` | Pershkrim i mallrave per carrier, docs dhe risk. | Ne order krijim. | Broker |
| `TotalWeight` | Kapacitet, quote, legal limits. | Ne order krijim/update. | Broker |
| `TotalPallets` | Planifikim equipment/handling. | Ne order krijim/update. | Broker |
| `TotalVolume` | Kapacitet, LTL/warehouse planning. | Opsional. | Broker |
| `DispatchNotes` | Instruksione per dispatcher/carrier. | Gjate planning. | Broker/Dispatcher |
| `DeliveryNotes` | Instruksione receiver/delivery. | Gjate planning. | Broker |
| `CustomerRate` | Sell-side/base charge ndaj customer-it. | Quote/order cost. | Broker/Accounting |

## 5. Order Routes

`OrderRoute` eshte plani i stops nga customer/order side. Ne load creation kopjohet ne `LoadStop`.

| Field | Pse duhet | Si plotesohet |
|---|---|---|
| `Sequence` | Rendi i stops. Pickup i pare zakonisht 1, delivery e fundit rendi me i larte. | Numer unik per order. |
| `StopType` | Pickup, Delivery, Transload, Storage. Ndikon status workflow. | Zgjidhet nga user. |
| `LocationName` | Emri i facility. Shfaqet ne lane/origin/destination. | `FreshMart Chicago DC`. |
| `AddressLine1/2`, `City`, `State`, `PostalCode`, `Country` | Adresa e sakte per routing, docs, dispatch. | Nga customer ose master data. |
| `Latitude`, `Longitude` | Map, ETA, distance. | Nga geocoding ose manual. |
| `PlannedArrivalFrom/To` | Appointment window per arrival. | Nga customer/shipper/receiver. |
| `PlannedDepartureFrom/To` | Window per departure kur dihet. | Opsional. |
| `AppointmentType` | Appointment, FCFS, etc. Percakton pritje/planifikim. | Default Appointment. |
| `FlexMinutes` | Tolerance per on-time calculation. | P.sh. 15/30/60 minuta. |
| `HasTime` | A ka kohe specifike apo vetem date. | True kur ka appointment. |
| `TimeZone` | Avoid gabime me appointment ne shtete te ndryshme. | Default UTC, duhet te permiresohet sipas location. |
| `AppointmentStatus` | Pending/confirmed state. | Dispatcher e perditeson. |
| `AppointmentConfirmed` | Boolean per check te shpejte. | True kur receiver/shipper e konfirmon. |
| `AppointmentConfirmationNumber` | Proof/reference i appointment. | Nga facility. |
| `StopReference` | Reference per pickup/delivery. | `FM-PICK-001`. |
| `AppointmentNumber` | Appointment number qe kerkohet ne gate. | `PU-99821`. |
| `PONumbers` | PO per stop specifik kur order ka disa PO. | Comma/list text. |
| `Notes` | Instruksione specifike te stop-it. | Gate, dock, lumper, temp. |
| `CopyToLoad` | A duhet te kopjohet ne load. Pickup i pare dhe delivery e fundit kopjohen edhe nese flag gabimisht eshte false. | Default true. |
| `IsActive` | Soft active flag per route. | False per route te hequr pa humb history. |

Pergjegjes: Broker e krijon planin; Dispatcher e konfirmon appointment dhe e korrigjon nese facility jep ndryshim para execution.

## 6. Order Items

`OrderItem` pershkruan mallin nga perspektiva e customer-it. Kopjohet ne `LoadItem` per execution snapshot.

| Field | Pse duhet | Si plotesohet |
|---|---|---|
| `Name` | Emri i freight/commodity line. | `Mixed Greens Cases`. |
| `Category` | Grupim opsional per reporting. | Produce, Electronics, Paper. |
| `CustomerReference` | SKU/PO line/customer ref. | Nga customer tender. |
| `LotNumber` | Traceability per food/pharma. | Nga shipper. |
| `Quantity`, `QuantityUnit` | Sa mall po transportohet. | `18 Pallets`. |
| `HandlingQuantity`, `HandlingUnit` | Si trajtohet fizikisht. | `18 Pallets`, `720 Cases`. |
| `UnitNetWeight`, `UnitGrossWeight`, `WeightUnit` | Kapacitet dhe legal weight. | Lb/kg. |
| `Length`, `Width`, `Height`, `DimensionUnit` | Kapacitet/trailer fit. | Inches/cm. |
| `Volume`, `VolumeUnit` | LTL/warehouse/cube planning. | CuFt/CBM. |
| `MinTemperature`, `MaxTemperature`, `TemperatureUnit` | Reefer requirement. | `34-38 F`. |
| `IsHazmat`, `HazardClass`, `ShippingName`, `IdentificationNumber` | Compliance hazmat. | Required vetem kur hazmat true. |
| `FreightClass`, `NmfcCode`, `NmfcSubCode` | LTL rating/classification. | Nga customer/rating tool. |
| `DeclaredValue`, `Currency` | Insurance/claims. | Kur customer kerkon declared value. |
| `CopyToLoad` | A kalon ne execution load. | Default true. |
| `Stackable` | Kapacitet dhe loading plan. | False per fragile/produce. |
| `Notes` | Instruksione te mallit. | `Do not freeze`. |

Pergjegjes: Broker merr te dhenat nga customer; Dispatcher verifikon me shipper nese ka ndryshim; Operator nuk duhet t'i ndryshoje keto pa approval.

## 7. Order Equipment

| Field | Pse duhet | Si plotesohet |
|---|---|---|
| `EquipmentType` | Kerkesa kryesore e pajisjes. | Reefer, Dry Van, Flatbed. |
| `EquipmentSize` | Madhesia. | `53 ft`. |
| `Quantity` | Sa pajisje/trailer nevojiten. | Zakonisht 1. |
| `MaxWeight`, `WeightUnit` | Limit per trailer/legal planning. | `44000 lb`. |
| `MinTemperature`, `MaxTemperature`, `TemperatureUnit` | Reefer set point/range. | `34-38 F`. |
| `IsMandatory` | Nuk mund te zevendesohet pa approval. | True per reefer/hazmat. |
| `IsPrefered` | Preferohet por mund te ndryshohet. | True kur customer ka preference. |
| `CopyToLoad` | A kopjohet ne load equipment. | Default true. |
| `Notes` | Specifika: air chute, straps, liftgate. | Tekst operativ. |

Pergjegjes: Broker e definon kerkesen; Dispatcher siguron carrier/equipment real.

## 8. Order Costs

`OrderCost` eshte sell-side/customer quote. Nuk duhet te perdoret per carrier pay.

| Field | Pse duhet |
|---|---|
| `BillTo` | Kush faturohet nese ndryshon nga customer default. |
| `Notes` | Arsye quote/approval notes. |
| `TaxRate` | Tax percent per quoted total nese aplikohet. |
| `TotalAmount` | Total i kalkuluar. |
| `QuotedTotal` | Totali qe i eshte premtuar customer-it. |
| `Accessorials` | Shuma e charges jo-linehaul. |
| `LineItems` | Detaj i charges. |

Line item:

| Field | Pse duhet | Rregull |
|---|---|---|
| `Type` | Linehaul, Fuel, Lumper, Detention, Other. | Zgjidhet nga lista. |
| `Qty`, `Price`, `Amount` | Kalkulim. | `Amount = Qty * Price`. |
| `Billable`/`IsCustomer` | A i faturohet customer-it. | True per order costs. |
| `IsCarrier` | Nuk duhet true ne order cost. | Carrier pay shkon te load cost. |
| `Notes` | Approval/context. | P.sh. fuel surcharge. |

Pergjegjes: Broker/Accounting. Operator mund ta shoh vetem nese ka `OrderCost_View`, nuk e ndryshon pa `OrderCost_Update`.

## 9. Load Fields

### Core/Relations

| Field | Pse duhet | Pergjegjes |
|---|---|---|
| `LoadNumber` | Identifikues execution. Auto `L-yyyyMMddHHmmss`. | System |
| `CustomerId` | Customer snapshot/link per load. | System/Broker |
| `CarrierId` | Carrier aktual/tender/assigned. | Dispatcher/Broker |
| `Status` | Execution state. | System/Dispatcher/Operator |
| `Origin`, `Destination` | Lane per list/search. Vjen nga first/last stop. | System, Dispatcher korrigjon kur duhet |
| `Mode` | TL/LTL/etc. | Broker/Dispatcher |
| `IsArchived` | Hide old/cancelled duplicate loads. | Admin |
| `CreatedByUserId` | Audit. | System |

### Financial Summary

| Field | Pse duhet | Pergjegjes |
|---|---|---|
| `CustomerRate` | Customer base/sell rate ne load. | Broker/Accounting |
| `CarrierRate` | Carrier base pay. | Broker/Dispatcher/Accounting |
| `Accessorials` | Summary e extras. | Accounting/Broker |
| `Cost` | Load cost details per billable/payable lines. | Accounting/Broker |

Operator mund ta shoh nese ka permission, por nuk duhet ta update-oje. Kjo eshte e rendesishme sepse operatori duhet ta percjelle shipment-in pa prekur margin/billing.

### Dispatch/Carrier Execution

| Field | Pse duhet | Kur plotesohet |
|---|---|---|
| `BolNumber` | Shipping doc reference. | Para pickup ose nga order. |
| `ProNumber` | Carrier PRO/tracking reference. | Kur carrier e jep. |
| `RateConfirmationNumber` | Carrier rate agreement. | Para dispatch. |
| `TrackingNumber` | Link/reference per tracking. | Para ose gjate execution. |
| `DriverName`, `DriverPhone`, `DriverEmail` | Driver contact. | Required per dispatch minimum name. |
| `TruckNumber` | Tractor/unit number. | Required per dispatch. |
| `TrailerNumber` | Trailer reference. | Kur dihet. |
| `CarrierSCAC` | EDI/customer reference. | Kur carrier ka SCAC. |

Pergjegjes: Dispatcher. Operator mund te telefonoje driver dhe te shtoje notes/activity, por nuk duhet te nderroje assignment pa permission.

### Tracking/Map

| Field | Pse duhet |
|---|---|
| `DistanceMiles` | ETA, pricing, performance. |
| `DurationMinutes` | Planifikim/transit time. |
| `EncodedPolyline` | Route display ne harte. |
| `LastKnownLatitude`, `LastKnownLongitude`, `LastKnownLocationAt` | Harta dhe tracking aktual. |
| `TrackingProvider`, `TrackingExternalId` | Integrim me provider te jashtem. |

Pergjegjes: System/integration/Operator. Roli Operations duhet ta shoh harten dhe tracking sepse ata e percjellin shipment-in.

### POD/Performance

| Field | Pse duhet |
|---|---|
| `PodReceivedAt` | Kur eshte pranuar POD. Duhet per billing readiness. |
| `PodUploadedBy` | Audit kush e ngarkoi POD. |
| `OnTimePickup`, `OnTimeDelivery` | Carrier/customer KPI. |
| `TransitTimeHours` | KPI dhe reporting. |
| `HasDelayRisk` | Alert/risk flag per operations. |

POD nuk duhet te vendoset manualisht nga update form nese dokumenti POD upload e ben automatikisht. Me mire te jete side-effect nga `LoadDocumentService`.

## 10. Load Stops

`LoadStop` eshte execution version i route-it. Ka planned, revised dhe actual fields.

| Field | Pse duhet | Kush e menaxhon |
|---|---|---|
| `Sequence`, `StopType` | Rendi dhe tipi i stop-it; percakton cilat butona lejohen. | System/Dispatcher |
| Location fields | Driver dispatch, map, documents. | Snapshot nga order; Dispatcher korrigjon nese duhet |
| `PlannedArrivalFrom/To`, `PlannedDepartureFrom/To` | Plan origjinal. | Snapshot nga order |
| `RevisedArrivalFrom/To` | Ndryshim operativ pa humb planin origjinal. | Dispatcher/Operator |
| `ActualArrival`, `ActualDeparture` | Real execution timestamps. | Operator/System |
| `ActualCheckedInTime`, `ActualCheckedOutTime` | Facility detention/onsite time. | Operator |
| `IsLateArrival`, `IsLateDeparture`, `IsOnTime`, `MinutesLate` | KPI dhe delay responsibility. | System |
| `AppointmentNumber`, `AppointmentConfirmationNumber` | Gate/facility proof. | Dispatcher |
| `StopReference`, `PONumbers` | Customer/facility references. | Broker/Dispatcher |
| `ContactName`, `ContactPhone` | Facility contact. | Dispatcher |
| `PredictedArrivalAt`, `IsAtRiskOfDelay`, `DelayRisk`, `MinutesLatePrediction`, `DelayRiskReason` | ETA/risk engine. | System/Operator |
| `Status` | Pending, EnRoute, Arrived, Loaded/Completed. | Operator/Dispatcher |
| `Notes` | Operative notes. | Operator/Dispatcher |

Execution rules:

- Pickup: `Pending -> EnRoute -> Arrived -> Loaded`.
- Delivery: `Pending -> EnRoute -> Arrived -> Completed`.
- `Loaded` lejohet vetem per pickup.
- `Unloaded/Completed` lejohet vetem per delivery.
- Load behet `Delivered` kur te gjitha delivery stops jane completed.

## 11. Load Items, Equipment, Costs, Docs, Notes

Load items jane freight snapshot. Ndryshimet gjate execution duhet te jene te kujdesshme: nese shipper ndryshon quantity, operator mund te raportoje exception/note; broker duhet te vendose nese order/customer billing ndryshon.

Load equipment tregon pajisjen realisht te perdorur. `SourceOrderEquipmentRequirementId` ruan trace nga order requirement.

Load costs jane execution financials:

- `CarrierRate`: base payable ndaj carrier.
- `LoadCostLineItem` me `IsCarrier=true`: payable extra, p.sh. lumper, detention.
- `LoadCostLineItem` me `IsCustomer=true`: billable extra, p.sh. approved detention pass-through.
- Mos e shto linehaul dy here nese `CarrierRate`/`CustomerRate` e permban.

Load documents:

- `POD`: proof of delivery, trigger per billing readiness.
- `BOL`: shipping proof.
- `RateConfirmation`: internal, carrier agreement.
- `LumperReceipt`: proof per lumper charge.

Load notes:

- External/customer-visible per updates.
- Internal per operations/accounting.

Load exceptions:

- Perdoren kur ka delay, refused freight, damage, shortage, temp issue, accessorial dispute.
- `ResponsibleParty` ndihmon ne claims/KPI.
- Operator mund te krijoje/update exception operative; Accounting/Broker e perdor per billing decision.

Stop service requirements:

- Lumper, liftgate, inside delivery, reefer pre-cool, appointment required, driver assist.
- Ndihmon dispatcher/operator ta dije cka duhet ne stop specifik.

Carrier assignments:

- Tender lifecycle: Tendered, Accepted, Rejected.
- Ruan offered rate, method, expiration, accepted/rejected audit.
- Kur accepted, load mund te kaloje ne `Accepted`; pastaj dispatch kerkon driver/truck.

## 12. Status Lifecycle

Order:

| Status | Phase | Kuptimi |
|---|---|---|
| Draft | Open | Order ende ne punim. |
| Submitted | Open | Gati per review/confirmation. |
| Confirmed | Plan | Customer/order confirmed. |
| Scheduled | Plan | Load/carrier planning fillon. |
| Dispatched | Ship | Carrier/driver dispatched. |
| AtPickup | Ship | Driver ka arritur pickup. |
| PickedUp/InTransit | Ship | Freight eshte ngarkuar dhe eshte ne rruge. |
| AtDelivery | Ship | Driver ka arritur delivery. |
| Delivered | Ship | Delivery complete, ende jo billing complete. |
| ReadyForBilling | Bill | Load completed, invoice/settlement mund te pergatiten. |
| Billed | Bill | Customer invoice sent/marked. |
| Completed | Complete | Financiarisht i mbyllur. |
| Cancelled | Cancelled | Cancelled. |

Load:

| Status | Kuptimi |
|---|---|
| Draft | Load i krijuar, pa tender/dispatch final. |
| Tendered | I eshte derguar carrier-it. |
| Accepted | Carrier e ka pranuar. |
| Dispatched | Driver/truck jane dispatchuar. |
| AtPickup | Arrived pickup. |
| InTransit/Loaded | Pickup loaded, ne rruge. |
| AtDelivery | Arrived delivery. |
| Delivered | Delivery completed. |
| Completed | Financial automation trigger eshte kryer. |
| Cancelled/Rejected | Nuk vazhdon. |

## 13. Data Entry Checklist

Para create load:

- Customer valid.
- Start/end date valid.
- Te pakten nje pickup dhe nje delivery route active.
- Pickup/delivery kane address, city/state/postal/country.
- Commodity, PO/BOL/PRO kur dihen.
- Items me quantity/weight/temp nese relevante.
- Equipment requirement me type/size/temp nese relevante.
- Customer quote/order cost i vendosur nese duhet margin.

Para dispatch:

- Carrier assigned/accepted.
- Carrier rate/rate confirmation reviewed.
- Driver name dhe truck number.
- Pickup/delivery stops te sakta.
- Appointment numbers/notes te plotesuara.

Gjate execution:

- Operator shenon EnRoute/Arrived/Loaded/Unloaded.
- Notes per calls, gate issues, ETA changes.
- Exceptions per delay/damage/shortage/temp.
- Map/tracking visible per Operations.

Para complete load:

- Load status `Delivered`.
- POD uploaded.
- Costs reviewed nga person me permission.
- Billing extras te ndara: customer billable vs carrier payable.

## 14. Validime Qe Duhet Te Ekzistojne

- `EndDate >= StartDate`.
- `PlannedDelivery >= PlannedPickup`.
- `PlannedPickup >= StartDate`.
- `PlannedDelivery <= EndDate`.
- Create load from order ndalohet per `Draft`.
- Create load from order kerkon pickup dhe delivery active.
- Pa split flow, order nuk duhet te kete me shume se nje active load.
- Dispatch lejohet vetem nga `Accepted`.
- Dispatch kerkon carrier dhe driver/truck.
- Complete load lejohet vetem nga `Delivered`.
- Completed load/order nuk duhet te editohet pa correction/admin flow.
- Costs update kerkon permission specifik.
- Operator mund te beje tracking/status/notes/activity, jo billing/cost/order commercial edits.

