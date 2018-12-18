using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : FSMState
{
    public override void BeginState()
    {
        _manager.hideMesh.enabled = false;
        _manager.playerMesh.enabled = true;
        _manager.Anim.SetBool("IsDead", true);
    }

    public override void EndState()
    {
    }

    protected override void Update()
    {
    }
}
