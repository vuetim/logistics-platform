import { Injectable } from "@angular/core";
import { GlobalSearchApi } from "./global-search.api";

@Injectable({ providedIn: 'root' })
export class GlobalSearchService {
    constructor(private api: GlobalSearchApi) { }

    search(query: string, take = 8) {
        return this.api.search(query, take);
    }
}
