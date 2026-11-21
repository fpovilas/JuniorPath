# JuniorPath

In this repo, I'm consolidating my C# knowledge and filling gaps by building 10 programs with the help of Claude.
Claude acts as my assistant and teacher, and it also created the curriculum.

## Troubleshooting

### Zed: false "type or namespace not found" errors

**Symptom:** Zed shows errors such as `The type or namespace name '...' could not be found`
(usually after adding a new file), but `dotnet build` succeeds.

**Cause:** the Roslyn language server has a stale view of the project.

**Fix:** open the command palette (`Ctrl+Shift+P`) and run `editor: restart language server`.

> Rule of thumb: if Zed and `dotnet build` disagree, trust `dotnet build`.
