using System.Collections.Generic;

/// <summary>
/// Parent to a single node, a DecoratorNode processes the result from its child and chooses what it decides to send up
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public abstract class DecoratorNode<X> : ParentBehaviourNode<X> {

    BehaviourNode<X> _child;

    public DecoratorNode(BehaviourNode<X> child) {
        _child = child;
    }

    public override bool Recalculate() {
        return false;
    }

    public override IEnumerable<BehaviourNode<X>> GetChilds() {
        yield return _child;
    }
}
