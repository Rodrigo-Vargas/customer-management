## Getting Started

1. Clone the repo
```sh
   git clone https://github.com/Rodrigo-Vargas/customer-management.git
```
2. Run the command to npm install packages
```sh
  npm install
```


## User Stories
As a user, I should login into the system with my email and password

## Tools
.NET Core

## 1. Solution to be implemented
A small business of sales is looking for a system to manage the customer contact. The system
should show a login page every time a user try to access the site. In this page, the sellers should be
identified using the email account of the company and his/her password in the system. After a successfully
login, the sellers must see and search for some data and classification (VIP, Regular and Sporadic) of

his/her clients and plan his/her future sales with the information. Currently, the company operates only
in Porto Alegre, but in the future, there is an expansion plan to other cities. For a better administration
and system control, the company would like a user with full access to contacts of all sellers.
2. Acceptance Criteria
2.1. Login Page (See Figure 1)
• The login page should be displayed whenever a user tries to access the system.
• The password must be encrypted with MD5;
• If a user types a wrong e-mail and/or password, the system must show the message “The email
and/or password entered is invalid. Please try again.” and the fields should be displayed with a
red border.
• When a user accesses