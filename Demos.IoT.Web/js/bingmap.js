$(document).ready(function () {

    LoadMap();

    // called automatically by loading page
    function LoadMap() {
        // create the map
        MAP = new Microsoft.Maps.Map(document.getElementById('map'), {
            credentials: 'AqL6HufCTSGkky_IGwoIynFD1auBGW89-WwrxMCOnrXP0aiuArJlzZpunPQUOXzn',
            center: new Microsoft.Maps.Location(52.456247, -31.485909),
            mapTypeId: Microsoft.Maps.MapTypeId.aerial,
            zoom: 2,
            showLocateMeButton: false,
            showMapTypeSelector: false,
            showScalebar: false,
            showDashboard: false
        });

        // load devices to show pushpins
        $.ajax({
            url: "/api/Device",
            type: "GET",
            dataType: "json",
            success: DevicesRetrieved
        });

        /// create a click event handler
        Microsoft.Maps.Events.addHandler(MAP, 'click', function () { highlight('mapClick'); });

        // do something on click
        function highlight(id) {

        }

        function DevicesRetrieved(data) {
            for (var i = 0, len = data.length; i < len; i++) {
                var device = data[i];
                try {
                    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(device.Longitude, device.Latitude), { text: (i+1).toString(), title: device.DeviceLocation });
                    MAP.entities.push(pushpin);
                } catch (e) { }

            }

        }
    }


});