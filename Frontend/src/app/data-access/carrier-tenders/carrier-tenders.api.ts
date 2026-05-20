import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { PublicCarrierTenderDto, RespondCarrierTenderDto } from "../../core/models/carriers/public-carrier-tender.dto";

@Injectable({ providedIn: 'root' })
export class CarrierTendersApi {
    constructor(private http: HttpClient) { }

    get(token: string) {
        return this.http.get<PublicCarrierTenderDto>(`${API_ENDPOINTS.publicCarrierTenders}/${token}`);
    }

    accept(token: string, dto: RespondCarrierTenderDto) {
        return this.http.post<void>(`${API_ENDPOINTS.publicCarrierTenders}/${token}/accept`, dto);
    }

    reject(token: string, dto: RespondCarrierTenderDto) {
        return this.http.post<void>(`${API_ENDPOINTS.publicCarrierTenders}/${token}/reject`, dto);
    }
}
