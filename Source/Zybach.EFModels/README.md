To regenerate the EF Models:
- install the following nuget package: Microsoft.EntityFrameworkCore.Tools version 2.2.4
- run the following PowerShell command:
	Scaffold-DbContext "Server=.\;Database=ZybachDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities/Generated -Project Zybach.EFModels -Context "ZybachDbContext" -Force -StartupProject Zybach.API -DataAnnotations -Schemas dbo -UseDatabaseNames
- grep the Generated folder and remove ""
- remove the following lines from ZybachDbContext:
-          if (!optionsBuilder.IsConfigured)
           {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=ZybachDB;Trusted_Connection=True;", x => x.UseNetTopologySuite());
            }
