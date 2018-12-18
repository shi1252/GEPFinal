using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : FSMState
{
    public override void BeginState()
    {
        _manager.hideMesh.enabled = true;
        _manager.playerMesh.enabled = false;
        _manager.Anim.SetBool("IsMoving", false);
    }

    public override void EndState()
    {
    }

    protected override void Update()
    {
        base.Update();
        if (_manager.MovingCheck())
        {
            _manager.SetState(PlayerState.Move);
            return;
        }
        else
        {
            if (_manager.CurrentState != PlayerState.Hide)
            {
                _manager.SetState(PlayerState.Idle);
                return;
            }
        }
    }
}
