$(document).ready(function () {
    // Función para evitar entrada de decimales en los campos Porcentaje y Gasolina
    function validarEntero(event) {
        if (event.which === 46 || event.key === '.') {
            event.preventDefault(); // Evitar el ingreso de punto decimal
        }
    }

    // Asignar la validación a los campos
    $('#porcentaje, #gasolina').on('keypress', validarEntero);

    // Evento al hacer clic en el botón "Añadir Cliente"
    $('#agregar-cliente').on('click', function (event) {
        event.preventDefault();

        // Obtener y validar los valores del formulario
        const nombre = $('#cliente').val();
        const capitan = $('#capitan').val();
        const porcentaje = parseInt($('#porcentaje').val()) / 100;
        const gasolina = parseInt($('#gasolina').val());
        const nuevaEmbarcacion = $('#nueva-embarcacion').is(':checked');
        const peon1Activo = $('#peon-primero').is(':checked');
        const peon2Activo = $('#peon-segundo').is(':checked');
        const peon1 = peon1Activo ? $('#peon1-nombre').val() : null;
        const peon2 = peon2Activo ? $('#peon2-nombre').val() : null;

        // Validaciones obligatorias
        if (!nombre) {
            alert("El nombre del cliente es obligatorio");
            return;
        }
        if (!capitan) {
            alert("El nombre del capitán es obligatorio");
            return;
        }
        //if (isNaN(porcentaje) || porcentaje <= 0) {
        //    alert("Porcentaje debe ser un número entero positivo");
        //    return;
        //}
        //if (isNaN(gasolina) || gasolina <= 0) {
        //    alert("Gasolina debe ser un número entero positivo");
        //    return;
        //}

        // Construir la entidad Cliente
        const cliente = {
            Nombre: nombre,
            Capitan: capitan,
            Porcentaje: porcentaje,
            Gasolina: gasolina,
            NuevaEmbarcacion: nuevaEmbarcacion,
            Peon1: peon1,
            Peon2: peon2
        };

        // Enviar la solicitud AJAX
        $.ajax({
            url: '/Cliente/AddCliente',
            type: 'POST',
            data: { cliente },
            success: function (response) {
                // Mostrar mensaje de éxito y limpiar campos
                Swal.fire({
                    icon: 'success',
                    title: 'Cliente agregado',
                    text: response,
                    timer: 1500,
                    showConfirmButton: false
                });

                // Limpiar los campos del formulario
                $('#cliente').val('');
                $('#capitan').val('');
                $('#porcentaje').val('');
                $('#gasolina').val('');
                $('#nueva-embarcacion').prop('checked', false);
                $('#peon-primero').prop('checked', false);
                $('#peon1-nombre').val('');
                $('#peon-segundo').prop('checked', false);
                $('#peon2-nombre').val('');
            },
            error: function (xhr) {
                // Mostrar mensaje de error
                Swal.fire({
                    icon: 'error',
                    title: 'Error al agregar cliente',
                    text: xhr.responseText || 'Hubo un problema al agregar el cliente',
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        });
    });
});
