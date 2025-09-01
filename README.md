Todo Assessment Project
Kaveendra Yapaarachchi

This is a full stack Todo application built with:

- Backend: ASP.NET Core Web API (.NET 8, EF Core, MSSQL)
- Frontend: Angular 18
- Database: Microsoft SQL Server
- Containerization: Docker & Docker Compose

 Project Structure
todo-assessment/
backend/
 |
  TodoApi/ ASP.NET Core Web API

frontend/
 |
 todo-frontend/ Angular frontend

docker-compose.yml # Docker Compose configuration
README.md


 Setup Instructions

 1. Clone the repository
git clone <your-repo-url>
cd todo-assessment
2. Configure the backend
Navigate to the backend:
cd backend/TodoApi
Update appsettings.json if needed:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=todo_mssql;Database=TodoDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
}

Apply EF Core migrations :
dotnet ef database update
Run locally:
dotnet run
API will be available at: http://localhost:5000


3. Configure the frontend
Navigate to the frontend:

cd frontend/todo-frontend
Install dependencies:
npm install
Update API base URL in environment.ts:

export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
Run locally:
ng serve
Frontend will be available at: http://localhost:4200

4. Run with Docker Compose
From the project root:
docker-compose up --build
This will start:

SQL Server (container: todo_mssql)

Backend API (container: todo_api)

Access:
API → http://localhost:5000
Frontend → http://localhost:4200 

<img width="1821" height="617" alt="image" src="https://github.com/user-attachments/assets/e8a9df9b-41cd-4567-9455-b65055c5df5c" />

Testing
Backend tests
cd backend/TodoApi
dotnet test
Frontend tests
cd frontend/todo-frontend
ng test



