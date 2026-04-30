# OSRS Claude Guide

A wiki-grounded AI assistant for Old School RuneScape. Ask questions in plain English, get accurate answers — the agent looks things up on the OSRS wiki instead of guessing.

> **BYOK** — Bring Your Own Anthropic API key. No account required. No usage tracked server-side.

---

## Why this exists

Language models hallucinate OSRS game data. Drop rates, quest steps, item requirements — the game is too niche and too frequently updated for a model to reliably recall from training alone. This project wraps Claude in a harness that forces it to search and read the OSRS wiki before answering.

---

## How it works

1. User submits a question and their Anthropic API key via the web UI
2. The server starts an agentic loop with Claude
3. Claude calls tools (`search_wiki`, `get_wiki_page`, optionally `get_ge_price`) as needed
4. Results from the wiki are passed back to Claude
5. Claude formulates a final answer grounded in the retrieved content
6. The answer is streamed back to the UI via HTMX

---

## Stack

| Layer | Choice |
|---|---|
| Backend | ASP.NET Core (C#), minimal API |
| Frontend | HTMX + plain HTML/CSS |
| Containerisation | Docker + docker-compose |
| CI/CD | GitLab CI (`.gitlab-ci.yml`) |
| Hosting | Fly.io (free tier) |
| External APIs | Anthropic API, OSRS MediaWiki API, prices.runescape.wiki |

---

## Getting started (local)

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, for container-based dev)
- An [Anthropic API key](https://console.anthropic.com/)

### Run directly

```bash
git clone https://gitlab.com/<your-username>/osrs-claude-guide.git
cd osrs-claude-guide
dotnet run --project src/OsrsGuide.Web
```

Open `http://localhost:5000`, paste your API key, and start asking questions.

### Run with Docker

```bash
docker compose up --build
```

---

## Project structure

```
osrs-claude-guide/
├── src/
│   └── OsrsGuide.Web/
│       ├── Program.cs              # Minimal API setup, endpoints
│       ├── Agent/
│       │   ├── OsrsAgent.cs        # Agentic loop (tool-use)
│       │   ├── Tools/
│       │   │   ├── WikiSearchTool.cs
│       │   │   ├── WikiPageTool.cs
│       │   │   └── GePriceTool.cs
│       │   └── WikiClient.cs       # MediaWiki API wrapper
│       └── wwwroot/
│           └── index.html          # HTMX frontend
├── .gitlab-ci.yml                  # CI/CD pipeline
├── Dockerfile
├── docker-compose.yml
└── README.md
```

---

## CI/CD pipeline

The pipeline is defined in `.gitlab-ci.yml` and runs on every push to `main`.

**Stages:**

1. `lint` — `dotnet format --verify-no-changes`
2. `build` — `dotnet build` + build and push Docker image to GitLab Container Registry
3. `deploy` — Deploy to Fly.io using `flyctl deploy`

**Required CI/CD variables** (set in GitLab → Settings → CI/CD → Variables):

| Variable | Description |
|---|---|
| `FLY_API_TOKEN` | Fly.io deploy token |
| `CI_REGISTRY_USER` | Auto-provided by GitLab |
| `CI_REGISTRY_PASSWORD` | Auto-provided by GitLab |

No Anthropic API key is stored server-side. Users provide their own key at query time.

---

## Deployment

### Fly.io (recommended, free tier)

```bash
# Install flyctl
curl -L https://fly.io/install.sh | sh

# Authenticate and launch (first time)
fly auth login
fly launch

# Deploy manually
fly deploy
```

Fly.io's free tier includes 3 shared-CPU VMs with 256 MB RAM — more than enough for this service.

### Alternative: Railway

Connect your GitLab repository in the Railway dashboard. Add a `railway.json` or use the Dockerfile. Free $5/month credits cover a small always-on service.

### Aarhus University

If you have access to department infrastructure (e.g. a VM or Kubernetes namespace via ITS or your supervisor), you can deploy the Docker image directly. Check with `it.au.dk` or your project supervisor for availability. The GitLab instance at `gitlab.au.dk` can be used as the remote with the same `.gitlab-ci.yml` — just update the `image:` registry references accordingly.

---

## API key security

- The user's API key is submitted per-request and used immediately
- It is **not** stored in a database, logged, or persisted anywhere on the server
- It travels over HTTPS and is never written to disk

---

## OSRS wiki tools

| Tool | API endpoint | Used for |
|---|---|---|
| `search_wiki` | `oldschool.runescape.wiki/api.php?action=query&list=search` | Finding relevant pages |
| `get_wiki_page` | `oldschool.runescape.wiki/api.php?action=parse&prop=wikitext` | Fetching full page content |
| `get_ge_price` | `prices.runescape.wiki/api/v1/latest?id=<item_id>` | Grand Exchange prices |

All endpoints are public and require no authentication. Please include a descriptive `User-Agent` header as requested by the wiki's API policy.

---

## Supported models

| Label | Model string |
|---|---|
| Claude Haiku (fast, cheap) | `claude-haiku-4-5-20251001` |
| Claude Sonnet (recommended) | `claude-sonnet-4-20250514` |
| Claude Opus (most capable) | `claude-opus-4-20250514` |

---

## Contributing

This is a personal side project. Issues and PRs are welcome via GitLab.

---

## License

MIT
