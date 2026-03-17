# Device Management System - Vanrise Internship

## Overview
This project is a web-based Device Management System developed as part of my internship at Vanrise. It is designed to securely track and manage equipment. The application features a responsive frontend, user authentication, and a robust C# backend integrated with a SQL Server database for persistent data storage.

## Features
## Features
## Features
- **Secure User Authentication:** A dedicated login portal that restricts system access to authorized personnel only.
- **Device Management Dashboard:** A comprehensive, interactive pages to oversee and track all equipment.
- **Full CRUD Functionality:** The ability to seamlessly add new devices, view current inventory details, update statuses, and remove obsolete records.
- **Responsive Modern UI:** An intuitive and aesthetically pleasing interface built with HTML, CSS, and JavaScript that adapts smoothly to different screen sizes.
- **Robust Relational Database:** Backend architecture powered by SQL Server, ensuring data integrity, reliable storage, and fast query retrieval.
- **Seamless Data Flow:** C# backend logic that efficiently handles client requests and bridges the frontend interface with the SQL database.

## Tech Stack
- **Frontend:** HTML5, CSS3, JavaScript
- **Backend:** C# (.NET)
- **Database:** SQL Server

## Getting Started

### Prerequisites
Before you begin, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) or an equivalent SQL editor
- Visual Studio or Visual Studio Code
- A modern web browser

### Installation & Setup

#### 1. Database Setup
1. Open SSMS and connect to your local SQL Server instance.
2. Navigate to the folder containing the database files in this repository.
3. Open and execute the SQL script to generate the necessary database, tables, and any initial seed data.

#### 2. Application Setup
1. Clone the repository to your local machine:
2. Open the project solution in your IDE.
3. Locate the configuration file ( `Web.config`) and update the database connection string to match your local SQL Server credentials.
4. Build the solution to restore any dependencies.
5. Run the application.

## Usage
Once the application is running, navigate to the local host URL provided by your IDE. 
You can use the following default credentials for initial testing:
- **Username:** `Admin`
- **Password:** `Password123`

## Acknowledgments
- Developed as an internship project at **Vanrise**.
