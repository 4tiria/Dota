declare type Guid = string;
declare type AccessLevel = 'Admin' | 'Default' | null;
declare type Palette = 'light' | 'dark';

declare module '*.scss' {
    const content: { [className: string]: string };
    // noinspection JSUnusedGlobalSymbols
    export default content;
}

