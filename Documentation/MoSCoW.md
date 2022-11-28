# Must Should Could Won't

## Must
- [X] Provide an API backend for creating units of time worked
- [X] Provide an API backend for creating units of time on break
- [X] Provide an API backend for creating units of time on lunch (must be differentiated from break)
- [X] Provide a UI front-end with the ability to start a working time block
- [X] Provide a UI front-end with the ability to end a working time block
- [X] Provide a UI front-end with the ability to start a break
- [X] Provide a UI front-end with the aibility to end a break
- [X] Provide a UI front-end with the ability to start a lunch
- [X] Provide a UI front-end with the ability to end a lunch
- [X] Provide a UI dashboard for the time entered for the emloyee

## Should
- [X] Only allow a user with a valid employee ID to enter work for themselvelves
- [x] Only allow a user with a valid employee ID to enter a break for themselvelves
- [X] Only allow a user with a valid employee ID to enter a lunch for themselvelves
- [X] Only allow a user with a valid employee ID to view their time dashboard
- [X] Not allow a user to start a shift while there is still an open shift
- [X] Not allow a user to start a break when they are not on an active shift
- [X] Not allow a user to start a lunch when they are not on an active shift
- [X] Not allow a user to start a break when they have an open break
- [X] Not allow a user to start a lunch when they have an open break

## Should (assumptions based on domain knowledge)
- [X] Not allow a user to start a break when they have an open lunch
- [X] Not allow a user to start a lunch when they have an open lunch

## Could
- [X] Allow users to register themselves
- [X] Create two account types, the new one being an admin account
- [X] Allow admin accounts the ability to view the time for any other employee
- [ ] Allow admin accounts the ability to add, edit, and delete timestamps (work, break, lunch) with no restrictions
- - [X] Add, edit shifts
- - [X] Add breaks
- - [ ] Edit breaks
- - [ ] Delete timestamps
- [X] Allow admin accounts the ability to view a report for all employees time sheets
- [X] Allow admin to filter the report of all employees time sheets
-- (No default filters given by stakeholder)
-- (No specification if this should be custom query creation, or fixed filters)

## Could (assumptions based on domain knowledge)
- [X] Allow setting passwords for users
- [X] Apply basic security principals to password storage
- [X] Data persistence of timesheets accross application instances
- [X] Store which admin edited a time stamp
- [ ] Store which admin deleted a time stamp
- [ ] Custom timelines for user dashboard - show weekly, biweekly, monthly
- [X] Provide a user-friendly failure mode if the employee ID is not registered, or none is entered

## Won't (not enough time, unclear requirements)
- Allow a user to enter a custom timestamp for the start or end of a shift, break, or lunch
- Allow editing or deleting to time data by a non-admin user
- Store an audit history of old values for edited or deleted timestamps
- Show a report of disputed timestamps altered by an admin