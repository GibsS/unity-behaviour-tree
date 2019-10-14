/// <summary>
/// Decorator that always returns FAILURE, regardless of the children result
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class FailureNode<X> : DecoratorNode<X> {

    public FailureNode(BehaviourNode<X> child) : base(child) { }

    public override State CalculateState(State childState) {
        return State.FAILURE;
    }
}
