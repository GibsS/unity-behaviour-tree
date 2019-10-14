# Unity Behaviour Tree

A convenient library to make behaviour trees in Unity3D

```C#
public class SomeNode : ComplexSequenceNode<Agent> {

  public override IEnumerable<BehaviourNode<Agent>> GetChilds() {
  
    yield return new WaitNode<BehaviourTreeTests>(1);
    
    // A second has passed
    
    // We can call a function on the agent
    ctx.agent.FireWeapon();

    // The weapon was fired
    // We can write some decision code here
    if (ctx.agent.IsInDanger) {
      // It's dangerous here, let's flee!
      yield return new FleeNode();
    } else {
      // Coast is clear, keep moving
      yield return new AdvanceNode();
    }
  }
}
```

## How to install

- Download this [unity package](https://github.com/GibsS/unity-behaviour-tree/releases/download/v1.0/behaviour-tree.unitypackage), it contains the source code for the package
- Import it in your project. 
- Rename and move the base folder at your convenience.

## Features

This is a library that allows you to create complex behaviour tree with ease. Nothing particularly fancy, it just gets the job done.

If you want a lesson on how behaviour trees work, check [this article out](https://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php). We don't use the exact same  language but the principles are the same and they explain behaviour trees very well.

To create an AI, make a class that inherit ```BehaviourTree``` and specify the RootNode. From there you just have to make a tree out of
the existing nodes or your custom nodes. See below for a list of the base nodes. The behaviour tree will execute the root node in a loop until you stop it.

One of the 'innovation' of this library is the use of C# IEnumerable function syntax to write complex sequences (I haven't seen this  anywhere else at least..). Traditional behaviour tree just let you write static sequences of nodes and you have to mess around with selectors to make anything more complicated. You can do stuff like the example above.

Neat right? It allows you to keep your behaviour trees a bit lighter and more readable than traditional behaviour trees.

## Create a behaviour tree

In this library, an "Agent" represents an entity the AI is meant to control. To create an AI that controls said Agent, write something like this:

```C#
public class YourAgent : MonoBehaviour {

  void Start() {
    // Create an AI
    var tree = new YourTree();
    
    // Call start to actually run the AI.
    // The first argument is the MonoBehaviour the tree attaches to and depends on (like a coroutine), 
    // the second is the agent to control
    tree.Start(this, this);
  }
}

class YourTree : BehaviourTree<BehaviourTreeTests> {

    protected override BehaviourNode<BehaviourTreeTests> GetRootNode() {
        return new YourRootNode();
    }
}
```

## Create your own node

Most 'structural node' (sequences, selectors, decorators..) are provided with the library so you'll only have to implement leaf nodes: the nodes that make your agent perfom actions.

To create your own node, you just need to create a class that implements ```BehaviourNode```:

```C#
public class YourNode : BehaviourNode<Agent> {
  
  // Called when the node is entered
  public State Start() {
    return State.IN_PROGRESS;
  }
  // Called every frame while the node is active
  public State Update() {
    // DO STUFF HERE
    if(Done()) {    
      return State.SUCCESS;
    } else {
      return State.IN_PROGRESS;
    }
  }
  
  ...
}

```

You can access your agent through ```ctx.agent``` inside your inherited class.

If you wish to create your own decorator or composite nodes, we recommend you look at the ```DecoratorNode``` and ```ParentBehaviourNode``` classes and inherit them.

## Boards (aka Blackboards)

These objects are shared between every node of your tree. They allow you to share information and calculation between the different nodes and they help keep the nodes stateless.

To define a board, write a class like this:

```C#
public class YourBoard : BehaviourTreeBoard<Agent> {

  public int someDataToShare;
}
```

To create and use it, write:

```C#
// Inside your node
var board = ctx.GetBoard<YourBoard>();

board.someDataToShare++;
```

The 'GetBoard' function will create the board if it doesn't exist yet.

## Nodes

The main nodes provided with this library:

*ComplexSequenceNode*

This is the type of node you'll use the most, it allows you to specify a 'dynamic' list of child nodes. If one of the child returns a FAILURE, the node returns a FAILURE. Otherwise it returns a SUCCESS.

```C#
public class SomeNode : ComplexSequenceNode<Agent> {

  public override IEnumerable<BehaviourNode<Agent>> GetChilds() {
  
    yield return new WaitNode<BehaviourTreeTests>(1);
    
    // A second has passed
    
    // We can call a function on the agent
    ctx.agent.FireWeapon();

    // The weapon was fired
    // We can write some decision code here
    if (ctx.agent.IsInDanger) {
      // It's dangerous here, let's flee!
      yield return new FleeNode();
    } else {
      // Coast is clear, keep moving
      yield return new AdvanceNode();
    }
  }
}
```

*SequenceNode*

A classic sequence. Simply executes all of it's children (specified in the constructor). If one of them returns a FAILURE, it returns a FAILURE, otherwise it returns a SUCCESS.


```C#
var sequence = new SequenceNode<Agent>(
  new SomeNode(),
  new SomeOtherNode(),
  new SomeNode()
);

```

*SelectorNode*

The ```SelectorNode``` is provided a list of nodes and a predicate for each node. On every frame, the selector will loop through all the nodes and pick the first one which has a predicate that returns true. It returns the same state as its selected child.

```C#
var selector = new SelectorNode<Agent>(
  new Selection<Agent>(ctx => PickIfInDanger(), new WhenInDangerNode()),
  new Selection<Agent>(ctx => PickIfSafe(), new WhenSafeNode())
)
```

*InvertorNode*

The ```InverterNode``` is given a child node and returns the IN_PROGRESS if the child returns IN_PROGRESS and the opposite otherwise: if the child returns SUCCESS, it will return FAILURE and vice versa.

*FailureNode*

The ```FailureNode```is given a child node and returns the IN_PROGRESS if the child returns IN_PROGRESS and FAILURE otherwise, regardless of if the child returns FAILURE or SUCCESS.

*SuccessNode*

The ```SuccessNode```is given a child node and returns the IN_PROGRESS if the child returns IN_PROGRESS and SUCCESS otherwise, regardless of if the child returns FAILURE or SUCCESS.

*WaitNode*

Pauses the AI for a certain duration.

*WaitForever*

Pauses the AI until this node is no longer activated (can be deactivated due to a selector change for example).

*WaitUntil*

Pauses until a provided condition is met.

*WaitWhile*

Pauses while a provided condition is met.
