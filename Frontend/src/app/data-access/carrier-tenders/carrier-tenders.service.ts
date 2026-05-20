import { Injectable } from "@angular/core";
import { CarrierTendersApi } from "./carrier-tenders.api";
import { RespondCarrierTenderDto } from "../../core/models/carriers/public-carrier-tender.dto";

@Injectable({ providedIn: 'root' })
export class CarrierTendersService {
    constructor(private api: CarrierTendersApi) { }

    get(token: string) {
        return this.api.get(token);
    }

    accept(token: string, dto: RespondCarrierTenderDto) {
        return this.api.accept(token, dto);
    }

    reject(token: string, dto: RespondCarrierTenderDto) {
        return this.api.reject(token, dto);
    }
}
