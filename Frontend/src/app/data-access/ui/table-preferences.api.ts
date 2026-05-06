import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { TablePreferenceDto, UpdateTablePreferenceDto } from "../../core/models/ui/table-preference.model";

@Injectable({ providedIn: 'root' })
export class TablePreferencesApi {
  private readonly baseUrl = API_ENDPOINTS.tablePreferences;

  constructor(private http: HttpClient) { }

  get(tableKey: string) {
    return this.http.get<TablePreferenceDto>(`${this.baseUrl}/${tableKey}`);
  }

  save(tableKey: string, dto: UpdateTablePreferenceDto) {
    return this.http.put<TablePreferenceDto>(`${this.baseUrl}/${tableKey}`, dto);
  }
}
