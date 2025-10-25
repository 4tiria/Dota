import { api, baseApiUrl } from 'api/http'
import { Account } from '../model/Account'

const baseAccountUrl = `${baseApiUrl}/account`

export async function getAll(): Promise<Account[]> {
    const response = await api.get<Account[]>(`${baseAccountUrl}/getAll`)
    return response.data
}
