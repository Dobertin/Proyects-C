// Función para cargar los clientes en el select
async function cargarClientes() {
    try {
        const response = await fetch('/Cuentas/clientes/listar');
        const clientes = await response.json();
        const clienteSelect = document.getElementById('cliente-select');

        const defaultOption = document.createElement('option');
        defaultOption.value = '0';
        defaultOption.text = 'Seleccione';
        defaultOption.disabled = true;
        defaultOption.selected = true;
        clienteSelect.add(defaultOption);

        clientes.forEach(cliente => {
            const option = document.createElement('option');
            option.value = cliente.codigo;
            option.text = cliente.descripcion;
            clienteSelect.add(option);
        });

    } catch (error) {
        console.error('Error cargando clientes:', error);
    }
}
$("#agregarCuentaBtn").on("click", function () {
    // Obtener los valores de los campos del formulario
    const descripcion = $("#descripcion").val();
    const idCliente = parseInt($("#cliente-select").val());
    const monto = parseFloat($("#monto").val());

    // Validar los campos obligatorios
    if (!descripcion) {
        alert("La descripción es obligatoria.");
        return;
    }
    if (!idCliente || isNaN(idCliente) || idCliente === 0) {
        alert("Debe seleccionar un cliente válido.");
        return;
    }
    if (!monto || isNaN(monto) || monto <= 0) {
        alert("El monto debe ser mayor a 0.");
        return;
    }

    // Crear la entidad Abono con los valores del formulario
    const abono = {
        Descripcion: descripcion,
        IdCliente: idCliente,
        FechaAbono: new Date().toISOString(),
        ValorAbono: monto,
        FechaCreacion: new Date().toISOString()
    };

    // Obtener la fecha actual en formato ddMMyyyyHHmm para el nombre del archivo
    const fechaActual = new Date();
    const fechaString = `${fechaActual.getDate().toString().padStart(2, '0')}${(fechaActual.getMonth() + 1).toString().padStart(2, '0')}${fechaActual.getFullYear()}${fechaActual.getHours().toString().padStart(2, '0')}${fechaActual.getMinutes().toString().padStart(2, '0')}`;
    const fileName = `${idCliente}_${fechaString}.pdf`;

    // Enviar la entidad Abono al servidor mediante AJAX
    $.ajax({
        url: '/Cuentas/AgregarAbono',
        type: 'POST',
        data: { abono },
        xhrFields: {
            responseType: 'blob' // Indica que esperamos un archivo blob en la respuesta
        },
        success: function (blob) {
            // Crear una URL para el blob y descargar el archivo
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);

            alert("Abono agregado y PDF descargado correctamente.");
        },
        error: function (xhr, status, error) {
            console.error("Error al agregar el abono:", xhr.responseText);
            alert("Hubo un problema al agregar el abono.");
        }
    });
});



