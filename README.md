# 📚 WebBiblioteca

Aplicación web desarrollada en **ASP.NET MVC 4.8.1** como proyecto final del Ciclo Superior de Desarrollo de Aplicaciones Web.  
El sistema permite la **gestión de usuarios, libros y préstamos** en una biblioteca, incluyendo control de stock, vencimientos, incidencias y estadísticas.

---

## 🚀 Tecnologías utilizadas
- **ASP.NET MVC 4.8.1**
- **Entity Framework** (Code First, control de concurrencia con `[Timestamp]`)
- **SQL Server** (base de datos relacional)
- **Bootstrap 5** (interfaz responsiva)
- **Identity** para autenticación de usuarios
- **LINQ** para consultas
- **C#** como lenguaje principal

---

## ✨ Características principales
- 🔑 **Autenticación y roles**  
  - Usuario normal: consultar catálogo, solicitar préstamos, devolver libros.  
  - Administrador: gestión completa de usuarios, libros y préstamos.  

- 📚 **Gestión de libros**  
  - CRUD con control de concurrencia.  
  - Control de stock disponible.  

- 🔄 **Gestión de préstamos**  
  - Solicitud de préstamos validada por administrador.  
  - Duración configurable de los préstamos.  
  - Estados automáticos: *Prestado*, *Devuelto*, *Vencido*, *Incidencia*.  
  - Bloqueo temporal de usuarios por retrasos.  

- 📊 **Estadísticas**  
  - Total de libros.  
  - Préstamos activos y vencidos.  
  - Incidencias.  
  - Libros más demandados.  

- 💡 **Sugerencias de usuarios**  
  - Sistema de buzón para proponer adquisiciones o mejoras.  

---

## 🛠 Instalación y ejecución

### 1️⃣ Requisitos previos
- Visual Studio 2022 (o superior) con soporte para ASP.NET  
- SQL Server (local o remoto)  
- .NET Framework 4.8.1  

### 2️⃣ Clonar el repositorio
```bash
git clone https://github.com/PedroLT95/WebBiblioteca.git

