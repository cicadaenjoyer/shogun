# Shogun

Shogun is a self-hosted media streaming platform. Owners run a **Shogun Server** on their own hardware to organize and stream their personal movie, TV, and music libraries, then invite a small group of friends and family to watch and listen with them. Accounts, groups, and chat are handled by a shared cloud service, while media always streams directly from the owner's server to the viewer.

> **Status:** early development. The server's foundation is in place (database schema, settings, logging, a health endpoint, tests, and CI); most features below are planned and not yet implemented.

---

## Features

- ~~**Personal media libraries:** movies, TV shows, and music, served from the owner's own storage~~
- ~~**Automatic metadata:** titles, artwork, and details matched from TMDB and MusicBrainz~~
- ~~**Direct play and transcoding:** files play as-is when the device supports them, with hardware-accelerated transcoding (via ffmpeg) when it doesn't~~
- ~~**Private groups:** owners invite members and control what each member can access~~
- ~~**Real-time social features:** group chat and live server status~~
- ~~**Mobile-first web app:** one app that works in desktop, tablet, and phone browsers~~

## Architecture

Shogun is made up of three components:

| Component         | Responsibility                                                                  | Technology                                                 |
| ----------------- | ------------------------------------------------------------------------------- | ---------------------------------------------------------- |
| **Shogun Hub**    | Accounts, groups, invites, server directory, server status, chat, notifications | Firebase (Auth, Cloud Firestore, Cloud Functions, Hosting) |
| **Shogun Server** | Libraries, scanning, metadata, streaming, watch progress, member permissions    | C#, ASP.NET Core, Entity Framework Core, SQLite, ffmpeg    |
| **Shogun Web**    | The primary client application                                                  | TypeScript, React, Vite                                    |

```mermaid
flowchart LR
    Web["Shogun Web<br/>(browser)"]
    Hub["Shogun Hub<br/>(Firebase)"]
    Server["Shogun Server<br/>(owner's hardware)"]

    Web -- "sign-in, groups, chat,<br/>server passes" --> Hub
    Server -- "registration,<br/>heartbeats, member list" --> Hub
    Web -- "API requests and<br/>media streams" --> Server
```

Key design principles:

- **Media never passes through the Hub.** Video, audio, and library data travel directly between a Server and the client.
- **Servers never see user credentials.** Firebase Auth manages sign-in; clients present a short-lived, server-specific pass signed by the Hub, which the Server verifies before granting access.
- **Servers stay usable offline.** Each Server keeps a local copy of its member list and permissions, so it continues working if the Hub is unreachable.

Native mobile and TV apps are planned for the future and will live in separate repositories, using the same Hub and Server APIs.

## Repository Structure

This is a monorepo containing the server, cloud functions, and web client.

```text
shogun/
├── .github/workflows/         # CI: builds the server and runs its tests
├── backend/
│   ├── Shogun.slnx            # Solution: every .NET project below
│   ├── Makefile               # Shortcuts for build, run, tests, migrations
│   ├── Shogun.Server/         # Shogun Server (ASP.NET Core Web API)
│   │   ├── Controllers/       # HTTP endpoints
│   │   ├── DTO/               # Shapes of API requests and responses
│   │   ├── Services/          # Logic behind the endpoints
│   │   ├── Options/           # Typed settings, validated at startup
│   │   ├── Entities/          # Database entities (libraries, media, members)
│   │   ├── Data/              # EF Core DbContext, entity configurations, converters
│   │   └── Migrations/        # EF Core database migrations
│   └── Shogun.Server.Tests/   # Unit, integration, and database tests (xUnit)
├── firebase/                  # Shogun Hub: Cloud Functions and security rules (planned)
├── web/                       # Shogun Web: React + Vite client (planned)
├── shared/                    # Shared schemas and TypeScript types (planned)
└── pnpm-workspace.yaml        # Ties firebase/, web/, and shared/ together
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- The Entity Framework Core CLI, needed only when you change the database schema:

    ```bash
    dotnet tool install --global dotnet-ef
    ```

- `make` (optional, for the convenience commands below)

### Running the Server

1. Create your local settings file. From `backend/Shogun.Server/`:

    ```bash
    cp appsettings.Development.json.example appsettings.Development.json
    ```

    Open the new file and set the three `Storage` folders to absolute paths your user can write to. The Server creates them on first start. Without this file the Server falls back to system folders such as `/var/lib/shogun` and will not start for a normal user.

2. Build and run. From `backend/`:

    ```bash
    # Restore packages and build every project in the solution
    dotnet build

    # Start the server
    dotnet run --project Shogun.Server
    ```

On startup the Server creates its SQLite database (`shogun.db`) inside the data folder and applies any pending migrations, so there is no separate database step.

The API listens on `http://localhost:5265` by default. To confirm it is running, open `http://localhost:5265/api/v1/health`. In the Development environment, interactive API documentation is available at `http://localhost:5265/swagger`.

### Running the Tests

From `backend/`:

```bash
dotnet test
```

The tests use temporary folders and databases, so they never touch your local data. The same command runs on GitHub Actions for every pull request.

### Development Commands

A Makefile in `backend/` wraps common tasks:

| Command                             | Description                                |
| ----------------------------------- | ------------------------------------------ |
| `make`                              | Run the server                             |
| `make build`                        | Build every project in the solution        |
| `make test`                         | Run the tests                              |
| `make migrate NAME=<MigrationName>` | Create a new EF Core migration             |
| `make db-update`                    | Apply migrations without running the app   |
| `make db-test-tables`               | Print the SQL for the current schema       |
| `make jwt-create NAME=<username>`   | Create a development JWT for local testing |
| `make jwt-list`                     | List development JWTs                      |
| `make jwt-list-user ID=<token-id>`  | Show the details of a development JWT      |

### Configuration

Server settings are read from `appsettings.json`, then overridden by [.NET user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in development and environment variables in production. **Secrets must never be committed to the repository.** `appsettings.Development.json` and local database files are excluded by `.gitignore`.

Settings are checked when the Server starts, and a missing or invalid value stops it with a message naming the setting. The `Storage` section decides where the Server keeps its own files: the data folder (database and server ID, the one folder to back up), the image cache, and transcoding scratch space.

## Tech Stack

| Area             | Technologies                                                                   |
| ---------------- | ------------------------------------------------------------------------------ |
| Server           | C#, .NET 10, ASP.NET Core, Entity Framework Core, SQLite                       |
| Media processing | ffmpeg (with Intel Quick Sync hardware acceleration)                           |
| Cloud            | Firebase Auth, Cloud Firestore, Cloud Functions (TypeScript), Firebase Hosting |
| Web client       | TypeScript, React, Vite                                                        |

## License

Shogun is licensed under the [Apache License 2.0](LICENSE). See [NOTICE](NOTICE) for attribution.
