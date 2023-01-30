export class Account {
    id: string;
    userName: string;
    email: string;
    password: string;
    accessLevel: AccessLevel;
    isConfirmed: boolean;
    avatar: Blob;
}