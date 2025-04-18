# Template9

This project implements the abstractions from _Your.Interfaces.Project_. The DbContext implementation contained in this package is not intended to be available outside of this project, but rather to be consumed by the interface implementations.

## Related Project Structure

Template9 is implemented using several projects in order separate the concerns of interface implementation, design time services, database migrations and integration testing. The project structure and dependency graph are as follows:

| Project              | Purpose                                                |
|----------------------|--------------------------------------------------------|
| Template9            | Interface implementations, as well as the DbContext    |
| Template9.Design     | Executable project used for database design operations |
| Template9.Migrations | Database migrations                                    |
| Template9.Test       | Integration tests for Template9 implementations        |

### Dependency Graph

```
                +----------------------+
                |      Template9       |
                +----------------------+
                    ^          ^
                    |          |
+----------------------+     +----------------------+
| Template9.Migrations |     |   Template9.Tests    |
+----------------------+     +----------------------+
           ^
           |
+----------------------+
|   Template9.Design   |
+----------------------+
```

## Project Organization

