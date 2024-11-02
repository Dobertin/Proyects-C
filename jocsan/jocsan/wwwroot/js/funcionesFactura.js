// Obtener Elementos
const agregarAListaBtn = document.getElementById('agregar-a-lista');
const checkboxGasolina = document.getElementById('regreso-gasolina');
const cantidadRegresoGasolina = document.getElementById('cantidad-regreso-gasolina');
const cantidadGasolinaInput = document.getElementById('cantidad-gasolina');
const cantidadHieloInput = document.getElementById('cantidad-hielo');
const gasHieloInput = document.getElementById('gas-hielo');
const precioGasolina = document.getElementById('precio-gasolina');
const precioHielo = document.getElementById('precio-hielo');
const cantidadProductoInput = document.getElementById('cantidad-producto');
const precioProducto = document.getElementById('precio-producto');
const subtotalProductoInput = document.getElementById('subtotal-producto');
const primerSubtotalInput = document.getElementById('primer-subtotal');
const tercerosInput = document.getElementById('terceros');
const peladoresInput = document.getElementById('peladores');
const segundoSubtotalInput = document.getElementById('segundo-subtotal');
const montoAbonoInput = document.getElementById('monto-abono');
const totalGeneralInput = document.getElementById('total-general');
const porcentajeClienteSelect = document.getElementById('porcentaje-cliente');
const pagoInput = document.getElementById('pago');
const vueltoInput = document.getElementById('vuelto');
const terceroClienteInput = document.getElementById('tercero-cliente');
const descuentoTotal25 = document.getElementById('decuento-total-25');
const emitirFacturaBtn = document.getElementById('btn-emitir');
const changePriceBtn = document.getElementById("changePrice");
const cancelButton = document.getElementById("btn-cancelar");
let primerSubtotalGeneral = 0;

document.addEventListener("DOMContentLoaded", function () {    
    // Llamar a las funciones cuando el DOM esté completamente cargado
    cargarClientes();
    cargarPorcentajes();
    cargarProductos();
    cargarHielo();
    obtenerUltimaFactura();
});
// Función para limpiar todos los controles
async function Limpiarcontroles() {
    tercerosInput.value = 0;
    peladoresInput.value = 0;
    subtotalProductoInput.value = "0.00";
    cantidadProductoInput.value = 0;
    gasHieloInput.value = "0.00";
    cantidadGasolinaInput.value = 0;
    cantidadHieloInput.value = 0;
    cantidadRegresoGasolina.value = "0.00";
    pagoInput.value = 0;
    // Actualizar el resto de los cálculos
    actualizarSubtotal();
}
// Cambiar el precio del producto temporalmente
changePriceBtn.addEventListener("click", function () {
    var newPrice = prompt("Ingrese el nuevo precio del producto:");
    if (newPrice) {
        document.getElementById("precio-producto").textContent = newPrice;
    }
});

