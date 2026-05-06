import { Injectable } from "@angular/core";
import { UpdateTablePreferenceDto } from "../../core/models/ui/table-preference.model";
import { TablePreferencesApi } from "./table-preferences.api";

@Injectable({ providedIn: 'root' })
export class TablePreferencesService {
  constructor(private api: TablePreferencesApi) { }

  get(tableKey: string) {
    return this.api.get(tableKey);
  }

  save(tableKey: string, dto: UpdateTablePreferenceDto) {
    return this.api.save(tableKey, dto);
  }
}
