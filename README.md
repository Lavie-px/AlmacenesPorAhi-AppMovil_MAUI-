#Requisitos
- Visual Studio 2026 o que soporte .NET 10.0
- .NET 10.0
- Tener instalado los paquetes de trabajo de MAUI
* **Base de datos local**
  - Xampp, wampp (xampp no recomendable)
* **Base de datos en la nube**
  - AWS, Azure, bluehosting o similares
 
#Instrucciones de instalacion
- instalar los paquetes de trabajo de maui en caso de no tenerlos con el comando "dotnet workload install maui" dentro del gestor de paquetes Nuget
- instalar el proveedor pomelo para la base de datos con el comando "Install-Package Pomelo.EntityFrameworkCore.MySql" dentro del gestor de paquetes Nuget
- dentro de la carpeta AlmacenesPorAhi encontraras el archivo apppsettings.json
- Cambiar los parametros dentro de la cadena de texto MySql (Server, Port, Database, User y Password)
