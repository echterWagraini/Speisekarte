# Speisekarte

## Setting up a sqlite db

First you have to install following 3 Packages (all in 7.0.15)

1. EntityFrameworkCore.Design
2. EntityFrameworkCore.SqLite
3. EntityFrameworkCore.Tools
   
Afterwards you create a SqLite Database Then an ApplicationDbContext File and add your Databases + Migration To make the Database complete
Commands:
add-migration [namen]
update-database

add following Lines in the Program.cs:
var connectionString = builder.Configuration.GetConnectionString("Name of Db"); 
builder.Services.AddDbContext(options => options.UseSqlite(connectionString));

https://www.devglan.com/online-tools/text-encryption-decryption
