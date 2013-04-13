function LookupCoordinates(zip) {
    $.post("/Home/LookupCoordinates",
        { Zip: zip, Country: "US" },
        function (data) {
            var result = eval('(' + data + ')');
            var coordinates = result.split(",");
            $("#Lat").val(coordinates[0]);
            $("#Lon").val(coordinates[1]);
        });
}