using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FSMManager))]
public class FSMState : MonoBehaviour
{
    protected FSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<FSMManager>();
    }

    public virtual void BeginState() { }
    public virtual void EndState() { }

    // Update is called once per frame
    protected virtual void Update()
    {
        _manager.InputCheck();
    }
}