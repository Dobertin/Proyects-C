// Función para llenar el combo de clientes
async function cargarClientes() {
    try {
        const response = await fetch('/Factura/clientes/listar'); // Llama a la API
        const clientes = await response.json(); // Convierte la respuesta a JSON

        const clienteSelect = document.getElementById('cliente-values');

        // Agregar una opción vacía o con un texto predeterminado
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

// Función para poblar la tabla con datos de la API
function cargarFacturas() {
    // Obtener los valores de los elementos HTML
    const idCliente = document.getElementById('cliente-values').value;
    const idFactura = document.getElementById('nro-factura').value || 0;

    // Crear el objeto FacturaQuery con los valores obtenidos
    const facturaQuery = {
        IdCliente: parseInt(idCliente) || 0,
        IdFactura: parseInt(idFactura) || 0
    };

    $.ajax({
        url: '/Factura/ObtenerFacturas',
        type: 'POST',
        data: { facturaQuery },
        success: function (data) {
            // Limpiar la tabla antes de agregar nuevos datos
            const table = $('#table-factura').DataTable();
            table.clear();

            // Verificar si hay datos
            if (data.length === 0) {
                // Mostrar mensaje de "No hay datos disponibles"
                table.row.add(["No hay datos disponibles", "", "", "", ""]).draw(false);
            } else {
                // Recorrer los datos y agregarlos a la tabla
                data.forEach(function (factura) {
                    table.row.add([
                        factura.idFactura,
                        factura.numproductos,
                        factura.total.toLocaleString("es-CR", { style: "currency", currency: "CRC" }),
                        factura.fecha,
                        `<button class="visualizar-btn" data-id="${factura.idFactura}">Visualizar</button>
                         <button class="eliminar-btn" data-id="${factura.idFactura}">Eliminar</button>`
                    ]).draw(false);
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al obtener las facturas:", xhr.responseText);
            alert("Hubo un problema al cargar las facturas.");
        }
    });
}

// Evento para el botón "Buscar Factura"
document.querySelector('.buscar-btn').addEventListener('click', function (event) {
    event.preventDefault(); // Evita el envío del formulario si el botón está dentro de un formulario
    cargarFacturas();
});

// Función para manejar el evento de Visualizar
function visualizarFactura(event) {
    const idFactura = $(event.target).data('id');

    $.ajax({
        url: `/Factura/visualizar/${idFactura}`,
        type: 'GET',
        xhrFields: {
            responseType: 'blob' // Recibe la respuesta como un Blob
        },
        success: function (blob) {
            // Crear una URL para el Blob del PDF
            const pdfUrl = URL.createObjectURL(blob);

            // Llamar a la función para mostrar el PDF en un modal
            mostrarPdfEnModal(pdfUrl);
        },
        error: function (xhr, status, error) {
            console.error("Error al obtener la factura:", xhr.responseText);
            alert("Hubo un problema al cargar la factura.");
        }
    });
}
function mostrarPdfEnModal(pdfUrl) {
    // Asignar la URL del PDF al iframe para mostrar el contenido
    document.getElementById('pdfViewer').src = pdfUrl;

    // Mostrar el modal
    const modal = document.getElementById('pdfModal');
    modal.style.display = 'block';

    // Cerrar el modal al hacer clic en el botón de cerrar
    const closeBtn = document.querySelector('.close-btn');
    closeBtn.onclick = function () {
        modal.style.display = 'none';
        URL.revokeObjectURL(pdfUrl); // Liberar la URL del Blob
    };

    // Cerrar el modal al hacer clic fuera de él
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = 'none';
            URL.revokeObjectURL(pdfUrl); // Liberar la URL del Blob
        }
    };
}

// Función para manejar el evento de Eliminar
function eliminarFactura(event) {
    const idFactura = $(event.target).data('id');
    if (confirm(`¿Estás seguro de que deseas eliminar la factura N° ${idFactura}?`)) {
        $.ajax({
            url: `/Factura/eliminar/${idFactura}`,
            type: 'DELETE',
            success: function () {
                alert("Factura eliminada correctamente.");
                cargarFacturas(); // Recargar los datos de la tabla después de eliminar
            },
            error: function (xhr, status, error) {
                console.error("Error al eliminar la factura:", xhr.responseText);
                alert("Hubo un problema al eliminar la factura.");
            }
        });
    }
}


// Document Ready para inicializar DataTable y eventos
$(document).ready(function () {
    // Inicializar DataTable con paginado de 10 registros
    $('#table-factura').DataTable({
        paging: true,
        pageLength: 10,
        searching: false,
        info: false,
        language: {
            emptyTable: "No hay datos disponibles"
        }
    })

    // Delegar eventos de Visualizar y Eliminar en el DataTable
    $('#table-factura').on('click', '.visualizar-btn', visualizarFactura);
    $('#table-factura').on('click', '.eliminar-btn', eliminarFactura);
});

// Cargar clientes cuando el DOM esté completamente cargado
document.addEventListener("DOMContentLoaded", function () {
    cargarClientes();
});
