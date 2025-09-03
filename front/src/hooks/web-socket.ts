import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { useEffect } from 'react'
import toast from 'react-hot-toast'

export const useWebSocket = () => {
    useEffect(() => {
        const connection: HubConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/notifications')
            .withAutomaticReconnect()
            .build()

        connection.on('SendMessage', (message: string) => {
            toast(message)
        })

        connection.start().catch((err) => console.error(err))

        return () => {
            connection.stop()
        }
    }, [])
}
