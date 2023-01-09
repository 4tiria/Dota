export function downloadBlob(blob: Blob) {
    if (navigator.msSaveBlob) {
        let filename = 'picture';
        navigator.msSaveBlob(blob, filename);
    } else {
        let link = document.createElement("a");

        link.href = URL.createObjectURL(blob);

        link.setAttribute('visibility', 'hidden');
        link.download = 'picture';

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}

export function getBlobFromBase64(base64: string): Blob {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], {type: "image/png"});
}