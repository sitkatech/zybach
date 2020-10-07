# This code has been moved to the main Zybach repo as of 10/7/2020 9:00 AM
# No further commits will be made on this repo

# Debugging

The trick to debugging is to use the --inspect parameter to node when starting the application,
to set up the launch configuration correctly, and to log a string that will tell VSCode when it's
time to attach. A sleep before emiting that string will make sure that VSCode has enough time to
get ready. We still need to make sure we're not running with --inspect in a production environment
and not doing the sleep-then-log thing in a production environment.

All the set-up is taken care of, just load the folder in VSCode and hit F5

# Name

This is the "Time Series Processing" module of Zybach. TSP. Teaspoon.