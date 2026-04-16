# Group10_iPERMITAPP

## Overview
This project is an implementation of the iPERMIT system for **CS 4320/7320 – Software Engineering I**.  
The system allows **Regulated Entities (RE)** to apply for environmental permits and enables **Environmental Officers (EO)** to review, approve/reject, and issue permits.

The implementation follows an **ASP.NET MVC architecture** and uses **SQL Server** for data storage.

---

## Project Structure

```text
Group10_Project/
├── Group10_Project.sln                 # Visual Studio solution file
├── README.md                           # Project documentation
├── dbsetup.sql                         # Database creation script
├── Group10_Values.sql                  # Seed/sample data

├── Group10_Project/                    # Main ASP.NET MVC Application
│   ├── App_Start/                      # MVC configuration (routing, bundles, filters)
│   │   ├── BundleConfig.cs
│   │   ├── FilterConfig.cs
│   │   └── RouteConfig.cs
│
│   ├── Controllers/                    # Control layer (business logic)
│   │   ├── REsController.cs
│   │   ├── RESitesController.cs
│   │   ├── PermitRequestsController.cs
│   │   ├── EnvironmentalPermitController.cs
│   │   ├── PaymentsController.cs
│   │   ├── DecisionsController.cs
│   │   ├── PermitController.cs
│   │   ├── EOController.cs
│   │   ├── OPS_CPPController.cs
│   │   ├── EmailArchiveController.cs
│   │   ├── RequestStatusController.cs
│   │   └── HomeController.cs
│
│   ├── Models/                         # Entity + EF data models
│   │   ├── RE.cs
│   │   ├── RESite.cs
│   │   ├── PermitRequest.cs
│   │   ├── EnvironmentalPermit.cs
│   │   ├── EO.cs
│   │   ├── Decision.cs
│   │   ├── RequestStatus.cs
│   │   ├── EmailArchive.cs
│   │   ├── OPS_CPP.cs
│   │   ├── Payment.cs
│   │   ├── Permit.cs
│   │   ├── REDashboardViewModel.cs
│   │
│   │   └── Entity Framework Files:
│   │       ├── Model1.edmx
│   │       ├── Model1.Context.cs
│   │       ├── Model1.Designer.cs
│   │       └── Model1.tt
│
│   ├── Views/                          # UI layer (Razor views)
│   │   ├── REs/
│   │   ├── RESites/
│   │   ├── PermitRequests/
│   │   ├── EnvironmentalPermit/
│   │   ├── Payments/
│   │   ├── Decisions/
│   │   ├── Permit/
│   │   ├── EO/
│   │   ├── OPS_CPP/
│   │   ├── EmailArchive/
│   │   ├── RequestStatus/
│   │   ├── Home/
│   │   └── Shared/
│
│   ├── Content/                        # CSS / styling
│   ├── Scripts/                        # JavaScript libraries (jQuery, Bootstrap)
│   ├── App_Data/                       # Local database storage (if used)
│
│   ├── Global.asax                     # Application entry point
│   ├── Web.config                      # App configuration
│   └── Group10_Project.csproj          # Project file

└── packages/                           # NuGet dependencies (Entity Framework, MVC, etc.)
```

## Project Requirements

Install the following 
- **Visual Studio 2022** 
  - Then import the **.vsconfig** loacted in the repo:
    - modify --> import configuration 
- **Microsoft SQL Server 2022**
- **SQL Server Management Studio (SSMS)**

## Database setup
1. Open SSMS
2. Connect to local SQL Server instance
3. Open: **dbsetup.sql**
4. Execute the script

This creates:
- DB Schema
- Required Tables
- Initial Data

## Project Setup
1. Open VS 2022 
2. Use "Clone a repository" to clone and open the project
3. Set startup project:
  - Right-click Group10_Project
  - Select Set as Startup Project
4.  At the top of VS, select the **Tools** dropdown and click **Connect to Database...**
5. You should see the Server Explorer window pop up on the far left side
6. Under **Data Connections**, there should be a DB named **Group10_DBEntities (Group10_Project)** to connect to
7. Finally, at the top of VS, select the **Build** tab and build the solution
5. You can then run the project






























