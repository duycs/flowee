**WorkerContext relationship**
- 1 Worker have 1 Role
- 1 Worker in n Group, 1 group have n Worker
- 1 Department have n Group
- 1 TimeKeeping have 1 Worker checkin then checkout in a shift

**Migrations CLI**
dotnet ef migrations add [AddedFileName] -o DataAccess/Migrations
dotnet ef migrations remove
dotnet ef database update