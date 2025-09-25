# EducationPortal

A comprehensive learning management system built with ASP.NET Core MVC that enables users to create, manage, and take courses with various learning materials.

## 🚀 Features

### Core Functionality
- **Course Management**: Create, edit, and manage educational courses
- **User Authentication**: Secure user registration and login with ASP.NET Core Identity
- **Role-based Access Control**: Admin and regular user roles with different permissions
- **Learning Materials**: Support for multiple material types (Books, Videos, Articles)
- **Skill Tracking**: Track and manage user skills and course completion
- **Progress Monitoring**: Real-time course progress tracking and completion status
- **Dashboard**: Personalized user dashboard with course statistics

### Material Types
- **Books**: Author, page count, format, publication date
- **Videos**: Duration, quality settings
- **Articles**: Publication date, source/resource information

### User Experience
- **Course Enrollment**: Easy course enrollment for students
- **Progress Tracking**: Visual progress indicators and completion percentages
- **Material Completion**: Mark materials as completed to track progress
- **Skill Acquisition**: Automatic skill assignment upon course completion

## 🏗️ Architecture

The project follows a clean architecture pattern with clear separation of concerns:

### Project Structure
```
EducationPortal.App/
├── EducationPortal.Data/          # Data Access Layer
│   ├── Models/                    # Entity models and DbContext
│   └── Migrations/               # Entity Framework migrations
├── EducationPortal.Logic/         # Business Logic Layer
│   ├── Services/                 # Business logic services
│   ├── Interfaces/               # Service interfaces
│   └── DTOs/                     # Data Transfer Objects
├── EducationPortal.Repo/          # Repository Pattern
│   └── Repositories/              # Data access repositories
├── EducationPortal.WebMVC/        # Presentation Layer
│   ├── Controllers/              # MVC Controllers
│   ├── Views/                    # Razor views
│   └── Models/                   # View models
└── EducationaPortal.Logic.Tests/   # Unit Tests
```

### Key Components

#### Data Layer
- **Entity Framework Core** with SQLite database
- **Identity Framework** for user management
- **Repository Pattern** for data access abstraction
- **Unit of Work** pattern for transaction management

#### Business Logic Layer
- **Service Layer** with dependency injection
- **DTO Pattern** for data transfer
- **Interface-based design** for testability

#### Presentation Layer
- **ASP.NET Core MVC** with Razor views
- **Bootstrap** for responsive UI
- **AJAX** for dynamic content loading
- **Role-based authorization**

## 🛠️ Technology Stack

- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core MVC** - Web application framework
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight database
- **ASP.NET Core Identity** - Authentication and authorization
- **Bootstrap 5** - Frontend CSS framework
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework for tests

## 📋 Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd educationportal/EducationPortal.App
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Update Database
```bash
dotnet ef database update --project EducationPortal.Data --startup-project EducationPortal.WebMVC
```

### 4. Run the Application
```bash
dotnet run --project EducationPortal.WebMVC
```

The application will be available at `https://localhost:5195`

## 🗄️ Database Schema

### Core Entities
- **Users**: User accounts with authentication
- **Courses**: Educational courses with descriptions
- **Skills**: Skills that can be learned
- **Materials**: Learning materials (Books, Videos, Articles)
- **UserCourses**: Many-to-many relationship for course enrollment
- **UserSkills**: User skill tracking
- **UserMaterials**: Material completion tracking

### Key Relationships
- Users can create multiple courses
- Courses contain multiple materials and skills
- Users can enroll in multiple courses
- Users can complete materials and acquire skills

## 🧪 Testing

The project includes comprehensive unit tests covering:
- Service layer functionality
- Repository operations
- Business logic validation
- Data access patterns

### Running Tests
```bash
dotnet test
```

## 🔧 Configuration

### Database Connection
The application uses SQLite by default. Connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=EducationPortal.db;Cache=Shared"
  }
}
```

### Identity Configuration
- Password requirements are configured for development
- Email confirmation is disabled for easier testing
- Admin user is automatically created on first run

## 📱 User Interface

### Key Pages
- **Home/Dashboard**: User overview with enrolled courses and progress
- **Courses Index**: Browse all available courses
- **Course Details**: View course content, materials, and skills
- **Course Creation**: Create new courses with materials and skills
- **Course Editing**: Modify existing courses (creator/admin only)

### Responsive Design
- Mobile-friendly Bootstrap layout
- Dynamic content loading with AJAX
- Progress indicators and completion tracking
- Material type-specific detail views

## 🔐 Security Features

- **Authentication**: ASP.NET Core Identity
- **Authorization**: Role-based access control
- **Data Protection**: Built-in data protection keys
- **CSRF Protection**: Anti-forgery tokens
- **Input Validation**: Model validation and sanitization

## 🚀 Deployment

### Development
```bash
dotnet run --project EducationPortal.WebMVC --environment Development
```

### Production
1. Update connection strings for production database
2. Configure HTTPS certificates
3. Set up proper logging
4. Deploy to your preferred hosting platform

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## 📄 License

This project is part of a learning exercise and is available for educational purposes.

## 🎯 Future Enhancements

- [ ] Course categories and tags
- [ ] Advanced search and filtering
- [ ] Course ratings and reviews
- [ ] Certificate generation
- [ ] Email notifications
- [ ] Video streaming capabilities
- [ ] Advanced analytics and reporting

## 📞 Support

For questions or support, please open an issue in the repository.

---

**Built with ❤️ using ASP.NET Core and Entity Framework**
