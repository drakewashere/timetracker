# User Stories

### Assumptions made
- Times set for start and end of a shift, break, or lunch should be the current time instead of allowing user input.
- Times cannot be edited or deleted except by admins
- Timestamp deletes will delete the entire shift, break, or lunch, and not just the start or end time
- Users are signed in for any authentication-required tasks

## Viewing user dashboard
A user goes to their home page and views their time entered in some form of graphic or table that displays start times, end times, breaks, and lunches.
They would like to see see a weekly/biweekly/monthly summary based on payroll, but is not needed.

## Entering a work shift
A user goes to their time entry form, and if there is no active work shift, is able to start a new work shift with the current time as the start time. 
At the end of their time working, they are able to stop their current workshift with the current time as their end time.

## Entering a break or lunch
A user is currently in an active work shift.
They go to the time entry form and add a new lunch or break time slot with a start time of the current time.
After they complete their lunch or break, they are able to end it with a timestamp of the current time

## Registering a user
A user enters their company id and desired password into a registration form.
The values are saved, and they are now able to log in and track their time

# Admin User Stories

## Viewing employee report
An admin sees a report of all hours entered by all employees.
They would like this report to be filterable; any filter is a good filter.

## Adding employee timestamp
An admin loads the report for a specific employee, and is can create a new shift, break or lunch entry with no restrictions.

## Editing employee timestamp
An admin loads the report for a specific employee, and can end the start and end times of a shift, break, or lunch with no restrictions.

## Deleting employee timestamp
An admin loads the report for a specific employee, and can delete the entire timestamp entry of a shift, break, or lunch with no restrictions

# Unauthorized User Stories

## Logging in successfully
A user goes to the site landing page, and is able to enter their employee id and passwork.
They are taken to their homepage.

## Logging in unsuccessfully
A user goes to the site landing page, and enters either an invalid employee id and password combination, or submits missing one or both entries.
They are displayed an error that their login was unsuccessful, and are not taken to an employee homepage.