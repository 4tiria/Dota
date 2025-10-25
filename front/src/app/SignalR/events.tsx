import { UserAccount } from 'models/UserAccount'

export type SignalREvents = {
    AccountFeedUpdated: UserAccount
    NewNotification: Notification
}
