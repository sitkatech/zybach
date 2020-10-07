
// we need our start times to always be on-the-hour so that our time series are always normalized to :00, :15, ;30, :45
// this function chops off all the minutes/seconds/milliseconds from a given ISO formatted date.
function normalizeISOStringTime(isoString){
    const ticks = Date.parse(isoString);

    let date = new Date(ticks);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);

    return date.toISOString();
}

module.exports.normalizeISOStringTime = normalizeISOStringTime;