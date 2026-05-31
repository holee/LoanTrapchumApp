const _charts = {};

window.BarChart = {
    create(id, labels, ds1Values, ds1Label, ds2Values, ds2Label) {
        const canvas = document.getElementById(id);
        if (!canvas) return;

        if (_charts[id]) {
            _charts[id].destroy();
            delete _charts[id];
        }

        _charts[id] = new Chart(canvas, {
            type: 'bar',
            data: {
                labels,
                datasets: [
                    {
                        label: ds1Label,
                        data: ds1Values,
                        backgroundColor: 'rgba(79, 125, 243, 0.82)',
                        borderColor: 'rgba(79, 125, 243, 1)',
                        borderWidth: 1.5,
                        borderRadius: 6,
                        borderSkipped: false,
                    },
                    {
                        label: ds2Label,
                        data: ds2Values,
                        backgroundColor: 'rgba(52, 201, 139, 0.82)',
                        borderColor: 'rgba(52, 201, 139, 1)',
                        borderWidth: 1.5,
                        borderRadius: 6,
                        borderSkipped: false,
                    },
                ],
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            pointStyle: 'circle',
                            padding: 20,
                            font: { size: 12, family: "'Inter','Roboto',sans-serif" },
                        },
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        callbacks: {
                            label: ctx => ` ${ctx.dataset.label}: ${ctx.parsed.y.toLocaleString()}`
                        }
                    },
                },
                scales: {
                    x: {
                        grid: { display: false },
                        ticks: { font: { size: 12 } },
                    },
                    y: {
                        grid: { color: 'rgba(0,0,0,0.05)' },
                        ticks: {
                            font: { size: 11 },
                            callback: v => v >= 1_000_000
                                ? (v / 1_000_000).toFixed(1) + 'M'
                                : v >= 1_000
                                    ? (v / 1_000).toFixed(0) + 'K'
                                    : v,
                        },
                    },
                },
            },
        });
    },

    destroy(id) {
        if (_charts[id]) {
            _charts[id].destroy();
            delete _charts[id];
        }
    },

    update(id, labels, ds1Values, ds1Label, ds2Values, ds2Label) {
        if (!_charts[id]) {
            window.BarChart.create(id, labels, ds1Values, ds1Label, ds2Values, ds2Label);
            return;
        }
        const chart = _charts[id];
        chart.data.labels = labels;
        chart.data.datasets[0].data = ds1Values;
        chart.data.datasets[0].label = ds1Label;
        chart.data.datasets[1].data = ds2Values;
        chart.data.datasets[1].label = ds2Label;
        chart.update();
    },
};
