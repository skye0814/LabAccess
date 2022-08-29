$(document).ready(function () {

    // Fetch timetable at start
    if ($('.facilityDropdown').val() != "") {
        GetClassScheduleByFacilityID();
    }

    $('.facilityDropdown').on("change", function () {
        GetClassScheduleByFacilityID();
    });

    // Timetable code
    function GetClassScheduleByFacilityID() {
        $.ajax({
            type: 'GET',
            url: 'Home/GetClassScheduleByFacilityID',
            data: {
                FacilityID: $('.facilityDropdown').val()
            },
            dataType: "json",
            traditional: false,
            success: function (data) {

                if (data.length == 0) {
                    var timetable = new Timetable();
                    var todayDateOnly = moment(new Date()).format("M/D/YYYY");
                    timetable.setScope(7, 21); // optional, only whole hours between 0 and 23
                    timetable.useTwelveHour();
                    timetable.addLocations(['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']);

                    var renderer = new Timetable.Renderer(timetable);
                    renderer.draw('.timetable');
                }
                else {
                    var timetable = new Timetable();
                    var todayDateOnly = moment(new Date()).format("M/D/YYYY");
                    timetable.setScope(7, 21); // optional, only whole hours between 0 and 23
                    timetable.useTwelveHour();
                    timetable.addLocations(['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']);

                    for (var i = 0; i < data.length; i++) {
                        timetable.addEvent(data[i].CourseName, data[i].DayOfTheWeek, new Date(data[i].TimeIn), new Date(data[i].TimeOut));
                    }
                    

                    /*timetable.addEvent('Frankadelic', 'Nile', new Date("7/23/2022 7:30 AM"), new Date("7/23/2022 9:30 AM"));*/

                    var renderer = new Timetable.Renderer(timetable);
                    renderer.draw('.timetable');
                }
            },
            error: function (error) {
                console.log(error);
            },
            beforeSend: function () {
            },
            complete: function () {
            }
        });
    }
    


    // ChartJS code
    // Fetch table data at start
    var DateTo = new Date().toLocaleString("en-US", {}).split(",").join(" ");
    var DateFrom = getDaysAgo(30).toLocaleString("en-US", {}).split(",").join(" ");
    GetEquipmentCategoryUsageByDate(DateFrom, DateTo);

    function GetEquipmentCategoryUsageByDate(DateFrom, DateTo) {
        $.ajax({
            type: 'GET',
            url: getEquipmentUsageByDate,
            data: {
                DateFrom: DateFrom,
                DateTo: DateTo
            },
            dataType: "json",
            traditional: false,
            success: function (data) {

                if (data.length == 0){
                    // Show empty message
                    $('.barchart-message').css('display','block');

                    // Hide date dropdown
                    $('.classic').css('display','none');

                    // Hide the chart element
                    $('#myChart').css('display','none');
                }
                else{
                    var xValues = [];
                    var yValues = [];

                    // Push data values to the array -- for barchart values
                    for (var i = 0; i < data.length; i++) {
                        xValues[i] = data[i].CategoryCode;
                        yValues[i] = data[i].TotalNumberBorrowed
                    }

                    // console.log(xValues);
                    // console.log(yValues);
                    
                    // Instantiate new table
                    var ctx = document.getElementById('myChart');
                    var newChart = new Chart(ctx, {
                        type: "bar",
                        data: {
                            labels: xValues,
                            datasets: [{
                                backgroundColor: [
                                    'rgba(255,141,41,1)'
                                ],
                                borderColor: [
                                    'rgba(255,98,0,1)'
                                ],
                                borderWidth: 3,
                                data: yValues,
                                barThickness: 35,
                                label: "Most Used Equipments",
                                borderRadius: 5
                            }]
                        },
                        options: {
                            //responsive: true,
                            //maintainAspectRatio: false,
                            resizeDelay: 0,
                            indexAxis: 'x',
                            scales: {
                                x: {
                                    grid: {
                                        display: false
                                    },
                                    title: {
                                        font: {
                                            family: 'Product Sans'
                                        }
                                    },
                                    font: {
                                        family: 'Product Sans'
                                    }
                                },
                                y: {
                                    grid: {
                                        display: false
                                    },
                                    ticks: {
                                        precision: 0
                                    }
                                      
                                }
                            },
                            layout: {
                                padding: 0
                            },
                            plugins: {
                                legend: {
                                    display: false,
                                    labels: {
                                        font: {
                                            family: 'Product Sans'
                                        }
                                    }
                                },
                                title: {
                                    display: false,
                                    text: "Equipment Usage"
                                }
                            }
                        },
                    });
                    
                    // Destroy chart if the dropdown is changed
                    $('.classic').change(function(){
                        newChart.destroy();
                    });
                }
            },
            error: function (error) {
                console.log(error);
            },
            beforeSend: function () {
            },
            complete: function () {
            }
        });
    }

    // Function for subtracting days from date
    function getDaysAgo(date) {
        var a = new Date;
        a.setDate(a.getDate() - date);
        return a
    };

    // If the dropdown is changed, pass its values from ajax to controller
    $('.classic').change(function () {
        if ($('.classic').val() == 'Last 30 days') {
            var DateTo = new Date().toLocaleString("en-US", {}).split(",").join(" ");
            var DateFrom = getDaysAgo(30).toLocaleString("en-US", {}).split(",").join(" ");

            GetEquipmentCategoryUsageByDate(DateFrom, DateTo);
        }
        if ($('.classic').val() == 'Last 7 days') {

            var DateTo = new Date().toLocaleString("en-US", {}).split(",").join(" ");
            var DateFrom = getDaysAgo(7).toLocaleString("en-US", {}).split(",").join(" ");

            GetEquipmentCategoryUsageByDate(DateFrom, DateTo);
        }
        if ($('.classic').val() == 'Last 1 day') {

            var DateTo = new Date().toLocaleString("en-US", {}).split(",").join(" ");
            var DateFrom = getDaysAgo(1).toLocaleString("en-US", {}).split(",").join(" ");

            GetEquipmentCategoryUsageByDate(DateFrom, DateTo);
        }
    });
    
});