/// <summary>
/// Decorator that always returns SUCCESS, regardless of the children result
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class SuccessNode<X> : DecoratorNode<X> {

    public SuccessNode(BehaviourNode<X> child) : base(child) { }

    public override State CalculateState(State childState) {
        return State.SUCCESS;
    }
}
