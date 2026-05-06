export interface OrderItemDto {
    id: string;
    lineItemNumber?: number;

    name: string;
    customerReference?: string | null;

    quantity: number;
    actualQuantity?: number;
    status?: string | null;
    quantityUnit: string;
    handlingQuantity?: number | null;
    handlingUnit?: string | null;
    unitNetWeight?: number | null;
    unitGrossWeight?: number | null;
    weightUnit?: string | null;
    length?: number | null;
    width?: number | null;
    height?: number | null;
    dimensionUnit?: string | null;
    volume?: number | null;
    volumeUnit?: string | null;
    minTemperature?: number | null;
    maxTemperature?: number | null;
    temperatureUnit?: string | null;

    isHazmat: boolean;
    freightClass?: string | null;
    hazardClass?: string | null;

    identificationNumber?: string | null;
    declaredValue?: number | null;
    currency?: string | null;
    stackable?: boolean;
    copyToLoad?: boolean;
    notes?: string | null;
}
