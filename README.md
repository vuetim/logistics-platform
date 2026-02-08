#  Logistics Platform

## **Enterprise Transportation Management System (TMS)**

A **production oriented Logistics & Transportation Management Platform** built with **ASP.NET Core (.NET) + Angular**, designed using **Clean Architecture, Domain-Driven Design (DDD), and scalable enterprise patterns**.

This is **not a demo or tutorial project**.  
It is structured as a **real, deployable system** that can be used by logistics companies to manage operations, customers, shipments, and financial workflows.

---

# ** Current System Status**

## **Backend**
 Fully implemented core domain  
 Production-ready architecture  
 Orders, Loads, Carriers logic completed  
 Requires minor refactors & feature additions only  

## **Frontend**
 Actively being built  
 Core screens available  
 Additional modules being added incrementally  

---

# ** Implemented Capabilities**

## ** Authentication & Security**
- JWT authentication
- Refresh tokens
- Role-based authorization
- Permissions system
- Secure APIs
- Audit logs

## ** User & Role Management**
- Users
- Roles
- Permissions
- Access control
- Activity tracking

## ** Customer Management**
- Full CRUD
- Contacts & billing data
- Credit limits
- Status tracking

## ** Logistics Domain (Backend)**
- Orders
- Loads / Shipments
- Carriers
- Relationships between entities
- Business rules & validations
- Cost structure foundation

## ** Architecture**
- Clean Architecture
- DDD aggregates
- CQRS (light)
- Repository pattern
- Modular structure
- Separation of concerns

---

# ** Architecture Overview**

## Backend – .NET

```
Domain
Application
Infrastructure
API
```

### Domain
- Entities
- Business rules
- Aggregates
- Value Objects

### Application
- DTOs
- Services
- Commands / Queries
- Interfaces

### Infrastructure
- EF Core
- Repositories
- External integrations

### API
- Controllers
- Auth
- Middlewares
- Swagger

---

## Frontend – Angular

```
src/app
 ├── core
 ├── shared
 ├── features
 └── ui
```

Designed for:
- reusable components
- scalable modules
- enterprise maintainability

---

## DevOps
- Docker
- Swagger
- GitHub

---

# ** Run Locally**

## Backend
```bash
dotnet restore
dotnet ef database update
dotnet run
```

Swagger:
```
https://localhost:5001/swagger
```

## Frontend
```bash
npm install
ng serve
```

App:
```
http://localhost:4200
```

---

# ** Roadmap (Next Enhancements)**

- Chat / internal notes
- Additional dashboards
- Reporting & analytics
- Financial workflows
- UI polishing
- Multi-tenant SaaS
- Real-time updates

---

# ** Project Goal**

Build a **real world enterprise logistics platform** that:

- replaces spreadsheets/manual workflows
- supports daily operational teams
- scales with company growth
- follows professional engineering standards
- can be deployed to production

Focus:
**correct architecture first → features second → scale third**

---

# **👨‍💻 Author**

**Shpetim**  
Full stack Developer (.NET + Angular)  
Specialized in enterprise & logistics systems

---

# **📄 License**

MIT
