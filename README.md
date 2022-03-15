# Sharp Crokite

## Introduction
This tool assists players by offering quick access to handy overviews, aggregations and calculations, all based on up-to-date in-game information gathered from various web resources.

I started this project because I have built multiple complicated spreadsheets for the game, which I all aim to replace with this tool. It's a hobby project to tinker with in my spare time, while also serving as a display of my abilities and knowledge.

## Running the application
In order to run the application, follow these steps:
1. Open and build the solution using Visual Studio.
2. Run the following command from within the console to create the database:

```
Update-Database -Project SharpCrokite.DataAccess
```
3. Run the application, and use the `Update Static Data` and `Update Prices` buttons to populate the database.