import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { GlobalSearchResultDto } from "../../core/models/search/global-search-result.dto";

@Injectable({ providedIn: 'root' })
export class GlobalSearchApi {
    constructor(private http: HttpClient) { }

    search(q: string, take = 8) {
        return this.http.get<GlobalSearchResultDto[]>(API_ENDPOINTS.globalSearch, {
            params: { q, take }
        });
    }
}
