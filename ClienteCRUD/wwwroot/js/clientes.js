/**
 * Lógica para la gestión de Clientes
 */

const form = document.getElementById("clienteForm");
const tabla = document.getElementById("clientesTable");
const tablaContainer = document.getElementById("tablaContainer");
const sinDatos = document.getElementById("sinDatos");
const alertContainer = document.getElementById("alertContainer");

let clientes = [];
let clienteAEliminar = null;

// Cargar clientes al abrir la página
document.addEventListener("DOMContentLoaded", () => {
    cargarClientes();
});

// Manejar el envío del formulario
form.addEventListener("submit", async (e) => {
    e.preventDefault();
    await guardarCliente();
});

/**
 * Carga todos los clientes desde la API
 */
async function cargarClientes() {
    try {
        clientes = await ApiClient.get("/clientes");
        mostrarClientes();
    } catch (error) {
        mostrarAlerta("Error al cargar los clientes", "danger");
        console.error(error);
    }
}

/**
 * Muestra la tabla de clientes o mensaje de sin datos
 */
function mostrarClientes() {
    if (clientes.length === 0) {
        tablaContainer.style.display = "none";
        sinDatos.style.display = "block";
        return;
    }

    tablaContainer.style.display = "block";
    sinDatos.style.display = "none";

    tabla.innerHTML = clientes.map(c => `
        <tr>
            <td><strong>${c.id}</strong></td>
            <td>${c.nombre} ${c.apellido}</td>
            <td>${c.email || "—"}</td>
            <td>${c.telefono || "—"}</td>
            <td>${c.direccion || "—"}</td>
            <td>
                <button class="btn btn-sm btn-warning" onclick="cargarEdicion(${c.id})">
                    ✏️ Editar
                </button>
                <button class="btn btn-sm btn-danger" onclick="abrirModalEliminar(${c.id}, '${c.nombre} ${c.apellido}')">
                    🗑️ Eliminar
                </button>
            </td>
        </tr>
    `).join("");
}

/**
 * Guarda o actualiza un cliente
 */
async function guardarCliente() {
    const id = document.getElementById("clienteId").value;
    const nombre = document.getElementById("nombre").value.trim();
    const apellido = document.getElementById("apellido").value.trim();
    const email = document.getElementById("email").value.trim();
    const telefono = document.getElementById("telefono").value.trim();
    const direccion = document.getElementById("direccion").value.trim();

    // Validaciones
    if (!nombre || !apellido) {
        mostrarAlerta("El nombre y apellido son obligatorios", "warning");
        return;
    }

    if (email && !validarEmail(email)) {
        mostrarAlerta("El email no es válido", "warning");
        return;
    }

    const cliente = {
        nombre,
        apellido,
        email: email || null,
        telefono: telefono || null,
        direccion: direccion || null
    };

    try {
        if (id) {
            // Actualizar
            await ApiClient.put(`/clientes/${id}`, cliente);
            mostrarAlerta("Cliente actualizado correctamente", "success");
        } else {
            // Crear nuevo
            await ApiClient.post("/clientes", cliente);
            mostrarAlerta("Cliente creado correctamente", "success");
        }

        limpiarFormulario();
        await cargarClientes();
    } catch (error) {
        mostrarAlerta("Error al guardar el cliente", "danger");
        console.error(error);
    }
}

/**
 * Carga los datos de un cliente en el formulario para editar
 */
async function cargarEdicion(id) {
    try {
        const cliente = await ApiClient.get(`/clientes/${id}`);
        
        document.getElementById("clienteId").value = cliente.id;
        document.getElementById("nombre").value = cliente.nombre;
        document.getElementById("apellido").value = cliente.apellido;
        document.getElementById("email").value = cliente.email || "";
        document.getElementById("telefono").value = cliente.telefono || "";
        document.getElementById("direccion").value = cliente.direccion || "";
        
        document.getElementById("btnText").textContent = "✏️ Actualizar Cliente";
        
        // Scroll hasta el formulario
        document.getElementById("clienteForm").scrollIntoView({ behavior: "smooth" });
    } catch (error) {
        mostrarAlerta("Error al cargar los datos del cliente", "danger");
        console.error(error);
    }
}

/**
 * Abre el modal de confirmación de eliminación
 */
function abrirModalEliminar(id, nombreCliente) {
    clienteAEliminar = id;
    document.getElementById("modalMensaje").textContent = 
        `¿Estás seguro de que deseas eliminar al cliente "${nombreCliente}"? Esta acción no se puede deshacer.`;
    
    const modal = new bootstrap.Modal(document.getElementById("modalEliminar"));
    modal.show();
}

/**
 * Confirma la eliminación del cliente
 */
document.getElementById("btnConfirmarEliminar").addEventListener("click", async () => {
    if (clienteAEliminar) {
        await eliminarCliente(clienteAEliminar);
        bootstrap.Modal.getInstance(document.getElementById("modalEliminar")).hide();
    }
});

/**
 * Elimina un cliente
 */
async function eliminarCliente(id) {
    try {
        await ApiClient.delete(`/clientes/${id}`);
        mostrarAlerta("Cliente eliminado correctamente", "success");
        await cargarClientes();
    } catch (error) {
        mostrarAlerta("Error al eliminar el cliente", "danger");
        console.error(error);
    }
}

/**
 * Limpia el formulario y resetea a modo creación
 */
function limpiarFormulario() {
    document.getElementById("clienteForm").reset();
    document.getElementById("clienteId").value = "";
    document.getElementById("btnText").textContent = "💾 Guardar Cliente";
}

/**
 * Valida el formato del email
 */
function validarEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

/**
 * Muestra una alerta al usuario
 */
function mostrarAlerta(mensaje, tipo = "info") {
    const alertDiv = document.createElement("div");
    alertDiv.className = `alert alert-${tipo} alert-dismissible fade show`;
    alertDiv.role = "alert";
    alertDiv.innerHTML = `
        ${mensaje}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    alertContainer.innerHTML = "";
    alertContainer.appendChild(alertDiv);

    // Auto-descartar después de 5 segundos
    setTimeout(() => {
        alertDiv.remove();
    }, 5000);
}
