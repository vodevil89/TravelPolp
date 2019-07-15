module calendar

open System
open System.Drawing
open System.Windows.Forms

let calendarDayStartX = 128
let calendarDayStartY = 128
let calendarDayWidth = 128
let calendarDayHeight = 128

type Day(pDate) =

    let date = pDate

    member this.Date with get() = date

let startDate = new DateTime(2019, 1, 1)
let endDate = new DateTime(2019, 12, 31)
let mutable tempDate = startDate

let calendar =  [|  while tempDate <= endDate do
                        yield new Day(tempDate)
                        tempDate <- tempDate.AddDays(1.0)
                |]

let mutable selectedDay = new Day(startDate)

for day in calendar do
    if day.Date = DateTime.Today then
        selectedDay <- day

let calendarButtons:Control[] = [|
                                    for j in 0 .. 5 do
                                        for i in 0 .. 6 do
                                            yield new Button(   Width = calendarDayWidth, Height = calendarDayHeight,
                                                                Location = new Point(calendarDayStartX + (calendarDayWidth * i), calendarDayStartY + (calendarDayHeight * j))
                                                            )
                                |]

let calendarMonthBackwardButton = new Button(Text="<", Width = 64, Height = 64, Location = new Point(calendarDayStartX, calendarDayStartY - 64 - 10))
let calendarMonthForwardButton = new Button(Text=">", Width = 64, Height = 64, Location = new Point(calendarDayStartX + 64 + 10, calendarDayStartY - 64 - 10)) 

let calculateCalendarMonth() =

    for i in 0 .. calendarButtons.Length - 1 do
        calendarButtons.[i].Visible <- false
        calendarButtons.[i].Text <- ""

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

            calendarButtons.[tempButtonIndex].Visible <- true
            calendarButtons.[tempButtonIndex].BackColor <- Color.Gainsboro
            calendarButtons.[tempButtonIndex].Text <- day.Date.ToShortDateString()
            tempButtonIndex <- tempButtonIndex + 1