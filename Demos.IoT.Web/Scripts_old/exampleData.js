var myExampleData = {};



//pie Chart sample data and options
myExampleData.pieChartData = [{
    data: [[0, 4]],
    label: "Comedy"
}, {
    data: [[0, 3]],
    label: "Action"
}, {
    data: [[0, 1.03]],
    label: "Romance",
    pie: {
        explode: 50
    }
}, {
    data: [[0, 3.5]],
    label: "Drama"
}];

myExampleData.pieChartOptions = {
    HtmlText: false,
    grid: {
        verticalLines: false,
        horizontalLines: false
    },
    xaxis: {
        showLabels: false
    },
    yaxis: {
        showLabels: false
    },
    pie: {
        show: true,
        explode: 6
    },
    mouse: {
        track: true
    },
    legend: {
        position: "se",
        backgroundColor: "#D2E8FF"
    }
};

//Pie chart sample data ends here

//bar Chart sample data and options

myExampleData.constructBubbleChartData = function () {
    var d1 = [];
    var d2 = []
    var point
    var i;

    for (i = 0; i < 10; i++) {
        point = [i, Math.ceil(Math.random() * 10), Math.ceil(Math.random() * 10)];
        d1.push(point);

        point = [i, Math.ceil(Math.random() * 10), Math.ceil(Math.random() * 10)];
        d2.push(point);
    }
    return [d1, d2];
};
myExampleData.bubbleChartData = myExampleData.constructBubbleChartData();

myExampleData.bubbleChartOptions = {
    bubbles: {
        show: true,
        baseRadius: 5
    },
    xaxis: {
        min: -4,
        max: 14
    },
    yaxis: {
        min: -4,
        max: 14
    },
    mouse: {
        track: true,
        relative: true
    }
};

//bar chart sample data ends here

//bar Chart sample data and options

var deviceTicks = [[1, "Factory 1"], [2, "Factory 2"], [3, "Factory 3"], [4, "Factory 4"]]// Ticks for the Y-Axis

myExampleData.constructBarChartData = function () {

    // get the url for the web api call
    var url = "/api/Thermostat";

    $.getJSON(url, null, function (data) {

        var dataPoints = [];
        //    alert(data.length);
        for (var i = 0, len = data.length; i < len; i++) {
            var result = data[i];
          // alert(result.Internaltemp);
            point = [result.Internaltemp, i + 1];
            dataPoints.push(point);
        }

        dataPoints = [[4, 1], [1, 2], [2, 3], [3, 4]];

        myExampleData.barChartData = dataPoints;

        myExampleData.barChartOptions = {
            bars: {
                show: true,
                horizontal: true,
                shadowSize: 2,
                barWidth: 0.5
            },
            mouse: {
                track: true,
                relative: true
            },
            yaxis: {
                //ticks: deviceTicks,
                min: 1,
                autoscaleMargin: 1
            },
            xaxis: {
                min: 19,
                max: 29
            }
        };
    });
};


myExampleData.constructBarChartData();


//bar chart sample data ends here

//line Chart sample data and options

myExampleData.constructLineChartData = function () {
    var d1 = [[0, 3], [4, 8], [8, 5], [9, 13]];
    var d2 = [];
    var i;

    for (i = 0; i < 14; i += 0.5) {
        d2.push([i, Math.sin(i)]);
    }
    return [d1, d2];
};
myExampleData.lineChartData = myExampleData.constructLineChartData();

myExampleData.lineChartOptions = {
    xaxis: {
        minorTickFreq: 4
    },
    grid: {
        minorVerticalLines: true
    },
    selection: {
        mode: "x",
        fps: 30
    }
};

//line chart sample data ends here

//table Widget sample data and options

myExampleData.constructTableWidgetData = function () {
    return ["Trident" + Math.ceil(Math.random() * 10), "IE" + Math.ceil(Math.random() * 10), "Win" + Math.ceil(Math.random() * 10)]
};

myExampleData.tableWidgetData = {
    "aaData": [myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData(),
	myExampleData.constructTableWidgetData()
    ],

    "aoColumns": [{
        "sTitle": "Engine"
    }, {
        "sTitle": "Browser"
    }, {
        "sTitle": "Platform"
    }],
    "iDisplayLength": 25,
    "aLengthMenu": [[1, 25, 50, -1], [1, 25, 50, "All"]],
    "bPaginate": true,
    "bAutoWidth": false
};
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//table widget sample data ends here
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
