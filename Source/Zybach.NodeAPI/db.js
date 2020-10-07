// DB.js config for your database  
const sql = require('mssql');
const secrets = require('./secrets');
const config = {
    user: secrets.ZYBACH_DB_USER,
    password: secrets.ZYBACH_DB_PASSWORD,
    server: secrets.ZYBACH_DB_SERVER,
    database: secrets.ZYBACH_DB_NAME
}
const poolPromise = new sql.ConnectionPool(config)
    .connect()
    .then(pool => {
        console.log('Connected to MSSQL')
        return pool
    })
    .catch(err => console.log('Database Connection Failed! Bad Config: ', err))

module.exports = {
    sql, poolPromise
}  