module main

open System
open System.Drawing
open System.Drawing.Drawing2D
open System.Runtime.InteropServices
open System.Threading
open System.Windows.Forms

open calendar
open user
open trip

[<DllImport("user32.dll")>]
extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam)

type ControlDelegate = delegate of Control[] -> unit

type ThreadSafeForm() as this =
    inherit Form()

    let mutable suspendCounter = 0

    do
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer ||| ControlStyles.UserPaint ||| ControlStyles.AllPaintingInWmPaint,true)
    
    member this.Suspend() =
    
        if(suspendCounter = 0) then
            SendMessage(this.Handle, 0x000B, IntPtr.Zero, IntPtr.Zero) |> ignore
            suspendCounter <- suspendCounter + 1


    member this.Resume() =

        suspendCounter <- suspendCounter - 1 

        if (suspendCounter = 0) then
            SendMessage(this.Handle, 0x000B, new IntPtr(1), IntPtr.Zero) |> ignore
            this.Refresh()

    member this.ClearControls() =

        if (this.InvokeRequired) then
            let delegateMethod = new MethodInvoker(this.ClearControls)
            this.Invoke(delegateMethod) |> ignore
        
        else
            this.Suspend()
            this.Controls.Clear()
            this.Resume()
            
    member this.RemoveControls(controls:Control[]) =

        if (this.InvokeRequired) then
            let delegateMethod = new ControlDelegate(this.RemoveControls)
            this.Invoke(delegateMethod, controls) |> ignore
        
        else
            this.Suspend()
            for control in controls do
                if this.Controls.Contains(control) then
                    this.Controls.Remove(control)
            this.Resume()

    member this.AddControls(controls:Control[]) =

        if (this.InvokeRequired) then
            let delegateMethod = new ControlDelegate(this.AddControls)
            this.Invoke(delegateMethod, controls) |> ignore
        
        else            
            this.Suspend()
            let controlCount = this.Controls.Count
            this.Controls.AddRange(controls)
            for i in 0 .. controlCount-1 do
                this.Controls.Item(i).SendToBack()
            this.Resume()

    member this.ReplaceControls(controls:Control[]) =

        if (this.InvokeRequired) then
            let delegateMethod = new ControlDelegate(this.ReplaceControls)
            this.Invoke(delegateMethod, controls) |> ignore
        
        else            
            this.Suspend()
            this.Controls.Clear()
            this.Controls.AddRange(controls)
            this.Resume()

   
let form = new ThreadSafeForm(
                    Text = "TravelPolp",
                    WindowState=FormWindowState.Maximized)

form.Controls.AddRange(calendarButtons)
form.Controls.Add(calendarMonthBackwardButton)
form.Controls.Add(calendarMonthForwardButton)

calculateCalendarMonth()

calculateCalendarTrips()

calendarMonthBackwardButton.Click.Add(fun _ ->
                                        if selectedDay.Date.AddMonths(-1) >= startDate then
                                            selectedDay <- new Day(selectedDay.Date.AddMonths(-1))
                                            calculateCalendarMonth()
                                            calculateCalendarTrips()
                                            form.Invalidate()
)
calendarMonthForwardButton.Click.Add(fun _ ->
                                        if selectedDay.Date.AddMonths(1) <= endDate then
                                            selectedDay <- new Day(selectedDay.Date.AddMonths(1))
                                            calculateCalendarMonth()
                                            calculateCalendarTrips()
                                            form.Invalidate()
)

[<STAThread>]
Application.EnableVisualStyles();
Application.Run(form)