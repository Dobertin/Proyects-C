document.addEventListener("DOMContentLoaded", function () {

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
