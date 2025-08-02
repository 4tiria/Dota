import { centrifugoTokenPath } from 'api/apiPaths'
import { api } from 'api/http'
import { useEffect, useState } from 'react'

export const useCentrifugoToken = () => {
    const [token, setToken] = useState<string | null>(null)

    useEffect(() => {
        api.get<string>(centrifugoTokenPath)
            .then((response) => setToken(response.data))
            .catch(() => {
                setToken(null)
            })
    }, [])

    return token
}
