##Claymore [![Build status](https://ci.appveyor.com/api/projects/status/x7b5l5xuh2tuiwrg/branch/master?svg=true)](https://ci.appveyor.com/project/jaredmcguire/claymore/branch/master)

This is a simple tool to generate and update DDL scripts for an SQL database. This makes the database structure visable to your VCS. After you make changes to the database, run Claymore and the apropriate DDL scripts will be updated. Your VCS will then see the changed file and you will have a log of database object changes just like your code files.

To configure the tool, update the json file.