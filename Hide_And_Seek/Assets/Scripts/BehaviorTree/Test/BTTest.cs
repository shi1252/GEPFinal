using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTest : MonoBehaviour
{
    BlackBoard bb;
    ICompositeNode<BlackBoard> rootNode;

    // Start is called before the first frame update
    void Start()
    {
        bb = new BlackBoard();
        bb.passedTime = 0.0f;
        bb.timer = 5.0f;
        bb.from = this.transform;
        bb.to = GameObject.FindGameObjectWithTag("Player").transform;
        bb.speed = 3.0f;

        rootNode = new SequenceNode<BlackBoard>();
        rootNode.Add(NodeBuilder.WaitNode());
        rootNode.Add(NodeBuilder.MoveFromTo());
    }

    // Update is called once per frame
    void Update()
    {
        BTStatus status = rootNode.Tick(Time.deltaTime, bb);
        Debug.Log(status + "passedTime: " + bb.passedTime);
        if (status == BTStatus.Success)
            bb.passedTime = 0.0f;
    }
}
