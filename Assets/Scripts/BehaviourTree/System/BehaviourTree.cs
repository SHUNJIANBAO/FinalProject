using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    public NodeAssets NodeAssets;
    ActionNode _curNode;

    public float Interval = 0f;
    float _timeCount;

    private void Start()
    {
        if (NodeAssets != null)
        {
            NodeAssets.RootNode.SetTree(this);
            var owner = GetComponentInChildren<Character>();
            if (owner == null) Debug.LogError("没有获取到Character");
            NodeAssets.RootNode.SetCharacter(owner);
            NodeAssets.RootNode.Init();
        }
    }

    private void Update()
    {
        if (RuntimeTest.Instance.CloseAI)
        {
            return;
        }
        if (NodeAssets != null)
        {
            if (NodeAssets.RootNode == null) return;
            _timeCount += Time.deltaTime;
            if (_timeCount > Interval)
            {
                _timeCount = 0;
                E_NodeStatus result = NodeAssets.RootNode.GetTrick();
                if (result == E_NodeStatus.Failure)
                {
                    SetCurNode(null);
                }
            }

            if (_curNode != null)
            {
                _curNode.OnStay();
            }
        }
    }

    public void SetCurNode(NodeBase node)
    {
        if (node == null || node is ActionNode)
        {
            if (node == _curNode)
            {
                return;
            }
            if (_curNode != null) _curNode.OnExit();
            _curNode = node as ActionNode;
            if (_curNode != null) _curNode.OnEnter();
        }
    }
}
