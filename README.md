# TaskManagement

## User Story

**As a user**,  
I want to be able to create, read, update, and delete tasks,  
So that I can manage my tasks effectively.

## Acceptance Criteria

1. **User Registration and Authentication**:
   - Users can register with a username and password.
   - Users can log in using their username and password.
   - Authentication is required for all task management operations.

2. **Task Management**:
   - **Create Todo Item**: Create a new Todo Item.
   - **Read Todo Items**: Obtain all Todo Items.
   - **Read Todo Item by Id**: Retrieve a Todo Item by id.
   - **Update Todo Item**: It can update an existing Todo Item.
   - **Delete Task**: System can delete a Todo Item.

## Technical Details

1. **Database**:
   - Use SQL Server running in a Docker container.
   - Two tables: `Users` and `TodoItem`.

2. **API**:
   - ASP.NET Core Web API.
   - Endpoints for user registration, login, and task management.
   - Middleware for authenticating requests to task endpoints using headers for username and password.

3. **Data Layer**:
   - Custom repository pattern (no Entity Framework, Dapper, or MediatR).
   - CRUD operations for tasks and user management.

4. **Business Logic Layer**:
   - Validation and business rules separate from data access and API layers.
   - Services for user authentication and task management.

5. **Unit Tests**:
   - Comprehensive tests for controllers, services, and middleware using XUnit.

6. **Docker**:
   - SQL Server container setup.
   - ASP.NET Core application container setup.

## How to run this application?

1. **Clone Repository**
    - Github clone repository: https://github.com/ferlossi/TaskManagement.git
2. **Install Docker Desktop**
    - After installing docker desktop, configure it to run Linux containers.
3. **Create volumes for docker compose**
    - In order to be able to preserve data even when containers get destroyed, create the following folders on "C" drive:
      -  C:/DockerVolumes/data
      -  C:/DockerVolumes/log
      -  C:/DockerVolumes/secrets
4. **Build and run docker compose**
    - Open a Powershell on project root and run the following commands
      - `docker-compose build `
      - `docker-compose up `
5. **You are ready!**
    - You can start playing around with the available postman collection. You can locate it [here](https://github.com/ferlossi/TaskManagement/blob/main/TaskManager.postman_collection.json)
    - There are a couple of postman environment availables as well [Docker](https://github.com/ferlossi/TaskManagement/blob/main/Docker.postman_environment.json) [Local](https://github.com/ferlossi/TaskManagement/blob/main/Local.postman_environment.json)

## Additional info ##
1. Database is originally configured to be available on:
    - Port: 1450
    - URL: 127.0.0.1 (localhost)
2. Service is running on DEVELOPMENT mode, so swagger is available [here](http://localhost:1540/swagger/index.html)