// Agrega funcionalidad al botón de eliminar
cancelButton.addEventListener("click", function (event) {
    const confirmed = confirm("¿Estás seguro que deseas cancelar y regresar al inicio?");
    if (confirmed) {
        window.location.href = "/Home/Index"; // Cambia "/Home/Index" por la ruta a tu página de inicio
    } else {
        event.preventDefault(); // Cancela la redirección si no se confirma
    }
});
// Función para Emitir una Factura
emitirFacturaBtn.addEventListener('click', async function () {
    // Validar que haya al menos una fila en la tabla de productos
    const tableBody = document.getElementById('table_products').getElementsByTagName('tbody')[0];
    const rows = tableBody.getElementsByTagName('tr');

    if (rows.length === 0) {
        alert("Debe agregar al menos un producto antes de emitir la factura.");
        return;
    }

    // Crear la entidad Factura con sus datos
    const factura = {
        IdCliente: document.getElementById('cliente-values').value,
        FechaVenta: new Date().toISOString(),
        Porcentaje: parseFloat(document.getElementById('porcentaje-cliente').value) || 0,
        Galones: parseInt(cantidadGasolinaInput.value) || 0,
        Hielo: parseInt(cantidadHieloInput.value) || 0,
        SubTotalProd: primerSubtotalGeneral || 0,
        GH: parseFloat(gasHieloInput.value) || 0,
        SubTotalGH: parseFloat(primerSubtotalInput.value) || 0,
        Terceros: parseFloat(tercerosInput.value) || 0,
        Peladores: parseFloat(peladoresInput.value) || 0,
        SubTotal: parseFloat(segundoSubtotalInput.value) || 0,
        Valor25: parseFloat(descuentoTotal25.innerText) || 0,
        Valor13: parseFloat(terceroClienteInput.value) || 0,
        Abono: parseFloat(montoAbonoInput.value) || 0,
        TotalVenta: parseFloat(totalGeneralInput.value) || 0,
        DetalleFacturas: []
    };

    // Recorrer las filas de la tabla y crear DetalleFactura para cada una
    Array.from(rows).forEach(row => {
        const cells = row.getElementsByTagName('td');

        // Crear el objeto DetalleFactura con los valores de la fila
        const detalleFactura = {
            IdProducto: parseInt(cells[5].innerText) || 0, // Columna oculta de IdProducto
            PrecioUnitario: parseFloat(cells[1].innerText) || 0,
            Cantidad: parseInt(cells[2].innerText) || 0,
            SubTotalParcial: parseFloat(cells[3].innerText) || 0,
            TotalParcial: parseFloat(cells[3].innerText) || 0,
        };

        // Agregar el detalle a la lista de DetalleFacturas
        factura.DetalleFacturas.push(detalleFactura);
    });

    // Enviar la factura al controlador
    $.ajax({
        url: '/Factura/AgregarFactura',
        type: 'POST',
        data: { factura },
        xhrFields: {
            responseType: 'blob' // Permite manejar la respuesta como un Blob para descargar el PDF
        },
        success: function (blob) {
            // Generar la fecha actual en formato yyyyMMdd
            const fechaActual = new Date().toISOString().slice(0, 10).replace(/-/g, '');
            const idCliente = factura.IdCliente;
            const fileName = `${idCliente}_${fechaActual}.pdf`;

            // Crear un enlace de descarga para el PDF
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName; // Cambia el nombre del archivo si es necesario
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
            alert("Factura emitida y PDF descargado correctamente.");
            Limpiarcontroles();
        },
        error: function (xhr, status, error) {
            const errorMessage = xhr.responseText || "Hubo un problema al emitir la factura.";
            console.error("Error al enviar la factura:", errorMessage);
            alert("Error al emitir factura: " + errorMessage);
        }
    });

});
// Función para actualizar los cálculos
function actualizarCalculos() {
    // Obtener y validar los valores necesarios
    const cantidadProducto = parseFloat(cantidadProductoInput.value) || 0;
    const precioProductoValor = parseFloat(precioProducto.innerText) || 0;

    // Calcular el subtotal del producto
    const subtotalProducto = cantidadProducto * precioProductoValor ;
    subtotalProductoInput.value = subtotalProducto.toFixed(2);

    // Realizar las operaciones paso a paso
    let primerSubtotal = primerSubtotalGeneral+ subtotalProducto - parseFloat(gasHieloInput.value) || 0;
    primerSubtotalInput.value = primerSubtotal.toFixed(2);

    let segundoSubtotal = primerSubtotal - (parseFloat(tercerosInput.value) || 0);
    segundoSubtotal -= (parseFloat(peladoresInput.value) || 0);
    segundoSubtotalInput.value = segundoSubtotal.toFixed(2);

    // Calcular el 13% de segundoSubtotal y asignarlo a tercero-cliente
    const terceroCliente = segundoSubtotal * 0.13;
    terceroClienteInput.value = terceroCliente.toFixed(2);    

    // Colocar el resultado en el elemento descuento-total-25
    const descuento25 = segundoSubtotal * 0.25;
    descuentoTotal25.innerText = descuento25.toFixed(2);

    // Obtener el porcentaje seleccionado en el combo 
    const porcentajeSeleccionado = parseFloat(porcentajeClienteSelect.value) || 0;

    // Calcular monto abono como porcentaje del segundo subtotal
    const montoAbono = segundoSubtotal * porcentajeSeleccionado;
    montoAbonoInput.value = montoAbono.toFixed(2);

    // Calcular total general restando monto abono del segundo subtotal
    const totalGeneral = segundoSubtotal - montoAbono;
    totalGeneralInput.value = totalGeneral.toFixed(2);

    // Calcular el vuelto si el pago es mayor que el total general
    actualizarVuelto();
}
// Función para calcular el vuelto
function actualizarVuelto() {
    const totalGeneral = parseFloat(totalGeneralInput.value) || 0;
    const pago = parseFloat(pagoInput.value) || 0;

    // Verificar si el pago es mayor que el total general y calcular el vuelto
    if (pago > totalGeneral) {
        const vuelto = pago - totalGeneral;
        vueltoInput.value = vuelto.toFixed(2);
    } else {
        vueltoInput.value = "0.00";  // Si el pago no es mayor, el vuelto es 0
    }
}
// Escuchar cambios en el campo de Terceros
tercerosInput.addEventListener('input', function () {
    if (pagoInput.value < 0) {
        pagoInput.value = 0;  // Evitar valores negativos en el pago
    }
    actualizarCalculos();
});
// Escuchar cambios en el campo de Peladores
peladoresInput.addEventListener('input', function () {
    if (pagoInput.value < 0) {
        pagoInput.value = 0;  // Evitar valores negativos en el pago
    }
    actualizarCalculos();
});
// Escuchar cambios en el campo de pago para actualizar el vuelto
pagoInput.addEventListener('input', function () {
    if (pagoInput.value < 0) {
        pagoInput.value = 0;  // Evitar valores negativos en el pago
    }
    actualizarVuelto();
});
// Escuchar cambios en el campo cantidad-producto
cantidadProductoInput.addEventListener('input', function () {
    if (cantidadProductoInput.value < 0) {
        cantidadProductoInput.value = 0;
    }
    actualizarCalculos();
});
// Función para actualizar el valor de gas-hielo
function actualizarGasHielo() {
    let total = 0;

    // Obtener y validar los valores
    const cantidadGasolina = parseFloat(cantidadGasolinaInput.value) || 0;
    const precioGasolinaValor = parseFloat(precioGasolina.innerText) || 0;

    const cantidadHielo = parseFloat(cantidadHieloInput.value) || 0;
    const precioHieloValor = parseFloat(precioHielo.innerText) || 0;

    // Validar si la cantidad de gasolina es mayor a cero
    if (cantidadGasolina > 0) {
        const subtotalGasolina = cantidadGasolina * precioGasolinaValor;
        total += subtotalGasolina;
        // Guardar el subtotal en el campo cantidad-regreso-gasolina
        cantidadRegresoGasolina.value = subtotalGasolina.toFixed(2);
    } else {
        cantidadRegresoGasolina.value = "0.00"; // Restablecer si la cantidad es 0
    }

    // Validar si la cantidad de hielo es mayor a cero
    if (cantidadHielo > 0) {
        total += cantidadHielo * precioHieloValor;
    }
    // Actualizar el campo gas-hielo con el total calculado
    gasHieloInput.value = total.toFixed(2);
    actualizarCalculos();
}
// Escuchar cambios en el input de cantidad-gasolina
cantidadGasolinaInput.addEventListener('input', function () {
    // Validar que la cantidad de gasolina sea mayor a 0
    if (cantidadGasolinaInput.value >= 0) {
        actualizarGasHielo();
    }
});

