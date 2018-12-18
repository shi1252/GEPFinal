using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : AIFSMState
{
    public override void BeginState()
    {
        base.BeginState();
        _manager.Agent.isStopped = true;
    }

    public override void EndState()
    {
        base.EndState();
        _manager.Agent.isStopped = false;
    }

    protected override void Update()
    {
        if (_manager.Target)
        {
            GameLib.RotateFromTo(transform, _manager.Target.transform);
            if (Vector3.Distance(transform.position, _manager.Target.transform.position) > _manager.Stat.AttackRange)
            {
                _manager.SetState(AIState.Chase);
                return;
            }
        }
        else if (_manager.SoundPos != _manager.nullSoundPos)
            GameLib.RotateFromTo(transform, _manager.SoundPos);
        else
            _manager.SetState(AIState.Rage);
    }

    public void AttackCheck()
    {
        if (_manager.Target)
        {
            if (Vector3.Distance(transform.position, _manager.Target.transform.position) <= _manager.Stat.AttackRange)
            {
                PlayerStat target = _manager.Target.GetComponent<PlayerStat>();
                target.TakeDamage(_manager.Stat.AttackDamage);
            }
        }
        _manager.ResetSound();
    }
}