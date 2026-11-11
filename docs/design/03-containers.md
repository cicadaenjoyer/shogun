```mermaid
---
title: Shogun Container Diagram
---
flowchart TD
    B([Visitor])
    C([Account Holder])
    D([Server Owner])
    E([Member])
    F([Shogun Admin])

    subgraph browser ["User's browser"]
        WEB["Shogun Web<br/>(React, TypeScript)"]
    end

    subgraph google ["Google's cloud (Firebase project)"]
        HOST["Web Hosting<br/>(Firebase Hosting)"]
        FN["Hub Functions<br/>Cloud Functions, TypeScript"]
        FS[("Hub Database<br/>(Cloud Firestore)")]
        STORE[("Theme Images<br/>(Cloud Storage)")]
        AUTH[[Firebase Auth]]
        FCM[[Firebase Cloud Messaging]]
    end

    subgraph home ["Owner's hardware"]
        SRV["Shogun Server<br/>(ASP.NET Core, C#)"]
        DB[("Server Database<br/>(SQLite)")]
        FF["ffmpeg<br/>separate process"]
        MEDIA[("Owner's media folders")]
    end

    REMOTE[[Remote access network]]
    MB[[MusicBrainz]]
    TMDB[[TMDB]]

    %% ---------- People -> Shogun ----------
    B -->|"Signs up using"| WEB
    C -->|"Designs, shares, and installs themes using"| WEB
    D -->|"Claims servers, sets up libraries, and manages members using"| WEB
    E -->|"Browses, plays, comments, and chats using"| WEB
    F -->|"Moderates reported themes and accounts using"| WEB
    D -->|"Reads the claim code from [console and logs]"| SRV
    D -->|"Adds media files to"| MEDIA
    D -->|"Sets up and shares remote access through"| REMOTE

    %% ---------- Browser -> Google's cloud ----------
    WEB -->|"Loads the app's files [HTTPS]"| HOST
    WEB -->|"Signs in and refreshes sessions [Firebase Auth SDK, HTTPS]"| AUTH
    WEB -->|"Reads servers, groups, and invites, and listens for status changes [Firestore SDK, real-time listener]"| FS
    WEB -->|"Gets server passes, manages invites, publishes themes [callable functions, HTTPS + Firebase login token]"| FN
    WEB -->|"Uploads and downloads theme preview images [Storage SDK, HTTPS]"| STORE

    %% ---------- Inside Google's cloud ----------
    FN -->|"Reads and writes Hub data [Admin SDK]"| FS
    FN -->|"Disables accounts [Admin SDK]"| AUTH
    FN -->|"Sends push notifications [FCM API]"| FCM
    FCM -->|"Delivers push notifications [Web Push, to the service worker]"| WEB

    %% ---------- Browser -> owner's Server ----------
    WEB -->|"Calls the API, streams media, and chats [HTTPS/JSON, HLS, SignalR WebSocket + server pass]"| REMOTE
    REMOTE -->|"Carries outside connections to [HTTPS]"| SRV

    %% ---------- Server -> everything else ----------
    SRV -->|"Registers, sends heartbeats, downloads the member list, requests push notifications [HTTPS/JSON + server credential]"| FN
    SRV -->|"Fetches the public keys that check server passes [HTTPS, JWKS]"| HOST
    SRV -->|"Reads and writes library, chat, and member data [EF Core]"| DB
    SRV -->|"Starts to read file details and convert media [child process]"| FF
    SRV -->|"Scans for new and changed files [file system]"| MEDIA
    FF -->|"Reads media files from [file system]"| MEDIA
    SRV -->|"Looks up music details [HTTPS/JSON]"| MB
    SRV -->|"Looks up movie and show details [HTTPS/JSON + owner's API key]"| TMDB

    classDef person fill:#08427b,stroke:#052e56,color:#fff
    classDef container fill:#1168bd,stroke:#0b4884,color:#fff
    classDef external fill:#8a8a8a,stroke:#5e5e5e,color:#fff
    class B,C,D,E,F person
    class WEB,HOST,FN,FS,STORE,SRV,DB,FF container
    class AUTH,FCM,MEDIA,REMOTE,MB,TMDB external
```

## Containers

| Name | Technology | Responsibility | Runs where | Run by whom |
|------|------------|----------------|------------|-------------|
| Shogun Web | React, TypeScript, Vite | The app every person uses: sign-in, server list, browsing, the player, chat, themes, and the admin page | The user's browser | Each user |
| Web Hosting | Firebase Hosting | Serves the web app's files and the public keys (JWKS) that Servers use to check server passes | Google's cloud | You |
| Hub Functions | Cloud Functions, TypeScript | All Hub logic that needs trust: claiming, heartbeats, server passes, invites, member lists, push requests, moderation | Google's cloud | You |
| Hub Database | Cloud Firestore | Profiles, servers and their addresses and status, group memberships, invites, push tokens, Workshop theme data | Google's cloud | You (Google runs the database) |
| Theme Images | Cloud Storage | Workshop theme preview images | Google's cloud | You (Google runs the storage) |
| Shogun Server | ASP.NET Core, C# | The REST API, streaming, chat, library scanning, metadata matching, and checking server passes | The owner's hardware | Each server owner |
| Server Database | SQLite (EF Core) | Libraries, files, metadata, a copy of the member list, permissions, watch progress, chat, and comments | The owner's hardware | Each server owner |
| ffmpeg | ffmpeg | Reads file details and converts media the browser can't play | The owner's hardware, started by the Server | Each server owner |

## Decisions and Explanations

## Legend

| Shape | Color | Meaning |
|-------|-------|---------|
| Rounded (stadium) box | Dark blue | A **person** (same as the context diagram) |
| Rectangle | Blue | A **container** you build or configure: an app or a set of functions |
| Cylinder | Blue | A **data store** you design (Google or the owner runs the hardware) |
| Double-sided box or grey cylinder | Grey | An **external system** or storage that Shogun doesn't build |
| Outlined group | | **Where it runs.** Arrows that cross a group's edge cross a trust boundary (Step 4). |
