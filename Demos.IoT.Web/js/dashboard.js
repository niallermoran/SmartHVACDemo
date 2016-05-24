$(document).ready(function () {

    var iteration = 0;
    var LatestThermoStatOptions;
    var chart = document.getElementById("myDashboard");

    // setup the latest thermostat reading bar chart
    SetupLatestThermostatBarChart();

    function SetupLatestThermostatBarChart() {

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
   
        function onDataReceived(data) {

            var chartData = [];

            for (var i = 0, len = data.length; i < len; i++) {
                var temp = data[i];
                chartData[i] = [i + 1, temp.Internaltemp];
            }

            //alert(chartData);

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
            Flotr.draw(chart, [chartSeries], LatestThermoStatOptions)

           

        }

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

    function FetchDeviceData(callback) {

        $.ajax({
            url: "/api/Device",
            type: "GET",
            dataType: "json",
            success: callback
        });
    }



});

