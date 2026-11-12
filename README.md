# Smart Factory

A desktop simulation of a small factory, built with C# and Avalonia. A robot moves between four coloured production lines, picks up bricks, drops them on a delivery line and sends a random share of them to recycling.

It is the semester project for the module Softwaretechnik (Wintersemester 2026, TH Köln). The project grew step by step over the lectures and four lab sessions, and the commit history follows that order. The window labels are in German.

## What it does

- **Production lines.** Four conveyors (red, blue, yellow, green) carry four bricks each. A timer moves the bricks back and forth along their line.
- **Robot control.** The robot moves in eight directions and rotates in 5° steps. A reset button sets the heading back to 0°. Movement is relative to the robot's heading: "up" means forward, not up on the screen.
- **Two movement modes.** In step mode a click moves the robot one step. In continuous mode the robot keeps its last command running until you press stop, reset the rotation or switch modes.
- **Collision.** The robot and the conveyors are treated as rotated rectangles and tested against each other with the separating axis theorem. At a conveyor the robot slides along it instead of stopping dead, and it will not rotate into a conveyor.
- **Docking.** A brick within 10 px of the gripper tip is picked up. The brick then follows every move and rotation of the robot. The robot holds one brick at a time.
- **Delivery and recycling.** A brick can only be released over the delivery line. After the release the robot backs off four steps, and it refuses to release if that move would hit a wall or a conveyor. Each released brick has a 50 % chance of being defective. Defective bricks travel to one of four recycle slots, are repaired there and can be picked up again. Good bricks go to the customer. If all four slots are taken, the brick goes to the customer.
- **Feedback.** After each release a message says what happened: delivered, defective, nothing docked, not over the delivery line, or robot blocked.

## Structure

The code follows the MVVM pattern with CommunityToolkit.Mvvm.

| Folder | Content |
|---|---|
| `Models/` | Game logic with no UI dependency: `Factory`, `Robot`, `LegoBrick`, `ProductionLine`, the shared base class `Beweger` (movement, rotation, docking, collision) and the constants in `Parameters` and `ViewParameters`. |
| `ViewModels/` | `MainViewModel` holds the timers, the movement commands and the release feedback. `LegoBrickViewModel` wraps one brick for the view. |
| `Views/` | `MainWindow.axaml` draws the factory on a canvas and the control panel next to it. The code-behind only forwards button clicks to the view model. |
| `Assets/` | Factory plan, conveyors, robot and arrows as PNG files. |

## Built with

- C# on .NET 10
- Avalonia 12.1.3 (Fluent theme)
- CommunityToolkit.Mvvm 8.4.2

## Run it

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/akievi/smartFactoryApp.git
cd smartFactoryApp
dotnet run
```

## Controls

| Control | Action |
|---|---|
| Start / Stop (Bänder) | Start or stop the conveyors |
| Schritt / Kontinuierlich | Switch between step and continuous movement |
| Arrow pad | Move the robot in eight directions, the centre button stops it |
| ↺ / ↻ | Rotate counterclockwise or clockwise |
| 0° | Reset the heading |
| Loslassen | Release the brick over the delivery line |
