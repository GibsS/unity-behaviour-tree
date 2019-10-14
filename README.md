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

This is a library that allows the creation of complex behaviour trees in code with ease. Nothing particularly fancy, it just gets the job done.

To create an AI, make a class that inherit ```BehaviourTree``` and specify the RootNode. From there you just have to make a tree out of
the existing nodes or your custom nodes. See below for a list of the base nodes. The behaviour tree will execute the root node in a loop until you stop it.

One of the 'innovation' of this library is the use of C# IEnumerable function syntax to write complex sequences (I haven't seen anywhere else at least..). Traditional behaviour tree just let you write sequences and you have to mess around with selectors to make anything more complicated. With this library you can do stuff like the example above.

Neat right? It allows you to keep your behaviour trees a bit lighter and readable than traditional behaviour trees.

## Nodes

*ComplexSequenceNode*

This is the main type of node you'll use the most, it allows to specify a 'dynamic' list of child nodes.

