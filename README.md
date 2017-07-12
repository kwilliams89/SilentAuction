# SilentAuction
This is a Group Project web application for the CS 3750 class at Weber State University Summer Semester 2017

Do NOT add migrations to the project. If you modifiy the models/context or need to clear the database, then delete the SilentAuction database from the SQL Server Object Explorer and then press F5 (debug) to recreate the database with the seed data.

I have called context.Database.EnsureCreated(); in the seed data which creates the database if it is not already created.
If it wasn't in the seed data then it would be called at the end of the Startup.cs at the end of Configure.
