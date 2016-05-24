'use strict';

console.log("Starting ..");

var Five = require("johnny-five");
var Edison = require("edison-io");

// Iot Hub objects
var Protocol = require('azure-iot-device-amqp').Amqp;
var Client = require('azure-iot-device').Client;
var Message = require('azure-iot-device').Message;

var connectionString = 'HostName=[eventhubname].azure-devices.net;DeviceId=[device id];SharedAccessKeyName=[shared access key];SharedAccessKey=[key]';

var client = Client.fromConnectionString(connectionString, Protocol);

// Define the sensors you will use
var temp;

var lcd;

// Define the board, which is an abstraction of the Intel Edison
var board = new Five.Board({
	io: new Edison()
});

// The board.on() executes the anonymous function when the
// board reports back that it is initialized and ready. 
board.on("ready", function () {
	
	console.log("Board connected...");
	
	temp = new Five.Thermometer({
		controller: "GROVE",
		pin: "A0",
		freq: 1000
    });
    
    //lcd = new Five.LCD({
    //    controller: 'JHD1313M1',
    //    backlight: 6,
    //    rows: 2,
    //    cols: 20
    //});
   
  //  lcd.print("Board Ready");
});


var connectCallback = function (err) {
    if (err) {
        console.error('Could not connect: ' + err.message);
    } else {
        console.log('Client connected');
        
        
        var isHeatingOn = 0;

        client.on('message', function (msg) {
            
            // get the action sent from the cloud
            var action = msg.data;

             
            if (action == "TurnOn" && isHeatingOn == 0) {
                
                // receives messages sent from the cloud
                console.log('Message received : ' + action);
                

                isHeatingOn = 1;
                //lcd.clear();
                //lcd.print("Heating On");

            }
            
            if (action == "TurnOff" && isHeatingOn == 1) {
                
                // receives messages sent from the cloud
                console.log('Message received : ' + action);
                
                
                isHeatingOn = 0;
                
                //lcd.clear();
                //lcd.print("Heating Off");

            }

            client.complete(msg, printResultFor('completed'));
        });
        
        // Create a message and send it to the IoT Hub every second
        var sendInterval = setInterval(function () {
            
            var data = JSON.stringify( {
                DeviceId: '00255c2004',
                DeviceName : 'Factory 5',
                FloorArea: '30000',
                Internaltemp: temp.celsius,
                ExternalTemp: temp.celsius - 15,
                IsHeatingOn: isHeatingOn,
                NumberofPeople: 75,
                Time: new Date()
            });

            var message = new Message(data);
         //   console.log('Sending message: ' + message.getData());
            client.sendEvent(message, printResultFor('sent temp of ' + temp.celsius.toString() + ( ( isHeatingOn == 1 ) ? "Heating ON": "Heating Off" ) ));
        }, 2000);
        
        client.on('error', function (err) {
            console.error(err.message);
        });
        
        client.on('disconnect', function () {
            clearInterval(sendInterval);
            client.removeAllListeners();
            client.connect(connectCallback);
        });
    }
};

client.open(connectCallback);

// Helper function to print results in the console
function printResultFor(op) {
    return function printResult(err, res) {
        if (err) console.log(op + ' error: ' + err.toString());
        if (res) console.log(op + ' status: ' + res.constructor.name);
    };
}
