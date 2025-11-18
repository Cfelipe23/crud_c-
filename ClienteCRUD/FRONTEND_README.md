# Sistema CRUD con Frontend HTML + Bootstrap

## 🎉 Implementación Completada

Se ha implementado un **Frontend HTML + Bootstrap** moderno y responsivo para tu aplicación CRUD, convirtiendo tu proyecto C# de consola en una **Aplicación Web ASP.NET Core** con API REST.

## 📁 Estructura del Proyecto

```
ClienteCRUD/
├── Controllers/                    # Controladores API REST
│   ├── ClientesController.cs
│   ├── ProductosController.cs
│   └── LotesController.cs
├── Modules/                        # Módulos con lógica de negocio
│   ├── cliente/
│   ├── producto/
│   └── lote/
├── wwwroot/                        # Archivos estáticos (Frontend)
│   ├── index.html                  # Página principal
│   ├── css/
│   │   ├── bootstrap.min.css       # Bootstrap CDN
│   │   └── style.css               # Estilos personalizados
│   ├── js/
│   │   ├── api-client.js           # Cliente HTTP para API
│   │   ├── clientes.js             # Lógica de Clientes
│   │   ├── productos.js            # Lógica de Productos
│   │   └── lotes.js                # Lógica de Lotes
│   └── pages/
│       ├── clientes.html           # Página CRUD Clientes
│       ├── productos.html          # Página CRUD Productos
│       └── lotes.html              # Página CRUD Lotes
├── Program.cs                      # Configuración ASP.NET Core
└── ClienteCRUD.csproj            # Archivo de proyecto
```

## 🚀 Cómo Ejecutar la Aplicación

### 1. Compilar el Proyecto
```bash
cd ClienteCRUD
dotnet build
```

### 2. Ejecutar la Aplicación
```bash
dotnet run
```

La aplicación se ejecutará en: **http://localhost:5000**

## 🌐 Acceso al Frontend

Una vez que la aplicación esté ejecutándose, abre tu navegador e ingresa a:

- **Página Principal**: http://localhost:5000
- **Gestión de Clientes**: http://localhost:5000/pages/clientes.html
- **Gestión de Productos**: http://localhost:5000/pages/productos.html
- **Gestión de Lotes**: http://localhost:5000/pages/lotes.html

## ✨ Características Implementadas

### Frontend HTML + Bootstrap

✅ **Interfaz Responsiva**
- Diseño mobile-first con Bootstrap 5
- Compatible con dispositivos móviles, tablets y desktops
- Navegación intuitiva con menú responsive

✅ **Gestión de Clientes**
- Crear, leer, actualizar y eliminar clientes
- Campos: Nombre, Apellido, Email, Teléfono, Dirección
- Validación de email en tiempo real
- Modal de confirmación para eliminación

✅ **Gestión de Productos**
- CRUD completo de productos
- Campos: Nombre, Precio, Stock, Descripción
- Estadísticas en tiempo real:
  - Total de productos
  - Stock total
  - Valor total del inventario
  - Productos sin stock
- Badges para indicar estado del stock

✅ **Gestión de Lotes**
- CRUD completo de lotes
- Campos: Código, Producto, Fecha de Ingreso, Cantidad
- Cálculo automático de antigüedad
- Estados: Muy Reciente, Reciente, Normal, Antiguo
- Dropdown dinámico de productos
- Estadísticas:
  - Total de lotes
  - Lotes recientes (últimos 30 días)
  - Cantidad total
  - Lotes antiguos (más de 90 días)

✅ **Backend API REST**
- Controladores ASP.NET Core:
  - `GET /api/clientes` - Obtener todos
  - `GET /api/clientes/{id}` - Obtener por ID
  - `POST /api/clientes` - Crear
  - `PUT /api/clientes/{id}` - Actualizar
  - `DELETE /api/clientes/{id}` - Eliminar
- Equivalente para Productos y Lotes
- CORS habilitado para comunicación entre frontend y backend

✅ **Experiencia de Usuario**
- Alertas contextuales (éxito, error, advertencia)
- Scroll automático al formulario al editar
- Limpiar formulario después de guardar
- Confirmación visual de acciones
- Tablas con ordenamiento y hover effects
- Deshabilitación automática de inputs inválidos