// Escuchar cambios en el input de cantidad-hielo
cantidadHieloInput.addEventListener('input', function () {
    // Validar que la cantidad de hielo sea mayor a 0
    if (cantidadHieloInput.value >= 0) {
        actualizarGasHielo();
    }
});

// Función que se ejecuta cuando el checkbox cambia de estado
checkboxGasolina.addEventListener('change', function () {
    if (this.checked) {
        // Si el checkbox está activado
        cantidadRegresoGasolina.disabled = false; // Hacer editable el campo
    } else {
        // Si el checkbox está desactivado
        cantidadRegresoGasolina.disabled = true;  // Hacer no editable el campo
    }
});

// Función para llenar el combo de clientes
async function cargarClientes() {
    try {
        const response = await fetch('/Factura/clientes/listar'); // Llama a la API
        const clientes = await response.json(); // Convierte la respuesta a JSON

        const clienteSelect = document.getElementById('cliente-values');

        // Agregar una opción vacía o con un texto predeterminado
        const defaultOption = document.createElement('option');
        defaultOption.value = '';  // Valor vacío
        defaultOption.text = 'Seleccione';  // Texto visible
        defaultOption.disabled = true;  // Deshabilita la opción para que no pueda ser seleccionada
        defaultOption.selected = true;  // Marca la opción como seleccionada por defecto
        clienteSelect.add(defaultOption);

        clientes.forEach(cliente => {
            const option = document.createElement('option');
            option.value = cliente.codigo; // Establece el valor
            option.text = cliente.descripcion; // Establece el texto visible
            clienteSelect.add(option);
        });

        // Evento para capturar el cambio de cliente seleccionado
        clienteSelect.addEventListener('change', function () {
            const idClienteSeleccionado = clienteSelect.value; // Obtiene el idCliente seleccionado
            obtenerDatosCliente(idClienteSeleccionado); // Llama a la función para obtener los datos del cliente
        });
    } catch (error) {
        console.error('Error cargando clientes:', error);
    }
}

