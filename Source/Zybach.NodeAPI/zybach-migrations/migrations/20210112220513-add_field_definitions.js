module.exports = {
  async up(db, client) {
    const session = client.startSession();
    try {
      // use transaction API to ensure that entire script executes
      await session.withTransaction(async () => {
        // Insert our first field definition 
        await db.collection("fieldDefinitions").insertOne(
          {
            FieldDefinitionID: 1,
            FieldDefinitionName: "Name",
            FieldDefinitionDisplayName: "Name",
            FieldDefinitionValue: "Default definition for Name"
          }
        );

        // declare indexes on the fields we require to be unique
        await db.collection("fieldDefinitions").createIndex({
          "FieldDefinitionID": 1
        }, {
          unique: true
        })
        await db.collection("fieldDefinitions").createIndex({
          "FieldDefinitionName": 1
        }, {
          unique: true
        })
        await db.collection("fieldDefinitions").createIndex({
          "FieldDefinitionDisplayName": 1
        }, {
          unique: true
        })
      })
    }
    finally {
      await session.endSession();
    }
  },

  async down(db, client) {
    await db.collection("fieldDefinitions").drop();
  }
};
