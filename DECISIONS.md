# DECISIONS.md

> The "vibe coding" reflection. AI tooling used: **Claude (Claude Code)** for scaffolding and code generation, with my own direction, review, and fixes throughout.

---

## 1. What did AI generate, and what did I write or modify myself?

**AI generated (under my direction):**
- The Clean Architecture project layout and most boilerplate: entity classes, EF Core `IEntityTypeConfiguration` classes, the `DbContext`, DTOs, FluentValidation validators, the service implementations, controllers, JWT wiring, the global exception middleware, and the Angular components/services.
- Assisted in The Angular UI styling, with Tailwind in Angular.

**I decided / wrote / modified myself:**
- **Architecture & stack choices:** I chose PostgreSQL over SQLite, .NET 8 LTS, Angular 17, and how the layers depend on each other.
- Designed the Clean Architecture project structure and defined dependencies between Domain, Application, Infrastructure, and API layers.
- **Edited Models & DTOs** — I modified some of the models and DTOs the AI generated (including using `int` keys instead of the AI's suggested `Guid`).
- **Authentication approach:** I rejected the AI's first idea of storing demo credentials in `appsettings.json` and instead required a proper **`User` entity with a BCrypt-hashed password**, seeded at startup.
- **Access model:** I decided the **entire Angular app should sit behind login** — an auth route guard redirects unauthenticated users to a login screen, and a JWT HTTP interceptor attaches the token to every API call.
- Added a small `GET /api/dsps` endpoint so the "distribute" screen can list real DSPs.
- Reviewed every file, ran the migrations, wired the frontend to the API, and verified the endpoints.

---

## 2. What security issues did I find (or introduce) in the AI-generated code? How did I handle them?

1. **Secrets in committed config.** The AI's initial wiring placed demo credentials directly in `appsettings.json`. That's a real exposure for a public repo. I handled it by:
   - Replacing the config-based demo login with a **BCrypt-hashed `User` row** seeded at startup — the plaintext password (`Admin123!`) is never stored, only the salted hash.
   - Keeping the **database connection string out of the repo** via .NET user-secrets, with only an empty placeholder committed.
   - **Known trade-off (JWT signing key):** for local-run convenience the JWT signing key currently lives in `appsettings.json`. I'm aware this is not production-safe — anyone with the key can forge tokens — so for a real deployment it must move to user-secrets / environment variables (the code already binds it from configuration, so no code change is needed, only where the value lives).
2. **Permissive CORS.** The default suggestion allowed any origin. I locked CORS to the Angular dev origin (`http://localhost:4200`), read from config.
3. **Information leakage on errors.** Unhandled exceptions could return stack traces. I added global exception-handling middleware that returns sanitized RFC 7807 `ProblemDetails` and a generic 500 message, logging the real error server-side only.
4. **Over-posting / mass assignment.** Requests bind to purpose-built **DTOs**, not entities — clients can't set server-controlled fields like `Id` or `Status` on create.
5. **Exact token expiry.** Set `ClockSkew = TimeSpan.Zero` so JWT lifetime is enforced precisely instead of the default 5-minute grace.
6. **Frontend auth.** The whole SPA is gated by a route guard; the JWT is stored in `localStorage` and sent via an interceptor. (Trade-off: `localStorage` is readable by JS, so it's vulnerable to XSS; an httpOnly cookie would be stronger for production. Acceptable for this scope.)

---

## 3. One thing the AI got wrong that I had to fix

**EF Core seed data with non-deterministic / non-UTC values.**

The AI's first cut at seeding used runtime values (`DateTime.Now`, and a `DateTime` with unspecified `Kind`) inside EF Core's `HasData`. This is wrong for two reasons:

1. **`HasData` requires deterministic values.** EF bakes seed data into the migration snapshot and compares it on every model build. A `DateTime.Now` (or `Guid.NewGuid()`) changes every build, so EF either keeps generating spurious "model changed" migrations or throws outright.
2. **Npgsql requires UTC for `timestamp with time zone`.** A `DateTime` with `Kind = Unspecified`/`Local` throws at insert time (`Cannot write DateTime with Kind=...`).

**Fix:** I used a single `static readonly` UTC constant (`new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc)`) for all seeded timestamps, fixed integer IDs, and `DateOnly` literals for release dates — making the seed both deterministic and Npgsql-compatible. For runtime inserts I use `DateTime.UtcNow`.

> A secondary fix: an EF Core package version mismatch (8.0.11 vs 8.0.28) produced an `MSB3277` warning; aligning the EF Core/Design package versions across the API and Infrastructure projects cleared it.
