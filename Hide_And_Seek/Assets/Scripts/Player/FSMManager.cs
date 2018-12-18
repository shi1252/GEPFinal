using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle = 0,
    Move,
    Hide,
    Dead
}

[RequireComponent(typeof(PlayerStat))]
[ExecuteInEditMode]
public class FSMManager : MonoBehaviour, IFSMManager
{
    bool _isInit = false;
    public PlayerState startState = PlayerState.Idle;
    Dictionary<PlayerState, FSMState> _states = new Dictionary<PlayerState, FSMState>();

    [SerializeField]
    PlayerState _currentState;
    public PlayerState CurrentState { get { return _currentState; } }

    public FSMState CurrentStateComponent { get { return _states[_currentState]; } }

    CharacterController _cc;
    public CharacterController CC { get { return _cc; } }

    Animator _anim;
    public Animator Anim { get { return _anim; } }

    Vector3 _dir = Vector3.zero;
    public Vector3 Dir { get { return _dir; } }//set { _dir = value; } }

    bool _walk = false;
    public bool Walk { get { return _walk; } }//set { _walk = value; } }

    PlayerStat _stat;
    public PlayerStat Stat { get { return _stat; } }

    bool _dead = false;
    public bool Dead { get { return _dead; } }

    public SkinnedMeshRenderer playerMesh;
    public MeshRenderer hideMesh;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        _stat = GetComponent<PlayerStat>();
        _isInit = false;

        PlayerState[] stateValues = (PlayerState[])System.Enum.GetValues(typeof(PlayerState));
        foreach (PlayerState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Player" + s.ToString());
            FSMState state = (FSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (FSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }
    }

    void Start()
    {
        SetState(startState);
        _isInit = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _anim.SetBool("IsWalking", true);
            _walk = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _anim.SetBool("IsWalking", false);
            _walk = false;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        _dir = (new Vector3(h, 0.0f, v)).normalized;
    }

    public void SetState(PlayerState newState)
    {
        if (_isInit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
    }

    public bool MovingCheck()
    {
        return Vector3.SqrMagnitude(_dir) >= Vector3.kEpsilon;
    }

    public void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentState != PlayerState.Hide)
                SetState(PlayerState.Hide);
            else
                SetState(PlayerState.Idle);
        }
    }

    public void SetDeadState()
    {
        _dead = true;
        SetState(PlayerState.Dead);
    }

    private void OnDrawGizmos()
    {
        Color c = Color.yellow;
        c.a = 0.3f;
        Gizmos.color = c;

        if (Walk)
            Gizmos.DrawSphere(transform.position, Stat.WalkSoundRadius);
        else
            Gizmos.DrawSphere(transform.position, Stat.RunSoundRadius);
    }
}
