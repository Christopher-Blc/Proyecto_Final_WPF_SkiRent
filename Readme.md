# 🎿 SkiRent — Gestión de Alquiler de Esquí

Aplicación de escritorio desarrollada en C# con WPF para la gestión integral de un negocio de alquiler de material de esquí y snowboard.

---

## Descripción

SkiRent es una aplicación creada para la asignatura Desarrollo de Interfaces (2º DAM).

Permite gestionar clientes, material, alquileres y generar informes profesionales aplicando arquitectura por capas y buenas prácticas de desarrollo.

---

## Arquitectura

La solución está separada en distintos proyectos:

SkiRent  
│  
├── SkiRentModel        → Entidades y acceso a datos (Entity Framework)  (model)
├── SkiRentController   → Lógica de negocio   (Controller)
├── Proyecto_WPF_SkiRent → Interfaz gráfica WPF (view) 
├── SkiRentInformes     → Informes Crystal Reports  
└── SkiRentTest         → Pruebas unitarias e integración  

Patrón utilizado: MVC 

---

## Tecnologías

- C#
- .NET Framework
- WPF (XAML)
- Entity Framework 6
- SQL Server
- Crystal Reports
- Testing (Unit Test + Integración)
- GitHub

---

## Base de Datos

Tablas principales:

- Cliente
- CategoriaMaterial
- Material
- Alquiler
- LineaAlquiler

Relaciones:

- Un Cliente puede tener varios Alquileres
- Un Alquiler puede tener varias LineasAlquiler
- Cada LineaAlquiler referencia un Material
- El stock se controla automáticamente

El script SQL está incluido en el repositorio.

---

## Funcionalidades

- CRUD completo de Clientes
- CRUD completo de Material
- Gestión de Alquileres
- Control automático de stock
- Validaciones de datos
- Uso de DataGrid
- Navegación entre pantallas
- Informes con agrupación y totalización

---

## Informes

Incluye mínimo 3 informes:

1. Informe simple
2. Informe agrupado
3. Informe con totalización

---

## Testing

Incluye:

- 2 pruebas unitarias
- 1 prueba de integración

---

## Instalación

1. Clonar repositorio
2. Restaurar paquetes NuGet
3. Ejecutar script de base de datos
4. Configurar cadena de conexión en App.config
5. Compilar y ejecutar

Incluye carpeta con instalador.

---

## 👨‍💻 Autor

Christopher Bolocan  
CFGS DAM — Desarrollo de Aplicaciones Multiplataforma  
Proyecto final de la asignatura Desarrollo de Interfaces
