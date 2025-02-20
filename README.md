# Hospital management program

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technical Stack](#technical-stack)

## Overview

Welcome to the Hospital Management Console Program, a powerful C# application designed for efficient hospital administration. Built with NHibernate ORM, local database storage, and a robust unit testing framework, this program offers a comprehensive set of features to simplify and enhance the management of hospital resources.

## Features

### Login
- **Login:** Allows users to log into the system.
- **Logout:** Enables users to log out of their current session.

### Manage Employees
- **Create Employee:** Adds a new employee to the system.
- **Delete Employee:** Removes an existing employee from the system.
- **Display Employees:** Shows a list of all employees in the system.

### Manage Patients
- **Assign to Doctor:** Associates a patient with a specific doctor.
- **Change Health Status:** Modifies the health status of a patient.
- **Create Patient:** Adds a new patient to the system.
- **Delete Patient:** Removes an existing patient from the system.
- **Display Patients:** Shows a list of all patients in the system.

### Manage Users
- **Change User Rank:** Adjusts the rank or role of a user in the system.
- **Create User:** Adds a new user to the system.
- **Delete User:** Removes an existing user from the system.
- **Display Users:** Shows a list of all users in the system.

### Manage Wards
- **Change Ward Owner:** Updates the owner of a specific ward.
- **Create Ward:** Adds a new ward to the system.
- **Delete Ward:** Removes an existing ward from the system.
- **Display Wards:** Shows a list of all wards in the system.

## Technical Stack

### Database
- **Local Database:** All data is stored in a local database, which is automatically created during the program's first startup.

### ORM Integration
- **NHibernate ORM Integration:** Leverage the capabilities of NHibernate ORM for seamless and organized data management.

### Navigation Function
- **Console GUI Menu:** Provides a user-friendly console GUI menu that allows navigation using arrows and the enter button.

### Error Logger
- **Error Logging:** Records all errors into a .txt file. The error log file is created in the same way as the database file, providing a record of program issues for debugging and analysis.
