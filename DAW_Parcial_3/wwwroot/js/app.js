function resumenAreas() {
    fetch("Analisis/resumenAreas")
        .then((response) => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then((dataJson) => {
            const labels = dataJson.map((item) => item.nombre);
            const values = dataJson.map((item) => item.cantidad);

            console.log(labels);
            console.log(values);

            const data = {
                labels: labels,
                datasets: [{
                    label: 'Cantidad',
                    data: values,
                    backgroundColor: [
                        'rgb(255, 99, 132)',
                        'rgb(54, 162, 235)',
                        'rgb(255, 205, 86)'
                    ],
                    hoverOffset: 4
                }]
            };

            const config = {
                type: 'doughnut',
                data: data,
            };

            const canvasDona = document.getElementById("chartProductos");
            const graficoDona = new Chart(canvasDona, config);
        })
        .catch((error) => {
            console.log("error", error);
        });
}

$(document).ready(() => {
    resumenAreas();
});


function resumenTickets() {
    fetch("Analisis/resumenTickets")
        .then((response) => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then((dataJson) => {

            console.log(dataJson)


            const labels = dataJson.map((item) => item.prioridad);
            const values = dataJson.map((item) => item.valor);

            console.log(labels);
            console.log(values);



            const data = {
                labels: labels,
                datasets: [{
                    label: 'valor ',
                    data: values,
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderWidth: 1
                }]
            };

            const config = {
                type: 'bar',
                data: data,
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                },
            };
            const canvasBarra = document.getElementById("chartPrioridad");
            const graficoBarra = new Chart(canvasBarra, config);

        })
        .catch((error) => {
            console.log("error", error);
        });
}

$(document).ready(() => {
    resumenTickets();
});