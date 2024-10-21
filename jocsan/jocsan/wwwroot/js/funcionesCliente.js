document.addEventListener("DOMContentLoaded", function () {
    const peon1Checkbox = document.getElementById("peon1");
    const peon2Checkbox = document.getElementById("peon2");
    const peon1Nombre = document.getElementById("peon1-nombre");
    const peon2Nombre = document.getElementById("peon2-nombre");

    // Habilitar o deshabilitar los nombres de los peones según el checkbox
    peon1Checkbox.addEventListener("change", function () {
        peon1Nombre.disabled = !this.checked;
    });

    peon2Checkbox.addEventListener("change", function () {
        peon2Nombre.disabled = !this.checked;
    });

    // Puedes agregar más lógica según lo que necesites.
});
