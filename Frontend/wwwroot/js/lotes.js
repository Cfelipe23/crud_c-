
 //Lógica para la gestión de Lotes
 

const form = document.getElementById("loteForm");
const tabla = document.getElementById("lotesTable");
const tablaContainer = document.getElementById("tablaContainer");
const sinDatos = document.getElementById("sinDatos");
const alertContainer = document.getElementById("alertContainer");
const productoSelect = document.getElementById("productoId");

let lotes = [];
let productos = [];
let loteAEliminar = null;

// Cargar datos al abrir la página
document.addEventListener("DOMContentLoaded", () => {
    cargarProductos();
    cargarLotes();
    // Establecer fecha de hoy como valor por defecto
    document.getElementById("fechaIngreso").valueAsDate = new Date();
});

// Manejar el envío del formulario
form.addEventListener("submit", async (e) => {
    e.preventDefault();
    await guardarLote();
});


 //Carga todos los productos para el select
 
async function cargarProductos() {
    try {
        productos = await ApiClient.get("/productos");
        actualizarSelectProductos();
    } catch (error) {
        mostrarAlerta("Error al cargar los productos", "danger");
        console.error(error);
    }
}


  //Actualiza el select de productos
 
function actualizarSelectProductos() {
    productoSelect.innerHTML = '<option value="">Seleccionar producto...</option>';
    productos.forEach(p => {
        const option = document.createElement("option");
        option.value = p.id;
        option.textContent = `${p.nombre} (Stock: ${p.stock})`;
        productoSelect.appendChild(option);
    });
}


 //Carga todos los lotes desde la API
 
async function cargarLotes() {
    try {
        lotes = await ApiClient.get("/lotes");
        mostrarLotes();
        actualizarEstadisticas();
    } catch (error) {
        mostrarAlerta("Error al cargar los lotes", "danger");
        console.error(error);
    }
}


 //Muestra la tabla de lotes o mensaje de sin datos
 
function mostrarLotes() {
    if (lotes.length === 0) {
        tablaContainer.style.display = "none";
        sinDatos.style.display = "block";
        return;
    }

    tablaContainer.style.display = "block";
    sinDatos.style.display = "none";

    tabla.innerHTML = lotes.map(l => {
        const diasAntiguedad = calcularDiasAntiguedad(l.fechaIngreso);
        const estado = obtenerEstadoLote(diasAntiguedad);
        const badge = obtenerBadgeEstado(diasAntiguedad);

        return `
            <tr>
                <td><strong>${l.id}</strong></td>
                <td><strong>${l.codigo}</strong></td>
                <td>${l.productoId}</td>
                <td>${formatearFecha(l.fechaIngreso)}</td>
                <td>${l.cantidad}</td>
                <td>${diasAntiguedad} días</td>
                <td>${badge}</td>
                <td>
                    <button class="btn btn-sm btn-warning" onclick="cargarEdicion(${l.id})">
                        ✏️ Editar
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="abrirModalEliminar(${l.id}, '${l.codigo}')">
                        🗑️ Eliminar
                    </button>
                </td>
            </tr>
        `;
    }).join("");
}


  //Actualiza las estadísticas de lotes
 
function actualizarEstadisticas() {
    const totalLotes = lotes.length;
    const lotesRecientes = lotes.filter(l => calcularDiasAntiguedad(l.fechaIngreso) <= 30).length;
    const cantidadTotal = lotes.reduce((sum, l) => sum + l.cantidad, 0);
    const lotesAntiguos = lotes.filter(l => calcularDiasAntiguedad(l.fechaIngreso) > 90).length;

    document.getElementById("totalLotes").textContent = totalLotes;
    document.getElementById("lotesRecientes").textContent = lotesRecientes;
    document.getElementById("cantidadTotal").textContent = cantidadTotal;
    document.getElementById("lotesAntiguos").textContent = lotesAntiguos;
}


  //Calcula los días de antigüedad de un lote
 
function calcularDiasAntiguedad(fechaIngreso) {
    const fecha = new Date(fechaIngreso);
    const hoy = new Date();
    const diferencia = hoy - fecha;
    return Math.floor(diferencia / (1000 * 60 * 60 * 24));
}


  //Obtiene el estado del lote basado en su antigüedad
 
function obtenerEstadoLote(dias) {
    if (dias <= 7) return "Muy Reciente";
    if (dias <= 30) return "Reciente";
    if (dias <= 90) return "Normal";
    return "Antiguo";
}


  //Obtiene el HTML para el estado del lote
 