// Función para obtener los datos del cliente usando su id
async function obtenerDatosCliente(idcliente) {
    try {
        const response = await fetch(`/Factura/clientes/${idcliente}`); // Llama a la API con el idcliente
        const cliente = await response.json(); // Convierte la respuesta a JSON

        // Asigna los valores obtenidos a los elementos del HTML
        document.getElementById('duenio-cliente').value = cliente.nombre; // Asigna el nombre del cliente
        document.getElementById('precio-gasolina').innerText = cliente.gasolina; 
    } catch (error) {
        console.error('Error obteniendo datos del cliente:', error);
    }
}
async function obtenerUltimaFactura() {
    try {
        const response = await fetch(`/Factura/ultimonumero`); // Llama a la API con el idcliente
        const numero = await response.json(); // Convierte la respuesta a JSON

        // Asigna el número de la factura al elemento <h2> con id "numero-factura"
        document.getElementById('numero-factura').innerText = `Factura NRO ${numero}`;
    } catch (error) {
        console.error('Error obteniendo datos :', error);
    }
}
// Función para llenar el combo de porcentajes
async function cargarPorcentajes() {
    try {
        const response = await fetch('/Factura/Parametro/porcentajes'); // Llama a la API
        const porcentajes = await response.json();

        const porcentajeSelect = document.getElementById('porcentaje-cliente');

        // Agregar una opción vacía o con un texto predeterminado
        const defaultOption = document.createElement('option');
        defaultOption.value = '';  // Valor vacío
        defaultOption.text = 'Seleccione';  // Texto visible
        defaultOption.disabled = true;  // Deshabilita la opción para que no pueda ser seleccionada
        defaultOption.selected = true;  // Marca la opción como seleccionada por defecto
        porcentajeSelect.add(defaultOption);

        porcentajes.forEach(porcentaje => {
            const option = document.createElement('option');
            option.value = porcentaje.codigo; // Establece el valor
            option.text = porcentaje.descripcion; // Establece el texto visible
            porcentajeSelect.add(option);
        });

        // Evento para capturar el cambio de Producto seleccionado
        porcentajeSelect.addEventListener('change', function () {
            actualizarCalculos();
        });
    } catch (error) {
        console.error('Error cargando porcentajes:', error);
    }
}

// Función para llenar el combo de productos
async function cargarProductos() {
    try {
        const response = await fetch('/Factura/producto/listar'); // Llama a la API
        const productos = await response.json();

        const productoSelect = document.getElementById('producto-values');

        // Agregar una opción vacía o con un texto predeterminado
        const defaultOption = document.createElement('option');
        defaultOption.value = '';  // Valor vacío
        defaultOption.text = 'Seleccione';  // Texto visible
        defaultOption.disabled = true;  // Deshabilita la opción para que no pueda ser seleccionada
        defaultOption.selected = true;  // Marca la opción como seleccionada por defecto
        productoSelect.add(defaultOption);

        productos.forEach(producto => {
            const option = document.createElement('option');
            option.value = producto.codigo; // Establece el valor
            option.text = producto.descripcion; // Establece el texto visible
            productoSelect.add(option);
        });

        // Evento para capturar el cambio de Producto seleccionado
        productoSelect.addEventListener('change', function () {
            const idProductoSeleccionado = productoSelect.value; // Obtiene el idProducto seleccionado
            obtenerDatosProducto(idProductoSeleccionado); // Llama a la función para obtener los datos del Producto
        });
    } catch (error) {
        console.error('Error cargando productos:', error);
    }
}
// Función para obtener el precio del hielo
async function cargarHielo() {
    try {
        const response = await fetch('/Factura/Parametro/preciohielo'); // Llama a la API
        const productos = await response.json();

        productos.forEach(producto => {
            document.getElementById('precio-hielo').innerText = producto.codigo;
        });
    } catch (error) {
        console.error('Error cargando productos:', error);
    }
}
// Función para obtener los datos del cliente usando su id
async function obtenerDatosProducto(idproducto) {
    try {
        const response = await fetch(`/Factura/producto/${idproducto}`); // Llama a la API
        const producto = await response.json(); // Convierte la respuesta a JSON

        // Asigna los valores obtenidos a los elementos del HTML
        document.getElementById('nombre-producto').innerText = producto.nombreLocal;
        document.getElementById('precio-producto').innerText = producto.precio;
    } catch (error) {
        console.error('Error obteniendo datos del producto:', error);
    }
}

