# EF Core Courses from Pluralsight

Entity Framework Core courses code that was written throughout Pluralsight courses - list can be found below.

## Table of Contents

1. [Entity Framework Core 2: Getting Started](#entity-framework-core-2-getting-started)
2. [Entity Framework Core 2: Mappings](#entity-framework-core-2-mappings)
3. [Relevant Commands](#relevant-commands)
4. [Relevant Links](#relevant-links)

## Entity Framework Core 2 Getting Started

- [Course](https://app.pluralsight.com/library/courses/entity-framework-core-2-getting-started)
- [Repository](https://github.com/davikawasaki/csharp-ef2-core-pluralsight/tree/master/SamuraiAppEFCoreGettingStarted)
- [Classes Notes](https://github.com/davikawasaki/csharp-ef2-core-pluralsight/blob/master/SamuraiAppEFCoreGettingStarted/Notes.csv)

## Entity Framework Core 2 Mappings
- [Course](https://app.pluralsight.com/library/courses/e-f-core-2-beyond-the-basics-mappings)

## Relevant Commands

- Getting a list of EF Core command-line options (run in Package Manager Console):

```
get-help entityframeworkcore
```

- Creating a new migration with EF Core (run in Package Manager Console):

```
add-migration <name-of-the-migration>
```

- Generates a raw SQL script from existing migrations with EF Core (run in Package Manager Console):

```
script-migration
```

- Removes the last migration with EF Core (run in Package Manager Console):

```
remove-migration
```

- Gather all non-executed migrations from catalog and update selected database  with EF Core (run in Package Manager Console):

```
update-database
```

- Drop database using migrations rules with EF Core (run in Package Manager Console):

```
drop-database
```

- Generate entity types and a database context from a database connection with EF Core (run in Package Manager Console):

```
scaffold-dbcontext
```

## Relevant Links

1. [Entity Framework Core Open Source Code](github.com/aspnet/entityframework)

2. [Entity Framework 6 Source Code](github.com/aspnet/entityframework6)

3. [Database Providers](docs.microsoft.com/en-us/ef/core/providers)

4. [EF Core Change-Tracking Behavior: Unchanged, Modified and Added](https://msdn.microsoft.com/magazine/mt767693)

5. [Refactoring an ASP.NET 5/EF6 Project and Dependency Injection](https://msdn.microsoft.com/magazine/mt632269)

6. [DDD-Friendlier EF Core 2.0 - Part 1](https://msdn.microsoft.com/magazine/mt842503)

7. [DDD-Friendlier EF Core 2.0 - Part 2](https://msdn.microsoft.com/magazine/mt826347)

8. [Some Insights into Features (Besides EDMX) Being Dropped in the Move From EF6 to EF Core](http://thedatafarm.com/data-access/droppedfromefcore/)

9. [EF Core Power Tools Extensions - With Model Visualizar and extra](https://erikej.github.io/EFCorePowerTools/index.html)

10. [EF Core migrations with existing database schema and data](https://cmatskas.com/ef-core-migrations-with-existing-database-schema-and-data/)

11. [EF Core Main Documentation](https://docs.microsoft.com/en-us/ef/core/)

12. [Bulk Operations Commands on EF Core Explanation](https://www.brentozar.com/archive/2017/05/case-entity-framework-cores-odd-sql/)

13. [BreezeJS - Tracking database changes in Javascript clients](http://www.getbreezenow.com)

14. [Trackable Entities - Change-tracking across service boundaries](http://trackableentities.github.io/)

15. [EF Core GitHub Issue Query: projected collection navigations don't get tracked if the collection is composed](https://github.com/aspnet/EntityFrameworkCore/issues/8999)

16. [Building UWP Apps for Local and Cloud Data Storage](https://msdn.microsoft.com/magazine/mt814412)

17. [Fiddler - Web Debugging Tool](https://www.telerik.com/fiddler)
