module trip

type Trip() =

    let mutable name = ""
    let mutable startDate = null
    let mutable endDate = null

    member this.Name with get() = name and set(x) = name <- x
    member this.StartDate with get() = startDate and set(x) = startDate <- x
    member this.EndDate with get() = endDate and set(x) = endDate <- x
