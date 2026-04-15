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
