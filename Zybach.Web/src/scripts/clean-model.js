const fs = require("fs");
const modelsPath = process.argv[2] + '/model';
const apiPath = process.argv[2] + '/api';

// process.argv.forEach(function (val, index, array) {
//     console.log(index + ': ' + val);
//});

try {
    fs.rmdirSync(modelsPath, { recursive: true });
    console.log("Folder removed:", modelsPath);
} catch (err) {
    console.error(err);
}

try {
    fs.rmdirSync(apiPath, { recursive: true });
    console.log("Folder removed:", apiPath);
} catch (err) {
    console.error(err);
}