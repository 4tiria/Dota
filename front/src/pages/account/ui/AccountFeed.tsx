import { CardContent, Typography } from '@mui/material'
import { useSignalR } from 'app/SignalR/SignalRProvider'
import React, { useEffect, useState } from 'react'
import { Card } from 'react-bootstrap'
import { getAll } from '../api/account-api'
import { Account } from '../model/Account'
import styles from './AccountFeed.module.scss'

const AccountFeed = () => {
    const [accounts, setAccounts] = useState<Account[]>([])
    const { on, off, send } = useSignalR()

    useEffect(() => {
        const onAccount = (m: any) => {
            console.log('new account received')
            setAccounts((prev) => {
                const idx = prev.findIndex((x) => x.id === m.id)
                if (idx === -1) return [m, ...prev]
                const copy = [...prev]
                copy[idx] = m
                return copy
            })
        }

        getAll().then((accounts) => {
            setAccounts(accounts)
            on('AccountFeedUpdated', onAccount)
        })
    }, [])

    return (
        <div className={styles['accounts-feed-container']}>
            {accounts
                .sort(
                    (a, b) =>
                        b.creationDate?.getTime() - a.creationDate?.getTime()
                )
                .map((account) => renderAccount(account))}
        </div>
    )
}

const renderAccount = (account: Account): JSX.Element => {
    return (
        <Card key={account.id}>
            <CardContent>
                <Typography>id: {account.id}</Typography>
                <Typography variant="h5" component="div">
                    name: {account.nickName}
                </Typography>
                <Typography>
                    id: {account.creationDate.toDateString()}
                </Typography>
            </CardContent>
        </Card>
    )
}

export default AccountFeed
