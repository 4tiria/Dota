export function ToCamelCase(value: string){
    let a =  value.split(' ');
    let b = a.map(word => word.charAt(0).toUpperCase() + word.slice(1));
    return b.join("");
}