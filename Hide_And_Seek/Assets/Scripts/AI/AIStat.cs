using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStat : CharacterStat
{
    float _attackRange = 3.0f;
    public float AttackRange { get { return _attackRange; } }

    float _attackDamage = 40.0f;
    public float AttackDamage { get { return _attackDamage; } }

    float _rageDamage = 15.0f;
    public float RageDamage { get { return _rageDamage; } }

    protected override void Awake()
    {
        base.Awake();
        _attackRange = ((AIStatData)playerStat).attackRange;
        _attackDamage = ((AIStatData)playerStat).attackDamage;
        _rageDamage = ((AIStatData)playerStat).rageDamage;
    }
}
