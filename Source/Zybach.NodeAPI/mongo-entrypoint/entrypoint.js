var db = connect("mongodb://admin:password@localhost:27017/admin");

db = db.getSiblingDB('zybachDb'); // we can not use "use" statement here to switch db

db.createUser(
    {
        user: "zybachLocalUser",
        pwd: "password",
        roles: [ { role: "readWrite", db: "zybachDb"} ],
        passwordDigestor: "server",
    }
)