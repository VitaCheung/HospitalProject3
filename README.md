# HospitalProject
There are 10 entities in this Hospital Project, which aims to simulate the real structure of a Hospital website. Our team used the [Women's College Hospital](https://www.womenscollegehospital.ca/) as reference when building our entity framework.

## Updates
Chnages to the project since the MVP include completed views and CRUD functionality through the views. We have also added some page styling and made changes to the navigation bar for easier routing to list pages.

## Future Improvements
In the future, there are a few things our team would add/change about the project's current state:
1. Naming convention of Entities - our entities were all named using plural formats which led to confusions/inconsistencies in syntax later on, for example when it came to ViewModels like "UpdateDepartments" which should have been "UpdateDepartment", since only one thing is being updated. We decided not to chnage this for our final project because although using the plural form does not make as much sense, we had all been consistent with using plurals so across the project it was succinct.
2. Improving functionality - some CRUD functions still need attention in resolving errors across their relationships, namely edit and add functions.
3. Authentication - ???
4. Page styling - ???

## Contribution
### Departments - by 
### Staff - by
### Programs - by Michelle Parlevliet
- Full CRUD 
- 1-M relationship (Department to Programs)
- Views (List, Details, Update, Add, Delete)
### Services - by Michelle Parlevliet
- Full CRUD 
- 1-M relationship (Programs to Services)
- Views (List, Details, Update, Add, Delete)
### Appointments - by Carrie Ng
- Full CRUD
- 1-M relatioship to Patients Entity
- 1-M relationship to Staffs Entity
- Views for List Details, Update, Add, and Delete
### Patients - by Carrie Ng
- Full CRUD
- Views for List Details, Update, Add, and Delete
### Careers - by Tin Wai Cheung 
- Full CRUD and views
- Administrator functionality (Only Admin can update and delete)
- List Careers for department (M-1 relationship)
### Volunteers - by Tin Wai Cheung
- Full CRUD and views
- Role-based authorization (Guest can only view his/her record details, Admin can create, update and delete all records)
- List Volunteers for program (M-1 relationship)
### Donations - by 
### FAQ - by 

## Running this project
1. Project > HospitalProject2 Properties > Change target framework to 4.7.1 -> Change back to 4.7.2
2. Create a 'App_Data' folder in the project if there is not one (Right click solution > View in File Explorer)
3. Tools > Nuget Package Manager > Package Manage Console > Update-Database
4. Check that the database is created using (View > SQL Server Object Explorer > MSSQLLocalDb > ..)

