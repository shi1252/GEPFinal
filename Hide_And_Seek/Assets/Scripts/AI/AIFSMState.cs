using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFSMState : MonoBehaviour
{
    protected AIFSMManager _manager;

    protected virtual void Awake()
    {
        _manager = GetComponent<AIFSMManager>();
    }

    public virtual void BeginState()
    {
    }

    public virtual void EndState()
    {
    }

    protected virtual void Update()
    {
        
    }
}
