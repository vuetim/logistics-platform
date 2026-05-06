export interface TablePreferenceDto {
  tableKey: string;
  jsonConfig: string;
  updatedAt: string;
}

export interface UpdateTablePreferenceDto {
  jsonConfig: string;
}
