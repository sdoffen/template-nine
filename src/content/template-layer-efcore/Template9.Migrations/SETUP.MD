# Setting Up Migrations

_Adapted from [Using a Separate Migration Project](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli) article._

In order to store our migrations in the `Template9.Migrations` project instead of the default `Template9` project, we need to follow these steps when creating the very first migration.

## Create the Initial Migration

As soon as there are models to add to the database, we can create our first migration using the command:

```
dotnet ef migrations add InitialMigration --project ./src/Template9 --startup-project ./src/Template9.Design
```

This will add a `Migrations` folder in the `Template9` project that contains the initial migration and a model snapshot.

```
src/
├── Template9/
|   └── Migrations/
|       ├── YYYYMMDDhhmmss_InitialMigration.cs
|       ├── YYYYMMDDhhmmss_InitialMigration.cs
|       └── ProjectDbContextModelSnapshot.cs
├── Template9.Design/
├── Template9.Migrations/
└── Template9.Tests/
```

> [!NOTE]
> `YYYYMMDDhhmmss` in the above file name will actually be a timestamp for when the migration was created, representing the four digit year, and 2 digits each for month, day, hour, minute and second.

## Move the Migrations Folder

Move the entire `Migrations` folder generated in the first step into the `Template9.Migrations` folder.

```
src/
├── Template9/
├── Template9.Design/
├── Template9.Migrations/
|   └── Migrations/
|       ├── YYYYMMDDhhmmss_InitialMigration.cs
|       ├── YYYYMMDDhhmmss_InitialMigration.cs
|       └── ProjectDbContextModelSnapshot.cs
└── Template9.Tests/
```

## Modify the Project Configuration

Modify the [`Template9.Design.csproj`](../Template9.Design/Template9.Design.csproj) file to reference `Template9.Migrations` instead of `Template9` directly.

```csproj
  <ItemGroup>
    <ProjectReference Include="..\Template9.Migrations\Template9.Migrations.csproj" />
  </ItemGroup>
```

The [appsettings.Design.json](../Template9.Design/appsettings.Design.json) file in the `Template9.Design` project should be modified by setting the `DatabaseOptions.UseMigrationsAssembly` property to true.

```json
{
  "DatabaseOptions": {
    "MigrationsAssembly": "Template9.Migrations",
    "UseMigrationsAssembly": true
  }
}
```

Also ensure that the value for `DatabaseOptions.MigrationsAssembly` correctly points to the assembly that now contains your migrations.

> [!NOTE]
> While the configuration for other projects (e.g. Tests or WebApi) can also be updated with the values specified above, this is not necessary, as only the `Template9.Design` project will be running migrations.

## Clean Up

Once these instructions have been completed, this document can (and should) be deleted, as well as the section in the [README.md](./README.md) that refers to it.