// Document ready para cargar clientes y manejar el evento de búsqueda
$(document).ready(function () {
    cargarClientes();

    const tableCredito = $('#table-credito').DataTable({
        paging: true,
        pageLength: 10,
        lengthChange: false,
        searching: false,
        info: false,
        language: {
            emptyTable: "No hay datos disponibles"
        }
    });

    const tableAbono = $('#table-abono').DataTable({
        paging: true,
        pageLength: 10,
        lengthChange: false,
        searching: false,
        info: false,
        language: {
            emptyTable: "No hay datos disponibles"
        }
    });

    const tableFactura = $('#table-factura').DataTable({
        paging: true,
        pageLength: 10,
        lengthChange: false,
        searching: false,
        info: false,
        language: {
            emptyTable: "No hay datos disponibles"
        }
    });

    $("#buscar-cuentas").on("click", function (event) {
        event.preventDefault();

        const idCliente = $("#cliente-select").val();
        if (!idCliente || idCliente === "0") {
            alert("Debe seleccionar un cliente.");
            return;
        }

        $.ajax({
            url: `/Cuentas/cuentas/${idCliente}`,
            type: "GET",
            success: function (data) {
                // Mostrar totales de créditos, abonos y descripción de la cuenta
                $("#total-creditos").text(data.creditoTotal.totalValorCreditos.toFixed(2));
                $("#total-abonos").text(data.abonoTotal.totalValorAbono.toFixed(2));
                $("#total-factura").text(data.facturasTotal.totalValorFactura.toFixed(2));
                $("#descripcion-cuenta").text(data.ultimaCuenta.comentario);
                $("#fecha-ultima-monto").text(data.ultimaCuenta.monto.toFixed(2));
                $("#fecha-ultima-cuenta").text(data.ultimaCuenta.fechaCuenta);
                $("#fecha-ultima-cliente").text(data.ultimaCuenta.nomCliente);

                // Calcular la deuda total
                const deudaTotal = (
                    data.creditoTotal.totalValorCreditos -
                    data.abonoTotal.totalValorAbono -
                    data.facturasTotal.totalValorFactura
                ).toFixed(2);

                // Actualizar el valor y texto de la deuda total
                $("#deuda-total").text(Math.abs(deudaTotal).toFixed(2));

                if (deudaTotal < 0) {
                    $("#texto-deuda").text("SE Le DEBE");
                    $("#deuda-total").css("color", "red");
                } else {
                    $("#texto-deuda").text("DEBE");
                    $("#deuda-total").css("color", "green");
                }

                // Tabla de créditos
                tableCredito.clear();
                data.creditoTotal.creditos.forEach(credito => {
                    tableCredito.row.add([
                        credito.descripcion,
                        `${credito.valorCredito.toFixed(2)}`,
                        credito.fechaCredito,
                        `<button class="editar-btn btn btn-success" data-id="${credito.idCredito}" data-origen="credito"><i class="fa fa-search" aria-hidden="true"></i></button>`
                    ]);
                });
                tableCredito.draw();

                // Tabla de abonos
                tableAbono.clear();
                data.abonoTotal.abonos.forEach(abono => {
                    tableAbono.row.add([
                        abono.descripcion,
                        `${abono.valorAbono.toFixed(2)}`,
                        abono.fechaAbono,
                        `<button class="editar-btn btn btn-success" data-id="${abono.idAbono}" data-origen="abono"><i class="fa fa-search" aria-hidden="true"></i></button>`
                    ]);
                });
                tableAbono.draw();

                // Tabla de facturas
                tableFactura.clear();
                data.facturasTotal.facturas.forEach(factura => {
                    tableFactura.row.add([
                        factura.idFactura,
                        factura.numproductos,
                        `${factura.total.toFixed(2)}`,
                        factura.fecha,
                        `<button class="editar-btn btn btn-success" data-id="${factura.idFactura}" data-origen="factura"><i class="fa fa-search" aria-hidden="true"></i></button>`
                    ]);
                });
                tableFactura.draw();

                // Función para manejar los botones de "ver" en las tablas
                $(".editar-btn").on("click", function () {
                    const id = $(this).data("id");
                    const origen = $(this).data("origen");

                    // Enviar la solicitud AJAX para obtener el PDF
                    $.ajax({
                        url: '/Cuentas/VerDocumento',
                        type: 'POST',
                        data: { id: id, tipodoc: origen },
                        xhrFields: {
                            responseType: 'blob' // Indica que esperamos un blob como respuesta
                        },
                        success: function (blob) {
                            // Crear una URL para el blob y asignarla al iframe
                            const pdfUrl = URL.createObjectURL(blob);
                            $('#pdfViewer').attr('src', pdfUrl);

                            // Mostrar el modal
                            $('#pdfModal').show();

                            // Limpiar la URL del blob cuando se cierre el modal
                            $('.close-btn').on('click', function () {
                                $('#pdfModal').hide();
                                URL.revokeObjectURL(pdfUrl); // Liberar la URL del Blob
                            });
                        },
                        error: function (xhr, status, error) {
                            console.error("Error al obtener el PDF:", xhr.responseText);
                            alert("Hubo un problema al obtener el documento.");
                        }
                    });
                });

                // Cerrar el modal al hacer clic fuera de él
                window.onclick = function (event) {
                    const modal = document.getElementById('pdfModal');
                    if (event.target == modal) {
                        modal.style.display = 'none';
                        $('#pdfViewer').attr('src', ''); // Limpiar la fuente del iframe
                    }
                };

            },
            error: function (xhr, status, error) {
                console.error("Error al obtener datos:", xhr.responseText);
                alert("Hubo un problema al obtener los datos de la cuenta.");
            }
        });
    });

});
