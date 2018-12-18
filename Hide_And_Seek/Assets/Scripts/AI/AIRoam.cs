using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRoam : AIFSMState
{
    Vector3 _roamPos;
    public override void BeginState()
    {
        base.BeginState();
        _manager.Agent.speed = _manager.Stat.MoveSpeed;
        _roamPos = new Vector3(Random.Range(-25.0f, 25.0f), 0.0f, Random.Range(-25.0f, 25.0f));
        _manager.Agent.SetDestination(_roamPos);
    }

    public override void EndState()
    {
        base.EndState();
    }

    protected override void Update()
    {
        _manager.TargetCheck();
        if (_manager.Target || _manager.SoundCheck())
        {
            _manager.SetState(AIState.Chase);
            return;
        }
        if (Vector3.Distance(transform.position, _manager.Agent.pathEndPosition) <= 2.0f)
        {
            _manager.SetState(AIState.Idle);
            return;
        }
    }
}