✅ **Estilos Personalizados**
- Tema profesional con colores consistentes
- Botones con efectos hover y animaciones
- Tarjetas con sombras y espaciado adecuado
- Navbarfija con logotipo
- Footer informativo
- Responsive para todas las resoluciones

## 🔧 Cambios en el Proyecto

### Program.cs (Transformación a ASP.NET Core)
```csharp
// Antes: Aplicación de consola
// Después: Servidor web ASP.NET Core
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(...);
builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.MapControllers();
```

### ClienteCRUD.csproj
- Cambio de SDK: `Microsoft.NET.Sdk` → `Microsoft.NET.Sdk.Web`
- Framework: `.net9.0`

### Nuevos Archivos Creados
1. **Controladores API** - `Controllers/*.cs`
2. **Frontend HTML** - `wwwroot/index.html`, `wwwroot/pages/*.html`
3. **JavaScript** - `wwwroot/js/*.js`
4. **CSS** - `wwwroot/css/style.css`

## 📊 Endpoints de la API

### Clientes
```
GET    /api/clientes
GET    /api/clientes/{id}
POST   /api/clientes
PUT    /api/clientes/{id}
DELETE /api/clientes/{id}
```

### Productos
```
GET    /api/productos
GET    /api/productos/{id}
POST   /api/productos
PUT    /api/productos/{id}
DELETE /api/productos/{id}
```

### Lotes
```
GET    /api/lotes
GET    /api/lotes/{id}
POST   /api/lotes
PUT    /api/lotes/{id}
DELETE /api/lotes/{id}
```

## 🎨 Tecnologías Utilizadas

- **Backend**: ASP.NET Core 9.0, C#
- **Frontend**: HTML5, CSS3, JavaScript Vanilla, Bootstrap 5
- **Base de Datos**: SQLite
- **API**: REST con JSON

## 💡 Notas Importantes

1. **CORS Habilitado**: El frontend puede comunicarse libremente con el backend
2. **Base de Datos**: Utiliza SQLite (`clientes.db`) en el directorio del proyecto
3. **Validaciones**: Se realizan tanto en frontend (JavaScript) como en backend (C#)
4. **Rutas por Defecto**: El frontend se sirve como contenido estático desde `wwwroot`

## 🔒 Validaciones Implementadas

### Frontend
- Email válido (formato)
- Campos requeridos
- Números positivos para precios y cantidades
- Confirmación de eliminación con modal

### Backend
- Validación de ModelState
- Verificación de existencia de registros
- Manejo de errores HTTP
- Respuestas consistentes en JSON

## 📱 Responsividad

La aplicación es completamente responsiva gracias a Bootstrap:
- **Desktop**: Todas las columnas visibles
- **Tablet**: Ajuste automático de columnas
- **Móvil**: Una columna, navegación vertical

## 🚨 Troubleshooting

### La aplicación no inicia
```bash
dotnet clean
dotnet build
dotnet run
```

### Puerto 5000 en uso
Cambiar el puerto en `launchSettings.json` o usar:
```bash
dotnet run --urls "http://localhost:5001"
```

### Base de datos no encontrada
La base de datos se crea automáticamente en la primera ejecución. Asegúrate de tener permisos de escritura en el directorio.

## 📝 Próximas Mejoras Sugeridas

- Agregar autenticación y autorización
- Implementar búsqueda y filtros avanzados
- Exportar datos a CSV/Excel
- Gráficos y reportes
- Sistema de paginación
- Caching de datos
- Validación avanzada con librerías
- Internacionalización (i18n)

---

## ✅ Resumen Final

Has transformado exitosamente tu aplicación CRUD de consola en una **aplicación web moderna** con:
- ✅ API REST completamente funcional
- ✅ Frontend HTML + Bootstrap responsivo
- ✅ Interfaz intuitiva y profesional
- ✅ Gestión completa de tres módulos (Clientes, Productos, Lotes)
- ✅ Validaciones en frontend y backend
- ✅ Todas las operaciones CRUD implementadas

**¡La aplicación está lista para usar!**
