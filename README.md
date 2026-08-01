# AssetFlow

A full-stack asset-management application for tracking company assets, assigning them to
employees, and keeping a record of who has what and since when.

Built as a portfolio project with an emphasis on production-style engineering on a focused
feature set — Clean Architecture, CQRS, a Result-based error model, and a deployed vertical
slice — rather than breadth of features.

## Live demo

| | |
|---|---|
| **App** | https://assetflow-demo.netlify.app |
| **API** | https://assetflow-api-wgfn.onrender.com |
| **API docs (Swagger)** | https://assetflow-api-wgfn.onrender.com/swagger |

**Demo accounts** — these are intentionally public so the app can be explored without signing up:

| Role | Email | Password |
|---|---|---|
| Admin | `admin@gmail.com` | `Admin123!` |
| Employee | `employee@gmail.com` | `Employee123!` |

## Features

- **JWT authentication** with two roles, `Admin` and `Employee`. Reads are open to any
  authenticated user; writes are Admin-only.
- **Admin approval workflow** — self-registration does *not* grant access. New accounts land in
  a `Pending` state and return no token. An admin reviews them, and approving creates the linked
  `Employee` record and activates the account in a single atomic transaction.
- **Asset CRUD** with paging and filtering, soft delete, categories, and status tracking.
- **Assignment tracking** — assets are assigned to and returned from employees, with the active
  assignment record as the source of truth for whether an asset is out.
- **Responsive UI** — Angular Material with a custom brand palette, a drawer that switches
  between persistent and overlay at 900px, and distinct loading / error / empty states on every
  list page.

## Tech stack

**Backend** — .NET 10 · Clean Architecture · CQRS via MediatR · EF Core · PostgreSQL ·
JWT + BCrypt · FluentValidation 

**Frontend** — Angular 22 (standalone components, signals) · Angular Material 22 · SCSS ·
reactive forms

**Infrastructure** — Docker · Render (API + Postgres) · Netlify (frontend)

## Architecture

The backend follows Clean Architecture with dependencies pointing inward —
`API → Infrastructure → Application → Domain`, never the reverse.

```
AssetFlow-backend/src/
  AssetFlow.Domain/          entities, value objects, enums, Result/Error types
  AssetFlow.Application/     CQRS handlers, DTOs, repository interfaces, validation behavior
  AssetFlow.Infrastructure/  DbContext, EF configurations, repository implementations, JWT, seeding
  AssetFlow.API/             controllers, middleware, composition root
tests/                       handler-level unit tests
```

A few decisions worth calling out:

- **Result pattern over exceptions** for expected failures. Handlers return `Result<T>`;
  controllers map errors to HTTP status codes in one place, so no business logic throws for
  control flow.
- **Rich domain entities.** Business rules live on the entity as methods returning `Result`
  (`asset.Assign(...)`, `user.Approve(...)`), not in the handlers. Entities manage their own
  identity and timestamps.
- **Manual DTO mapping** via static `FromEntity` factories — no AutoMapper. Explicit, greppable,
  and it fails at compile time rather than at runtime.
- **One command, one save.** Multi-entity operations such as approving a user (which creates an
  employee, links it, and flips account status) commit in a single `SaveChangesAsync` so they
  are atomic.

The frontend mirrors this split: one service per API resource, a global HTTP interceptor for
token attachment, route guards for auth and roles, and design tokens in a single SCSS file that
every component references.

## API

All endpoints require a bearer token except registration and login.

| Method | Route | Access |
|---|---|---|
| `POST` | `/api/auth/register` | Public — creates a Pending account, returns no token |
| `POST` | `/api/auth/login` | Public |
| `GET` | `/api/assets` | Authenticated — paged |
| `GET` | `/api/assets/{id}` | Authenticated |
| `POST` `PUT` `DELETE` | `/api/assets` · `/api/assets/{id}` | Admin |
| `GET` | `/api/categories` | Authenticated |
| `GET` `POST` | `/api/employees` · `/api/employees/{id}` | Admin |
| `GET` | `/api/users/pending` | Admin |
| `PUT` | `/api/users/{id}/approve` · `/api/users/{id}/reject` | Admin |
| `GET` | `/api/assignments/active` | Admin |
| `POST` | `/api/assignments` | Admin |
| `PUT` | `/api/assignments/{id}/return` | Admin |

Full request/response schemas are browsable in [Swagger](https://assetflow-api-wgfn.onrender.com/swagger).

## Running locally

**Prerequisites:** .NET 10 SDK, Node 22+, PostgreSQL 17.

### Backend

Create `AssetFlow-backend/src/AssetFlow.API/appsettings.Development.json` (git-ignored):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=assetflow;Username=postgres;Password=your-password"
  }
}
```

```bash
cd AssetFlow-backend
dotnet run --project src/AssetFlow.API
```

Migrations and seed data are applied automatically on startup, so an empty database is enough.
The API listens on `https://localhost:7155`, with Swagger at `/swagger`.

### Frontend

```bash
cd AssetFlow-frontend
npm install
npm start
```

Runs at `http://localhost:4200` and targets the local API via `environment.development.ts`.

### Tests

```bash
dotnet test
```

### Running the API in Docker

```bash
docker build -t assetflow-api .
docker run -p 8080:8080 -e PORT=8080 \
  -e "ConnectionStrings__DefaultConnection=Host=host.docker.internal;Database=assetflow;Username=postgres;Password=your-password" \
  -e "Jwt__Secret=a-local-secret-at-least-32-characters-long" \
  -e "Jwt__Issuer=AssetFlow" -e "Jwt__Audience=AssetFlowClient" \
  -e "Cors__AllowedOrigins__0=http://localhost:4200" \
  assetflow-api
```

## Deployment

The API is packaged as a multi-stage Docker image (SDK for build, ASP.NET runtime for the final
layer) and deployed to Render, which injects the listening port as `$PORT`. The frontend is a
static Angular build on Netlify, with a `/*  /index.html  200` rewrite so client-side routes
survive a page refresh. Both auto-deploy from `main`.

All environment-specific configuration — connection string, JWT secret, allowed CORS origins —
is supplied through environment variables rather than committed files:

| Variable | Purpose |
|---|---|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string (Npgsql key/value format) |
| `Jwt__Secret` · `Jwt__Issuer` · `Jwt__Audience` | Token signing and validation |
| `Cors__AllowedOrigins__0` | Frontend origin permitted by the CORS policy |

## Roadmap

- [ ] Assignment UI — the API is complete; the frontend flow for assigning and returning assets
      is the next piece of work
- [ ] Rebuild the handler unit tests against the current approval-flow contract
- [ ] Asset assignment history on the detail page
