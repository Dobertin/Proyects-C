$(document).ready(function () {
    let idClienteEnMemoria = null;
    let clienteOriginal = {}; // Variable para almacenar los datos originales del cliente

    // Inicializar DataTable con paginación de 20 registros y opción de búsqueda
    const table = $('#clientesTable').DataTable({
        paging: true,
        pageLength: 10,
        searching: true,
        responsive: true,
        language: {
            emptyTable: "No hay datos disponibles",
            search: "Buscar:",
            lengthMenu: "Mostrar _MENU_ registros por página",
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            paginate: {
                first: "Primero",
                last: "Último",
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });

    // Función para obtener y llenar la tabla con datos de clientes
    function cargarClientes() {
        $.ajax({
            url: '/Clientes/clientes/listar',
            type: 'GET',
            success: function (data) {
                table.clear();
                data.forEach(cliente => {
                    table.row.add([
                        cliente.nombre,
                        cliente.capitan,
                        cliente.porcentaje,
                        cliente.gasolina,
                        `<button class="editar-btn" data-id="${cliente.idCliente}">Editar</button>
                         <button class="eliminar-btn" data-id="${cliente.idCliente}">Eliminar</button>`
                    ]);
                });
                table.draw();
            },
            error: function (xhr) {
                console.error("Error al obtener los clientes:", xhr.responseText);
                alert("Hubo un problema al cargar los datos de los clientes.");
            }
        });
    }

    cargarClientes();

    // Evento para el botón "Eliminar"
    $('#clientesTable').on('click', '.eliminar-btn', function () {
        const idCliente = $(this).data("id");
        if (confirm(`¿Estás seguro de que deseas eliminar el cliente con ID ${idCliente}?`)) {
            $.ajax({
                url: `/Clientes/eliminar/${idCliente}`,
                type: 'DELETE',
                success: function () {
                    Swal.fire({
                        title: "Éxito",
                        text: "Cliente eliminado correctamente.",
                        icon: "success",
                        confirmButtonText: "Aceptar"
                    }).then(() => {
                        cargarClientes();
                    });
                },
                error: function (xhr) {
                    Swal.fire({
                        title: "Error",
                        text: "Error al eliminar cliente: " + xhr.responseText,
                        icon: "error",
                        confirmButtonText: "Aceptar"
                    });
                }
            });
        }
    });

    // Mostrar el modal de edición y cargar datos del cliente al hacer clic en "Editar"
    $('#clientesTable').on('click', '.editar-btn', async function () {
        idClienteEnMemoria = $(this).data('id');
        try {
            const response = await fetch(`/Clientes/clientes/${idClienteEnMemoria}`);
            clienteOriginal = await response.json();

            // Mapear los datos obtenidos a los campos del formulario
            $("#cliente").val(clienteOriginal.nombre);
            $("#capitan").val(clienteOriginal.capitan);
            $("#porcentaje").val(Math.max(0, clienteOriginal.porcentaje));
            $("#gasolina").val(Math.max(0, clienteOriginal.gasolina));
            $("#nueva-embarcacion").prop('checked', clienteOriginal.nuevaEmbarcacion);

            $("#peon-primero").prop('checked', !!clienteOriginal.peon1);
            $("#peon1-nombre").val(clienteOriginal.peon1 || "");

            $("#peon-segundo").prop('checked', !!clienteOriginal.peon2);
            $("#peon2-nombre").val(clienteOriginal.peon2 || "");

            $("#clienteModal").show();
        } catch (error) {
            console.error("Error al obtener datos del cliente:", error);
            alert("Hubo un problema al cargar los datos del cliente.");
        }
    });

    // Función para cerrar el modal
    $(".close-btn, #cancelar").on("click", function () {
        $("#clienteModal").hide();
    });

    // Validaciones para que los campos "Porcentaje" y "Gasolina" sean enteros positivos
    function validarNumeroEnteroPositivo(input) {
        input.value = input.value.replace(/\D/g, "");
        if (input.value < 0) {
            input.value = 0;
        }
    }

    $("#porcentaje, #gasolina").on("input", function () {
        validarNumeroEnteroPositivo(this);
    });

    // Lógica para guardar los cambios
    $("#guardar").on("click", async function (event) {
        event.preventDefault();

        // Crear clienteActualizado a partir de los datos en clienteOriginal y los cambios en el formulario
        const clienteActualizado = {
            ...clienteOriginal, // Copia los datos originales
            Nombre: $("#cliente").val(),
            Capitan: $("#capitan").val(),
            Porcentaje: parseInt($("#porcentaje").val(), 10) || 0,
            Gasolina: parseInt($("#gasolina").val(), 10) || 0,
            NuevaEmbarcacion: $("#nueva-embarcacion").prop('checked'),
            Peon1: $("#peon-primero").is(':checked') ? $("#peon1-nombre").val() : null,
            Peon2: $("#peon-segundo").is(':checked') ? $("#peon2-nombre").val() : null
        };

        $.ajax({
            url: '/Cliente/Editar',
            type: 'POST',
            data: { cliente : clienteActualizado },
            success: function (response) {
                Swal.fire({
                    title: "Éxito",
                    text: response,
                    icon: "success",
                    confirmButtonText: "Aceptar"
                }).then(() => {
                    $("#clienteModal").hide();
                    cargarClientes();
                });
            },
            error: function (xhr) {
                const errorMessage = xhr.responseText || "Hubo un problema al actualizar el cliente.";
                Swal.fire({
                    title: "Error",
                    text: "Error al actualizar el cliente: " + errorMessage,
                    icon: "error",
                    confirmButtonText: "Aceptar"
                });
            }
        });
    });
});
