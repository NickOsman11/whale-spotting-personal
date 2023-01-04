# Whale Spotting

Welcome to Whale Spotting! Whale Spotting is a React frontend and C# ASP.NET backend application.

Recently we have finally seen an increase in whale numbers across the globe due to conservation efforts, and we would love to keep the public and scientific community engaged with these efforts. We have been tasked with creating a new website to help encourage and track whale spotting. Inspired by the Washington Whale Museum who have been tracking whale sightings in the Salish Sea but has recently been getting less traffic, we have been hired to create a new website that integrates with their API to grab their current sightings data and expand it to cover the whole world!

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

See the below instructions for running the database - the first time you run it will download the official PostgreSQL Docker image and set up the database for the first time, which takes a little while!

## Project structure

The project consists of three separate services: the **database**, running inside a Docker container; the **API**, a C# ASP.NET app; and the **frontend**, a React single-page application.

The database runs through `docker-compose` in a Docker container with a persistent storage volume.

The backend is an ASP.NET web API, and exposes a set of API endpoints to be used by the frontent.

The frontend is written in Typescript React, using [React Router](https://reactrouter.com/) for routing.

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
```

Navigate to the `backend` folder and run:

```
dotnet watch run
```

It will run a file watcher and update when files are changed. Stop it with Ctrl+C.

### Running the frontend

Navigate to the `frontend` folder and run:

```
npm start
```

It will run a file watcher and update when files are changed. Stop it with Ctrl+C.

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
