import { Account } from 'models/Account'

export type SignalREvents = {
    AccountFeedUpdated: Account
    NewNotification: Notification
}
