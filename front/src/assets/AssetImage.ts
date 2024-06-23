import { getBlobFromBase64 } from "../helpers/blobHelper";

export class AssetImage {
    blob: Blob;
    path: string;

    constructor(base64: string) {
        this.blob = getBlobFromBase64(base64);
        this.path = window.URL.createObjectURL(this.blob);
    }
}