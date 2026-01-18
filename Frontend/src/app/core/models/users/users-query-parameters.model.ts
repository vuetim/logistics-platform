import { QueryParameters } from '../pagination/query-parameters.model';

export interface UsersQueryParameters extends QueryParameters {
    isActive?: boolean | null;
    role?: string | null;
}
