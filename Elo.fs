namespace Elo

open System
open System.Collections.Generic

type Competitor(competitorId:string, rating:double) = 
    let mutable _rating = rating
    member this.CompetitorId = competitorId
    member this.Rating 
        with get() = _rating
        and set(value:double) = _rating <- value

    member this.TransformedRating() = 
        10.0 ** (this.Rating / 400.0)

    member private this.CalculateExpected(opponent:Competitor) =
        this.TransformedRating() / (this.TransformedRating() + opponent.TransformedRating())

    member this.CalculatePoints(opponent:Competitor, result:double, k:double) =
        k * (result - this.CalculateExpected(opponent))

    override this.ToString() = 
        String.Format("{0}: {1}", this.CompetitorId, this.Rating)
        

type Result(competitorId1:string, competitorId2:string, result:double) =
    member this.CompetitorId1 = competitorId1

    member this.CompetitorId2 = competitorId2

    member this.Result = result


type EloSettings (k:double, initialRating:double) =
    member this.K = k
    member this.DefaultInitialRating = initialRating


type EloSystem(settings:EloSettings) =
    let mutable _competitors = new Dictionary<string, Competitor>()

    member this.Settings = settings

    member this.Competitors 
        with get() = _competitors

    member this.AddResults (results:seq<Result>) =
        // 1. initialise all the competitors
        let competitorsFromThisRound = 
            results 
            |> Seq.collect (fun r -> seq { yield r.CompetitorId1; yield r.CompetitorId2 })
            |> Seq.distinct

        // 2. accumulate each competitors's rating points from this round of results
        let accumulatedPoints = new Dictionary<string, double>()
        for competitorId in competitorsFromThisRound do 
            accumulatedPoints.Add(competitorId, 0.0)
        
        for result in results do   
            let c1:Competitor = this.GetCompetitor(result.CompetitorId1)
            let c2:Competitor = this.GetCompetitor(result.CompetitorId2)
            accumulatedPoints.[result.CompetitorId1] <- 
                accumulatedPoints.[result.CompetitorId1] + c1.CalculatePoints(c2, result.Result, this.Settings.K) 
            accumulatedPoints.[result.CompetitorId2] <- 
                accumulatedPoints.[result.CompetitorId2] + c2.CalculatePoints(c1, 1.0 - result.Result, this.Settings.K) 

        // 3. add the rating points to each competitor
        for competitorId in accumulatedPoints.Keys do
            let competitor:Competitor = this.GetCompetitor(competitorId)
            competitor.Rating <- competitor.Rating + accumulatedPoints.[competitor.CompetitorId]

    member this.Rankings =
        this.Competitors.Values 
        |> Seq.sortByDescending (fun c -> c.Rating)

    member private this.GetCompetitor (name: string) =
        match this.Competitors.TryGetValue name with
        | true, competitor -> competitor
        | _                -> this.CreateCompetitor(name)

    member private this.CreateCompetitor (name:string) =
        let competitor = Competitor(name, this.Settings.DefaultInitialRating)
        this.Competitors.[name] <- competitor
        competitor