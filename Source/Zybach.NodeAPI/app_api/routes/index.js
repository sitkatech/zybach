var express = require('express');
var router = express.Router();
const ctrlWells = require('../controllers/wells');

//pumpedVolume
router.route('/wells/:wellid/pumpedVolume').get(ctrlWells.getPumpedVolume)

router.route('/').get((req, res) => {
    res.status(200).json({status:"success"});
})

module.exports = router;
