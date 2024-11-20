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
document.getElementById("generar-voucher").addEventListener("click", function () {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({
        unit: "mm",
        format: [80, 100], // Tamaño ajustado para impresora térmica
        orientation: "portrait"
    });

    // Definir márgenes y posiciones iniciales
    const marginLeft = 10;
    const contentWidth = 60; // Ancho del contenido dentro del margen
    let currentY = 10; // Posición vertical inicial

    // Obtener la fecha actual
    const fechaActual = new Date().toLocaleDateString("es-CR");

    // Obtener el nombre del cliente del select
    const clienteNombre = document.getElementById("cliente-select").selectedOptions[0].text;

    // Obtener el texto de la deuda
    const textoDeuda = document.getElementById("texto-deuda").innerText;
    const deudaTotal = document.getElementById("deuda-total").innerText;

    // Configurar fuente
    doc.setFont("Helvetica", "normal");
    doc.setFontSize(10);

    // Agregar el versículo al PDF
    doc.text("El dinero mal habido pronto se acaba; quien ahorra, poco a poco se enriquece - Proverbios 13:11", marginLeft, currentY, { maxWidth: contentWidth });

    // Línea de separación
    currentY += 10;
    doc.line(marginLeft, currentY, 70, currentY);
    currentY += 5;

    // Agregar texto principal
    doc.setFont("Helvetica", "bold");
    doc.text(`Hoy ${fechaActual} la persona ${clienteNombre}`, marginLeft, currentY, { maxWidth: contentWidth });
    currentY += 5;
    doc.text("tiene una deuda", marginLeft, currentY, { maxWidth: contentWidth });
    currentY += 10;

    // Texto de deuda
    doc.setFontSize(14);
    doc.setTextColor(255, 0, 0); // Rojo para texto de deuda
    doc.text(`${textoDeuda} ${deudaTotal}`, marginLeft, currentY, { maxWidth: contentWidth });

    // Generar el Blob del PDF
    const pdfBlob = doc.output("blob");

    // Crear la URL del Blob
    const pdfUrl = URL.createObjectURL(pdfBlob);

    // Mostrar el PDF en el iframe del modal
    const pdfViewer = document.getElementById("pdfViewer");
    pdfViewer.src = pdfUrl;

    // Mostrar el modal
    const modal = document.getElementById("pdfModal");
    modal.style.display = "block";

    // Cerrar el modal al hacer clic en el botón de cerrar
    document.querySelector(".close-btn").onclick = function () {
        modal.style.display = "none";
        pdfViewer.src = ""; // Limpiar el iframe
        URL.revokeObjectURL(pdfUrl); // Liberar la URL del Blob
    };

    // Cerrar el modal al hacer clic fuera de él
    window.onclick = function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
            pdfViewer.src = ""; // Limpiar el iframe
            URL.revokeObjectURL(pdfUrl); // Liberar la URL del Blob
        }
    };
});

$(document).ready(function () {

    cargarClientes();

    const tableVuelto = $('#table-vuelto').DataTable({
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
    // Evento para el botón Buscar Vueltos
    $("#buscar-vueltos").on("click", function (event) {
        event.preventDefault();

        const idCliente = $("#cliente-select").val();
        if (!idCliente || idCliente === "0") {
            alert("Debe seleccionar un cliente.");
            return;
        }

        // Llamada al método GET
        $.ajax({
            url: `/Vueltos/vuelto/${idCliente}`,
            type: "GET",
            success: function (data) {
                // Validar si los datos están presentes
                const { vueltoAbono, vueltoCargo, totalVueltoGeneral } = data;

                // Llenar datos en tabla de abonos
                tableAbono.clear();
                if (vueltoAbono?.vuelto?.length > 0) {
                    vueltoAbono.vuelto.forEach(abono => {
                        tableAbono.row.add([
                            abono.descripcion || "Sin descripción",
                            abono.valor.toFixed(2),
                            abono.fecha
                        ]);
                    });
                }
                tableAbono.draw();

                // Llenar datos en tabla de vueltos
                tableVuelto.clear();
                if (vueltoCargo?.vuelto?.length > 0) {
                    vueltoCargo.vuelto.forEach(vuelto => {
                        tableVuelto.row.add([
                            vuelto.descripcion || "Sin descripción",
                            vuelto.valor.toFixed(2),
                            vuelto.fecha
                        ]);
                    });
                }
                tableVuelto.draw();

                // Llenar totales
                $("#total-abonos").text(vueltoAbono?.totalVuelto?.toFixed(2) || "0.00");
                $("#total-vueltos").text(vueltoCargo?.totalVuelto?.toFixed(2) || "0.00");
                $("#generar-voucher").show();
                // Llenar deuda total y cambiar colores según sea positivo o negativo
                const deudaTotal = totalVueltoGeneral || 0;
                $("#deuda-total").text(Math.abs(deudaTotal).toFixed(2));

                if (deudaTotal >= 0) {
                    $("#texto-deuda").text("DEBE").css("color", "green");
                    $("#deuda-total").css("color", "green");
                } else {
                    $("#texto-deuda").text("SE LE DEBE").css("color", "red");
                    $("#deuda-total").css("color", "red");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener datos de vueltos:", xhr.responseText);
                alert("Hubo un problema al obtener los datos de vueltos.");
            }
        });
    });
    $("#agregarVueltoBtn").on("click", function () {
        // Obtener los valores de los campos del formulario
        const descripcion = $("#descripcion").val().trim();
        const idCliente = parseInt($("#cliente-select").val(), 10);
        const tipoVuelto = $("#vale-dado").is(":checked") ? 1 : 0;
        const monto = parseFloat($("#monto").val());

        // Validar que los campos requeridos estén completos
        if (!descripcion) {
            alert("La descripción no puede estar vacía.");
            return;
        }

        if (!idCliente || idCliente === 0) {
            alert("Debe seleccionar un cliente válido.");
            return;
        }

        if (isNaN(monto) || monto <= 0) {
            alert("Debe ingresar un monto válido mayor a 0.");
            return;
        }

        // Construir la entidad Vuelto
        const vuelto = {
            Comentario: descripcion,
            IdCliente: idCliente,
            Estado: 1,
            TipoVuelto: tipoVuelto,
            Monto: monto
        };

        // Enviar la entidad al servidor mediante AJAX
        $.ajax({
            url: "/Vueltos/AgregarVuelto",
            type: "POST",
            data: { vuelto },
            xhrFields: {
                responseType: "blob" // Indica que se espera un archivo binario en la respuesta
            },
            success: function (blob) {
                // Crear un enlace para descargar el archivo PDF
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement("a");
                link.href = url;

                // Generar el nombre del archivo
                const fechaActual = new Date();
                const fechaFormateada = fechaActual.toISOString().slice(0, 10).replace(/-/g, "");
                const fileName = `Vuelto_${idCliente}_${fechaFormateada}.pdf`;

                link.download = fileName;
                link.click();

                // Limpiar los campos del formulario
                $("#descripcion").val("");
                $("#cliente-select").val("0");
                $("#vale-dado").prop("checked", false);
                $("#monto").val("");

                alert("Vuelto agregado y archivo descargado correctamente.");
            },
            error: function (xhr, status, error) {
                console.error("Error al agregar el vuelto:", xhr.responseText);
                alert("Hubo un problema al agregar el vuelto. Intente nuevamente.");
            }
        });
    });
});