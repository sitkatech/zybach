var express = require('express');
var router = express.Router();
const ctrlWells = require('../controllers/wells');

/**
* @swagger
* definitions:
*   PumpedVolumeReturnObject:
*       properties:
*           status: 
*               type: string
*               example: success
*               description: status of the request
*           result:
*               type: object
*               description: an object containing the operation results
*               properties:
*                   intervalCountTotal:
*                       type: integer
*                       description: total number of intervals returned
*                   intervalWidthInMinutes:
*                       type: integer
*                       description: length of each interval in minutes
*                   intervalStart:
*                       type: string
*                       format: date-time
*                       description: date (as ISO string) when intervals began
*                   intervalEnd:
*                       type: string
*                       format: date-time
*                       description: date (as ISO string) when intervals ended
*                   durationInMinutes:
*                       type: integer
*                       description: total number of minutes between intervalStart and intervalEnd (rounded down)
*                   wellCount:
*                       type: integer
*                       description: number of unique wells contained in results
*                   volumesByWell:
*                       type: array
*                       description: Array of objects that contain well metadata and an array of time series objects for that well
*                       items: 
*                           type: object
*                           properties:
*                               wellRegistrationID:
*                                   type: string
*                                   example: G-118986
*                                   description: registration ID for the well
*                               intervalCount:
*                                   type: number
*                                   description: total number of intervals returned for this well
*                               intervalVolumes:
*                                   type: array
*                                   description: Array of objects that contain time series data representing the pump rate for a well at the end of an interval
*                                   items:
*                                       type: object
*                                       properties:
*                                           intervalEndTime:
*                                               type: string
*                                               format: date-time
*                                               description: date (as ISO string) when specified interval ended
*                                           gallonsPumped:
*                                               type: number
*                                               format: double
*                                               description: gallons pumped over the previous interval
*   Error:
*       properties:
*           status: 
*               type: string
*               description: status of the request
*           result:
*               type: string
*               description: details on why request failed
*/

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
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/PumpedVolumeReturnObject'
*           
*       400:
*         description: If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/Error'
*       401:
*         description: Unauthorized to perform request
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/Error'
*       403:
*         description: Forbidden
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/Error'
*/
router.route('/wells/:wellRegistrationID/pumpedVolume').get(ctrlWells.getPumpedVolume);


/**
* @swagger
* /api/wells/pumpedVolume:
*   get:
*     tags: 
*       - Wells
*     summary: Returns a time series representing pumped volume at a well or series of wells, averaged on a chosen reporting interval, for a given date range. Each point in the output time series represents the average pumped volume over the previous reporting interval.
*     parameters:
*       - name: filter
*         description: The Well Registration ID(s) for the requested Well(s). If left blank, will bring back data for every Well that has reported data within the time range.
*         in: query
*         required: false
*         schema:
*           type: array
*           items: 
*               type: string
*           minItems: 1
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
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/PumpedVolumeReturnObject'
*           
*       400:
*         description: If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/Error'
*       401:
*         description: Unauthorized to perform request
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/Error'
*       403:
*         description: Forbidden
*         content: 
*           application/json:
*               schema:
*                   type: object
*                   $ref: '#/definitions/Error'
*/
router.route('/wells/pumpedVolume').get(ctrlWells.getPumpedVolume);

/**
* @swagger
* /api/:
*   get:
*     tags:
*       - Test
*     summary: This should return a success response and shouldn't require authentication
*     responses: 
*       200:
*         description: Receive back success response.
*/
router.route('/').get((req, res) => {
    res.status(200).json({status:"success"});
});

module.exports = router;