function obtenerBadgeEstado(dias) {
    if (dias <= 7) return '<span class="badge bg-success">Muy Reciente</span>';
    if (dias <= 30) return '<span class="badge bg-info">Reciente</span>';
    if (dias <= 90) return '<span class="badge bg-warning">Normal</span>';
    return '<span class="badge bg-danger">Antiguo</span>';
}


  //Formatea una fecha en formato dd/MM/yyyy
 
function formatearFecha(fecha) {
    const date = new Date(fecha);
    const dia = String(date.getDate()).padStart(2, '0');
    const mes = String(date.getMonth() + 1).padStart(2, '0');
    const año = date.getFullYear();
    return `${dia}/${mes}/${año}`;
}


  //Guarda o actualiza un lote
 
async function guardarLote() {
    const id = document.getElementById("loteId").value;
    const codigo = document.getElementById("codigo").value.trim();
    const productoId = document.getElementById("productoId").value;
    const fechaIngreso = document.getElementById("fechaIngreso").value;
    const cantidad = document.getElementById("cantidad").value;

    // Validaciones
    if (!codigo) {
        mostrarAlerta("El código del lote es obligatorio", "warning");
        return;
    }

    if (!productoId) {
        mostrarAlerta("Debes seleccionar un producto", "warning");
        return;
    }

    if (!fechaIngreso) {
        mostrarAlerta("La fecha de ingreso es obligatoria", "warning");
        return;
    }

    if (!cantidad || parseInt(cantidad) < 1) {
        mostrarAlerta("La cantidad debe ser mayor que 0", "warning");
        return;
    }

    const lote = {
        codigo,
        productoId: parseInt(productoId),
        fechaIngreso: new Date(fechaIngreso).toISOString(),
        cantidad: parseInt(cantidad)
    };

    try {
        if (id) {
            // Actualizar
            await ApiClient.put(`/lotes/${id}`, lote);
            mostrarAlerta("Lote actualizado correctamente", "success");
        } else {
            // Crear nuevo
            await ApiClient.post("/lotes", lote);
            mostrarAlerta("Lote creado correctamente", "success");
        }

        limpiarFormulario();
        await cargarLotes();
    } catch (error) {
        mostrarAlerta("Error al guardar el lote", "danger");
        console.error(error);
    }
}


  //Carga los datos de un lote en el formulario para editar
 
async function cargarEdicion(id) {
    try {
        const lote = await ApiClient.get(`/lotes/${id}`);
        
        document.getElementById("loteId").value = lote.id;
        document.getElementById("codigo").value = lote.codigo;
        document.getElementById("productoId").value = lote.productoId;
        document.getElementById("fechaIngreso").value = lote.fechaIngreso.split('T')[0];
        document.getElementById("cantidad").value = lote.cantidad;
        
        document.getElementById("btnText").textContent = "✏️ Actualizar Lote";
        
        // Scroll hasta el formulario
        document.getElementById("loteForm").scrollIntoView({ behavior: "smooth" });
    } catch (error) {
        mostrarAlerta("Error al cargar los datos del lote", "danger");
        console.error(error);
    }
}


  //Abre el modal de confirmación de eliminación
 
function abrirModalEliminar(id, codigoLote) {
    loteAEliminar = id;
    document.getElementById("modalMensaje").textContent = 
        `¿Estás seguro de que deseas eliminar el lote "${codigoLote}"? Esta acción no se puede deshacer.`;
    
    const modal = new bootstrap.Modal(document.getElementById("modalEliminar"));
    modal.show();
}


  //Confirma la eliminación del lote
 
document.getElementById("btnConfirmarEliminar").addEventListener("click", async () => {
    if (loteAEliminar) {
        await eliminarLote(loteAEliminar);
        bootstrap.Modal.getInstance(document.getElementById("modalEliminar")).hide();
    }
});


 // Elimina un lote
 
async function eliminarLote(id) {
    try {
        await ApiClient.delete(`/lotes/${id}`);
        mostrarAlerta("Lote eliminado correctamente", "success");
        await cargarLotes();
    } catch (error) {
        mostrarAlerta("Error al eliminar el lote", "danger");
        console.error(error);
    }
}


  //Limpia el formulario y resetea a modo creación
 
function limpiarFormulario() {
    document.getElementById("loteForm").reset();
    document.getElementById("loteId").value = "";
    document.getElementById("btnText").textContent = "💾 Guardar Lote";
    document.getElementById("fechaIngreso").valueAsDate = new Date();
}


  //Muestra una alerta al usuario
 
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
