using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeTests : MonoBehaviour {

    List<BehaviourNode<BehaviourTreeTests>> _nodesToTest;
    int _idx = -1;

    BehaviourTree<BehaviourTreeTests> _currentTree;

    void Start() {
        _nodesToTest = new List<BehaviourNode<BehaviourTreeTests>> {
            new TestWaitNode(),
            new TestInvertorNode(),
            new TestSuccessNode(),
            new TestFailureNode(),
            new TestBoardNode(),
            new TestSequenceNode1(),
            new TestSequenceNode2(),
            //new TestFail()
        };

        RunNext();
    }

    public void Success() {
        Debug.Log("Test " + _nodesToTest[_idx].GetType().Name + " succeeded");

        RunNext();
    }
    public void Failure() {
        Debug.Log("Test " + _nodesToTest[_idx].GetType().Name + " failed");

        RunNext();
    }

    void RunNext() {
        _idx++;
        if(_idx >= _nodesToTest.Count) {
            Debug.Log("All " + _nodesToTest.Count + " tests are done");

            _currentTree.Stop();
            return;
        }

        var node = _nodesToTest[_idx];

        if (_currentTree != null) _currentTree.Stop();

        _currentTree = new TestTree<BehaviourTreeTests>(node);
        _currentTree.Start(this, this);
    }

    class TestTree<X> : BehaviourTree<X> {

        BehaviourNode<X> _node;

        public TestTree(BehaviourNode<X> node) {
            _node = node;
        }

        protected override BehaviourNode<X> GetRootNode() {
            return _node;
        }
    }

    class TestWaitNode : ComplexSequenceNode<BehaviourTreeTests> {

        public override bool Recalculate() {
            return false;
        }

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            var time = Time.time;
            yield return new WaitNode<BehaviourTreeTests>(1);

            if (Time.time - time > 0.99f) {
                ctx.agent.Success();
            } else {
                ctx.agent.Failure();
            }
        }
    }
    class TestInvertorNode : ComplexSequenceNode<BehaviourTreeTests> {

        public static int count;

        public override bool Recalculate() {
            return false;
        }

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            count++;

            if (count > 1) ctx.agent.Failure();

            yield return new InvertorNode<BehaviourTreeTests>(new HelperConstantNode(State.FAILURE));

            ctx.agent.Success();
        }
    }
    class TestFailureNode : ComplexSequenceNode<BehaviourTreeTests> {

        public static int count;

        public override bool Recalculate() {
            return false;
        }

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            count++;

            if(count > 1) ctx.agent.Failure();

            yield return new InvertorNode<BehaviourTreeTests>(new FailureNode<BehaviourTreeTests>(new HelperConstantNode(State.SUCCESS)));

            ctx.agent.Success();
        }
    }
    class TestSuccessNode : ComplexSequenceNode<BehaviourTreeTests> {

        public static int count;

        public override bool Recalculate() {
            return false;
        }

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            count++;

            if (count > 1) ctx.agent.Failure();

            yield return new SuccessNode<BehaviourTreeTests>(new HelperConstantNode(State.FAILURE));

            ctx.agent.Success();
        }
    }
    class TestBoardNode : ComplexSequenceNode<BehaviourTreeTests> {

        public override bool Recalculate() {
            return false;
        }

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            const int COUNT = 3;

            for (int i = 0; i < COUNT; i++) {
                yield return new IncrementNode();
            }

            if(COUNT == ctx.GetBoard<Board>().count) {
                ctx.agent.Success();
            } else {
                ctx.agent.Failure();
            }
        }
    }
    class TestSequenceNode1 : ComplexSequenceNode<BehaviourTreeTests> {

        public override bool Recalculate() {
            return false;
        }

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            yield return new SequenceNode<BehaviourTreeTests>(
                new IncrementNode(),
                new IncrementNode(),
                new IncrementNode(),
                new IncrementNode()
            );

            if(ctx.GetBoard<Board>().count == 4) {
                ctx.agent.Success();
            } else {
                ctx.agent.Failure();
            }
        }
    }
    class TestSequenceNode2 : ComplexSequenceNode<BehaviourTreeTests> {

        static int count;

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            count++;

            if(count > 1) ctx.agent.Success();

            yield return new SequenceNode<BehaviourTreeTests>(
                new IncrementNode(),
                new IncrementNode(),
                new HelperConstantNode(State.FAILURE),
                new IncrementNode()
            );

            ctx.agent.Failure();
        }
    }
    class TestFail : ComplexSequenceNode<BehaviourTreeTests> {

        public override IEnumerable<BehaviourNode<BehaviourTreeTests>> GetChilds() {
            ctx.agent.Failure();

            yield break;
        }
    }

    class IncrementNode : BehaviourNode<BehaviourTreeTests> {

        public override bool Recalculate() {
            return false;
        }

        public override State Start() {
            return State.IN_PROGRESS;
        }

        public override State Update() {
            var board = ctx.GetBoard<Board>();

            board.count++;

            return State.SUCCESS;
        }
    }

    class Board : BehaviourTreeBoard<BehaviourTreeTests> {

        public int count;
    }

    class HelperConstantNode : BehaviourNode<BehaviourTreeTests> {

        State _state;

        public HelperConstantNode(State state) {
            _state = state;
        }

        public override bool Recalculate() {
            return false;
        }

        public override State Start() {
            return State.IN_PROGRESS;
        }

        public override State Update() {
            return _state;
        }
    }
}