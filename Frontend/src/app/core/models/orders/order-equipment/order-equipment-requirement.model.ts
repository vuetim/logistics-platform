export interface OrderEquipmentRequirementDto {
    id: string;
    equipmentType: string;
    equipmentSize?: string | null;
    maxWeight?: number | null;
    weightUnit?: number | string | null;
    minTemperature?: number | null;
    maxTemperature?: number | null;
    temperatureUnit?: number | string | null;
    quantity: number;
    isMandatory: boolean;
    copyToLoad: boolean;
    notes?: string | null;
    isPrefered?: boolean | null;
}
