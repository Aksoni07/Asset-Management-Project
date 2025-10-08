# Asset Management Application

This is a Blazor Server application for managing company assets and their assignment to employees, built as part of the Sciforn Offcampus Hiring Assessment.

## Technology Stack
-   **Framework:** ASP.NET Core Blazor Server
-   **ORM:** Entity Framework Core
-   **Micro ORM:** Dapper
-   **Database:** Microsoft SQL Server

## How to Set Up and Run

1.  **Configure Connection String:** Open `AssetManagement.UI/appsettings.json` and set the `DefaultConnection` string to point to your local SQL Server instance.
2.  **Apply Migrations:** Open a terminal in the `AssetManagement.UI` folder and run the command `dotnet ef database update --project ../AssetManagement.DataAccess` to create the database.
3.  **Run the Application:** Run the command `dotnet run` from the `AssetManagement.UI` folder.
4.  **Access the App:** Open your browser and navigate to the localhost URL provided in the terminal.

## Admin Credentials

* **Username:** `admin@abc.com`
* **Password:** `Password123`