// Escuchar el evento de clic en el botón
agregarAListaBtn.addEventListener('click', function () {
    // Obtener los valores de los elementos de producto
    const nombreProducto = document.getElementById('nombre-producto').innerText;
    const precioProducto = parseFloat(document.getElementById('precio-producto').innerText) || 0;
    const cantidadProducto = parseFloat(document.getElementById('cantidad-producto').value) || 0;
    const subtotalProducto = parseFloat(document.getElementById('subtotal-producto').value) || 0;
    const productoSelect = document.getElementById('producto-values');
    const productoSeleccionado = productoSelect.options[productoSelect.selectedIndex].value;

    // Validar que los valores no estén vacíos
    if (nombreProducto && precioProducto > 0 && cantidadProducto > 0 && subtotalProducto > 0) {
        // Crear una nueva fila y celdas para la tabla
        const table = document.getElementById('table_products').getElementsByTagName('tbody')[0];
        const newRow = table.insertRow();

        const cellProducto = newRow.insertCell(0);
        const cellPrecio = newRow.insertCell(1);
        const cellCantidad = newRow.insertCell(2);
        const cellTotal = newRow.insertCell(3);
        const cellAcciones = newRow.insertCell(4);
        // Crear una celda oculta para almacenar el valor seleccionado del producto
        const cellHidden = newRow.insertCell(5);
        cellHidden.style.display = 'none'; // Ocultar la celda

        // Asignar los valores a las celdas
        cellProducto.innerText = nombreProducto;
        cellPrecio.innerText = precioProducto.toFixed(2);
        cellCantidad.innerText = cantidadProducto;
        cellTotal.innerText = subtotalProducto.toFixed(2);
        cellHidden.innerText = productoSeleccionado; // Almacenar el valor oculto del producto seleccionado

        // Crear el botón de eliminar y agregarlo a la celda de acciones
        const deleteButton = document.createElement('button');
        deleteButton.innerText = "❌";
        deleteButton.className = "btn-borrar-fila";
        cellAcciones.appendChild(deleteButton);

        subtotalProductoInput.value = "0.00";
        cantidadProductoInput.value = 0;
        //gasHieloInput.value = "0.00";
        //cantidadGasolinaInput.value = 0;
        //cantidadHieloInput.value = 0;
        //cantidadRegresoGasolina.value = "0.00";
        pagoInput.value = 0;
        // Actualizar el resto de los cálculos
        actualizarSubtotal();
        // Escuchar el evento de clic para eliminar la fila
        deleteButton.addEventListener('click', function () {
            newRow.remove(); // Eliminar la fila
            actualizarSubtotal();
        });

    } else {
        alert("Por favor, ingrese un producto con cantidad y precio válidos.");
    }
});

// Función para actualizar el primer subtotal
function actualizarSubtotal() {    

    // Obtener todas las filas de la tabla
    const rows = document.getElementById('table_products').getElementsByTagName('tbody')[0].rows;

    // Sumar los valores de cada subtotal en la tabla
    for (let i = 0; i < rows.length; i++) {
        const totalCell = rows[i].cells[3];
        primerSubtotalGeneral += parseFloat(totalCell.innerText) || 0;
    }
    const primerSubtotaltemp = primerSubtotalGeneral - parseFloat(gasHieloInput.value) || 0;
    // Asignar el valor total al primer subtotal
    primerSubtotalInput.value = primerSubtotaltemp.toFixed(2);

    let segundoSubtotal = primerSubtotaltemp - (parseFloat(tercerosInput.value) || 0);
    segundoSubtotal -= (parseFloat(peladoresInput.value) || 0);
    segundoSubtotalInput.value = segundoSubtotal.toFixed(2);

    // Calcular el 13% de segundoSubtotal y asignarlo a tercero-cliente
    const terceroCliente = segundoSubtotal * 0.13;
    terceroClienteInput.value = terceroCliente.toFixed(2);

    // Colocar el resultado en el elemento descuento-total-25
    const descuento25 = segundoSubtotal * 0.25;
    descuentoTotal25.innerText = descuento25.toFixed(2);

    // Obtener el porcentaje seleccionado en el combo 
    const porcentajeSeleccionado = parseFloat(porcentajeClienteSelect.value) || 0;

    // Calcular monto abono como porcentaje del segundo subtotal
    const montoAbono = segundoSubtotal * porcentajeSeleccionado;
    montoAbonoInput.value = montoAbono.toFixed(2);

    // Calcular total general restando monto abono del segundo subtotal
    const totalGeneral = segundoSubtotal - montoAbono;
    totalGeneralInput.value = totalGeneral.toFixed(2);

    // Calcular el vuelto si el pago es mayor que el total general
    actualizarVuelto();
}
