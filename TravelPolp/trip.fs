module trip

open calendar
open trip_leg

open System
open System.Drawing

type TripType = Trip | Commitment

type Trip(pTripType, pName, pStartdate, pEndDate, pTripLegs:TripLeg[]) =

    let tripType = pTripType
    let mutable name = pName
    let mutable startDate = pStartdate
    let mutable endDate = pEndDate
    let mutable tripLegs = pTripLegs

    member this.TripType with get() = tripType
    member this.Name with get() = name and set(x) = name <- x
    member this.StartDate with get() = startDate and set(x) = startDate <- x
    member this.EndDate with get() = endDate and set(x) = endDate <- x
    member this.TripLegs with get() = tripLegs and set(x) = tripLegs <- x

let trips = [|
                new Trip(TripType.Commitment, "Dentist", new DateTime(2019, 7, 20), new DateTime(2019, 7, 20), [| new TripLeg("Milan", "Italy", new DateTime(2019, 7, 20), new DateTime(2019, 7, 20)) |])
                new Trip(TripType.Trip, "Rome", new DateTime(2019, 7, 26), new DateTime(2019, 7, 28),   [|
                                                                                                            new TripLeg("Rome", "Italy", new DateTime(2019, 7, 26), new DateTime(2019, 7, 26))
                                                                                                            new TripLeg("Anzio", "Italy", new DateTime(2019, 7, 27), new DateTime(2019, 7, 28))
                                                                                                        |])
            |]

let calculateCalendarTrips() =

    let mutable tempButtonIndex = 0

    for day in calendar do
        if day.Date.Year = selectedDay.Date.Year &&  day.Date.Month = selectedDay.Date.Month then
            if day.Date.Day = 1 then
                match day.Date.DayOfWeek with
                | DayOfWeek.Tuesday -> tempButtonIndex <- 1
                | DayOfWeek.Wednesday -> tempButtonIndex <- 2
                | DayOfWeek.Thursday -> tempButtonIndex <- 3
                | DayOfWeek.Friday -> tempButtonIndex <- 4
                | DayOfWeek.Saturday -> tempButtonIndex <- 5
                | DayOfWeek.Sunday -> tempButtonIndex <- 6
                | _ -> ()

            let mutable tripDayDetails = ""

            for trip in trips do

                if trip.StartDate <= day.Date && day.Date <= trip.EndDate then

                    let mutable tripLegDayDetails = ""

                    for tripLeg in trip.TripLegs do
                        if tripLeg.StartDate <= day.Date && day.Date <= tripLeg.EndDate then

                            tripLegDayDetails <- if tripLegDayDetails = "" then
                                                        tripLeg.City
                                                    else
                                                        tripLegDayDetails + " - " + tripLeg.City

                    tripDayDetails <- tripDayDetails + Environment.NewLine
                                        + Environment.NewLine
                                        + trip.Name + Environment.NewLine
                                        + "(" + tripLegDayDetails + ")"

                    if trip.TripType = TripType.Trip then
                        calendarButtons.[tempButtonIndex].BackColor <- Color.LightCoral
                    else if trip.TripType = TripType.Commitment then
                        calendarButtons.[tempButtonIndex].BackColor <- Color.Violet

            if tripDayDetails = "" then

                tripDayDetails <- Environment.NewLine
                                    + Environment.NewLine
                                    + Environment.NewLine
                                    + Environment.NewLine

            calendarButtons.[tempButtonIndex].Text <- calendarButtons.[tempButtonIndex].Text + tripDayDetails

            tempButtonIndex <- tempButtonIndex + 1