# PSHOP

Project Overview

**PSHOP** is a desktop-based **Store Management System** built with **C# and .NET Windows Forms**.  
It provides an integrated platform for managing day-to-day store operations such as **sales, purchases, inventory tracking, and basic accounting**.  

The project demonstrates practical implementation of data handling, form design, dataset usage, and modular business logic — making it a strong portfolio example for desktop software development.
The system is also designed to integrate with a broader ERP (Enterprise Resource Planning) platform, allowing seamless data exchange between the store management module and other enterprise components such as accounting, HR, and logistics.
Key Features

- **Sales Management:** Create, edit, and track sales invoices.  
- **Purchase Management:** Record incoming stock and purchase requests.  
- **Inventory Management:** Monitor product quantities and stock levels.  
- **Accounting Integration:** Handle basic accounting and financial records.  
- **User Interface:** Built using Windows Forms with interactive forms and dataset bindings.  
- **Reports:** Generate detailed reports for transactions and stock.  
- **Modular Design:** Separate modules for sales, warehouse, accounts, and base data.

---

Project Architecture

The system follows a modular structure:
- `MainForm` acts as the central dashboard.
- Each business area (Sales, Purchase, Warehouse, Accounting) has its own form and logic layer.
- Data access is handled via **typed DataSets (.xsd)**, ensuring strong type safety and structured queries.
- Resources and icons are managed within the application for consistent UI design.

**Main Modules:**
| Module | Description |
| **BASE** | Core configuration, product info, and shared data. |
| **SALE** | Handles customer sales, invoicing, and related transactions. |
| **ACNT** | Accounting and financial tracking. |
| **WHRS** | Warehouse / inventory control. |
| **MAIN** | Central coordination and navigation hub. |


Technologies Used

- **Language:** C#  
- **Framework:** .NET Framework (Windows Forms)  
- **Data Management:** DataSet (.xsd, .xsc, .xss)  
- **Database:** SQL Server (LocalDB / Express)  
- **IDE:** Microsoft Visual Studio  
- **UI Components:** WinForms Designer, custom icons and resources



