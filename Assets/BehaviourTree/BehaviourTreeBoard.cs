/// <summary>
/// Aka blackboard. It allows the storage and processing of information outside of nodes 
/// so they can remain stateless and share information easily (for example enemy positions, danger level, ...)
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class BehaviourTreeBoard<X> {

    protected BehaviourTreeContext<X> ctx;

    /// <summary> Called to inject the board with the agent information </summary>
    public void _Inject(BehaviourTreeContext<X> ctx) {
        this.ctx = ctx;
    }
}