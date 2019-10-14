using System;

/// <summary>
/// Wait until a certain condition is met
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class WaitUntil<X> : BehaviourNode<X> {

    Func<BehaviourTreeContext<X>, bool> _endCondition;

    public WaitUntil(Func<BehaviourTreeContext<X>, bool> endCondition) {
        _endCondition = endCondition;
    }

    public override State Start() {
        return _endCondition(ctx) ? State.SUCCESS : State.IN_PROGRESS;
    }

    public override State Update() {
        return _endCondition(ctx) ? State.SUCCESS : State.IN_PROGRESS;
    }

    public override bool Recalculate() {
        return false;
    }
}
