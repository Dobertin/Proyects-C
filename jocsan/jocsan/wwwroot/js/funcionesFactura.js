// Seleccionar el botón de radio y el campo de cantidad
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

document.addEventListener("DOMContentLoaded", function () {
    
    // Llamar a las funciones cuando el DOM esté completamente cargado
    cargarClientes();
    cargarPorcentajes();
    cargarProductos();
    cargarHielo();

    // Ejemplo de función para cambiar el precio del producto temporalmente
    var changePriceBtn = document.getElementById("changePrice");   
    changePriceBtn.addEventListener("click", function () {
        var newPrice = prompt("Ingrese el nuevo precio del producto:");
        if (newPrice) {
            document.querySelector(".price-box:nth-child(3) p").textContent = newPrice;
        }
    });

    // Agrega funcionalidad al botón de eliminar
    const deleteButtons = document.querySelectorAll(".eliminar-btn");
    deleteButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            const confirmed = confirm("¿Estás seguro que deseas eliminar esta factura?");
            if (!confirmed) {
                event.preventDefault();  // Cancela la eliminación si no se confirma
            }
        });
    });
});

// Función para actualizar los cálculos
function actualizarCalculos() {
    // Obtener y validar los valores necesarios
    const cantidadProducto = parseFloat(cantidadProductoInput.value) || 0;
    const precioProductoValor = parseFloat(precioProducto.innerText) || 0;

    // Calcular el subtotal del producto
    const subtotalProducto = cantidadProducto * precioProductoValor;
    subtotalProductoInput.value = subtotalProducto.toFixed(2);

    // Realizar las operaciones paso a paso
    let primerSubtotal = subtotalProducto - parseFloat(gasHieloInput.value) || 0;
    primerSubtotalInput.value = primerSubtotal.toFixed(2);

    let segundoSubtotal = primerSubtotal - (parseFloat(tercerosInput.value) || 0);
    segundoSubtotal -= (parseFloat(peladoresInput.value) || 0);
    segundoSubtotalInput.value = segundoSubtotal.toFixed(2);

    // Calcular el 13% de segundoSubtotal y asignarlo a tercero-cliente
    const terceroCliente = segundoSubtotal * 0.13;
    terceroClienteInput.value = terceroCliente.toFixed(2);

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