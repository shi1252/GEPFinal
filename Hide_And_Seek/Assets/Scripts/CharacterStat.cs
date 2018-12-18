using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [SerializeField]
    private float _hp = 100.0f;
    public float Hp { get { return _hp; } }

    [SerializeField]
    private float _moveSpeed = 2.5f;
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    private float _turnSpeed = 540.0f;
    public float TurnSpeed { get { return _turnSpeed; } }

    public StatData playerStat;

    protected virtual void Awake()
    {
        _hp = playerStat.maxHp;
        _moveSpeed = playerStat.moveSpeed;
    }

    public virtual void TakeDamage(float damage)
    {
        _hp = Mathf.Clamp(_hp - damage, 0, playerStat.maxHp);
        if (_hp <= 0)
        {
            GetComponent<IFSMManager>().SetDeadState();
        }
    }
}
