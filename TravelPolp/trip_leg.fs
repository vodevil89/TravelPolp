module trip_leg

type Country = Italy

type TripLeg(pCity, pCountry, pStartDate, pEndDate) =

    let mutable city = pCity
    let mutable country = pCountry
    let mutable startDate = pStartDate
    let mutable endDate = pEndDate

    member this.City with get() = city and set(x) = city <- x
    member this.Country with get() = country and set(x) = country <- x
    member this.StartDate with get() = startDate and set(x) = startDate <- x
    member this.EndDate with get() = endDate and set(x) = endDate <- x