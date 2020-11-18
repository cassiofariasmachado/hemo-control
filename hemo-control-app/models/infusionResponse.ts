import { FactorResponse } from "./factorResponse";

export type InfusionResponse = {
    id: number,
    date: Date,
    userWeight?: number,
    factor?: FactorResponse
    isHemarthrosis: boolean,
    isBleeding: boolean,
    isTreatmentContinuation: boolean,
    bleedingLocal: string,
    treatmentLocal: string
}