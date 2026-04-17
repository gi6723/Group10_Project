# Group10_iPERMITAPP

## Overview

This project is an implementation of the iPERMIT system for **CS 4320/7320 – Software Engineering I**.

The system allows:

* **Regulated Entities (RE)** to apply for environmental permits
* **Environmental Officers (EO)** to review, approve/reject, and issue permits

The application follows an **ASP.NET MVC architecture** and uses **SQL Server** for data storage.

---

## Project Structure

```text
Group10_Project/
├── Group10_Project.sln
├── README.md
├── dbsetup.sql
├── Group10_Values.sql
├── Group10_Project/
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── App_Start/
│   ├── Content/
│   ├── Scripts/
│   ├── App_Data/
│   ├── Global.asax
│   ├── Web.config
│   └── Group10_Project.csproj
```

---

## System Requirements

Install:

* **Visual Studio 2022 Community Edition**

  * Workloads:

    * ASP.NET and Web Development
    * .NET Desktop Development
  * Import provided `.vsconfig` (optional)

* **Microsoft SQL Server 2022**

* **SQL Server Management Studio (SSMS)**

---

## Database Setup

1. Open SSMS
2. Connect to your local SQL Server instance
3. Open `dbsetup.sql`
4. Execute the script

This creates:

* Database schema
* Tables
* Initial seed data

---

## Running the Application

1. Open Visual Studio 2022
2. Clone the repository
3. Open the solution file
4. Set startup project:

   * Right-click `Group10_Project`
   * Click **Set as Startup Project**
5. Build the solution
6. Click **Run (IIS Express)**

---

## Default Credentials

### Environmental Officer (EO)

* ID: (as defined in seed data)
* Password: `password`

Password can be changed using the **Change Password** feature.

---

## RE Account

* Register a new account using the application
* Login after registration

---

## Email System

Email functionality is **simulated**.

All system-generated emails (payment confirmation, decision, permit issuance) are:

* Logged in the `EmailArchive` table
* Used to represent system notification behavior

---

## Executable / How to Run

To run the system:

* Open the solution in Visual Studio
* Click **Run (IIS Express)**

---

## Notes

* The system implements the full iPERMIT lifecycle:

  * Request → Payment → Review → Decision → Permit Issuance
* Status transitions follow the defined state chart:

  * Pending Payment → Submitted → Being Reviewed → Accepted/Rejected → Permit Issued

---
