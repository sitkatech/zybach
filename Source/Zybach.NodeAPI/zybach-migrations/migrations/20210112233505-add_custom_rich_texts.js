module.exports = {
  async up(db, client) {
    const session = client.startSession();
    try {
      await session.withTransaction(async () => {
        await db.collection('CustomRichText').insertMany([{
          CustomRichTextID: 1,
          CustomRichTextName: 'PlatformOverview',
          CustomRichTextDisplayName: 'Platform Overview',
          CustomRichTextContent: "<p>Platform overview goes here.</p>"
        },
        {
          CustomRichTextID: 2,
          CustomRichTextName: 'Disclaimer',
          CustomRichTextDisplayName: 'Disclaimer',
          CustomRichTextContent: "<p>Disclaimer content</p>"
        },
        {
          CustomRichTextID: 3,
          CustomRichTextName: 'HomePage',
          CustomRichTextDisplayName: 'Home Page',
          CustomRichTextContent: "<p>Welcome to the FRESCA, a real good soda.  It is designed to meet these objectives:</p>      <ul>          <li>Act as a base instance for every new SPA application that H2O creates.</li>          <li>Reflect the latest and greatest common functionality across all apps.</li>          <li>Should be kept up to date any time a common library is updated.</li>      </ul>"
        },
        {
          CustomRichTextID: 4,
          CustomRichTextName: 'Help',
          CustomRichTextDisplayName: 'Help',
          CustomRichTextContent: '<p>Help me please!</p>'
        },
        {
          CustomRichTextID: 5,
          CustomRichTextName: 'LabelsAndDefinitionsList',
          CustomRichTextDisplayName: 'Labels and Definitions List',
          CustomRichTextContent: "A list of Labels in the system and their Definitions"
        }
        ]);

        await db.collection("CustomRichText").createIndex({
          "CustomRichTextID": 1
        }, {
          unique: true
        });
        await db.collection("CustomRichText").createIndex({
          "CustomRichTextName": 1
        }, {
          unique: true
        });
        await db.collection("CustomRichText").createIndex({
          "CustomRichTextDisplayName": 1
        }, {
          unique: true
        });
      });
    } finally {
      session.endSession
    }
  },

  async down(db, client) {
    db.collection("CustomRichText").drop();
  }
};
