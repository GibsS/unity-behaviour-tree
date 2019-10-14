/// <summary>
/// Inherit to implement a dynamically defined sequence using the C# IEnumerable function syntax.
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public abstract class ComplexSequenceNode<X> : ParentBehaviourNode<X> {

    public override State CalculateState(State childState) {
        return childState;
    }
}