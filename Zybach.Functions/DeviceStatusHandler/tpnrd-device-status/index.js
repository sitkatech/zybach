let trello = require("../trello-handler");

module.exports = async function (context, request) {

    // when this function is published to Azure, Azure will post a validation event message to the function
    // to verify the endpoint. the validation event contains a validation code that must be returned in
    // the body of the response for the publication of the function to be successful

    if (request.headers['aeg-event-type'] === "SubscriptionValidation") {
        context.log('Received a webhook validation event.');

        if (!Array.isArray(request.body) || !request.body.length) {
            let errorMessage = 'Expected an array containing a validation event, but found none.';
            context.log(errorMessage);

            context.res = { 
                body: {message: errorMessage}
            }
            context.res.status = 400;
        }
        else {
            // echo back the validation code given to us
            let validationEvent = request.body[0];
            
            context.res = {
                body: {
                    validationResponse: validationEvent.data.validationCode
                }
            }
        }

        return;
    }

    // everything below here is for the "normal" device status event messages

    context.log('Received a webhook device status event.');

    // we use a client shared secret to prevent random folks from POSTing to this endpoint
    if (request.query.clientID !== '72e0785e-2f33-4520-8162-241916317f26') {
        context.res = {
            body: {message: 'Missing or invalid client id'}
        };
        context.res.status = 401;        
        return;
    }

    // i've never seen it, but there can be multiple events in a single message and we 
    // need to verify we have an array. 

    if (!Array.isArray(request.body))
    {
        let response = 'Expect body to be an array of events';
        context.log(response);
        context.res = {
            body: { message: response }
        };
        context.res.status = 400;        
        return;
    }

    let response = null;

    for (let e = 0; e < request.body.length; e++) {
        // only reports on the last response, but that's OK for now
        response = await trello.handleStatusEvent(context, request.body[e]);        
        
        // todo: add other handlers here if additional actions need to take palce
    }

    context.res = {
        body: { message: response }
    };

    // need better error reporting off the handleStatusEvent() call

    context.res.status = 200;
}