import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { CarrierListItem } from "../../core/models/carriers/carrier-list-item.model";

@Injectable({ providedIn: 'root' })
export class CarriersApi {
  private readonly baseUrl = API_ENDPOINTS.carriers;

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<CarrierListItem[]>(this.baseUrl);
  }
}

