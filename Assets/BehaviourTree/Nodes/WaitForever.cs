/// <summary>
/// Pause forever
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class WaitForever<X> : WaitNode<X> {

    public WaitForever() : base(100000000000) { }
}
