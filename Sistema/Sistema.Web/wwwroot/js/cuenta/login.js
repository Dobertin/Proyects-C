document.getElementById('loginForm').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevenir el envío tradicional del formulario

    const formData = new FormData(this); // Recoger los datos del formulario
    const url = '/tuControlador/ValidarUsuarioAsync'; // Asegúrate de que la URL sea la correcta

    fetch(url, {
        method: 'POST',
        body: JSON.stringify(Object.fromEntries(formData)),
        headers: {
            'Content-Type': 'application/json' // Importante para que el servidor pueda leer el JSON correctamente
        }
    })
        .then(response => response.json()) // Convertir la respuesta del servidor a JSON
        .then(data => {
            console.log(data); // Hacer algo con los datos (por ejemplo, redirigir o mostrar un mensaje)
        })
        .catch(error => console.error('Error:', error)); // Manejar posibles errores
});