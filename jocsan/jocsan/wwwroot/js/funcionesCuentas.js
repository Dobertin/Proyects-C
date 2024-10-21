document.addEventListener("DOMContentLoaded", function () {
    // Ejemplo: Función para manejar la búsqueda de cuentas
    const buscarBtn = document.querySelector(".buscar-btn");

    buscarBtn.addEventListener("click", function (event) {
        event.preventDefault();
        // Lógica para buscar cuentas basada en el cliente seleccionado
        alert("Buscar cuentas para el cliente seleccionado.");
    });

    // Puedes agregar más funcionalidades según sea necesario
});
