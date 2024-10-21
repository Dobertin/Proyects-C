document.addEventListener("DOMContentLoaded", function () {
    // Confirmar eliminación de un préstamo
    const deleteButtons = document.querySelectorAll(".eliminar-btn");

    deleteButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            const confirmed = confirm("¿Estás seguro de que deseas eliminar este préstamo?");
            if (!confirmed) {
                event.preventDefault();  // Cancela la eliminación si no se confirma
            }
        });
    });

    // Puedes agregar más funciones aquí para manejar eventos del botón Editar o Agregar
});
