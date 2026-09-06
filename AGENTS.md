# CascVel evaluations module

## Context and sources

The module provides expert evaluation of operator work in CascVel. Read
[ROADMAP.md](ROADMAP.md) when planning delivery. For evaluation-domain or
backend-design changes, consult the relevant sections of the
[current design record](docs/superpowers/specs/2026-09-05-evaluations-backend-design.md).
Its header says final review is pending: use it as design context, not as
permission to implement unapproved decisions. Current user decisions and
verified repository configuration take precedence over historical guidance.

The intended stack is C# 14 / ASP.NET Core 10, MassTransit with RabbitMQ,
PostgreSQL 18, Redis 7, and React 19 where needed. Use actual SDK/project
configuration for executable commands; older CONTRIBUTING stack/default-branch
notes are not authoritative when they conflict. Do not add unused infrastructure.
Use LikeC4 for architecture diagrams and keep them with the relevant docs.

## Language and repository process

Conversation is in Russian unless requested otherwise. New code, documentation,
logs, errors, tests, and GitHub content are in English. Preserve unrelated
existing material; translate it only when requested or changed within scope.

Keep the repository's license (Apache 2.0 by default), SemVer, Keep a Changelog,
and Conventional Commits. When branch creation is explicitly authorized, use
`feat/<issue-number>-<topic>`, `fix/<issue-number>-<topic>`, or `docs/<issue-number>-<topic>`;
omit the issue number when the task has none. Verify the actual target branch.
PR delivery uses required CI and at least one maintainer review.

## Work boundaries and completion

Preserve unrelated working-tree changes. Do not automatically pull, switch or
create branches, commit, push, create/update GitHub content, publish, or deploy;
these actions require explicit authorization. Local implementation requests
permit the scoped edits, validation, and fixes necessary to complete them.

Local work is complete when the requested change is implemented and affected
checks pass, with blockers or omitted checks reported. Remote PR delivery is
part of completion only when explicitly requested. Fix the cause of failures
within scope; report upstream blockers with evidence and a viable workaround
when one exists.

## Engineering and verification

Use the smallest design that preserves correctness and domain invariants.
Prefer composition and immutable values where their semantics fit. Seal
concrete classes by default when inheritance is not required. Introduce
interfaces for actual boundaries, not for every public method.
Use the existing `.editorconfig` and analyzers for style. Explain non-obvious
constraints in comments; keep error/log messages contextual and concise.

Choose focused behavior tests for changed logic and regressions. Use the
project's test framework, deterministic inputs, bounded waits, temporary
resources with cleanup, and ephemeral ports when networking is exercised.
Prefer fakes/stubs at external boundaries. Keep useful test diagnostics and
investigate flaky failures rather than retrying until green. Expand checks for
concurrency, persistence, and integration only when those paths are affected.
