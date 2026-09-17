# shared/ - code shared between the Server, the Hub, and the apps

One folder per package. This contains:

- the theme token schema (used by the workshop and by every app)
- the OpenAPI document exported from the Server
- shared TypeScript types generated from that document

Keeping these here means the Server, the Cloud Functions, and the web app can
change together in one commit.
