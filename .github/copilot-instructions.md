# Copilot Instructions

## Critical: No Pushing

**NEVER run `git push` unless the user explicitly asks for it.** This is the most important rule. Commits are fine — pushes require explicit human confirmation.

## Project Summary

Terminal.Forms is a C# WinForms-like UI framework for console applications, originally from 2009 (.NET Framework 2.0), being modernized to .NET 10.

## When Working on This Project

- Read `AGENTS.md` at the repo root for full instructions
- Check `docs/plans/00-roadmap.md` for current phase and progress
- Check `docs/issues/known-issues.md` before fixing bugs (it may already be cataloged)
- Follow `.editorconfig` for code style
- Use conventional commit messages (`feat:`, `fix:`, `refactor:`, `docs:`, `chore:`, `test:`)
- Git is at `C:\Program Files\SmartGit\git\cmd\git.exe` (not on PATH)

## Code Patterns

- Namespace: `Terminal.Forms`
- Private fields: `m_` prefix (legacy convention, will migrate later)
- Controls inherit from `Control` → `ScrollableControl` → `ContainerControl` → `Form`
- Rendering goes through `ConsoleCanvas` (singleton) and `ConsoleGraphics`
- Events follow the WinForms pattern: `protected virtual void OnEventName(EventArgs e)`
