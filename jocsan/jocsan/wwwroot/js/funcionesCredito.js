// Función para cargar los clientes en el selector
async function cargarClientes() {
    try {
        const response = await fetch('/Creditos/clientes/listar');
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

// Función para inicializar el DataTable
function inicializarDataTable() {
    //return $('#table-credito').DataTable({
    //    paging: true,
    //    pageLength: 10,
    //    searching: false,
    //    info: false,
    //    language: {
    //        emptyTable: "No hay datos disponibles"
    //    }
    //});
    return $('#table-credito').DataTable({
        paging: false,
        pageLength: false,
        searching: false,
        info: false,
        language: {
            emptyTable: "No hay datos disponibles"
        }
    });
}

// Función para buscar préstamos y poblar la tabla
function buscarPrestamos(table) {
    const idCliente = $('#cliente-values').val();

    if (idCliente === '0' || idCliente === null) {
        alert("Debe seleccionar un cliente antes.");
        return;
    }

    $.ajax({
        url: `/Creditos/listar/${idCliente}`,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            table.clear();

            if (data.length === 0) {
                table.row.add(["No hay datos disponibles", "", "", ""]).draw(false);
            } else {
                data.forEach(credito => {
                    table.row.add([
                        credito.descripcion,
                        `${credito.valorCredito.toLocaleString("es-CR", { style: "currency", currency: "CRC" })}`,
                        credito.fechaCredito,
                        `<button class="editar-btn btn btn-success" data-id="${credito.idCredito}"><i class="fa fa-search" aria-hidden="true"></i></button>
                         <button class="eliminar-btn btn btn-danger" data-id="${credito.idCredito}"><i class="fa fa-trash" aria-hidden="true"></i></button>`
                    ]).draw(false);
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al obtener los créditos:", xhr.responseText);
            alert("Hubo un problema al cargar los créditos.");
        }
    });
}

// Función para agregar un nuevo préstamo y descargar el PDF generado
function agregarPrestamo() {
    const descripcion = $("#descripcion").val();
    //const idCliente = parseInt($("#cliente-values").val());
    const valorCredito = parseFloat($("#monto").val());
    // Convertir idCliente a un número
    const idCliente = parseInt(document.getElementById('cliente-values').value, 10);

    // Validar si idCliente es 0, null, o NaN
    if (idCliente === 0 || idCliente == null || isNaN(idCliente)) {
        alert("Debe seleccionar un cliente antes de agregar el préstamo.");
        return;
    }
    // Validar que la descripción no esté vacía
    if (!descripcion) {
        alert("Debe ingresar una descripción para el préstamo.");
        return;
    }
    // Validar que el monto sea un número positivo
    if (isNaN(valorCredito) || valorCredito <= 0) {
        alert("Debe ingresar un monto válido y positivo para el préstamo.");
        return;
    }
    const credito = { Descripcion: descripcion, IdCliente: idCliente, ValorCredito: valorCredito };
    const fechaActual = new Date().toISOString().slice(0, 10).replace(/-/g, "");

    $.ajax({
        url: '/Credito/AgregarCreditos',
        type: 'POST',
        data: { credito },
        xhrFields: { responseType: 'blob' },
        success: function (blob) {
            const fileName = `Credito_${idCliente}_${fechaActual}.pdf`;
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);

            alert("Préstamo agregado y PDF descargado correctamente.");
            $("#descripcion").val("");
            $("#monto").val("");
            $("#cliente-values").val("0");
        },
        error: function (xhr, status, error) {
            console.error("Error al agregar el préstamo:", xhr.responseText);
            alert("Hubo un problema al agregar el préstamo.");
        }
    });
}

// Función para mostrar el PDF en un modal
function mostrarPdfEnModal(pdfUrl) {
    document.getElementById('pdfViewer').src = pdfUrl;
    const modal = document.getElementById('pdfModal');
    modal.style.display = 'block';

    document.querySelector('.close-btn').onclick = function () {
        modal.style.display = 'none';
        URL.revokeObjectURL(pdfUrl);
    };

    window.onclick = function (event) {
        if (event.target === modal) {
            modal.style.display = 'none';
            URL.revokeObjectURL(pdfUrl);
        }
    };
}

$(document).ready(function () {
    cargarClientes();
    const table = inicializarDataTable();

    $('.buscar-btn').on('click', function (event) {
        event.preventDefault();
        buscarPrestamos(table);
    });

    $("#agregarPrestamoBtn").click(agregarPrestamo);

    $('#table-credito').on('click', '.editar-btn', function () {
        const idCredito = $(this).data('id');
        $.ajax({
            url: `/Creditos/visualizar/${idCredito}`,
            type: 'GET',
            xhrFields: { responseType: 'blob' },
            success: function (blob) {
                const pdfUrl = URL.createObjectURL(blob);
                mostrarPdfEnModal(pdfUrl);
            },
            error: function (xhr, status, error) {
                console.error("Error al obtener el Credito:", xhr.responseText);
                alert("Hubo un problema al cargar el Credito.");
            }
        });
    });

    $('#table-credito').on('click', '.eliminar-btn', function () {
        const idCredito = $(this).data('id');
        if (confirm(`¿Estás seguro de que deseas eliminar el crédito N° ${idCredito}?`)) {
            $.ajax({
                url: `/Creditos/eliminar/${idCredito}`,
                type: 'DELETE',
                success: function () {
                    alert("Crédito eliminado correctamente.");
                    buscarPrestamos(table);
                },
                error: function (xhr, status, error) {
                    console.error("Error al eliminar el crédito:", xhr.responseText);
                    alert("Hubo un problema al eliminar el crédito.");
                }
            });
        }
    });
});
