# Whale Spotting

Welcome to Whale Spotting! Whale Spotting is a React frontend and C# ASP.NET backend application. 
Note: new sightings must be approved by a logged-in admin, so once submitted they won't immediately appear on the Sightings page - you 
will need to create a new admin user, navigate to the Pending page, and approve them - see the `backend` section for instructions


## Project setup

Install the dependencies for the backend (in the `backend` folder) with:

```
dotnet restore
```

and in the frontend (in the `frontend` folder) with:

```
npm install
```

(if you get an error that mentions lockfile versions, upgrade npm with `npm install -g npm`)

## Running the project locally

To run the project locally, we need to start all three services separately (preferably in the following order).

### Running the database

Ensure you have PostgreSQL installed and running. Open up [pgAdmin](https://www.pgadmin.org/), and create a new login-enabled role with the name and password of your choice (that you will remember). Create a database whose owner is that new role, probably called `whale-spotting` or similar. Then, edit the file `database.env` to contain the information you just created!

### Running the backend

First, set the required environment variables for connecting to the database.

For local running in powershell, run:

```
$env:DATABASE_URL = "postgres://username:password@localhost:5432/whale-spotting"
$env:USE_SSL = "false"
$env:POSITION_STACK_KEY = "e0ab3cbcba64dba8579d1dc9a79f2c78"
```

Navigate to the `backend` folder and run:

```
dotnet ef database update
dotnet watch run
```

This will seed some exaple data in the database and run a file watcher. 

To check out the pages only accessible to admins, you will need to `dotnet watch run` - a swagger page will open automatically.
Click on the debug /post method and click the "Try it out" button. Edit the default json with the desired details - a valid email is 
required, and the password must contain letters, numbers and special characters. Click execute, and an admin role with those credentials 
will be created. 

### Running the frontend

Navigate to the `frontend` folder and run:

```
npm start
```

It will run a file watcher and update when files are changed.
