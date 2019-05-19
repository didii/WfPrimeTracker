# Warframe Prime Tracker

Application to fetch all prime data including drop chance, relics, required resources and images.
The frontend is currently built to keep track of what item the user already has and which not.

## EF Core

Database management is done with Entity Framework Core.
Here are some helful commands.

* New migration: `dotnet ef migrations add <name> -s WfPrimeTracker.Server -p WfPrimeTracker.Data -c PrimeContext`
* Remove migration: `dotnet ef migrations remove -s WfPrimeTracker.Server -p WfPrimeTracker.Data -c PrimeContext`
* Apply migration: `dotnet ef database update -s WfPrimeTracker.Server -p WfPrimeTracker.Data -c PrimeContext`

Current database model is as follows:

![Database schema](DbSchema.png)

Every item with a primary ID that is not a Foreign key is called a `PersistentItem`.
This means that it's ID is calculated based on its properties.
This ensures that updates work correctly and duplicated items is almost impossible.
It also helps in identifying newly added items (when a new Prime item comes out) and for the frontend to properly save data locally based on the ID without suddenly losing changes.

## TODO

* Allow users to save data based on a unique GUID or OAuth
* Sort options
* Look into different icons for drop chance icons
* Look into better animation for checking an item
* Hide pictures option
* Move Vaulted icon to the left
* Make everything more robust by adding polling etc
* Use thumbnails to get images of items instead of trying to find it in the page
* Add undo when completing an item and hide completed is on
* ~~Add Wiki url links to Relics~~
* ~~Improve colors on drop chance icons~~
* ~~Animate on Scroll improvements~~
* ~~Add an about page~~