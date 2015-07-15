Hangfire.PostgreSql
===================
This is an plugin to the Hangfire to enable Sqlite as a storage system.
Read about hangfire here: https://github.com/HangfireIO/Hangfire#hangfire-
and here: http://hangfire.io/

Instructions
------------
Install Hangfire, see https://github.com/HangfireIO/Hangfire#installation

Download all files from this repository, add the Hangfire.Sqlite.csproj to your solution.
Reference it in your project, and you are ready to go by using:

```csharp
app.UseHangfire(config =>
{
    config.UseSqliteStorage("<connection string or its name>");
    config.UseServer();
});
```


Related Projects
-----------------

* [Hangfire.Core](https://github.com/HangfireIO/Hangfire)

License
--------

Copyright © 2015 Ilya B.

Hangfire.Sqlite is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as 
published by the Free Software Foundation, either version 3 
of the License, or any later version.

Hangfire.Sqlite  is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public 
License along with Hangfire.Sqlite. If not, see <http://www.gnu.org/licenses/>.

This work is based on the work of Sergey Odinokov, author of 
Hangfire. <http://hangfire.io/>
  
   Special thanks goes to him.