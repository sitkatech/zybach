const streamFlowZones = require("../data/streamflow.json");

console.log(streamFlowZones);

module.exports = {
  async up(db, client) {
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        db.collection("StreamFlowZone").insertMany(streamFlowZones);

        db.collection("StreamFlowZone").createIndex({geometry: "2dsphere"});
      });
    } finally {
      session.endSession();
    }
  },

  async down(db, client) {
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        db.collection("StreamFlowZone").drop();
      }); 
    } finally {
      session.endSession();
    }
  }
};
