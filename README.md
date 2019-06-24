# TicketSystem
A .NET Core CLI minimal Ticketing System. Demonstrates basic code-first ef scaffolding using SQLite

Assumptions
===========

Microsoft .NET core must be locally installed, ensuring that the 'dotnet' command is available to the shell or command line.

This file also makes use of local disk space using the appdata folder or equivalent to store and access the local database file.

Build Instructions
==================

To generate an executable for your platform, use a dotnet publish command appropriate for your system.

For example, for a Windows 10 machine, run, from the root solution folder:

`dotnet publish -c Release -r win10-x64`

The executable can then be run directly. Running without any command line options will display command line options.

Alternatively, the TicketSystem CLI project can be run directly using "dotnet run" from the TicketSystem\TicketSystem folder, without an explicit build step.

Building and running directly within Visual Studio Community Edition version 16.3.1 is also supported, by opening the root TicketSystem.sln.

Running TicketSystem
====================

TicketSystem supports a core set of commands to facilitate the discovery, temporary hold, and final reservation of seats in a venue.

All of the following dotnet commands must be run within the TicketSystem\TicketSystem folder.

To discover how many seats are available:

`dotnet run -- count`

This will print the number of non-held and non-reserved seats in the venue.

To place a hold on a certain number of seats:

`dotnet run -- hold -c 5 -e validuser@myvalidmail.com`

where '5' is the number of seats to hold and validuser@myvalidmail.com is an e-mail address of the customer.

This will return text indicating the temporary Hold Id, as well as information about the seats.

To reserve the seats:
`dotnet run -- reserve -i 1234567 -e validuser@myvalidmail.com`

where 1234567 is the Hold Id previously returned.

This will print the final reservation id for the venue.

In order to reset all holds and reservations, run:
`dotnet run -- reset`

Executing the Tests
===================

The tests can be executed within Visual Studio by opening the TicketSystem.sln and selecting TEST -> Run -> All Tests

The tests can also be run from the cli. From the TicketSystem\TicketTests folder, run:

`dotnet test`
