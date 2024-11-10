document.addEventListener("DOMContentLoaded", function () {
    cargarClientes();

    $("#precio, #cantidad").on("input", function () {
        actualizarMonto();
    });

    $("#btn-agregar-cuenta").on("click", async function (event) {
        event.preventDefault();

        // Validar que un cliente esté seleccionado
        const idCliente = $("#cliente-values").val();
        if (!idCliente || idCliente === "0") {
            Swal.fire("Error", "Debe seleccionar un cliente antes de cargar gasolina.", "error");
            return;
        }

        // Obtener los valores de los campos
        const comentario = $("#comentario").val();
        const precioGalonCargado = parseFloat($("#precio").val()) || 0;
        const cantGalonCargado = parseInt($("#cantidad").val(), 10) || 0;
        const totalGalonCargado = parseFloat($("#monto").val()) || 0;

        // Construir la entidad Gasolina
        const gasolina = {
            Comentario: comentario,
            IdCliente: parseInt(idCliente),
            PrecioGalonPagado: 0,
            CantGalonPagado: 0,
            PrecioGalonCargado: precioGalonCargado,
            CantGalonCargado: cantGalonCargado,
            TotalGalonCargado: totalGalonCargado,
            TotalGalonPagado: 0,
            FechaOperacion: new Date().toISOString(),
            UsuarioCreacion: "System",
            FechaCreacion: new Date().toISOString()
        };

        // Enviar la entidad Gasolina al servidor mediante AJAX
        $.ajax({
            url: '/Gasolina/AgregarGasolinaCargado',
            type: 'POST',
            data: { gasolina: gasolina },
            xhrFields: {
                responseType: 'blob' // Esperar un Blob como respuesta para el archivo
            },
            success: function (blob) {
                // Crear una URL para el Blob y descargar el archivo
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = `Gasolina_${idCliente}_${new Date().toISOString().slice(0, 10).replace(/-/g, '')}.pdf`;
                document.body.appendChild(link);
                link.click();
                link.remove();
                window.URL.revokeObjectURL(url);

                // Mostrar mensaje de éxito
                Swal.fire("Éxito", "Gasolina cargada correctamente y documento generado.", "success");

                // Limpiar campos
                $("#comentario").val('');
                $("#precio").val('');
                $("#cantidad").val('');
                $("#monto").val('');
                $("#cliente-values").val('0'); // Resetear el cliente a la opción predeterminada
            },
            error: function (xhr, status, error) {
                const errorMessage = xhr.responseText || "Hubo un problema al cargar la gasolina.";
                Swal.fire("Error", errorMessage, "error");
            }
        });
    });

    // Inicializar DataTables para las tablas pagados y cargados
    const tablePagados = $('#table-pagados').DataTable({
        paging: true,
        pageLength: 10,
        searching: false,
        lengthChange: false,
        info: false,
        responsive: true,
        language: {
            emptyTable: "No hay datos disponibles",
            paginate: {
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });

    const tableCargados = $('#table-cargados').DataTable({
        paging: true,
        pageLength: 10,
        searching: false,
        lengthChange: false,
        info: false,
        responsive: true,
        language: {
            emptyTable: "No hay datos disponibles",
            paginate: {
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });

    // Evento para el botón de búsqueda
    $("#btn-buscar").on("click", function () {
        const idCliente = $("#cliente-values").val();

        if (!idCliente || idCliente === "0") {
            alert("Debe seleccionar un cliente.");
            return;
        }

        $.ajax({
            url: `/Gasolina/gasolina/${idCliente}`,
            type: "GET",
            success: function (data) {
                console.log(data);
                // Mapear datos en los campos correspondientes
                $("#valor-galones-pagados").val(data.totalgalonpagado);
                $("#cant-galones-pagados").val(data.cantidadgalonpagado);
                $("#valor-galones-cargados").val(data.totalgaloncargado);
                $("#cant-galones-cargados").val(data.cantidadgaloncargado);
                $("#total-galones-deuda").val(data.totalgalondeuda);
                $("#cant-galones-deuda").val(data.cantidadgalondeuda);

                // Cambiar color de texto basado en el valor de la deuda
                if (data.totalgalondeuda < 0) {
                    $("#total-galones-deuda").css("color", "red");
                } else {
                    $("#total-galones-deuda").css("color", "green");
                }

                if (data.cantidadgalondeuda < 0) {
                    $("#cant-galones-deuda").css("color", "red");
                } else {
                    $("#cant-galones-deuda").css("color", "green");
                }

                // Llenar la tabla pagados
                tablePagados.clear();
                data.pagados.forEach(pagado => {
                    tablePagados.row.add([
                        pagado.precio,
                        pagado.cantidad,
                        pagado.total,
                        pagado.fecha
                    ]).draw(false);
                });

                // Llenar la tabla cargados
                tableCargados.clear();
                data.cargados.forEach(cargado => {
                    tableCargados.row.add([
                        cargado.precio,
                        cargado.cantidad,
                        cargado.total,
                        cargado.fecha
                    ]).draw(false);
                });
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener datos:", xhr.responseText);
                alert("Hubo un problema al obtener los datos de gasolina.");
            }
        });
    });
});

async function cargarClientes() {
    try {
        const response = await fetch('/Gasolina/gasolina/listar');
        const clientes = await response.json();
        const clienteSelect = document.getElementById('cliente-values');

        // Agregar una opción predeterminada
        const defaultOption = new Option('Seleccione', '0', true, true);
        defaultOption.disabled = true;
        clienteSelect.add(defaultOption);

        clientes.forEach(cliente => {
            const option = new Option(cliente.descripcion, cliente.codigo);
            clienteSelect.add(option);
        });

    } catch (error) {
        console.error('Error cargando clientes:', error);
    }
}
function actualizarMonto() {
    const precio = parseFloat($("#precio").val()) || 0;
    const cantidad = parseFloat($("#cantidad").val()) || 0;
    const monto = precio * cantidad;

    $("#monto").val(monto.toFixed(2));
}
