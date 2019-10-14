/// <summary>
/// Decorator that inverts the childs result (SUCCESS if the child returns a FAILURE, FAILURE if the child returns a SUCCESS)
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class InvertorNode<X> : DecoratorNode<X> {

    public InvertorNode(BehaviourNode<X> child) : base(child) { }

    public override State CalculateState(State childState) {
        return childState == State.SUCCESS ? State.FAILURE : State.SUCCESS;
    }
}
