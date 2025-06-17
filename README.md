**ExpenseTrackerAPI**

ExpenseTrackerAPI es una API RESTful desarrollada con .NET 9, diseñada para gestionar gastos, equipos e invitaciones con un enfoque en arquitectura limpia y mejores prácticas. Actualmente en desarrollo activo, este proyecto utiliza un enfoque Database First con Entity Framework Core y MySQL, un manejo robusto de excepciones y documentación completa con Swagger para facilitar su uso y pruebas.



**Descripción del Proyecto:**

**Esta API permite a los usuarios:**




- Crear y gestionar gastos personales y de equipos.



- Organizar equipos, enviar invitaciones y manejar la membresía.



- Acceder de forma segura a los endpoints con autenticación JWT.



- Obtener resúmenes detallados de gastos, incluyendo totales para usuarios individuales o equipos específicos.

Comparado con mi proyecto anterior (una API de gestión de vehículos con .NET Core, enfoque Code First y sin documentación), ExpenseTrackerAPI incorpora:





- .NET 9 para mejor rendimiento y características modernas.



- Enfoque Database First para integrarse con un esquema MySQL existente.



- Documentación con Swagger para explorar endpoints fácilmente.



- Manejo de excepciones personalizado para una mejor gestión de errores.



**Características:**




- Arquitectura por Capas: Organizada en Controllers, Services, Repositories y DTOs para una clara separación de responsabilidades.



- Integración con Swagger: Endpoints documentados para pruebas e interacción sencilla.



- Base de Datos MySQL: Entity Framework Core mapea entidades desde un esquema existente (users, teams, expenses, invitations, users_teams).



- Autenticación JWT: Acceso seguro a endpoints protegidos.



- Lógica de Negocio: Soporta creación de gastos, gestión de equipos, invitaciones y resúmenes de gastos con validaciones (por ejemplo, asegurando que los usuarios pertenezcan a equipos para gastos de equipo).



En Progreso: Refinando endpoints, agregando pruebas con xUnit y optimizando la lógica de negocio.

**Estado:**

🚧 En Desarrollo: La API es funcional pero está incompleta. Tareas pendientes incluyen:


- Finalizar casos extremos en la lógica de los endpoints.

- Implementar pruebas unitarias.

- Mejorar el manejo de errores y optimizaciones de rendimiento.

- Agregar funcionalidades avanzadas como categorización de gastos o reportes.
