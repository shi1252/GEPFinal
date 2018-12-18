using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdle : AIFSMState
{
    float idleTime;
    public override void BeginState()
    {
        base.BeginState();
        _manager.Agent.isStopped = true;
        idleTime = 3.0f;
    }

    public override void EndState()
    {
        base.EndState();
        _manager.Agent.isStopped = false;
    }

    protected override void Update()
    {
        _manager.TargetCheck();
        if (_manager.Target || _manager.SoundCheck())
        {
            _manager.SetState(AIState.Chase);
            return;
        }
        if (idleTime <= 0.0f)
        {
            _manager.SetState(AIState.Roam);
            return;
        }
        idleTime -= Time.deltaTime;
    }
}
