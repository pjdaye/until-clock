using UntilClock.ViewModels;

namespace UntilClock.Tests;

public class SetTargetFlowTests
{
    private static (MainViewModel vm, FakePersistenceService persistence, FakeTimerService timer)
        MakeVm(DateTime? dialogResult = null)
    {
        var timer = new FakeTimerService();
        var persistence = new FakePersistenceService();
        var vm = new MainViewModel(timer, persistence, new FakeDialogService(dialogResult));
        return (vm, persistence, timer);
    }

    [Fact]
    public void SetTargetCommand_Confirm_UpdatesTargetDateTime()
    {
        var newTarget = DateTime.Now.AddDays(10);
        var (vm, _, _) = MakeVm(newTarget);

        vm.SetTargetCommand.Execute(null);

        Assert.Equal(newTarget, vm.TargetDateTime);
    }

    [Fact]
    public void SetTargetCommand_Confirm_ResetsCountdownStartDateTime()
    {
        var newTarget = DateTime.Now.AddDays(10);
        var (vm, _, _) = MakeVm(newTarget);
        var beforeSet = DateTime.Now;

        vm.SetTargetCommand.Execute(null);

        Assert.True(vm.CountdownStartDateTime >= beforeSet);
    }

    [Fact]
    public void SetTargetCommand_Confirm_SavesPersistence()
    {
        var newTarget = DateTime.Now.AddDays(10);
        var (vm, persistence, _) = MakeVm(newTarget);
        persistence.SaveCalled = false;

        vm.SetTargetCommand.Execute(null);

        Assert.True(persistence.SaveCalled);
    }

    [Fact]
    public void SetTargetCommand_Cancel_DoesNotChangeTarget()
    {
        var (vm, persistence, _) = MakeVm(null); // null = cancel
        var originalTarget = vm.TargetDateTime;
        persistence.SaveCalled = false;

        vm.SetTargetCommand.Execute(null);

        Assert.Equal(originalTarget, vm.TargetDateTime);
        Assert.False(persistence.SaveCalled);
    }

    [Fact]
    public void IsComplete_False_WhenTargetInFuture()
    {
        var (vm, _, _) = MakeVm();
        // default target is 1h from now
        Assert.False(vm.IsComplete);
    }

    [Fact]
    public void SetTargetCommand_PastDate_SetsIsComplete()
    {
        var pastDate = DateTime.Now.AddSeconds(-1);
        var (vm, _, _) = MakeVm(pastDate);

        vm.SetTargetCommand.Execute(null);

        Assert.True(vm.IsComplete);
    }
}
