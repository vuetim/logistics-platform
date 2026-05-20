# Role and Permission Matrix

This document describes the default access model for the logistics platform. Admin is the only role with all permissions by default. Other roles receive scoped permissions, and individual users can receive more or less access through user-level permission overrides.

## Roles

### Admin

Admin has every permission in `Permission`. Admin is the system owner role and can:

- Manage users, roles, and permission overrides.
- Create, update, dispatch, tender, archive, and complete loads.
- View and update tracking, route map data, status events, stops, notes, documents, exceptions, and stop services.
- View and update order/load costs.
- View financials, update invoice and settlement status, and record payments.
- Manage customers and system records.

### Broker

Broker owns commercial load setup and customer-facing workflow. Broker can:

- View, create, update, archive, tender, dispatch, and change status on loads.
- Create loads from orders.
- View tracking/map data.
- View and update order costs and load costs.
- View financials and update invoice/settlement status and payments.
- View, create, and update customers.
- View and create load notes and upload load documents.
- View own user profile.

Broker should not manage users or assign roles by default.

### Operator

Operator is the operational execution role. This maps to shipment roles like Operator where the user follows the shipment, calls the driver, updates execution status, records arrival/departure activity, and writes operational notes.

Operator can:

- View loads and full operational load details.
- View tracking/map data.
- Update tracking/execution events such as enroute, arrived, loaded, and unloaded.
- Change load status where operational workflow requires it.
- View order costs, load costs, and financial records.
- View and create internal load notes.
- View load documents.
- View customers.
- View/create/update load exceptions.
- View/create stop services.
- View own user profile.

Operator cannot by default:

- Create or update loads as commercial records.
- Edit broker/customer/carrier financial values.
- Update order costs or load costs.
- Update invoice/settlement status or record payments.
- Manage users, roles, customers, or carriers.
- Delete documents or stop services.

### Dispatcher

Dispatcher owns carrier execution and shipment handoff. Dispatcher can:

- View loads.
- Dispatch loads and change operational status.
- Tender carrier offers and accept/reject carrier assignments.
- View and update tracking/map execution.
- View order/load costs but not edit cost values.
- View customers.
- View and create internal notes.
- View/create/update exceptions.
- View/create stop services.
- View own user profile.

Dispatcher cannot by default manage users, edit customers, edit costs, or record accounting payments.

### Accounting

Accounting owns financial workflow. Accounting can:

- View loads and customers.
- View order/load costs.
- View financials.
- Update invoice and settlement statuses.
- Record invoice and settlement payments.
- View own user profile.

Accounting should not change operational load data, users, customers, carriers, stops, or tracking by default.

## Permission Groups

### Loads

- `Load_View`: access load list and load detail.
- `Load_Create`: create new loads.
- `Load_Update`: edit core load commercial/operational fields.
- `Load_ChangeStatus`: change load status.
- `Load_Archive`: archive/cancel old loads.
- `Load_Dispatch`: dispatch accepted loads with driver/truck/trailer data.
- `Load_Tender`: tender loads to carriers.
- `Load_CreateFromOrder`: create a load from an order.
- `Load_CompletedCorrection`: edit locked completed-load operational data.

### Tracking and Execution

- `Load_Tracking_View`: view map, polyline, distance, stop coordinates, and last known location.
- `Load_Tracking_Update`: update shipment execution events such as enroute, arrived, loaded, and unloaded.

### Costs and Financials

- `OrderCost_View`: view order cost details.
- `OrderCost_Update`: edit order cost values, bill-to, tax, and line items.
- `LoadCost_View`: view load cost details and margin summary.
- `LoadCost_Update`: edit load cost line items and notes.
- `Financial_View`: view invoices and settlements.
- `Financial_Invoice_UpdateStatus`: mark invoice sent/cancelled/etc.
- `Financial_Invoice_RecordPayment`: record invoice payment.
- `Financial_Settlement_UpdateStatus`: mark settlement sent/cancelled/etc.
- `Financial_Settlement_RecordPayment`: record carrier settlement payment.

### Notes and Documents

- `LoadNote_View`: view load/order notes.
- `LoadNote_Create_Internal`: create internal notes.
- `LoadNote_Create_Public`: create public/customer-visible notes.
- `LoadDocument_View`: view documents.
- `LoadDocument_Upload`: upload documents.
- `LoadDocument_Delete`: delete documents.

