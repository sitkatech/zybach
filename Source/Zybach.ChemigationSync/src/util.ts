function getTwoDigitMonth(date: Date) : string {
    const month = date.getMonth() + 1;

    if (month < 10){
        return `0${month}`
    }

    return `${month}`;
}

export {getTwoDigitMonth}