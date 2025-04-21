# Template9.Migrations

A quickstart guide to using [Entity Framework Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) with this project.

## Target Database

The migration commands will target the database specified in the `DatabaseOptions` section of the [`appsettings.Design.json`](../Template9.Design/appsettings.Design.json) file.

## Developer Setup

In order to run and manage migrations using Entity Framework Core and this project, you will need to globally install or updated the dotnet [Entity Framework Core .NET Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) using the appropriate command(s).

```bash
# Install
$ dotnet tool install --global dotnet-ef

# Update
$ dotnet tool update --global dotnet-ef

# Verify Installation
$ dotnet ef
```

## Required CLI Options

The CLI commands refer to a _project_ and a _startup project_.

- The _project_ is also known as the _target project_ because it's where the commands add or remove files. By default, the project in the current directory is the target project. We will specify a different project as target project by using the `--project` option, because we want to [store migrations in a different project](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli) than the one containing our `DbContext`.

- The _startup project_ is the one that the tools build and run. The tools have to execute application code at design time to get information about the project, such as the database connection string and the configuration of the model. By default, the project in the current directory is the startup project. You can specify a different project as startup project by using the `--startup-project` option.

| Parameter           | Value                        |
|---------------------|------------------------------|
| `--project`         | Template9.Default.Migrations |
| `--startup-project` | Template9.Default.Design     |

> [!IMPORTANT]
> All commands in the examples below are assumed to be running in the root folder of the project unless otherwise specified.

## Common Migration Commands

This list of common migration commands is not exhaustive. See the [documentation](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) for complete details.

### List Available Migrations

To display a list of available migrations:

```bash
$ dotnet ef migrations list --project ./src/Template9.Migrations --startup-project ./src/Template9.Design
```

### Check For Pending Changes

To determine if there are any model changes that need to have a migration created:

```bash
$ dotnet ef migrations has-pending-model-changes --project ./src/Template9.Migrations --startup-project ./src/Template9.Design
```

### Create a New Migration

> [!TIP]
> It is recommended to use Pascal case when naming migrations, to avoid the static analysis warning [CS8981](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/warning-waves#cs8981---the-type-name-only-contains-lower-cased-ascii-characters). Nevertheless, this warning is disabled in the project file of the migrations project.

Create a new migration named `<MyMigration>`:

```bash
$ dotnet ef migrations add <MigrationName> --project ./src/Template9.Migrations --startup-project ./src/Template9.Design
```

### Apply Migrations to Lower Environments

Update the target database to the latest migrations:

```bash
$ dotnet ef database update --project ./src/Template9.Migrations --startup-project ./src/Template9.Design
```

> [!WARNING]
> This command should only be used to apply migrations to development (lower) environments.

### Remove Migrations from Lower Environments

Remove the last migration that was applied, rolling back the changes from that migration:

```bash
$ dotnet ef migrations remove --project ./src/Template9.Migrations --startup-project ./src/Template9.Design
```

### Apply Migrations to Higher Environments

Updates to production (higher) environments should be done via scripts. The command for creating the scripts requires parameters for the starting migration and the ending migration. Command line options provide the output file for the script. The `--idempotent` flag is not required, but is recommended to ensure the script does not fail unnecessarily.

```bash
$ dotnet ef migrations script 0 1 --idempotent --output ./script.sql --project ./src/Template9.Migrations --startup-project ./src/Template9.Design
```

> [!TIP]
> Migrations may be identified by name or by ID. The number 0 is a special case that means before the first migration. Defaults to 0.

### Remove Migrations from Higher Environments