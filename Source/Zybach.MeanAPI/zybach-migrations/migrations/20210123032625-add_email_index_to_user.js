module.exports = {
  async up(db, client) {
    
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        db.collection("User").createIndex({
          "Email": 1
        }, {
          unique: true,
          "name": "IX_User_EmailUnique"
        });
      });
    } finally {
      session.endSession();
    }
  },

  async down(db, client) {
    
    try {
      await session.withTransaction(async () => {
        db.collection("User").dropIndex("IX_User_EmailUnique");
      }); 
    } finally {
      session.endSession();
    }
  }
};
