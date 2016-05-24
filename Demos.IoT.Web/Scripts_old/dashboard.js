$(function () {

    var iteration = 0;
    var LatestThermoStatOptions;

    // setup the latest thermostat reading bar chart
    SetupLatestThermostatBarChart(document.getElementById("myDashboard"));

    function SetupLatestThermostatBarChart(element) {
        var iteration = 0;

        // Radar Labels
        var ticks = [
          [1, "Factory 1"],
          [2, "Factory 2"],
          [3, "Factory 3"],
          [4, "Factory 4"],
        ];

        LatestThermoStatOptions = {
            yaxis: { min: 19, max: 26, minorTickFreq: 0.5 },
             xaxis: { ticks: ticks },
            mouse: {
                track: true,
                relative: true
            }
        };

        FetchLatestThermostatData();
    }

    function FetchLatestThermostatData() {

        var element = document.getElementById("myDashboard");

        ++iteration;

        function onDataReceived(data) {

            var chartData = [];

            for (var i = 0, len = data.length; i < len; i++) {
                var temp = data[i];
                chartData[i] = [i + 1, temp.Internaltemp];
            }

            var chartSeries = { 
                data: chartData, bars: {
                    show: true,
                    barWidth: 0.8,
                    fillColor: ['#00A8F0', '#fff'],
                    fillOpacity: 0.8,
                }
            };

            // Load all the data in one pass; if we only got partial
            // data we could merge it with what we already have.
            Flotr.draw(element, [chartSeries], LatestThermoStatOptions)

           
        }

        $.ajax({
            url: "/api/Thermostat",
            type: "GET",
            dataType: "json",
            success: onDataReceived
        });
        setTimeout(FetchLatestThermostatData, 1500);
   
    }


    function SetupRealTimeLineChart(element) {
        var iteration = 0;

        LatestThermoStatOptions = {
            yaxis: { min: 19, max: 23, minorTickFreq: 0.5 },
            mouse: {
                track: true,
                relative: true
            }
        };

        FetchDeviceData(FetchLatestRealTimeData);
    }

    function FetchLatestRealTimeData(deviceData) {

        var chartseries = [];

        for (var i = 0, len = deviceData.length; i < len; i++) {
            var temp = deviceData[i];
            $.ajax({
                url: "/api/DeviceTemperatures/" + temp.Deviceid,
                type: "GET",
                dataType: "json",
                success: onDeviceRealtimeDataReceived
            });
        }


        function onDeviceRealtimeDataReceived(data) {

            var chartData = [];

            for (var i = 0, len = data.length; i < len; i++) {
                var temp = data[i];
                chartData[i] = [temp.Time, temp.Internaltemp];
            }

            var chartSeries = {
                label: data[0].Deviceid,
                data: chartData, bars: {
                    show: true,
                    barWidth: 0.8,
                    fillColor: ['#00A8F0', '#fff'],
                    fillOpacity: 0.8,
                }
            };

            // Load all the data in one pass; if we only got partial
            // data we could merge it with what we already have.
            Flotr.draw(element, [chartSeries], LatestThermoStatOptions)
        }

    

        //$.ajax({
        //    url: "/api/Device",
        //    type: "GET",
        //    dataType: "json",
        //    success: onDeviceDataReceived
        //});


        //setTimeout(FetchLatestThermostatData, 1500);

    }

    function FetchDeviceData( callback ) {

        $.ajax({
            url: "/api/Device",
            type: "GET",
            dataType: "json",
            success: callback
        });
    }



});


function getMap() {
    var map = new Microsoft.Maps.Map(document.getElementById('map'), {
        credentials: 'AqL6HufCTSGkky_IGwoIynFD1auBGW89-WwrxMCOnrXP0aiuArJlzZpunPQUOXzn',
        center: new Microsoft.Maps.Location(53.360817, -6.251108),
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        zoom: 2,
        showLocateMeButton: false,
        showMapTypeSelector: false,
        showScalebar: false,
    });



    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(53.125912, -7.377517), { text: '', title: 'Ireland' });
    map.entities.push(pushpin);

    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(35.339195, -78.400170), { text: '', title: 'US East Coast' });
    map.entities.push(pushpin);

    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(40.451631, 15.696828), { text: '', title: 'Italy' });
    map.entities.push(pushpin);
    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(59.890702, 10.661576), { text: '', title: 'Norway' });
    map.entities.push(pushpin);

    Microsoft.Maps.Events.addHandler(map, 'click', function () { highlight('mapClick'); });
}

function highlight(id) {
    document.getElementById(id).style.background = 'LightGreen';
    setTimeout(function () { document.getElementById(id).style.background = 'white'; }, 1000);
}

