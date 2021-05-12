module.exports = {
  async up(db, client) {
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        await db.collection('CustomRichText').insertMany([
          {
            CustomRichTextID: 6,
            CustomRichTextName: 'Training',
            CustomRichTextDisplayName: 'Training',
            CustomRichTextContent: "<p>Training content goes here.</p>"
          },
        {
          CustomRichTextID: 7,
          CustomRichTextName: 'RobustReviewScenario',
          CustomRichTextDisplayName: 'Robust Review Scenario',
          CustomRichTextContent: "<p>The Robust Review Scenario is currently under development. Click the button below to get the example .json file that the Robust Review Scenario in the Groundwater Evaluation Toolbox will consume.</p>"
        }]);
      });
    } finally {
      session.endSession
    }
  },

  async down(db, client) {
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        db.collection("CustomRichText").deleteMany( {"CustomRichTextID" : {$in: [6,7]}});
      }); 
    } finally {
      session.endSession();
    }
  }
};