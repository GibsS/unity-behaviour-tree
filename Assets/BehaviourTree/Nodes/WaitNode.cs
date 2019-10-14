using UnityEngine;

/// <summary>
/// Pause for a certain duration (time based on UnityEngine.Time.time)
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public class WaitNode<X> : BehaviourNode<X> {

    float _duration;

    float _endAtTime;

    public WaitNode(float duration) {
        _duration = duration;
    }

    public override State Start() {
        _endAtTime = Time.time + _duration;

        return State.IN_PROGRESS;
    }

    public override State Update() {
        return Time.time > _endAtTime ? State.SUCCESS : State.IN_PROGRESS;
    }

    public override bool Recalculate() {
        return false;
    }
}