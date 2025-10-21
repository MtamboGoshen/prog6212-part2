# Monthly Claim System (PROG6212 - Part 2)

This project implements a web-based Contract Monthly Claim System, built using **ASP.NET Core MVC** with **Entity Framework Core (SQLite)**. It includes robust features for lecturers to submit claims with supporting documents, and for Programme Coordinators and Academic Managers to review, approve, or reject these claims through a secure, role-based workflow.

---

## 🚀 Features Implemented (Part 2 Requirements)

This application has been developed to meet the following key functionalities:

### 1. Lecturer Claim Submission
* [cite_start]**Intuitive Form:** Lecturers can easily submit claims through a simple and intuitive form, including fields for lecturer name, programme, month, hours worked, hourly rate, and additional notes[cite: 13].
* [cite_start]**File Upload:** Implemented a feature that allows lecturers to upload supporting documents for their claims[cite: 14].
    * [cite_start]Uploaded files are securely linked to the corresponding claim[cite: 15].
    * [cite_start]Includes an "Upload" button on the submission form[cite: 17].
    * [cite_start]**Server-Side File Validation:** Files are restricted to `.pdf`, `.docx`, and `.xlsx` formats, with a size limit of 5MB[cite: 19]. [cite_start]Appropriate error messages are displayed for invalid files[cite: 21].
    * [cite_start]**Secure Storage:** All uploaded documents are **encrypted** using AES-256 before being stored on the server, ensuring data confidentiality[cite: 16]. Decryption occurs dynamically when viewed by authorized users.

### 2. Administrator (Coordinator/Manager) Claim Verification & Approval
* [cite_start]**Role-Based Access:** A dedicated "Approver Queue" view is designed for Programme Coordinators and Academic Managers to verify and approve claims[cite: 27, 31].
* [cite_start]**Pending Claims Display:** This view clearly displays all pending claims with necessary details for verification, including a secure link to view the supporting documentation[cite: 28, 29, 32, 33].
* [cite_start]**Action Buttons:** Each pending claim includes prominent "Approve" and "Reject" buttons for easy processing[cite: 30, 34].

### 3. Claim Status Tracking
* [cite_start]**Transparent Workflow:** The system implements a tracking mechanism that updates the status of each claim as it moves through the approval process[cite: 23].
* [cite_start]**Visual Status Indicator:** Claim status is represented using a clear, color-coded progress bar (Pending, Approved, Rejected) for easy at-a-glance tracking[cite: 24].
* [cite_start]**Real-time Updates:** The status updates automatically and in real-time when a Coordinator or Manager approves or rejects a claim, reflecting immediately in the lecturer's view[cite: 25].

### 4. Consistent & Reliable Information
* [cite_start]**Unit Testing:** A suite of **5+ unit tests** has been written for the `ClaimService` to ensure the core functionalities (add, get, update status, delete, get by ID) are robust and consistent[cite: 36].
* [cite_start]**Graceful Error Handling:** The application includes error handling mechanisms, particularly a custom "Resource Not Found" page for claims that do not exist, providing meaningful user messages[cite: 37].

---

## 🛠️ Technologies Used

* **ASP.NET Core 9.0 MVC**
* **Entity Framework Core**
* **SQLite** (for database storage)
* **ASP.NET Core Identity** (for authentication and authorization)
* **Bootstrap 5.3.3** (for responsive UI design)
* **xUnit** (for unit testing)
* **Moq** (for mocking in unit tests)
* **AES-256 Encryption** (for document security)

---

## 🏃 How to Run the Project

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/MtamboGoshen/prog6212-part2.git](https://github.com/MtamboGoshen/prog6212-part2.git)
    cd prog6212-part2/ContractMonthlyClaim
    ```
2.  **Open in Visual Studio:** Open the `ContractMonthlyClaim.sln` file in Visual Studio 2022.
3.  **Restore NuGet Packages:** Visual Studio should automatically restore packages. If not, run:
    ```bash
    dotnet restore
    ```
4.  **Update Database Migrations:** The project uses SQLite, so the database file (`students.db`) will be created automatically on first run via migrations and seeding. To ensure your database is up-to-date, run:
    ```bash
    dotnet ef database update
    ```
5.  **Run the Application:** Press `F5` in Visual Studio or run `dotnet run` from the `ContractMonthlyClaim` directory in your terminal. The application will launch in your browser and redirect to the login page.

---

## 🔑 Test Accounts

Use the following credentials to test the different roles:

* **Academic Manager:**
    * Username/Email: `manager@test.com`
    * Password: `Password123`
* **Programme Coordinator:**
    * Username/Email: `coordinator@test.com`
    * Password: `Password123`
* **Lecturer:**
    * Username/Email: `lecturer@test.com`
    * Password: `Password123`

---

## 🎥 Project Demonstration Video

[https://youtu.be/078JbbAy8Uk](https://youtu.be/078JbbAy8Uk)

---

## 🔗 GitHub Repository

[https://github.com/MtamboGoshen/prog6212-part2.git](https://github.com/MtamboGoshen/prog6212-part2.git)

---

## 📜 Version Control

[cite_start]This project has been developed using Git for version control, with a clear and descriptive commit history tracking all major feature implementations and bug fixes[cite: 39]. The repository contains over 10 commits, adhering to best practices.

---

## 📝 Lecturer Feedback & Documentation

[cite_start]This documentation reflects the implementation of functionalities based on the requirements for Part 2 and incorporates feedback for a robust and user-friendly application[cite: 41].
