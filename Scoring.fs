namespace Elo

// simple win/lose/draw
module BasicResult = 
    [<Literal>]
    let Win = 1.0

    [<Literal>]
    let Draw = 0.5

    [<Literal>]
    let Loss = 0.0

// an example of a result (like badminton or tennis) where the result is the first-to-three
module BestOfFive = 
    [<Literal>]
    let Score3_0 = 1.0

    [<Literal>]
    let Score3_1 = 0.8

    [<Literal>]
    let Score3_2 = 0.6

    [<Literal>]
    let Score2_3 = 0.4            

    [<Literal>]
    let Score1_3 = 0.2    

    [<Literal>]
    let Score0_3 = 0.0       