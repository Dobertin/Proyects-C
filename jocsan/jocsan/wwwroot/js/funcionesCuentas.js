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

document.getElementById("descargar-creditos").addEventListener("click", function () {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    const tableCredito = $('#table-credito').DataTable();
    const rows = tableCredito.rows({ search: 'applied' }).data(); // Obtener las filas visibles de la tabla
    const header = ["Descripción", "Monto", "Fecha"];
    const tableData = [];
    let totalMonto = 0;

    // Recorrer las filas de la tabla y preparar los datos para el PDF
    rows.each(function (rowData) {
        const descripcion = rowData[0]; // Columna Descripción
        const monto = parseFloat(rowData[1]) || 0; // Columna Monto
        const fecha = rowData[2]; // Columna Fecha

        totalMonto += monto; // Sumar el monto
        tableData.push([descripcion, monto.toFixed(2), fecha]);
    });

    // Agregar encabezado
    doc.setFontSize(14);
    doc.text("Reporte de Créditos", 105, 20, null, null, "center");

    // Agregar tabla
    doc.autoTable({
        head: [header],
        body: tableData,
        startY: 30
    });

    // Agregar la suma total al final
    doc.setFontSize(12);
    doc.text(`Total Monto: ${totalMonto.toFixed(2)} CRC`, 14, doc.lastAutoTable.finalY + 10);

    // Descargar el archivo PDF
    doc.save("Reporte_Creditos.pdf");
});
document.getElementById("descargar-abonos").addEventListener("click", function () {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    const tableCredito = $('#table-abono').DataTable();
    const rows = tableCredito.rows({ search: 'applied' }).data(); // Obtener las filas visibles de la tabla
    const header = ["Descripción", "Monto", "Fecha"];
    const tableData = [];
    let totalMonto = 0;

    // Recorrer las filas de la tabla y preparar los datos para el PDF
    rows.each(function (rowData) {
        const descripcion = rowData[0]; // Columna Descripción
        const monto = parseFloat(rowData[1]) || 0; // Columna Monto
        const fecha = rowData[2]; // Columna Fecha

        totalMonto += monto; // Sumar el monto
        tableData.push([descripcion, monto.toFixed(2), fecha]);
    });

    // Agregar encabezado
    doc.setFontSize(14);
    doc.text("Reporte de Abonos", 105, 20, null, null, "center");

    // Agregar tabla
    doc.autoTable({
        head: [header],
        body: tableData,
        startY: 30
    });

    // Agregar la suma total al final
    doc.setFontSize(12);
    doc.text(`Total Monto: ${totalMonto.toFixed(2)} CRC`, 14, doc.lastAutoTable.finalY + 10);

    // Descargar el archivo PDF
    doc.save("Reporte_Abonos.pdf");
});

