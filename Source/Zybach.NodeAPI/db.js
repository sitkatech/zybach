// DB.js config for your database  
const sql = require('mssql')  
const config = {  
user: 'ZybachWebLocal',  
password: 'password#1',  
server: "host.docker.internal",  
database: "ZybachDB"  
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