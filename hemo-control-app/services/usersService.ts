import environment from '../environment';
import { AccessTokenResponse, InfusionResponse, PagedResponse } from '../models';


const defaultHeaders: Record<string, string> = {
    Accept: 'application/json',
    'Content-Type': 'application/json',
}

export const login = async (username: string, password: string): Promise<AccessTokenResponse> => {
    const url = `${environment.apiUrl}/api/users/token`;

    return fetch(url, {
        method: 'POST',
        headers: {
            ...defaultHeaders
        },
        body: JSON.stringify({
            username,
            password
        })
    }).then(response => response.json())
}

export const getInfusions = async (accessToken: string, startDate?: Date, endDate?: Date): Promise<PagedResponse<InfusionResponse>> => {
    let url = `${environment.apiUrl}/api/users/infusions`;

    if (startDate && endDate)
        url += `startDate=${startDate}`

    if (endDate)
        url += `endDate=${endDate}`

    return fetch(url, {
        method: 'GET',
        headers: {
            ...defaultHeaders,
            'Authorization': `Bearer ${accessToken}`
        }
    }).then(response => response.json());
};