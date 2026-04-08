using UntilClock.Models;
using UntilClock.Services;
using UntilClock.ViewModels;

namespace UntilClock.Tests;

public class MainViewModelStartupTests
{
    [Fact]
    public void Constructor_WithNoSavedData_SetsIsFirstLaunchTrue()
    {
        var vm = new MainViewModel(new FakeTimerService(), new FakePersistenceService(), new FakeDialogService());
        Assert.True(vm.IsFirstLaunch);
    }

    [Fact]
    public void Constructor_WithSavedData_RestoresTargetDateTime()
    {
        var persistence = new FakePersistenceService();
        var target = DateTime.Now.AddDays(7);
        var start = DateTime.Now.AddDays(-1);
        persistence.Save(new CountdownData { TargetDateTime = target, CountdownStartDateTime = start });

        var vm = new MainViewModel(new FakeTimerService(), persistence, new FakeDialogService());

        Assert.False(vm.IsFirstLaunch);
        Assert.Equal(target, vm.TargetDateTime);
        Assert.Equal(start, vm.CountdownStartDateTime);
    }

    [Fact]
    public void Reset_SavesData()
    {
        var persistence = new FakePersistenceService();
        persistence.Save(new CountdownData { TargetDateTime = DateTime.Now.AddDays(1), CountdownStartDateTime = DateTime.Now });
        var vm = new MainViewModel(new FakeTimerService(), persistence, new FakeDialogService());

        persistence.SaveCalled = false;
        vm.ResetCommand.Execute(null);

        Assert.True(persistence.SaveCalled);
    }

    [Fact]
    public void SettingTargetDateTime_SavesData()
    {
        var persistence = new FakePersistenceService();
        var vm = new MainViewModel(new FakeTimerService(), persistence, new FakeDialogService());

        persistence.SaveCalled = false;
        vm.TargetDateTime = DateTime.Now.AddDays(3);

        Assert.True(persistence.SaveCalled);
    }

    [Fact]
    public void TimerTick_UpdatesRemainingAndDisplayText()
    {
        var timer = new FakeTimerService();
        var vm = new MainViewModel(timer, new FakePersistenceService(), new FakeDialogService());

        timer.FireTick();
        Assert.NotNull(vm.DisplayText);
        Assert.Matches(@"^\d{2}:\d{2}:\d{2}$|^\d+d \d{2}:\d{2}:\d{2}$", vm.DisplayText);
    }
}
