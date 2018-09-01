# Overview

This simple tool fetches weapons data from `mhw-db.com` and produces a `.csv` file for `https://github.com/gatheringhallstudios/MHWorldData`.

# How to run

## From Visual Studio (2017+)

Open the solution and run the project.<br/>

## From command line

You must have .NET Core SDK 2.1+ installed.

From the directory where the `.csproj` files is, run the following command:
```
dotnet run
```

# Output

If there is no error, it produces the file `weapon_sharpness.csv` in the same directory as the application.<br/>
It should be `bin/Debug/<runtime>/` if you didn't change any settings.
