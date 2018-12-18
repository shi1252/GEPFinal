using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimEvent : MonoBehaviour
{
    AIFSMManager _manager;

    private void Awake()
    {
        _manager = transform.root.GetComponent<AIFSMManager>();
    }

    void HitCheck()
    {
        AIAttack attackState = _manager.CurrentStateComponent as AIAttack;
        if (attackState)
            attackState.AttackCheck();
    }
}
