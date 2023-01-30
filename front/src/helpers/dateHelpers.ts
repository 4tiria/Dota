export function getFullDateString(date: Date): string {
    return `${('0' + date.getUTCDate()).slice(-2)}.${('0' + (date.getUTCMonth() + 1)).slice(-2)}.${date.getUTCFullYear()}
     ${('0' + date.getUTCHours()).slice(-2)}:${('0' + date.getUTCMinutes()).slice(-2)}`
}

export function getTimeString(date: Date): string {
    return `${date.getUTCHours()}:${('0' + (date.getUTCMinutes())).slice(-2)}`;
}
