import { Injectable } from "@angular/core";
import { CarriersApi } from "./carriers.api";

@Injectable({ providedIn: 'root' })
export class CarriersService {
  constructor(private api: CarriersApi) { }

  getAll() {
    return this.api.getAll();
  }
}

