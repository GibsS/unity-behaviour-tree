using System.Collections.Generic;

/// <summary>
/// Offer a simple implementation for multi-child composite nodes. When you inherit it, you just need to implement the 
/// function that defines the composite childs. The nodes get executed until one of them fails. If they all execute, 
/// the node sends a SUCCESS, otherwise it sends a FAIURE
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public abstract class ParentBehaviourNode<X> : BehaviourNode<X> {

    IEnumerator<BehaviourNode<X>> _childs;
    BehaviourNode<X> _current;

    public override State Start() {
        _childs = GetChilds().GetEnumerator();

        return ToNextNode();
    }

    public override State Update() {
        var state = _current.Recalculate() ? _current.Start() : _current.Update();

        if (state == State.FAILURE) {
            return CalculateState(State.FAILURE);
        } else if (state == State.SUCCESS) {
            return ToNextNode();
        } else {
            return State.IN_PROGRESS;
        }
    }

    State ToNextNode() {
        while (_childs.MoveNext()) {
            _current = _childs.Current;

            _current._Inject(ctx);
            var state = _current.Start();

            if (state == State.FAILURE) {
                return CalculateState(State.FAILURE);
            } else if (state == State.IN_PROGRESS) {
                return State.IN_PROGRESS;
            }
        }

        return CalculateState(State.SUCCESS);
    }

    /// <summary>
    /// Get the children for this node
    /// </summary>
    public abstract IEnumerable<BehaviourNode<X>> GetChilds();

    /// <summary>
    /// Defines what state to send up based on the result provided by the children (SUCCESS if all the children succeeded, FAILURE otherwise)
    /// </summary>
    public abstract State CalculateState(State childState);
}
