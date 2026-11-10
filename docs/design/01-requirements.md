# Shogun — Problem Statement & Requirements

**Contents:** [Problem Statement](#problem-statement) · [Actors](#actors) · [User Stories](#user-stories) · [Non-Functional Requirements](#non-functional-requirements) · [Out of Scope](#out-of-scope-v1) · [Decisions](#decisions)

---

## Problem Statement

People who own their movies, shows, and music have no easy way to enjoy that collection away from home, or to share it with the friends they'd watch it with. Paid streaming services only offer what they license, cost more every year, and scatter one person's viewing across many apps. Tools for streaming your own library exist, but they either charge for watching away from home or leave owners to wrestle with network settings, and none of them treat watching as something you do together. These tools are also lacking in first-party ports (like Mobile or Smart TV apps) and customization.

Shogun lets someone run a media server on their own computer, invite a small group of friends, and turn their video and music library into a shared space where the group can chat and comment on what they watch. Shogun also lets users create their own UI styles and upload them to a shared Workshop platform for other users to download and install in their app. Success looks like an owner with no networking knowledge setting Shogun up and having a friend watching a movie the same evening, at no cost.

## Actors

| Actor                  | Who they are                                                                                                                | What only they can do                                                                                                                                                                                                                                                                                                                |
| ---------------------- | --------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Visitor                | Someone who hasn't signed in or doesn't have an account yet.                                                                | Sign up and nothing else. Every other feature needs an account.                                                                                                                                                                                                                                                                      |
| Account holder         | Anyone signed in to Shogun. Every owner and member is also an account holder.                                               | Edit their profile, design and apply themes, browse, install, and publish Workshop themes, accept invites, and claim a server (which upgrades them to server owner).                                                                                                                                                                 |
| Server owner           | An account holder who has claimed a Shogun Server running on their own hardware.                                            | Add libraries, invite and remove members (up to 4), choose which libraries each member can see, manage collections, review reported messages, and delete any comment or chat message on their server. Because chat and comments are stored on their machine, the owner can read them. The owner can also do everything a member can. |
| Member                 | An account holder who accepted an owner's invite.                                                                           | Browse and play the libraries the owner allows, comment, chat with the group, message other members directly, and leave the group.                                                                                                                                                                                                   |
| Shogun admin           | The person running the Hub (the developer).                                                                                 | Review reports about themes and accounts, remove Workshop themes, and disable accounts or block them from publishing. Can't see chat messages or comments.                                                                                                                                                                           |
| Shogun Server (system) | The software on the owner's machine. It counts as an actor because it talks to the Hub on its own, with its own credential. | Ask to be claimed, report that it's online (heartbeats), download the current member list, and ask the Hub to send push notifications.                                                                                                                                                                                               |

**How accounts, servers, and groups relate:**

- One account can **own several servers** and be a **member of several others** at the same time. An owner of one server can be a member on someone else's.
- Each server has exactly **one group**: its owner plus up to 4 members. The member limit is per server, not per account.
- Everything group-related (chat, direct messages, comments, permissions) belongs to **one server**. Two people who share two servers have separate conversations on each.

## User Stories

_Priority uses MoSCoW: Must / Should / Could / Won't (for v1). "Phase" is the roadmap phase that delivers the story._

| ID                                                    | Story                                 | Role           | Priority                     | Phase       |
| ----------------------------------------------------- | ------------------------------------- | -------------- | ---------------------------- | ----------- |
| [US-01](#us-01-sign-up)                               | Sign up                               | Visitor        | Must                         | 2A, 4       |
| [US-02](#us-02-set-up-my-library)                     | Set up my library                     | Server owner   | Must                         | 2B          |
| [US-03](#us-03-link-my-server-to-my-account)          | Link my server to my account          | Server owner   | Must                         | 2A          |
| [US-04](#us-04-reach-my-server-from-anywhere)         | Reach my server from anywhere         | Server owner   | Must                         | 5           |
| [US-05](#us-05-invite-friends)                        | Invite friends                        | Server owner   | Must                         | 6           |
| [US-06](#us-06-remove-a-member)                       | Remove a member                       | Server owner   | Must                         | 6           |
| [US-07](#us-07-limit-what-a-member-can-see)           | Limit what a member can see           | Server owner   | Should                       | 6           |
| [US-08](#us-08-organize-collections)                  | Organize collections                  | Server owner   | Could                        | 5 (Stretch) |
| [US-09](#us-09-accept-an-invite)                      | Accept an invite                      | Member         | Must                         | 6           |
| [US-10](#us-10-see-whether-a-server-is-online)        | See whether a server is online        | Member         | Must                         | 2A, 4, 6    |
| [US-11](#us-11-browse-and-search-the-library)         | Browse and search the library         | Member         | Must                         | 2B, 4, 5    |
| [US-12](#us-12-play-media-in-my-browser)              | Play media in my browser              | Member         | Must                         | 3, 4        |
| [US-13](#us-13-resume-playback)                       | Resume playback                       | Member         | Should                       | 3, 4, 5     |
| [US-14](#us-14-comment-on-media)                      | Comment on media                      | Member         | Should                       | 6           |
| [US-15](#us-15-chat-with-my-group)                    | Chat with my group                    | Member         | Should                       | 6           |
| [US-16](#us-16-message-a-member-directly)             | Message a member directly             | Member         | Should                       | 6           |
| [US-17](#us-17-leave-a-group)                         | Leave a group                         | Member         | Must                         | 6           |
| [US-18](#us-18-design-themes)                         | Design themes                         | Account holder | Should                       | 4, 6        |
| [US-19](#us-19-share-themes-in-the-workshop)          | Share themes in the Workshop          | Account holder | Should                       | 6           |
| [US-20](#us-20-moderate-reported-themes-and-accounts) | Moderate reported themes and accounts | Shogun admin   | Should (Must if US-19 ships) | 6           |

### Visitor

#### US-01: Sign up

As someone new to Shogun, I want to sign up for a Shogun account so that I can host or access a server.

- Priority: Must · Phase: 2A, 4
- Acceptance criteria:
    - A visitor can create an account with email and password, or with Google, and lands on an empty server list.
    - An email address can belong to only one account.
    - Email accounts must verify their address before they can claim a server or accept an invite.
    - A user who forgot their password can reset it by email.

### Server owner

#### US-02: Set up my library

As a server owner, I want to run Shogun on my own computer and point it at my media folders so that my movies, shows, and music are organized into a library automatically.

- Priority: Must · Phase: 2B
- Acceptance criteria:
    - The owner can add a folder as a Movies, Shows, or Music library, and a scan starts with visible progress.
    - After the scan, each recognized movie, episode, and album shows a title, artwork, and basic details.
    - Files added to the folder later appear after the next scan without re-adding the library.
    - Files Shogun can't identify are still listed as "unmatched" (not hidden), and the owner can fix the match by hand.

#### US-03: Link my server to my account

As a server owner, I want to connect my new server to my Shogun account so that it shows up in my server list and I control it.

- Priority: Must · Phase: 2A
- Acceptance criteria:
    - On first start, the Server shows a claim code. Entering it in the web app while signed in links the server to that account.
    - A claim code expires after 30 minutes and works only once.
    - A wrong or expired code shows a clear error and links nothing.
    - A claimed server can't be claimed by another account.

#### US-04: Reach my server from anywhere

As a server owner, I want my server to be reachable from outside my home so that my members and I can watch from anywhere.

- Priority: Must · Phase: 5
- Acceptance criteria:
    - A member on a different network (for example, a phone on mobile data) can open the server and play media.
    - When a connection fails, the web app's connection test names the cause (no HTTPS, server doesn't allow the web app, browser blocked local network access).
    - Following the setup guide, an owner with no networking experience gets remote access working (see the Setup NFR).

#### US-05: Invite friends

As a server owner, I want to share my server with my friends so that they can view the same media as me.

- Priority: Must · Phase: 6
- Acceptance criteria:
    - The owner can create an invite link from the server's group page and share it any way they like.
    - An invite link works once and expires after 7 days.
    - A group can't go over 4 members, counting pending invites.
    - The owner can cancel a pending invite.

#### US-06: Remove a member

As a server owner, I want to remove someone from my server so that only people I trust can reach my library.

- Priority: Must · Phase: 6
- Acceptance criteria:
    - The owner can remove any member from the group page after confirming.
    - Within about 2 minutes, the removed member's playback stops, and the server disappears from their list.
    - Once the Server syncs the member list, the removed member can no longer read or post in the group chat, their direct messages on that server, or comments.
    - Their past messages and comments on that server stay, but are anonymized and shown as "Former member".

#### US-07: Limit what a member can see

As a server owner, I want to choose which libraries each member can see so that I can keep some of my media private.

- Priority: Should · Phase: 6
- Acceptance criteria:
    - New members can see every library until the owner changes it.
    - The owner can pick, per member, which libraries they can see.
    - A member restricted to one library sees only that library in browsing and search, and a direct link to a hidden item returns "not found".
    - Changes apply without the member signing out.

#### US-08: Organize collections

As a server owner, I want to create and manage collections so that I can order my media neatly.

- Priority: Could · Phase: 5 (Stretch)
- Acceptance criteria:
    - The owner can create a named collection and add or remove items from any library.
    - Collections appear when members browse, with items a member isn't allowed to see left out.
    - Deleting a collection doesn't delete any media.

### Member

#### US-09: Accept an invite

As a member, I want to join a friend's server from the invite they sent me so that I can start watching their media.

- Priority: Must · Phase: 6
- Acceptance criteria:
    - The invite shows the owner's name and the server's name before the member accepts.
    - Opening an invite while signed out leads through sign-in or sign-up and then back to the invite.
    - After accepting, the server appears in the member's list right away.
    - An expired, used, or cancelled invite shows a clear message.

#### US-10: See whether a server is online

As a member, I want to see whether my friend's server is up before I open it so that I know why something won't play.

- Priority: Must · Phase: 2A, 4, 6
- Acceptance criteria:
    - The server list shows each server as Online or "Offline since &lt;time&gt;".
    - When a server stops, members see it go offline within about 6 minutes without refreshing.
    - Opening an offline server explains that the server is off, not a generic error.

#### US-11: Browse and search the library

As a member, I want to browse and search my friend's library so that I can find something to watch or listen to.

- Priority: Must · Phase: 2B, 4, 5
- Acceptance criteria:
    - The member can browse movies, shows, and music with artwork, and open a detail page for any item.
    - Searching by title finds matching items across every library the member can see.
    - Browsing works on a phone-sized screen without scrolling sideways.

#### US-12: Play media in my browser

As a member, I want to play videos and music from my server in the browser on my phone or computer so that I can watch and listen without installing anything.

- Priority: Must · Phase: 3, 4
- Acceptance criteria:
    - Every file in a library plays in each supported browser (see the Devices NFR), with the Server converting it when the browser can't play it as is.
    - The member can pause, seek, and switch audio and subtitle tracks.
    - Music plays from a queue and keeps playing while the member browses other pages.
    - On a phone, the player has touch controls (tap to show controls, double-tap to skip 10 seconds).

#### US-13: Resume playback

As a member, I want my progress on videos saved so that I can resume where I left off.

- Priority: Should · Phase: 3, 4, 5
- Acceptance criteria:
    - After watching more than 30 seconds of a video, reopening it offers to resume within about 10 seconds of where the member stopped.
    - Progress is per member: one member's progress never changes another's.
    - Partly watched videos appear in a "Continue Watching" row.

#### US-14: Comment on media

As a member, I want to leave comments on a movie, episode, or song, optionally pinned to a moment in it, so that I can share my reactions with the rest of my group.

- Priority: Should · Phase: 6
- Acceptance criteria:
    - A member can post a comment on a movie, an episode, or a music track (not a whole album), and can choose to attach the current playback time.
    - A timestamped comment shows its time, and tapping it jumps playback to that moment.
    - Only members who can see the item's library can read its comments. Comments are stored on the owner's Server, never on the Hub.
    - A member can edit or delete their own comments, and the owner can delete any comment on their server.

#### US-15: Chat with my group

As a member, I want to chat with the other members of my server so that I can share my thoughts with them.

- Priority: Should · Phase: 6
- Acceptance criteria:
    - Members of the same group see each other's messages within a few seconds without refreshing.
    - A message sent while the member's connection is down is delivered exactly once when it comes back.
    - The owner can delete messages on their server, and anyone can report a message to the owner.
    - The chat screen says plainly that messages are stored on the owner's server, that the owner can read them, and that they aren't end-to-end encrypted.
    - While the server is offline, chat says it's unavailable instead of showing an empty conversation.

#### US-16: Message a member directly

As a member, I want to message another member of my server privately so that I can talk to them one-on-one.

- Priority: Should · Phase: 6
- Acceptance criteria:
    - A member can start a direct message with anyone on the same server, and it's stored on that server.
    - Two people who share two servers have a separate conversation on each.
    - A member who blocks someone stops receiving their direct messages on that server.
    - When either person leaves the server or is removed, neither can read the conversation any more.

#### US-17: Leave a group

As a member, I want to leave a group I no longer use so that it stops cluttering my server list.

- Priority: Must · Phase: 6
- Acceptance criteria:
    - A member can leave from the group page after confirming.
    - After leaving, the server disappears from their list, and they can't stream from it or read its chat.
    - Their past messages and comments on that server stay, but are anonymized and shown as "Former member".
    - The owner sees the member gone, and the slot is free for a new invite.

### Account holder

#### US-18: Design themes

As an account holder, I want to design my own Shogun themes so that I can have a custom UI experience.

- Priority: Should · Phase: 4, 6
- Acceptance criteria:
    - The theme editor shows a live preview of sample pages (home, detail, player, chat) while editing.
    - A theme includes both light and dark variants.
    - The editor warns when text would be hard to read against its background (contrast below 4.5:1).
    - An applied theme stays applied after closing and reopening the browser.

#### US-19: Share themes in the Workshop

As an account holder, I want to upload my themes to the Shogun Workshop so that others can view and download them.

- Priority: Should · Phase: 6
- Acceptance criteria:
    - Publishing checks the theme and rejects it with a clear reason if it's invalid.
    - Other account holders can browse, preview, and install published themes.
    - The author can unpublish their theme, and anyone can report a theme.

### Shogun admin

#### US-20: Moderate reported themes and accounts

As the Shogun admin, I want to review reported themes and accounts and act on them so that the Workshop and Shogun as a whole stay safe to use.

- Priority: Should (Must if US-19 ships) · Phase: 6
- Acceptance criteria:
    - The admin page lists reports about themes and accounts, with the reporter's reason.
    - The admin can remove a theme, block an account from publishing, or disable an account.
    - Only accounts with the admin role can open the admin page or use its actions.

## Non-Functional Requirements

| Category      | Requirement                                                                                                                                               | How to test it                                                                                                                                  |
| ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| Performance   | A direct-play video starts within 3 seconds of pressing play on the home network, and within 8 seconds when the Server has to convert it.                 | Time 10 plays of each kind on the reference mini PC; every one meets the target.                                                                |
| Performance   | Browsing a 5,000-item library loads each page within 2 seconds on the home network.                                                                       | Seed a 5,000-item test library and time page loads.                                                                                             |
| Performance   | A chat message reaches other connected members within 2 seconds.                                                                                          | Two browsers on different networks; time 20 messages.                                                                                           |
| Hardware      | The reference mini PC (Intel N100) handles at least 2 hardware-converted 1080p streams at once without stuttering.                                        | Phase 3 benchmark step.                                                                                                                         |
| Hardware      | The Server runs on Linux on an x86-64 machine. Intel Quick Sync is recommended; without it, the Server still works using software conversion.             | Install and play a file on a machine with Quick Sync and one without.                                                                           |
| Availability  | When the owner's server is off, members can still sign in and see their groups. That server's chat and comments show as unavailable with a clear message. | Stop the Server and check each screen.                                                                                                          |
| Availability  | During a Firebase outage, members who already hold a server pass keep streaming and chatting for up to 12 hours (without push notifications).             | Block the Server from reaching Firebase and keep playing and chatting with an existing pass.                                                    |
| Availability  | A removed member loses streaming and chat access within 2 minutes.                                                                                        | Remove a member mid-playback with chat open and time it.                                                                                        |
| Privacy       | The Hub never stores media files, library contents, watch progress, comments, or chat messages.                                                           | After a full test session, inspect every Firestore collection and function log: no titles, file names, progress, comment text, or message text. |
| Privacy       | Push notifications never contain message text or the sender. The Hub only learns which server and which members, and when.                                | Send a message and read both the notification and the function's input in the logs.                                                             |
| Privacy       | When someone leaves or is removed, the Server keeps no link between them and their past messages or comments.                                             | Remove a test member, then inspect the Server's database: none of their messages or comments still hold their user ID.                          |
| Privacy       | Chat messages are deleted after 1 year by default, and each owner can change that.                                                                        | Set a short retention on a test server and confirm the background job deletes older messages.                                                   |
| Security      | Every Server API request and chat connection except the health check needs a valid server pass.                                                           | Automated tests: no pass, an expired pass, and another server's pass are all rejected.                                                          |
| Security      | Server passes never appear in Server logs, including in chat connection URLs.                                                                             | Search the logs after a test session.                                                                                                           |
| Security      | The web app only talks to Servers over HTTPS.                                                                                                             | Point the app at an http:// address; it refuses and explains why.                                                                               |
| Security      | Claim codes and invite links are single-use, expire, and are stored only as hashes.                                                                       | Rules and function tests, plus inspecting stored documents.                                                                                     |
| Security      | The build fails if a dependency has a known high or critical vulnerability.                                                                               | CI run with a deliberately vulnerable package.                                                                                                  |
| Running cost  | Shogun is free for owners and members. No v1 feature needs payment.                                                                                       | Feature checklist review before launch.                                                                                                         |
| Running cost  | The Hub costs under $10 a month for the first 100 active users, with a budget alert at $10.                                                               | Monthly billing check during beta and after launch.                                                                                             |
| Devices       | The web app works in the latest two versions of Chrome, Firefox, and Safari on desktop, Safari on iPhone, and Chrome on Android.                          | Playwright tests plus the real-phone checklist each release.                                                                                    |
| Devices       | Every screen is usable at 375 px wide without scrolling sideways.                                                                                         | Playwright phone profiles and real-phone checks.                                                                                                |
| Accessibility | The default themes meet WCAG 2.2 AA contrast, and every control can be reached by keyboard.                                                               | Automated accessibility check plus a keyboard-only walkthrough.                                                                                 |
| Setup         | An owner with no networking experience can go from installing Shogun to a friend playing a movie within 60 minutes by following the setup guide.          | Time at least 3 beta testers; all finish within 60 minutes.                                                                                     |

## Out of Scope (v1)

- Native mobile and TV apps (planned for Phase 8, after the web launch).
- Downloading media to watch offline.
- Watching together in sync (play, pause, and seek together).
- End-to-end encrypted chat. Planned as a future update: the owner can turn it on per server, and members can always see whether their messages are encrypted.
- Direct messages between people who don't share a server.
- Casting to Chromecast or AirPlay from the web app.
- Running your own copy of the Hub (your own Firebase project).
- Groups of more than 4 members.
- Photos, books, audiobooks, podcasts, and live TV.
- Uploading media through the web app. Owners add files to their folders directly.
- Paid plans or any other way of charging users.

## Decisions

- Messages and comments from people who leave or are removed are anonymized, not deleted: they stay in the conversation, shown as "Former member", and the Server keeps no link back to the person. Conversations still make sense, and the person's identity is gone.
- Comments on music attach to a single track, not an album, so a timestamp always points to a moment in one piece of audio.
- Chat retention defaults to 1 year. Each owner can change it for their server.
- Native apps in the problem statement: the sentence about missing mobile and TV apps describes Shogun's long-term vision. v1 is web-only (see Out of Scope), and native apps follow in Phase 8.