document.getElementById("descargar-cuentas").addEventListener("click", function () {
    // Verificar que jsPDF esté disponible
    if (!window.jspdf) {
        console.error("jsPDF no está cargado.");
        alert("Error: jsPDF no está disponible. Verifica la inclusión de la biblioteca.");
        return;
    }

    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Obtener los valores de los elementos
    const fecha = new Date().toLocaleDateString();
    const cliente = document.getElementById("cliente-select").selectedOptions[0].text || "";
    const montoCredito = document.getElementById("total-creditos").innerText || "";
    const montoAbonos = document.getElementById("total-abonos").innerText || "";
    const fechaAnterior = document.getElementById("fecha-ultima-cuenta").innerText || "";
    const montoCuentaAnterior = document.getElementById("fecha-ultima-monto").innerText || "";
    const textoDeuda = document.getElementById("texto-deuda").innerText || "";
    const montoDeudaActual = document.getElementById("deuda-total").innerText || "";

    // Configuración del documento
    doc.setFontSize(10);
    doc.text("Reporte de Cuentas", 105, 10, null, null, "center");

    // Estructura de la tabla
    const tableData = [
        [{ content: "FECHA", styles: { halign: 'center', fillColor: [220, 220, 220] } }, fecha],
        [{ content: "CLIENTE", styles: { halign: 'center', fillColor: [220, 220, 220] } }, cliente],
        [{ content: "MONTO DE CREDITO", styles: { halign: 'center', fillColor: [220, 220, 220] } }, montoCredito],
        [{ content: "MONTO DE ABONOS", styles: { halign: 'center', fillColor: [220, 220, 220] } }, montoAbonos],
        [{ content: "FECHA CUENTA ANTERIOR A ESTA", styles: { halign: 'center', fillColor: [220, 220, 220] } }, fechaAnterior],
        [{ content: "MONTO DE CUENTA ANTERIOR", styles: { halign: 'center', fillColor: [220, 220, 220] } }, montoCuentaAnterior],
        [{ content: "MONTO ACTUAL DE DEUDA", styles: { halign: 'center', fillColor: [220, 220, 220] } }, `${textoDeuda} ${montoDeudaActual}`]
    ];

    // Generar la tabla con los datos
    doc.autoTable({
        body: tableData,
        startY: 20,
        theme: 'grid',
        styles: {
            fontSize: 10,
            halign: 'center',
            cellPadding: 2,
        },
        columnStyles: {
            0: { cellWidth: 70 },
            1: { cellWidth: 110 }
        },
    });

    // Descargar el PDF
    doc.save("Reporte_Cuentas.pdf");
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

                // Mostrar botones si hay datos
                if (data.creditoTotal.creditos.length > 0) {
                    $("#descargar-creditos").show();
                    $("#descargar-cuentas").show();
                }
                if (data.abonoTotal.abonos.length > 0) {
                    $("#descargar-abonos").show();
                    $("#descargar-cuentas").show();
                }

                // Función para manejar los botones de "ver" en las tablas
                $(".editar-btn").on("click", function () {
                    const id = $(this).data("id");
                    const origen = $(this).data("origen");

                    $.ajax({
                        url: '/Cuentas/VerDocumento',
                        type: 'POST',
                        data: { id: id, tipodoc: origen },
                        xhrFields: {
                            responseType: 'blob'
                        },
                        success: function (blob) {
                            const pdfUrl = URL.createObjectURL(blob);
                            $('#pdfViewer').attr('src', pdfUrl);
                            $('#pdfModal').show();

                            $('.close-btn').on('click', function () {
                                $('#pdfModal').hide();
                                URL.revokeObjectURL(pdfUrl);
                            });
                        },
                        error: function (xhr, status, error) {
                            console.error("Error al obtener el PDF:", xhr.responseText);
                            alert("Hubo un problema al obtener el documento.");
                        }
                    });
                });

                window.onclick = function (event) {
                    const modal = document.getElementById('pdfModal');
                    if (event.target == modal) {
                        modal.style.display = 'none';
                        $('#pdfViewer').attr('src', '');
                    }
                };
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener datos:", xhr.responseText);
                alert("Hubo un problema al obtener los datos de la cuenta.");
            }
        });
    });

    // Evento para descargar créditos
    $("#descargar-creditos").on("click", function () {
        const idCliente = $("#cliente-select").val();
        //$.ajax({
        //    url: `/Cuentas/DescargarCreditos/${idCliente}`,
        //    type: "GET",
        //    xhrFields: {
        //        responseType: 'blob'
        //    },
        //    success: function (blob) {
        //        const fileName = `Creditos_${idCliente}.pdf`;
        //        const url = window.URL.createObjectURL(blob);
        //        const a = document.createElement('a');
        //        a.href = url;
        //        a.download = fileName;
        //        document.body.appendChild(a);
        //        a.click();
        //        a.remove();
        //        window.URL.revokeObjectURL(url);
        //    },
        //    error: function (xhr, status, error) {
        //        console.error("Error al descargar créditos:", xhr.responseText);
        //        alert("Hubo un problema al descargar los créditos.");
        //    }
        //});
    });

    // Evento para descargar abonos
    $("#descargar-abonos").on("click", function () {
        const idCliente = $("#cliente-select").val();
        //$.ajax({
        //    url: `/Cuentas/DescargarAbonos/${idCliente}`,
        //    type: "GET",
        //    xhrFields: {
        //        responseType: 'blob'
        //    },
        //    success: function (blob) {
        //        const fileName = `Abonos_${idCliente}.pdf`;
        //        const url = window.URL.createObjectURL(blob);
        //        const a = document.createElement('a');
        //        a.href = url;
        //        a.download = fileName;
        //        document.body.appendChild(a);
        //        a.click();
        //        a.remove();
        //        window.URL.revokeObjectURL(url);
        //    },
        //    error: function (xhr, status, error) {
        //        console.error("Error al descargar abonos:", xhr.responseText);
        //        alert("Hubo un problema al descargar los abonos.");
        //    }
        //});
    });
});

