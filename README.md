# HospitalProject
There are 10 entities in this Hospital Project. 

## Contribution
### Departments - by 
### Staff - by
### Programs - by Michelle Parlevliet
- Full CRUD 
- 1-M relationship to Departments enitity
- Views (List, Details, Update, Add, Delete)
### Services - by Michelle Parlevliet
- Full CRUD 
- 1-M relationship to Programs entity
- Views (List, Details, Update, Add, Delete)
### Appointments - by 
### Patients - by  
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

