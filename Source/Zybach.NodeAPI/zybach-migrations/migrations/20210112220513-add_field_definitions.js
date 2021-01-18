module.exports = {
  async up(db, client) {
    const session = client.startSession();
    try {
      // use transaction API to ensure that entire script executes
      await session.withTransaction(async () => {
        // Insert our first field definition 
        await db.collection("FieldDefinition").insertOne(
          {
            FieldDefinitionID: 1,
            FieldDefinitionName: "Name",
            FieldDefinitionDisplayName: "Name",
            FieldDefinitionValue: "Default definition for Name"
          }
        );

        // declare indexes on the fields we require to be unique
        await db.collection("FieldDefinition").createIndex({
          "FieldDefinitionID": 1
        }, {
          unique: true
        })
        await db.collection("FieldDefinition").createIndex({
          "FieldDefinitionName": 1
        }, {
          unique: true
        })
        await db.collection("FieldDefinition").createIndex({
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
    await db.collection("FieldDefinition").drop();
  }
};