### Users and Customers

- `User_View_All`: view all users.
- `User_View_Self`: view own profile.
- `User_Update`: update users.
- `User_Delete`: delete users.
- `User_AssignRole`: assign roles and permission overrides.
- `User_Disable`: disable users.
- `Customer_View`: view customers.
- `Customer_Create`: create customers.
- `Customer_Update`: update customers.
- `Customer_Delete`: delete customers.

### Carrier Offers and Exceptions

- `CarrierOffer_View`: view carrier offers for relevant loads.
- `CarrierOffer_View_All`: view all carrier offers page.
- `CarrierOffer_Create`: create/tender carrier offers.
- `CarrierOffer_Accept`: accept carrier offer.
- `CarrierOffer_Reject`: reject carrier offer.
- `LoadException_View`: view operational exceptions.
- `LoadException_Create`: create operational exceptions.
- `LoadException_Update`: update operational exceptions.

### Stop Services

- `LoadStopService_View`: view stop service requirements.
- `LoadStopService_Create`: add stop service requirements.
- `LoadStopService_Delete`: delete stop service requirements.

## Verification Checklist

Use these scenarios after login with each role.

### Operator

- Load list opens.
- Load detail opens.
- Route map is visible.
- Stops tab allows enroute, arrive, loaded, and unloaded buttons when status sequence allows it.
- Notes tab allows internal note creation.
- Costs and Billing are visible read-only.
- Cost fields, add/remove line buttons, and save cost buttons are not available.
- Invoice/settlement status and payment buttons are not available.
- Users page is blocked.
- Customer create/update/delete actions are not available.

### Dispatcher

- Load list and load detail open.
- Dispatch action is available on accepted loads with assigned carrier.
- Tracking map is visible.
- Stop execution buttons work.
- Carrier offer/tender actions are available.
- Costs are visible read-only.
- Billing payment/status actions are not available.
- Users page is blocked.

### Accounting

- Financials page opens.
- Invoice and settlement status/payment buttons are visible.
- Load cost/order cost views are visible.
- Load edit/dispatch/tracking update actions are not available.
- Users and operational edit flows are blocked.

### Broker

- Order/load creation flows are available.
- Cost edit flows are available.
- Tender/dispatch/status flows are available.
- Customer view/create/update are available.
- User management is blocked.

### Admin

- All pages and actions are available.

## Dummy Data Needed For Testing

Create one load for each major workflow state:

- Draft load with customer, origin, destination, planned pickup/delivery, one pickup stop, one delivery stop.
- Tendered load with at least one carrier offer.
- Accepted load with assigned carrier, carrier rate, customer rate, and no dispatch details.
- Dispatched load with driver name, phone, truck number, trailer number, tracking number, map coordinates, and stop coordinates.
- In transit load with pickup arrived/loaded and delivery pending.
- Delivered load with POD timestamp and documents.
- Completed load with generated invoice and carrier settlement.

For each load, include:

- Customer name, bill-to data, customer reference, PO, BOL, PRO.
- Carrier name, SCAC, MC/DOT if available, dispatcher contact, driver contact.
- Stops with location name, address, city/state/postal/country, latitude/longitude, appointment type/status, planned/revised/actual times, time zone, confirmation number, PO numbers, and notes.
- Items with commodity/name, quantity, handling quantity, weight, dimensions, hazmat fields when applicable, temperature range when applicable.
- Equipment with type, length, weight, and temperature requirements.
- Cost lines with at least one customer linehaul, one customer accessorial, one carrier linehaul, and one carrier accessorial.
- Notes: one internal operations note and one public/customer-facing note.
- Documents: rate confirmation, BOL, POD, invoice PDF, settlement PDF.
- Activity events for creation, dispatch, stop arrival, loaded/unloaded, delivered, cost update, and payment update.

## Known Follow-Up

Billing `GET` endpoints currently auto-create or refresh draft invoices/settlements. For strict read-only semantics, split those into:

- `GET`: read existing invoice/settlement only.
- `POST /generate`: create or refresh draft financial documents, guarded by accounting/broker/admin permission.

This matters because a user with `Financial_View` should not mutate billing data just by opening the Billing tab.
