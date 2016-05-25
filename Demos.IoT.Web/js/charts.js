$(document).ready(function () {

    // populate the charts
    PopulateCharts();

    function PopulateCharts() {
        PopulateRealTimeChart();
    }

    function PopulateRealTimeChart()
    {
        // get the latest figures for all devices
        $.ajax({
            url: "/api/TemperatureReadings?count=50",
            type: "GET",
            dataType: "json",
            success: onDataReceived
        });


        function onDataReceived(data) {
            
            CreateTemperatureChart(data);
            CreateFootfallChart(data);

            PopulateRealTimeChart();
            //setTimeout(PopulateRealTimeChart, 2000);
        }

        function CreateTemperatureChart( data )
        {

            var chart = document.getElementById("realtimeChart");

            if (data != null && data.length > 0) {


                var rollingseries = [];
                var ticks = [];
                var colors = ["Green", "Blue", "Black", "Yellow", "Purple"];
                
                for (var i = 0; i < data.length; i++) {
                    try {

                        // get the device object as we will want some data from it
                        var device = data[i].Device;

                        // get the sound data for the device
                        var deviceTempData = data[i].TemperatureReadings;

                        // set up max 10 ticks
                        var indexCount = (deviceTempData.length < 10 ? deviceTempData.length : 10);
                        var indexDelta = Math.floor(deviceTempData.length / indexCount);
                
                        if ( ticks.length == 0 && indexCount > 0) {
                            ticks = [];
                              for (var bar = 1; bar <= deviceTempData.length - 1; bar += indexDelta) {
                                var point = [];
                                point = [bar, deviceTempData[bar - 1].TimeLabelShort];
                                ticks.push(point);
                            }
                        }

                        // get the chart data for this device
                        var chartData = [];
                        for (var foo = 0; foo < deviceTempData.length; foo++) {
                            var temp = deviceTempData[foo];
                            var point = [foo, temp.Internaltemp];
                            chartData.push(point);
                        }

                        rollingseries.push({ data: chartData, color: colors[i], label: device.DeviceName, lines: { fill: false } });

                    }
                    catch (e) {
                        alert(e.message);
                    }
                }


                function labelAVGFn(label) {
                    return " - " + label;
                }

                // Load all the data in one pass; if we only got partial
                // data we could merge it with what we already have.
                Flotr.draw(chart, rollingseries, {
                    xaxis: {
                        mode: 'time',
                        ticks: ticks,
                        labelsAngle: 45
                    },
                    yaxis: {
                        min: 10,
                        max: 45
                    },
                    grid: {
                        minorVerticalLines: true
                    },
                    mouse: {
                        track: true,		// => true to track mouse
                        relative: true,		// => position to show the track value box
                        margin: 3,		// => margin for the track value box
                        color: '#ff3f19',	// => color for the tracking points, null to hide points
                        trackDecimals: 1,	// => number of decimals for track values
                        radius: 3,		// => radius of the tracking points
                        sensibility: 2		// => the smaller this value, the more precise you've to point with the mouse
                    },
                    title: 'Latest Thermostat Data',
                    legend: {
                        position: 'sw',            // Position the legend 'south-east'.
                        labelFormatter: labelAVGFn,   // Format the labels.
                        backgroundColor: '#D2E8FF' // A light blue background color.
                    },
                });

            }
            else {
                var chartDiv = $("div#realtimeChart");
                chartDiv.html('<p style="text-align:center">Sorry no data to display, try changing the filter condition</p>');
            }
        }

        function CreateFootfallChart(data) {

            var chart = document.getElementById("realtimeFootFallChart");

            if (data != null && data.length > 0) {


                var rollingseries = [];
                var ticks = [];
                var colors = ["Green", "Blue", "Black", "Yellow", "Purple"];

                for (var i = 0; i < data.length; i++) {
                    try {

                        // get the device object as we will want some data from it
                        var device = data[i].Device;

                        // get the sound data for the device
                        var deviceTempData = data[i].TemperatureReadings;

                        // set up max 10 ticks
                        var indexCount = (deviceTempData.length < 10 ? deviceTempData.length : 10);
                        var indexDelta = Math.floor(deviceTempData.length / indexCount);

                        if ( ticks.length == 0 && indexCount > 0) {
                            ticks = [];
                            for (var bar = 1; bar <= deviceTempData.length - 1; bar += indexDelta) {
                                var point = [];
                                point = [bar, deviceTempData[bar - 1].TimeLabelShort];
                                ticks.push(point);
                            }
                        }

                        // get the chart data for this device
                        var chartData = [];
                        for (var foo = 0; foo < deviceTempData.length; foo++) {
                            var temp = deviceTempData[foo];
                            var point = [foo, temp.NumberofPeople];
                            chartData.push(point);
                        }

                        rollingseries.push({ data: chartData, color: colors[i], label: device.DeviceName, lines: { fill: false } });

                    }
                    catch (e) {
                        alert(e.message);
                    }
                }


                function labelAVGFn(label) {
                    return " - " + label;
                }

                // Load all the data in one pass; if we only got partial
                // data we could merge it with what we already have.
                Flotr.draw(chart, rollingseries, {
                    xaxis: {
                        mode: 'time',
                        ticks: ticks,
                        labelsAngle: 45
                    },
                    yaxis: {
                        min: 0
                    },
                    grid: {
                        minorVerticalLines: true
                    },
                    mouse: {
                        track: true,		// => true to track mouse
                        relative: true,		// => position to show the track value box
                        margin: 3,		// => margin for the track value box
                        color: '#ff3f19',	// => color for the tracking points, null to hide points
                        trackDecimals: 1,	// => number of decimals for track values
                        radius: 3,		// => radius of the tracking points
                        sensibility: 2		// => the smaller this value, the more precise you've to point with the mouse
                    },
                    title: 'Latest Footfall Data',
                    legend: {
                        position: 'sw',            // Position the legend 'south-east'.
                        labelFormatter: labelAVGFn,   // Format the labels.
                        backgroundColor: '#D2E8FF' // A light blue background color.
                    },
                });

            }
            else {
                var chartDiv = $("div#realtimeChart");
                chartDiv.html('<p style="text-align:center">Sorry no data to display, try changing the filter condition</p>');
            }
        }
    }

});