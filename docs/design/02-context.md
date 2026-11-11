```mermaid
---
title: Shogun System Context Diagram
---
flowchart TD
    A[Shogun]
    B([Visitor])
    C([Account Holder])
    D([Server Owner])
    E([Member])
    F([Shogun Admin])

    %% Leaving Firebase outside of Shogun because Google runs the
    %% platform - all Shogun does is call it as an external source. We
    %% create the cloud functions
    G[[Firebase]]
    H[[MusicBrainz]]
    I[[TMDB]]
    J[("Owner's media folders")]
    K[[Remote access network]]

    B -->|Signs up for an account using| A
    C -->|Designs, shares, and installs themes using| A
    D -->|Claims servers, sets up libraries, and manages members using| A
    E -->|Browses, plays, comments, and chats using| A
    F -->|Moderates reported themes and accounts using| A

    D -->|Adds movie, show, and music files to| J
    D -->|Sets up and shares remote access through| K
    E -->|Connects from outside the owner's home through| K
    K -->|Carries outside connections to| A

    A -->|Signs users in, stores accounts and groups, and sends push notifications using| G
    A -->|Looks up music details from| H
    A -->|Looks up movie and show details from| I
    A -->|Reads media files from| J

    classDef person fill:#08427b,stroke:#052e56,color:#fff
    classDef system fill:#1168bd,stroke:#0b4884,color:#fff
    classDef external fill:#8a8a8a,stroke:#5e5e5e,color:#fff
    class B,C,D,E,F person
    class A system
    class G,H,I,J,K external
```


## Decisions and Explanations

### How Roles Change
A Visitor becomes an Account Holder by signing up. An Account Holder becomes a Server Owner by claiming a server, and becomes a Member by accepting an invite. Every Owner and Member is still an Account Holder, so each box's arrow lists only what that role adds.

### Why Remote Access is a Generic Box
The owner chooses how their Server is reached from outside the home (Tailscale, their own domain with port forwarding, or a tunnel), so the diagram names the role, not a vendor. US-04 (Reach my server from anywhere) breaks without some remote access path, so it belongs on the diagram even though Shogun doesn't run it.

## Legend

| Shape | Color | Meaning |
|-------|-------|---------|
| Rounded (stadium) box | Dark blue | A **person** who uses Shogun |
| Plain rectangle | Blue | **Shogun**, the system being built. Everything inside it is drawn in the container diagram (Step 3). |
| Double-sided box | Grey | An **external system** that Shogun depends on but doesn't build or run |
| Cylinder | Grey | **External storage**: files Shogun reads but doesn't own |
