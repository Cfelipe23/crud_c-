/**
 * Lógica para la gestión de Productos
 */

const form = document.getElementById("productoForm");
const tabla = document.getElementById("productosTable");
const tablaContainer = document.getElementById("tablaContainer");
const sinDatos = document.getElementById("sinDatos");
const alertContainer = document.getElementById("alertContainer");

let productos = [];
let productoAEliminar = null;

// Cargar productos al abrir la página
document.addEventListener("DOMContentLoaded", () => {
    cargarProductos();
});

// Manejar el envío del formulario
form.addEventListener("submit", async (e) => {
    e.preventDefault();
    await guardarProducto();
});

/**
 * Carga todos los productos desde la API
 */
async function cargarProductos() {
    try {
        productos = await ApiClient.get("/productos");
        mostrarProductos();
        actualizarEstadisticas();
    } catch (error) {
        mostrarAlerta("Error al cargar los productos", "danger");
        console.error(error);
    }
}

/**
 * Muestra la tabla de productos o mensaje de sin datos
 */
function mostrarProductos() {
    if (productos.length === 0) {
        tablaContainer.style.display = "none";
        sinDatos.style.display = "block";
        return;
    }

    tablaContainer.style.display = "block";
    sinDatos.style.display = "none";

    tabla.innerHTML = productos.map(p => {
        const valorTotal = (p.precio * p.stock).toFixed(2);
        const estado = p.stock === 0 ? 
            '<span class="badge bg-danger">Sin Stock</span>' : 
            p.stock < 10 ? 
            '<span class="badge bg-warning">Bajo Stock</span>' : 
            '<span class="badge bg-success">En Stock</span>';

        return `
            <tr>
                <td><strong>${p.id}</strong></td>
                <td>${p.nombre}</td>
                <td>${p.descripcion || "—"}</td>
                <td>$${parseFloat(p.precio).toFixed(2)}</td>
                <td>${p.stock}</td>
                <td>$${valorTotal}</td>
                <td>${estado}</td>
                <td>
                    <button class="btn btn-sm btn-warning" onclick="cargarEdicion(${p.id})">
                        ✏️ Editar
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="abrirModalEliminar(${p.id}, '${p.nombre}')">
                        🗑️ Eliminar
                    </button>
                </td>
            </tr>
        `;
    }).join("");
}

/**
 * Actualiza las estadísticas de productos
 */
function actualizarEstadisticas() {
    const totalProductos = productos.length;
    const totalStock = productos.reduce((sum, p) => sum + p.stock, 0);
    const valorInventario = productos.reduce((sum, p) => sum + (p.precio * p.stock), 0);
    const sinStock = productos.filter(p => p.stock === 0).length;

    document.getElementById("totalProductos").textContent = totalProductos;
    document.getElementById("totalStock").textContent = totalStock;
    document.getElementById("valorInventario").textContent = `$${valorInventario.toFixed(2)}`;
    document.getElementById("productosSinStock").textContent = sinStock;
}

/**
 * Guarda o actualiza un producto
 */
async function guardarProducto() {
    const id = document.getElementById("productoId").value;
    const nombre = document.getElementById("nombre").value.trim();
    const precio = document.getElementById("precio").value;
    const stock = document.getElementById("stock").value;
    const descripcion = document.getElementById("descripcion").value.trim();

    // Validaciones
    if (!nombre) {
        mostrarAlerta("El nombre del producto es obligatorio", "warning");
        return;
    }

    if (!precio || parseFloat(precio) < 0) {
        mostrarAlerta("El precio debe ser un número positivo", "warning");
        return;
    }

    if (!stock || parseInt(stock) < 0) {
        mostrarAlerta("El stock debe ser un número positivo", "warning");
        return;
    }

    const producto = {
        nombre,
        precio: parseFloat(precio),
        stock: parseInt(stock),
        descripcion: descripcion || null
    };

    try {
        if (id) {
            // Actualizar
            await ApiClient.put(`/productos/${id}`, producto);
            mostrarAlerta("Producto actualizado correctamente", "success");
        } else {
            // Crear nuevo
            await ApiClient.post("/productos", producto);
            mostrarAlerta("Producto creado correctamente", "success");
        }

        limpiarFormulario();
        await cargarProductos();
    } catch (error) {
        mostrarAlerta("Error al guardar el producto", "danger");
        console.error(error);
    }
}

/**
 * Carga los datos de un producto en el formulario para editar
 */
async function cargarEdicion(id) {
    try {
        const producto = await ApiClient.get(`/productos/${id}`);
        
        document.getElementById("productoId").value = producto.id;
        document.getElementById("nombre").value = producto.nombre;
        document.getElementById("precio").value = producto.precio;
        document.getElementById("stock").value = producto.stock;
        document.getElementById("descripcion").value = producto.descripcion || "";
        
        document.getElementById("btnText").textContent = "✏️ Actualizar Producto";
        
        // Scroll hasta el formulario
        document.getElementById("productoForm").scrollIntoView({ behavior: "smooth" });
    } catch (error) {
        mostrarAlerta("Error al cargar los datos del producto", "danger");
        console.error(error);
    }
}

/**
 * Abre el modal de confirmación de eliminación
 */
function abrirModalEliminar(id, nombreProducto) {
    productoAEliminar = id;
    document.getElementById("modalMensaje").textContent = 
        `¿Estás seguro de que deseas eliminar el producto "${nombreProducto}"? Esta acción no se puede deshacer.`;
    
    const modal = new bootstrap.Modal(document.getElementById("modalEliminar"));
    modal.show();
}

/**
 * Confirma la eliminación del producto
 */
document.getElementById("btnConfirmarEliminar").addEventListener("click", async () => {
    if (productoAEliminar) {
        await eliminarProducto(productoAEliminar);
        bootstrap.Modal.getInstance(document.getElementById("modalEliminar")).hide();
    }
});

/**
 * Elimina un producto
 */
async function eliminarProducto(id) {
    try {
        await ApiClient.delete(`/productos/${id}`);
        mostrarAlerta("Producto eliminado correctamente", "success");
        await cargarProductos();
    } catch (error) {
        mostrarAlerta("Error al eliminar el producto", "danger");
        console.error(error);
    }
}

/**
 * Limpia el formulario y resetea a modo creación
 */
function limpiarFormulario() {
    document.getElementById("productoForm").reset();
    document.getElementById("productoId").value = "";
    document.getElementById("btnText").textContent = "💾 Guardar Producto";
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
