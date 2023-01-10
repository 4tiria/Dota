declare type Guid = string;

declare module '*.scss' {
    const content: { [className: string]: string };
    // noinspection JSUnusedGlobalSymbols
    export default content;
}
