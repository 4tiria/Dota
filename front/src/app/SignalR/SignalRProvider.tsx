import {
    HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel,
} from '@microsoft/signalr'
import React, {
    createContext,
    ReactNode,
    useCallback,
    useContext,
    useEffect,
    useMemo,
    useState,
} from 'react'
import { SignalREvents } from './events'

type SignalRContextValue = {
    connection: HubConnection | null
    state: HubConnectionState
    send: (method: string, ...args: any[]) => Promise<void>
    on: <K extends keyof SignalREvents>(
        event: K,
        cb: (payload: SignalREvents[K]) => void
    ) => void
    off: (event: keyof SignalREvents, cb?: (...args: any[]) => void) => void
}

const SignalRContext = createContext<SignalRContextValue | null>(null)

export const SignalRProvider: React.FC<{
    url: string
    children?: ReactNode
}> = ({ url, children }) => {
    const [connection, setConnection] = useState<HubConnection | null>(null)
    const [state, setState] = useState<HubConnectionState>(
        HubConnectionState.Disconnected
    )

    useEffect(() => {
        const conn = new HubConnectionBuilder()
            .withUrl(url)
            .withAutomaticReconnect([0, 2000, 10000, 30000]) // immediate, 2s, 10s, 30s
            .configureLogging(LogLevel.Warning)
            .build()

        conn.onreconnecting(() => setState(HubConnectionState.Reconnecting))
        conn.onreconnected(() => setState(HubConnectionState.Connected))
        conn.onclose(() => setState(HubConnectionState.Disconnected))

        conn.start()
            .then(() => setState(conn.state))
            .catch(() => {
                setState(HubConnectionState.Disconnected)
            })

        setConnection(conn)

        return () => {
            conn.stop().catch(() => {})
        }
    }, [url])

    const send = useCallback(
        async (method: string, ...args: any[]) => {
            if (!connection) {
                throw new Error('SignalR: no connection')
            }

            if (connection.state !== HubConnectionState.Connected) {
                await connection.start()
            }

            await connection.invoke(method, ...args)
        },
        [connection]
    )

    const on = useCallback(
        <K extends keyof SignalREvents>(
            event: K,
            cb: (payload: SignalREvents[K]) => void
        ) => {
            connection?.on(event as string, cb as any)
        },
        [connection]
    )

    const off = useCallback(
        (event: keyof SignalREvents, cb?: (...args: any[]) => void) => {
            if (cb) connection?.off(event as string, cb)
            else connection?.off(event as string)
        },
        [connection]
    )

    const value = useMemo(
        () => ({
            connection,
            state,
            send,
            on,
            off,
        }),
        [connection, state, send, on, off]
    )

    return (
        <SignalRContext.Provider value={value}>
            {children}
        </SignalRContext.Provider>
    )
}

export const useSignalR = () => {
    const context = useContext(SignalRContext)
    if (!context) {
        throw new Error('useSignalR must be used inside SignalRProvider')
    }

    return context
}
