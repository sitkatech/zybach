#!
mongodump --out=/dump $CONNECTION_STRING_PROD
mongorestore -d zybachDb --drop $CONNECTION_STRING_QA /dump/zybachDbProd