var express = require('express');
var router = express.Router();
const ctrlWells = require('../controllers/wells');

//pumpedVolume
router.route('/wells/:wellid/pumpedVolume').get(ctrlWells.getPumpedVolume)

module.exports = router;
