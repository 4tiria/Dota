import { Centrifuge } from 'centrifuge'
import { useEffect } from 'react'
import { toast } from 'react-hot-toast'

export const useCentrifugo = (token: string) => {
    useEffect(() => {
        const client = new Centrifuge(
            'ws://localhost:8000/connection/websocket',
            { token }
        )

        client.on('connected', (ctx) => {
            console.log('✅ Connected:', ctx)
        })

        client.on('disconnected', (ctx) => {
            console.log('❌ Disconnected:', ctx)
        })

        const sub = client.newSubscription('channel1')

        sub.on('publication', (ctx) => {
            const message = ctx.data
            console.log('📨 New message:', message)
            toast(message)
        })

        sub.subscribe()
        client.connect()

        return () => {
            sub.unsubscribe()
            client.disconnect()
        }
    }, [token])
}
