export interface CreateOrderItemDto {
    name: string;
    customerReference?: string;

    quantity: number;
    actualQuantity?: number;
    status?: string;
    quantityUnit: string;
    handlingQuantity?: number;
    handlingUnit?: string;
    unitNetWeight?: number;
    unitGrossWeight?: number;
    weightUnit?: string;
    length?: number;
    width?: number;
    height?: number;
    dimensionUnit?: string;
    volume?: number;
    volumeUnit?: string;
    minTemperature?: number;
    maxTemperature?: number;
    temperatureUnit?: string;

    isHazmat: boolean;

    freightClass?: string;
    hazardClass?: string;
    identificationNumber?: string;
    declaredValue?: number;
    currency?: string;
    stackable?: boolean;
    copyToLoad?: boolean;

    notes?: string;
}
