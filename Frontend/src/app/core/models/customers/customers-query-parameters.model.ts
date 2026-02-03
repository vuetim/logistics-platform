import { QueryParameters } from "../pagination/query-parameters.model";

export interface CustomersQueryParameters extends QueryParameters {
    isActive?: boolean | null;
}
