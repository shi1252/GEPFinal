using UnityEngine;

public class PlayerStat : CharacterStat
{
    float _walkSoundRadius = 2.0f;
    public float WalkSoundRadius { get { return _walkSoundRadius; } }

    float _runSoundRadius = 5.0f;
    public float RunSoundRadius { get { return _runSoundRadius; } }

    protected override void Awake()
    {
        base.Awake();
        _walkSoundRadius = ((PlayerStatData)playerStat).walkSoundRadius;
        _runSoundRadius = ((PlayerStatData)playerStat).runSoundRadius;
    }

    public override void TakeDamage(float damage)
    {
        FSMManager _manager = GetComponent<FSMManager>();
        if (!_manager.Dead)
        {
            if (_manager.Anim.GetBool("IsMoving"))
                _manager.Anim.SetTrigger("Damaged");
            else
            {
                _manager.SetState(PlayerState.Idle);
                _manager.Anim.SetTrigger("Damaged");
            }
            base.TakeDamage(damage);
        }
    }
}