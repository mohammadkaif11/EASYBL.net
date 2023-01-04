GetData()

function GetData() {
    $.ajax({
        type: "GET",
        url: '/Bill/MonthReview',
        success: function (data) {
            Showgraph(data);
        },
        error: function (error) {
            Showgraph([])
            console.log(error)
        }
    });
}

function Showgraph(arr) {
    const month = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    const d = new Date();
    let monthName = month[d.getMonth()];
        let labels = new Array();
        for (var i = 1; i < arr.length; i++) {
            labels[i] = i;
        }
        const data = {
            labels: labels,
            datasets: [{
                label: 'Month Review',
                backgroundColor: '#f8f9fa',
                borderColor: 'black',
                data: arr
            }]
        };

        const config = {
            type: 'line',
            data: data,
            options: {
                scales: {
                    y: {
                        title: {
                            display: true,
                            text: "Income",
                            font: {
                                size: 15
                            }
                        },
                    },
                    x: {
                        title: {
                            display: true,
                            text: monthName,
                            font: {
                                size: 15
                            }

                        }
                    }
                },
            }
        };
        const myChart = new Chart(
            document.getElementById('myChart'),
            config
        );
}


