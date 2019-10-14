/// <summary>
/// The base class for any behaviour tree node
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public abstract class BehaviourNode<X> {

    protected BehaviourTreeContext<X> ctx;

    /// <summary> Called to inject the node with the agent information </summary>
    public void _Inject(BehaviourTreeContext<X> ctx) {
        this.ctx = ctx;
    }

    /// <summary> Called when the node is entered </summary>
    public abstract State Start();

    /// <summary> Called every frame if the node part of the behaviour trees active "stack" of nodes </summary>
    public abstract State Update();

    /// <summary> Return true if the node should be restarted. !! Needs to be called on every frame by its parent and restart if necessary !! </summary>
    public virtual bool Recalculate() {
        return false;
    }

    public enum State {
        IN_PROGRESS,
        SUCCESS,
        FAILURE
    }
}