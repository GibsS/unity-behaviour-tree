using System.Collections.Generic;

/// <summary>
/// A simple sequence of nodes: provide a list of nodes and they will be executed in a sequence. If on of them returns a FAILURE, the node will return a FAILURE.
/// Otherwise, at the end of the sequence, returns a SUCCESS
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class SequenceNode<X> : ParentBehaviourNode<X> {

    BehaviourNode<X>[] _childs;

    public SequenceNode(params BehaviourNode<X>[] childs) {
        _childs = childs;
    }

    public override bool Recalculate() {
        return false;
    }

    public override IEnumerable<BehaviourNode<X>> GetChilds() {
        return _childs;
    }

    public override State CalculateState(State childState) {
        return childState;
    }
}