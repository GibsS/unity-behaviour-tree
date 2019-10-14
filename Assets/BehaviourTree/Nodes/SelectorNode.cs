using System;

/// <summary>
/// Contains a node and a predicate that determines whether the node should be excuted or not
/// </summary>
/// <typeparam name="X">the agent type</typeparam>
public class Selection<X> {

    public Func<BehaviourTreeContext<X>, bool> shouldPick;
    public BehaviourNode<X> node;

    public Selection(Func<BehaviourTreeContext<X>, bool> shouldPick, BehaviourNode<X> node) {
        this.shouldPick = shouldPick;
        this.node = node;
    }
}

/// <summary>
/// Node that selects between several possible nodes based on predicates provided for each of them. 
/// The selector checks every node until one of them should be picked, if none can be picked, returns SUCCESS
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class SelectorNode<X> : BehaviourNode<X> {

    Selection<X>[] _childs;

    BehaviourNode<X> _currentNode;

    public SelectorNode(params Selection<X>[] childs) {
        _childs = childs;
    }

    public override State Start() {
        return DefineNode();
    }

    public override State Update() {
        return _currentNode.Recalculate() ? _currentNode.Start() : _currentNode.Update();
    }

    public override bool Recalculate() {
        foreach(var s in _childs) {
            var pick = s.shouldPick(ctx);
            var current = s.node == _currentNode;

            if(pick) {
                if(current) {
                    return false;
                } else {
                    return true;
                }
            }
        }

        return false;
    }

    State DefineNode() {
        foreach (var s in _childs) {
            var pick = s.shouldPick(ctx);

            if (pick) {
                _currentNode = s.node;
                _currentNode._Inject(ctx);

                var state = _currentNode.Start();

                return state;
            }
        }

        return State.SUCCESS;
    }
}
