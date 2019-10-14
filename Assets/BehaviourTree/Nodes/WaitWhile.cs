using System;

/// <summary>
/// Wait until a certain condition is met
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class WaitWhile<X> : BehaviourNode<X> {

    Func<BehaviourTreeContext<X>, bool> _condition;

    public WaitWhile(Func<BehaviourTreeContext<X>, bool> condition) {
        _condition = condition;
    }

    public override State Start() {
        return _condition(ctx) ? State.IN_PROGRESS : State.SUCCESS;
    }

    public override State Update() {
        return _condition(ctx) ? State.IN_PROGRESS : State.SUCCESS;
    }

    public override bool Recalculate() {
        return false;
    }
}