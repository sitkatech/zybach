// Turn enum into array
export function EnumToArray(enumme: any) {
    return Object.keys(enumme)
        .map(key => enumme[key]);
}
