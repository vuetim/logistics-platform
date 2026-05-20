import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";

export interface GeocodingResult {
  label: string;
  latitude: number;
  longitude: number;
  city?: string | null;
  state?: string | null;
  postalCode?: string | null;
  country?: string | null;
  addressLine1?: string | null;
}

interface NominatimResult {
  display_name: string;
  lat: string;
  lon: string;
  address?: {
    road?: string;
    house_number?: string;
    city?: string;
    town?: string;
    village?: string;
    municipality?: string;
    state?: string;
    postcode?: string;
    country?: string;
  };
}

@Injectable({ providedIn: 'root' })
export class GeocodingService {
  constructor(private http: HttpClient) {}

  search(query: string) {
    return this.http.get<NominatimResult[]>('https://nominatim.openstreetmap.org/search', {
      params: {
        q: query,
        format: 'jsonv2',
        addressdetails: '1',
        limit: '5',
        countrycodes: 'us'
      }
    }).pipe(
      map(results => results.map(result => this.mapResult(result)))
    );
  }

  private mapResult(result: NominatimResult): GeocodingResult {
    const address = result.address || {};
    const street = [address.house_number, address.road].filter(Boolean).join(' ');

    return {
      label: result.display_name,
      latitude: Number(result.lat),
      longitude: Number(result.lon),
      city: address.city || address.town || address.village || address.municipality || null,
      state: address.state || null,
      postalCode: address.postcode || null,
      country: address.country || null,
      addressLine1: street || null
    };
  }
}
