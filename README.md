# Whale Spotting

Welcome to Whale Spotting! Whale Spotting is a React frontend and C# ASP.NET backend application.


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

It will run a file watcher and update when files are changed.

### Running the frontend

Navigate to the `frontend` folder and run:

```
npm start
```

It will run a file watcher and update when files are changed. Stop it with Ctrl+C.
