export interface CreateOrderEquipmentRequirementDto {
    equipmentType: string;
    equipmentSize?: string | null;
    maxWeight?: number | null;
    weightUnit: number;
    minTemperature: number;
    maxTemperature: number;
    temperatureUnit: number;
    quantity: number;
    isMandatory: boolean;
    copyToLoad: boolean;
    isPrefered: boolean;
    notes?: string | null;
}
