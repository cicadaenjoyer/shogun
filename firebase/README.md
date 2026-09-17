# firebase/ - the Shogun Hub

The one Firebase project everyone shares: accounts, groups, invites, the server
list and status, chat, push notifications, and the theme workshop.

Expected contents:

- `firebase.json`, `.firebaserc` - project config
- `firestore.rules`, `firestore.indexes.json`, `storage.rules` - security rules
- `functions/` - Cloud Functions in TypeScript (a pnpm workspace package)

The Hub never sees video, music, or library contents. Media travels directly between a Shogun Server and the app.
