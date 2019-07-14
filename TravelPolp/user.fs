module user

type Sex = Male | Female

type User() =

    let mutable firstName = ""
    let mutable lastName = ""
    let mutable sex = Male
    let mutable address = ""
    let mutable city = ""
    let mutable country = ""
    let mutable email = ""
    let mutable phone = null

    member this.FirstName with get() = firstName and set(x) = firstName <- x
    member this.LastName with get() = lastName and set(x) = lastName <- x
    member this.Sex with get() = sex and set(x) = sex <- x
    member this.Address with get() = address and set(x) = address <- x
    member this.City with get() = city and set(x) = city <- x
    member this.Country with get() = country and set(x) = country <- x
    member this.Email with get() = email and set(x) = email <- x
    member this.Phone with get() = phone and set(x) = phone <- x
