import moment from "moment";

export default function agGridDateFormatter(params) {
    if (params.value) {
      const time = moment(params.value)
      //const timepiece = time.format('h:mm a');
      return time.format('M/D/yyyy ');// + timepiece;
    }
    else {
      return null;
    }
  }