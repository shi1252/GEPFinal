using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : FSMState
{
    public override void BeginState()
    {
        _manager.hideMesh.enabled = false;
        _manager.playerMesh.enabled = true;
        _manager.Anim.SetBool("IsMoving", true);
        StartCoroutine(MakeNoise());
    }

    public override void EndState()
    {
        StopAllCoroutines();
    }

    protected override void Update()
    {
        base.Update();
        if (!_manager.MovingCheck())
        {
            if (_manager.CurrentState != PlayerState.Hide)
            {
                _manager.SetState(PlayerState.Idle);
                return;
            }
        }
        _manager.CC.CKMove(_manager.Dir, _manager.Stat);
    }

    IEnumerator MakeNoise()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject[] AIs = GameObject.FindGameObjectsWithTag("Seeker");
            foreach (var ai in AIs)
            {
                AIFSMManager aimanager = ai.GetComponent<AIFSMManager>();
                if (_manager.Walk)
                {
                    if (Vector3.Distance(transform.position, ai.transform.position) <= _manager.Stat.WalkSoundRadius)
                    {
                        aimanager.HearSound();
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, ai.transform.position) <= _manager.Stat.RunSoundRadius)
                    {
                        aimanager.HearSound();
                    }
                }
            }
        }
    }
}