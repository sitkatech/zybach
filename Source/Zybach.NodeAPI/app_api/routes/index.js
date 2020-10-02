var express = require('express');
var router = express.Router();
const ctrlWells = require('../controllers/wells');

/**
* @swagger
* /api/wells/{wellRegistrationID}/pumpedVolume:
*   get:
*     tags: 
*       - Wells
*     summary: Returns a time series representing pumped volume at a well, averaged on a chosen reporting interval, for a given date range. Each point in the output time series represents the average pumped volume over the previous reporting interval.
*     parameters:
*       - name: wellRegistrationID
*         description: The Well Registration ID for the requested Well
*         in: path
*         required: true
*         schema:
*           type: string
*       - name: startDate
*         description: The start date for the report, formatted as an ISO date string with a timezone (eg. 2020-06-23T17:24:56+00:00)
*         in: query
*         required: true
*         schema:
*           type: string
*       - name: endDate
*         description: The end date for the report, formatted as an ISO date string with a timezone (eg. 2020-06-23T17:24:56+00:00). Defaults to today's date
*         in: query
*         required: false
*         schema:
*           type: string
*       - name: interval
*         description: The reporting interval, in minutes. Defaults to 60.
*         in: query
*         required: false
*         schema:
*           type: integer
*     security:
*       - bearerAuth: [] 
*     responses: 
*       200:
*         description: Returns the requested time series
*       400:
*         description: If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)
*       401:
*         description: Unauthorized to perform request
*       403:
*         description: Forbidden
*/
router.route('/wells/:wellRegistrationID/pumpedVolume').get(ctrlWells.getPumpedVolume);

/**
* @swagger
* /api/:
*   get:
*     tags:
*       - Test
*     summary: This should return a success response
*     responses: 
*       200:
*         description: Receive back success response.
*/
router.route('/').get((req, res) => {
    res.status(200).json({status:"success"});
});

module.exports = router;
