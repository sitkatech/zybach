// DB.js config for your database  
import sql from 'mssql';
import secrets from './secrets';

const config = {
    user: secrets.ZYBACH_DB_USER,
    password: secrets.ZYBACH_DB_PASSWORD,
    server: secrets.ZYBACH_DB_SERVER,
    database: secrets.ZYBACH_DB_NAME
}

async function getConnectionPool(){

    const pool1 = new sql.ConnectionPool(config);
    const pool1Connect = pool1.connect();
    pool1Connect.catch(err => console.log('Database Connection Failed! Bad Config: ', err));
    
    await pool1Connect;

    return pool1;
}

const poolPromise = getConnectionPool();

export {sql, poolPromise};

