jQuery(function ($) {   





    google.charts.load('current', {'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    var totalEvento = parseInt( $("#txtTotal").val());   

    function drawChart() {

        var data = google.visualization.arrayToDataTable([
        ['Task', 'Hours per Day'],
        ['Work', 11],
        ['Eat', 2],
        ['Commute', 2],
        ['Cond/equipe', 2],
        ['Total de Eventos', totalEvento]
        ]);

            var options = {
            title: 'Gráfico de Eventos'
        };

        var chart = new google.visualization.PieChart(document.getElementById('piechart'));

        chart.draw(data, options);
    }


    google.charts.load('current', { packages: ['corechart', 'bar'] });
    google.charts.setOnLoadCallback(drawDualX);

    function drawDualX() {
        var data = google.visualization.arrayToDataTable([
            ['City', '2010 Population', '2000 Population'],
            ['New York City, NY', 8175000, 8008000],
            ['Los Angeles, CA', 3792000, 3694000],
            ['Chicago, IL', 2695000, 2896000],
            ['Houston, TX', 2099000, 1953000],
            ['Philadelphia, PA', 1526000, 1517000]
        ]);

        var materialOptions = {
            chart: {
                title: 'Population of Largest U.S. Cities',
                subtitle: 'Based on most recent and previous census data'
            },
            hAxis: {
                title: 'Total Population'
            },
            vAxis: {
                title: 'City'
            },
            bars: 'horizontal',
            series: {
                0: { axis: '2010' },
                1: { axis: '2000' }
            },
            axes: {
                x: {
                    2010: { label: '2010 Population (in millions)', side: 'top' },
                    2000: { label: '2000 Population' }
                }
            }
        };
        var materialChart = new google.charts.Bar(document.getElementById('chart_div'));
        materialChart.draw(data, materialOptions);
    }

});
