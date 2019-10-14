using System.Collections.Generic;
using System.Collections;

using UnityEngine;

/// <summary>
/// The behaviour tree. Any AI class using the library needs to inherit this class
/// </summary>
/// <typeparam name="X">The agent type</typeparam>
public abstract class BehaviourTree<X> {

    BehaviourNode<X> currentNode;

    BehaviourTreeContext<X> _ctx;

    MonoBehaviour _dependent;
    IEnumerator _routine;

    bool _started;

    /// <summary>
    /// Start the execution of the AI
    /// </summary>
    /// <param name="dependent">The behaviour the AI is attached to. The AI is disposed if the behaviour is destroyed</param>
    /// <param name="agent">The agent the AI is meant to control</param>
    public void Start(MonoBehaviour dependent, X agent) {
        if (_started) return;

        _started = true;

        _dependent = dependent;
        _ctx = new BehaviourTreeContext<X>(agent);

        Resume();
    }

    /// <summary> 
    /// Pause the execution of the AI 
    /// </summary>
    public void Pause() {
        if (_routine == null) return;

        _dependent.StopCoroutine(_routine);
        _routine = null;
    }
    /// <summary>
    /// Resume the execution of the AI if it was paused 
    /// </summary>
    public void Resume() {
        if (_routine != null) return;

        _routine = Routine();
        _dependent.StartCoroutine(_routine);
    }
    /// <summary> 
    /// Stop the execution of the AI and wipes every nodes currently being updated 
    /// </summary>
    public void Stop() {
        Pause();

        currentNode = null;
    }
    /// <summary> 
    /// Restarts the AI 
    /// </summary>
    public void Restart() {
        Pause();

        currentNode = null;

        Resume();
    }

    /// <summary> 
    /// Specify the root node of the tree 
    /// </summary>
    protected abstract BehaviourNode<X> GetRootNode();

    IEnumerator Routine() {
        while(true) {
            if (currentNode == null) {
                DefineNode();
            }

            if (currentNode != null) {
                var state = currentNode.Recalculate() ? currentNode.Start() : currentNode.Update();

                if (state != BehaviourNode<X>.State.IN_PROGRESS) {
                    currentNode = null;
                }
            }

            yield return null;
        }
    }
    
    void DefineNode() {
        currentNode = GetRootNode();

        currentNode._Inject(_ctx);
        var state = currentNode.Start();

        if (state != BehaviourNode<X>.State.IN_PROGRESS) {
            currentNode = null;
            Debug.LogWarning("[BehaviourTree] Execution of the tree was instantaneous");
        }
    }
}