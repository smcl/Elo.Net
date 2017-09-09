# Elo

This is a library which implements a simple [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system) in F#.

## Installing & Using

Install from [NuGet](https://www.nuget.org/packages/Elo/) via Visual Studio's package manager console

    PM> install-package Elo

... or using the `dotnet` command:

    $ dotnet add package Elo

... and import the `Elo` namespace, initialise an `EloSystem` where each team starts with having 1000 points, with [K=32](https://en.wikipedia.org/wiki/Elo_rating_system#Most_accurate_K-factor) and add results to the system. Below is an example in F# of how we'd initialize a ranking system and populate it with the results from the first six games of the [2017 Confederations Cup](http://www.fifa.com/confederationscup/matches/index.html):

    open Elo;

    let rankings = EloSystem(EloSettings(32.0, 1000.0))

    // Add one round of results which will all use the same "original" ratings - this might be a tournament.
    // The result is from the perspective of the first team or player, and is simply a float in the range 
    // 0.0 .. 1.0, where 1.0 is a win.
    let results1 = seq {
        yield Result("Russia", "New Zealand", BasicResult.Win)   // Russia beat New Zealand
        yield Result("Portugal", "Mexico", BasicResult.Draw)     // Brazil and Afghanistan draw
        yield Result("Cameroon", "Chile", BasicResult.Loss)      // Cameron lose to Chile
        yield Result("Australia", "Germany", BasicResult.Loss)   // Australia lose to Germany
        yield Result("Russia", "Portugal", BasicResult.Loss)     // Russia lose to Portgual
        yield Result("Mexico", "New Zealand", BasicResult.Win) } // Mexico beat New Zealand

    for team in rankings.Rankings do printfn "%A" team

This will output the below:

    Portugal: 1016
    Mexico: 1016
    Chile: 1016
    Germany: 1016
    Russia: 1000
    Cameroon: 984
    Australia: 984
    New Zealand: 968
