using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    Factory factory;
    DispatcherTimer timer;
    [ObservableProperty] private string undockInfo = "";
    public ObservableCollection<LegoBrickViewModel> RedBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public ObservableCollection<LegoBrickViewModel> BlueBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public ObservableCollection<LegoBrickViewModel> YellowBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public ObservableCollection<LegoBrickViewModel> GreenBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public int ProductionRedX => factory.ProductionRedX;
    public int ProductionRedY => factory.ProductionRedY;
    public double ProductionRedAngle => factory.ProductionRedAngle;
    public int ProductionBlueX => factory.ProductionBlueX;
    public int ProductionBlueY => factory.ProductionBlueY;
    public double ProductionBlueAngle => factory.ProductionBlueAngle;
    public int ProductionYellowX => factory.ProductionYellowX;
    public int ProductionYellowY => factory.ProductionYellowY;
    public double ProductionYellowAngle => factory.ProductionYellowAngle;
    public int ProductionGreenX => factory.ProductionGreenX;
    public int ProductionGreenY => factory.ProductionGreenY;
    public double ProductionGreenAngle => factory.ProductionGreenAngle;
    public int ProductionRecycleX => factory.ProductionRecycleX;
    public int ProductionRecycleY => factory.ProductionRecycleY;
    public double ProductionRecycleAngle => factory.ProductionRecycleAngle;
    public int ProductionDeliveryX => factory.ProductionDeliveryX;
    public int ProductionDeliveryY => factory.ProductionDeliveryY;
    public double ProductionDeliveryAngle => factory.ProductionDeliveryAngle;
    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double RobotRotationAngle => factory.RobotRotationAngle;
    public int ToRecycleX => ViewParameters.ToRecycleLinePosition.X;
    public int ToRecycleY => ViewParameters.ToRecycleLinePosition.Y;
    public int ToCustomerX => ViewParameters.ToCustomerLinePosition.X;
    public int ToCustomerY => ViewParameters.ToCustomerLinePosition.Y;
    public MainViewModel()
    {
        factory = new Factory();
        foreach (var brick in factory.RedBricks)
        {
            RedBricks.Add(new LegoBrickViewModel(brick));
        }
        foreach (var brick in factory.BlueBricks)
        {
            BlueBricks.Add(new LegoBrickViewModel(brick));
        }
        foreach (var brick in factory.GreenBricks)
        {
            GreenBricks.Add(new LegoBrickViewModel(brick));
        }
        foreach (var brick in factory.YellowBricks)
        {
            YellowBricks.Add(new LegoBrickViewModel(brick));
        }
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
        timer.Tick += Timer_Tick;
        robotTimer = new DispatcherTimer();
        robotTimer.Interval = TimeSpan.FromMilliseconds(robotTimerInterval);
        robotTimer.Tick += RobotTimer_Tick;
    }
    // Eigener Timer fuer die kontinuierliche Roboterbewegung, unabhaengig vom Baender-Timer
    private DispatcherTimer robotTimer;
    // Aktueller Fahrbefehl des Roboters: die Richtungen, die er bei jedem Roboter-Tick ausfuehrt.
    // Eine Diagonale besteht aus zwei Richtungen (z.B. up + left).
    private List<Direction> fahrbefehl = new List<Direction>();

    // Bewegungsmodus, umschaltbar ueber die zwei RadioButtons im Panel
    private bool kontinuierlich = false;
    public bool Schrittbewegung
    {
        get { return !kontinuierlich; }
        set
        {
            if (value)
            {
                ModusSetzen(false);
            }
        }
    }
    public bool KontinuierlicheBewegung
    {
        get { return kontinuierlich; }
        set
        {
            if (value)
            {
                ModusSetzen(true);
            }
        }
    }
    // Beim Umschalten haelt der Roboter immer an, damit kein alter Befehl im neuen Modus weiterlaeuft
    private void ModusSetzen(bool neuKontinuierlich)
    {
        Stop();
        kontinuierlich = neuKontinuierlich;
        OnPropertyChanged(nameof(Schrittbewegung));
        OnPropertyChanged(nameof(KontinuierlicheBewegung));
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksMove();
        RobotPoseChanged();   // ein Stein kann gerade gegriffen worden sein; ruft auch BricksPoseChanged auf
    }
    private void RobotTimer_Tick(object? sender, EventArgs e)
    {
        BefehlAusfuehren(fahrbefehl);
        RobotPoseChanged();
    }
    public void StartTimer()
    {
        timer.Start();
    }
    public void StopTimer()
    {
        timer.Stop();
    }
    // Stop-Button: kontinuierliche Roboterbewegung beenden, die Baender laufen weiter
    public void Stop()
    {
        robotTimer.Stop();
        fahrbefehl.Clear();
    }
    public void MoveUp()
    {
        BefehlSetzen(Direction.up);
    }
    public void MoveDown()
    {
        BefehlSetzen(Direction.down);
    }
    public void MoveLeft()
    {
        BefehlSetzen(Direction.left);
    }
    public void MoveRight()
    {
        BefehlSetzen(Direction.right);
    }
    public void MoveUpLeft()
    {
        BefehlSetzen(Direction.up, Direction.left);
    }
    public void MoveUpRight()
    {
        BefehlSetzen(Direction.up, Direction.right);
    }
    public void MoveDownLeft()
    {
        BefehlSetzen(Direction.down, Direction.left);
    }
    public void MoveDownRight()
    {
        BefehlSetzen(Direction.down, Direction.right);
    }
    public void RotateRight()
    {
        BefehlSetzen(Direction.clockwise);
    }
    public void RotateLeft()
    {
        BefehlSetzen(Direction.counterclockwise);
    }
    // Kontinuierlicher Modus: Befehl speichern (ersetzt den vorherigen) und den Roboter-Timer starten,
    // der Roboter faehrt dann bis Stop, 0° oder Moduswechsel weiter.
    // Schrittmodus: genau ein Schritt pro Klick.
    private void BefehlSetzen(params Direction[] richtungen)
    {
        if (kontinuierlich)
        {
            fahrbefehl = new List<Direction>(richtungen);
            robotTimer.Start();
        }
        else
        {
            BefehlAusfuehren(richtungen);
            RobotPoseChanged();
        }
    }
    // Fuehrt einen Schritt pro Richtung aus: Drehrichtungen drehen, alle anderen fahren
    private void BefehlAusfuehren(IEnumerable<Direction> richtungen)
    {
        foreach (Direction richtung in richtungen)
        {
            if (richtung == Direction.clockwise || richtung == Direction.counterclockwise)
            {
                factory.RobotRotate(richtung);
            }
            else
            {
                factory.RobotMove(richtung);
            }
        }
    }
    public void ResetRotation()
    {
        Stop(); // 0° beendet eine laufende kontinuierliche Bewegung
        factory.RobotResetRotation();
        RobotPoseChanged();
    }
    public void Undock()
    {
        UndockResult ergebnis = factory.RobotUndock();
        if (ergebnis == UndockResult.delivered)
        {
            UndockInfo = "Brick delivered, going to customer";
        }
        else if (ergebnis == UndockResult.defect)
        {
            UndockInfo = "Brick is defective, going to recycling";
        }
        else if (ergebnis == UndockResult.noBrickDocked)
        {
            UndockInfo = "No brick docked: pick up a brick first";
        }
        else if (ergebnis == UndockResult.notOnDeliveryLine)
        {
            UndockInfo = "Brick is not on the delivery line: move it there first";
        }
        else if (ergebnis == UndockResult.blocked)
        {
            UndockInfo = "Robot is blocked";
        }
        RobotPoseChanged();
    }
    private void BricksPoseChanged()
    {
        foreach (var brick in RedBricks) brick.Refresh();
        foreach (var brick in BlueBricks) brick.Refresh();
        foreach (var brick in YellowBricks) brick.Refresh();
        foreach (var brick in GreenBricks) brick.Refresh();
    }
    private void RobotPoseChanged()
    {
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
        OnPropertyChanged(nameof(RobotRotationAngle));
        // ein gegriffener Stein bewegt sich mit dem Roboter, also auch die Steine neu anzeigen
        BricksPoseChanged();
    }
}
