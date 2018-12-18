using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRage : AIFSMState
{
    Vector3 _pos;
    float timer;
    public GameObject mesh;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void BeginState()
    {
        base.BeginState();
        mesh.SetActive(true);
        timer = 5.0f;
        StartCoroutine(DamageInArea());

        _pos = new Vector3(Mathf.Clamp(transform.position.x + Random.Range(-10.0f, 10.0f), -25.0f, 25.0f), 
            0.0f, Mathf.Clamp(transform.position.z + Random.Range(-10.0f, 10.0f), -25.0f, 25.0f));

        _manager.Agent.SetDestination(_pos);
    }

    public override void EndState()
    {
        base.EndState();
        StopAllCoroutines();
        mesh.SetActive(false);
    }

    protected override void Update()
    {
        _manager.TargetCheck();
        if (_manager.Target)
        {
            _manager.SetState(AIState.Chase);
            return;
        }
        if (timer <= 0.0f)
        {
            _manager.SetState(AIState.Idle);
            return;
        }
        if (Vector3.Distance(transform.position, _pos) <= 1.0f)
        {
            _pos = new Vector3(Mathf.Clamp(transform.position.x + Random.Range(-10.0f, 10.0f), -25.0f, 25.0f),
                0.0f, Mathf.Clamp(transform.position.z + Random.Range(-10.0f, 10.0f), -25.0f, 25.0f));

            _manager.Agent.SetDestination(_pos);
        }
        timer -= Time.deltaTime;
    }

    IEnumerator DamageInArea()
    {
        PlayerStat player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= _manager.Stat.AttackRange)
                player.TakeDamage(_manager.Stat.RageDamage);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
