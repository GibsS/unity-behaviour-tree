using System.Collections.Generic;
using System;

/// <summary>
/// Shared to all nodes and boards, BehaviourTreeContext contains a reference to the agent and to all boards.
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class BehaviourTreeContext<X> {

    public X agent { get; private set; }

    Dictionary<Type, BehaviourTreeBoard<X>> _blackboard;

    public BehaviourTreeContext(X agent) {
        this.agent = agent;

        _blackboard = new Dictionary<Type, BehaviourTreeBoard<X>>();
    }
    
    /// <summary>
    /// Get a board for the AI, created if it is missing
    /// </summary>
    public Y GetBoard<Y>() where Y : BehaviourTreeBoard<X>, new() {
        var type = typeof(Y);

        BehaviourTreeBoard<X> board;

        if(!_blackboard.TryGetValue(type, out board)) {
            board = new Y();
            board._Inject(this);

            _blackboard[type] = board;
        }

        return board as Y;
    }
}