import { QueryParameters } from "../pagination/query-parameters.model";

export interface OrdersQueryParameters extends QueryParameters {


    customerId?: string | null;

    preferredCarrierId?: string | null;

    status?: number | null;   // enum
    phase?: number | null;    // enum
    direction?: number | null; // enum

    fromDate?: string | null; // ISO date
    toDate?: string | null;
}


