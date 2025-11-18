/**
 * Cliente de API HTTP para comunicación con el backend
 * Maneja todas las llamadas GET, POST, PUT, DELETE
 */

const API_BASE = "http://localhost:5000/api";

class ApiClient {
    /**
     * Realiza una solicitud GET
     * @param {string} endpoint - Ruta del API (ej: /clientes)
     * @returns {Promise<any>} Datos retornados por el servidor
     */
    static async get(endpoint) {
        try {
            const response = await fetch(`${API_BASE}${endpoint}`, {
                method: "GET",
                headers: { "Content-Type": "application/json" }
            });
            if (!response.ok) throw new Error(`Error ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error en GET:", error);
            throw error;
        }
    }

    /**
     * Realiza una solicitud POST
     * @param {string} endpoint - Ruta del API
     * @param {any} data - Datos a enviar
     * @returns {Promise<any>} Respuesta del servidor
     */
    static async post(endpoint, data) {
        try {
            const response = await fetch(`${API_BASE}${endpoint}`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });
            if (!response.ok) throw new Error(`Error ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error en POST:", error);
            throw error;
        }
    }

    /**
     * Realiza una solicitud PUT para actualizar datos
     * @param {string} endpoint - Ruta del API (ej: /clientes/1)
     * @param {any} data - Datos a actualizar
     * @returns {Promise<any>} Respuesta del servidor
     */
    static async put(endpoint, data) {
        try {
            const response = await fetch(`${API_BASE}${endpoint}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });
            if (!response.ok) throw new Error(`Error ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Error en PUT:", error);
            throw error;
        }
    }

    /**
     * Realiza una solicitud DELETE
     * @param {string} endpoint - Ruta del API (ej: /clientes/1)
     * @returns {Promise<void>}
     */
    static async delete(endpoint) {
        try {
            const response = await fetch(`${API_BASE}${endpoint}`, {
                method: "DELETE",
                headers: { "Content-Type": "application/json" }
            });
            if (!response.ok) throw new Error(`Error ${response.status}`);
        } catch (error) {
            console.error("Error en DELETE:", error);
            throw error;
        }
    }

    /**
     * Muestra un mensaje de alerta al usuario
     * @param {string} message - Mensaje a mostrar
     * @param {string} type - Tipo: 'success', 'error', 'info', 'warning'
     */
    static showAlert(message, type = "info") {
        const alertDiv = document.createElement("div");
        alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
        alertDiv.role = "alert";
        alertDiv.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        const container = document.querySelector(".container") || document.body;
        container.insertBefore(alertDiv, container.firstChild);

        // Auto-descartar después de 4 segundos
        setTimeout(() => {
            alertDiv.remove();
        }, 4000);
    }
}
