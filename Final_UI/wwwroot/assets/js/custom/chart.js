$(function () {
  'use strict';

  async function fetchChartData(endpoint) {
    try {
      const response = await fetch(endpoint);
      const responseData = await response.json();
      if (responseData.statusCode === 200) {
        const data = responseData.data;
        const labels = Object.keys(data).map(label => label.replace(/^For/, '').trim());
        const values = Object.values(data);
        return { labels, values };
      } else {
        console.error('API response error:', responseData.message);
        return { labels: [], values: [] }; // Return empty data on error
      }
    } catch (error) {
      console.error('Error fetching chart data:', error);
      return { labels: [], values: [] }; // Return empty data on error
    }
  }

  async function initializeCharts() {
    const apiUrl = $('#api-url').val();

    // Fetch chart data
    const [lineChartData, barChartData, doughnutChartData, pieChartData1, pieChartData2] = await Promise.all([
      fetchChartData(`${apiUrl}/trait/Charts/line`),
      fetchChartData(`${apiUrl}/trait/Charts/bar`),
      fetchChartData(`${apiUrl}/trait/Charts/doughnut`),
      fetchChartData(`${apiUrl}/trait/Charts/pie1`),
      fetchChartData(`${apiUrl}/trait/Charts/pie2`)
    ]);


    // Define chart configurations
    const lineData = {
      labels: lineChartData.labels,
      datasets: [{
        label: '# of Orders',
        data: lineChartData.values,
        backgroundColor: [
          'rgba(255, 99, 132, 0.2)',
          'rgba(54, 162, 235, 0.2)',
          'rgba(255, 206, 86, 0.2)',
          'rgba(75, 192, 192, 0.2)',
          'rgba(153, 102, 255, 0.2)',
          'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
          'rgba(255,99,132,1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)',
          'rgba(75, 192, 192, 1)',
          'rgba(153, 102, 255, 1)',
          'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
      }]
    };

    const lineOptions = {
      scales: {
        yAxes: [{
          ticks: {
            beginAtZero: true
          },
          gridLines: {
            color: "rgba(204, 204, 204,0.1)"
          }
        }],
        xAxes: [{
          gridLines: {
            color: "rgba(204, 204, 204,0.1)"
          }
        }]
      },
      legend: {
        display: false
      },
      elements: {
        point: {
          radius: 2
        }
      }
    };

    const barData = {
      labels: barChartData.labels,
      datasets: [{
        label: '# of Users',
        data: barChartData.values,
        backgroundColor: [
          'rgba(255, 99, 132, 0.2)',
          'rgba(54, 162, 235, 0.2)',
          'rgba(255, 206, 86, 0.2)',
          'rgba(75, 192, 192, 0.2)',
          'rgba(153, 102, 255, 0.2)',
          'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
          'rgba(255,99,132,1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)',
          'rgba(75, 192, 192, 1)',
          'rgba(153, 102, 255, 1)',
          'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
      }]
    };

    const barOptions = {
      scales: {
        yAxes: [{
          ticks: {
            beginAtZero: true
          },
          gridLines: {
            color: "rgba(204, 204, 204,0.1)"
          }
        }],
        xAxes: [{
          gridLines: {
            color: "rgba(204, 204, 204,0.1)"
          }
        }]
      },
      legend: {
        display: false
      },
      elements: {
        point: {
          radius: 2
        }
      }
    };

    const doughnutData = {
      labels: doughnutChartData.labels,
      datasets: [{
        data: doughnutChartData.values,
        backgroundColor: [
          'rgba(75, 192, 192, 0.5)',
          'rgba(153, 102, 255, 0.5)',
          'rgba(255, 159, 64, 0.5)'
        ],
        borderColor: [
          'rgba(75, 192, 192, 1)',
          'rgba(153, 102, 255, 1)',
          'rgba(255, 159, 64, 1)'
        ]
      }]
    };

    const doughnutOptions = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top'
        },
        tooltip: {
          callbacks: {
            label: function (context) {
              let label = context.label || '';
              if (label) {
                label += ': ';
              }
              if (context.raw !== null) {
                label += context.raw;
              }
              return label;
            }
          }
        }
      }
    };

    const pieData1 = {
      labels: pieChartData1.labels,
      datasets: [{
        data: pieChartData1.values,
        backgroundColor: [
          'rgba(255, 99, 132, 0.5)',
          'rgba(54, 162, 235, 0.5)',
          'rgba(255, 206, 86, 0.5)'
        ],
        borderColor: [
          'rgba(255,99,132,1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)'
        ]
      }]
    };

    const pieOptions1 = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top'
        },
        tooltip: {
          callbacks: {
            label: function (context) {
              let label = context.label || '';
              if (label) {
                label += ': ';
              }
              if (context.raw !== null) {
                label += context.raw;
              }
              return label;
            }
          }
        }
      }
    };

    const pieData2 = {
      labels: pieChartData2.labels,
      datasets: [{
        data: pieChartData2.values,
        backgroundColor: [
          'rgba(255, 99, 132, 0.5)',
          'rgba(54, 162, 235, 0.5)',
          'rgba(255, 206, 86, 0.5)'
        ],
        borderColor: [
          'rgba(255,99,132,1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)'
        ]
      }]
    };

    const pieOptions2 = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top'
        },
        tooltip: {
          callbacks: {
            label: function (context) {
              let label = context.label || '';
              if (label) {
                label += ': ';
              }
              if (context.raw !== null) {
                label += context.raw;
              }
              return label;
            }
          }
        }
      }
    };

    // Initialize charts
    if ($("#barChart").length) {
      const barChartCanvas = $("#barChart").get(0).getContext("2d");
      new Chart(barChartCanvas, {
        type: 'bar',
        data: barData,
        options: barOptions
      });
    }

    if ($("#lineChart").length) {
      const lineChartCanvas = $("#lineChart").get(0).getContext("2d");
      new Chart(lineChartCanvas, {
        type: 'line',
        data: lineData,
        options: lineOptions
      });
    }

    if ($("#doughnutChart").length) {
      const doughnutChartCanvas = $("#doughnutChart").get(0).getContext("2d");
      new Chart(doughnutChartCanvas, {
        type: 'doughnut',
        data: doughnutData,
        options: doughnutOptions
      });
    }

    if ($("#pieChart1").length) {
      const doughnutChartCanvas = $("#pieChart1").get(0).getContext("2d");
      new Chart(doughnutChartCanvas, {
        type: 'pie',
        data: pieData1,
        options: pieOptions1
      });
    }

    if ($("#pieChart2").length) {
      const doughnutChartCanvas = $("#pieChart2").get(0).getContext("2d");
      new Chart(doughnutChartCanvas, {
        type: 'pie',
        data: pieData2,
        options: pieOptions2
      });
    }
  }

  // Call initializeCharts to fetch data and render charts
  initializeCharts();
});
