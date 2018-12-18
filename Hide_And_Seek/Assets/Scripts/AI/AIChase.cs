using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : AIFSMState
{
    public override void BeginState()
    {
        base.BeginState();
        _manager.Agent.speed = _manager.Stat.MoveSpeed * 1.2f;
    }

    public override void EndState()
    {
        base.EndState();
    }

    protected override void Update()
    {
        _manager.TargetCheck();
        if (_manager.Target)
        {
            _manager.Agent.SetDestination(_manager.Target.transform.position);
            if (Vector3.Distance(transform.position, _manager.Target.transform.position) <= _manager.Stat.AttackRange - 1.0f)
            {
                _manager.SetState(AIState.Attack);
                return;
            }
        }
        else if (Vector3.Distance(_manager.SoundPos, _manager.nullSoundPos) > Mathf.Epsilon)
        {
            _manager.Agent.SetDestination(_manager.SoundPos);
            if (Vector3.Distance(transform.position, _manager.Agent.pathEndPosition) <= _manager.Stat.AttackRange - 1.0f)
            {
                _manager.SetState(AIState.Attack);
                return;
            }
        }
        else
            _manager.SetState(AIState.Idle);
    }
}
