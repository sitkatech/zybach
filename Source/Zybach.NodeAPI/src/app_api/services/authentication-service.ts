import secrets from '../../secrets';
import { Request, Response, NextFunction } from 'express';

const checkApiKey = function (req: Request, res: Response, next: NextFunction) {
    let keyValue = secrets.API_KEY_VALUE;
    let keySent = req.get('authorization');
    if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
        return res.status(401).json({ status: 'error', result: 'unauthorized' });
    }
    next();
};

export { checkApiKey };