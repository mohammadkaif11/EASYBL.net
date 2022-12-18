const labels = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'may',
    'may',
    'may',
    'may',
    'may',
    'may',

];

const data = {
    labels: labels,
    datasets: [{
        label: 'Month Review',
        backgroundColor: '#f8f9fa',
        borderColor: 'black',
        data: [0, 10, 5, 2, 20, 30, 45, 3, 35, 34, 12, 31, 31, 10, 31, 40]
    }]
};

const config = {
    type: 'line',
    data: data,
    options: {}
};
const myChart = new Chart(
    document.getElementById('myChart'),
    config
);