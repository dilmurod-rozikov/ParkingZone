arking Zone Management Application 🚗
🚀 Introduction
This web application manages parking slots through two portals: Admin and Client. It supports CRUD operations for parking zones and slots, and allows clients to reserve parking slots.

🌟 Features
Admin Portal
Manage parking zones and slots (CRUD)
View and filter parking zones and slots
Manage reservation history
View current cars in parking zones
Client Portal
Manage profile
List and reserve free slots
Update parking reservations

🛠️ Technologies Used
ASP.NET Web API
ASP.NET MVC
Entity Framework
SQL Server

📋 Requirements
.NET Framework 4.7.2 or later
SQL Server 2017 or later
📦 Installation
Clone the repository:

🧑‍💼 Admin Actions
Manage Parking Zones: CRUD operations
Manage Parking Slots: CRUD operations
Filter Slots: By zone, category, availability
View Current Cars: List all cars in a parking zone
Manage Reservations: Filter by car plate, slot, period

🧑‍ Client Actions
Profile Management: Edit name and phone number
List Free Slots: View available slots for a specific period
Create Reservation: Book a slot with car number, start time, duration
Update Reservation: Prolong booking duration

🏗️ Features Breakdown
Authentication & Authorization: Admin and Client roles
Layered Architecture: Repository, Service, ViewModels
Unit Tests: Services, Controllers (xUnit, Moq)
GitHub Actions: Continuous integration for tests
Reports for Admins: Generate various reports using jQuery and AJAX
