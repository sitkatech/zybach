const secrets = require('../../secrets');

const checkApiKey = function (req, res, next) {
        let keyValue = secrets.API_KEY_VALUE;
        let keySent = req.get('authorization');
        if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
            return res.status(401).json({ status: 'error', result: 'unauthorized' });
        }
        next();
};

module.exports = {checkApiKey}