using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle = 0,
    Roam,
    Dead,
    Attack,
    Chase,
    Rage
}

[RequireComponent(typeof(AIStat))]
[ExecuteInEditMode]
public class AIFSMManager : MonoBehaviour, IFSMManager
{
    bool _isInit = false;
    public AIState startState = AIState.Idle;
    Dictionary<AIState, AIFSMState> _states = new Dictionary<AIState, AIFSMState>();

    [SerializeField]
    AIState _currentState;
    public AIState CurrentState { get { return _currentState; } }

    public AIFSMState CurrentStateComponent { get { return _states[_currentState]; } }

    NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    Animator _anim;
    public Animator Anim { get { return _anim; } }

    AIStat _stat;
    public AIStat Stat { get { return _stat; } }

    bool _dead = false;
    public bool Dead { get { return _dead; } }

    public GameObject _target;
    public GameObject Target { get { return _target; } }

    Vector3 _soundPos;
    public Vector3 SoundPos { get { return _soundPos; } }

    public readonly Vector3 nullSoundPos = new Vector3(-10.0f, -10.0f, -10.0f);

    Camera _sight;

    public int _soundCount = 0;
    public int SoundCount { get { return _soundCount; } }

    float _soundResetTimer = 5.0f;
    float _soundResetTime;

    float _howLongNearByPlayer = 0.0f;
    public float HowLongNearByPlayer { get { return _howLongNearByPlayer; } }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
        _stat = GetComponent<AIStat>();
        _sight = GetComponentInChildren<Camera>();
        _isInit = false;

        _target = null;
        _soundResetTime = _soundResetTimer;

        AIState[] stateValues = (AIState[])System.Enum.GetValues(typeof(AIState));
        foreach (AIState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("AI" + s.ToString());
            AIFSMState state = (AIFSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (AIFSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }
    }

    void Start()
    {
        SetState(startState);
        _isInit = true;

        _agent.angularSpeed = _stat.TurnSpeed;
        _agent.stoppingDistance = _stat.AttackRange * 0.5f;
    }

    private void LateUpdate()
    {
        if (_target)
            _soundPos = _target.transform.position;
        if (_soundResetTime <= 0.0f)
        {
            ResetSound();
        }
        _soundResetTime -= Time.deltaTime;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, player.transform.position) <= 5f)
        {
            _howLongNearByPlayer += Time.deltaTime;
        }
        else
        {
            _howLongNearByPlayer = 0.0f;
        }
    }

    public void DecreaseTimerForAddScore(float time)
    {
        _howLongNearByPlayer = Mathf.Clamp(_howLongNearByPlayer - time, 0.0f, int.MaxValue);
    }

    public void SetState(AIState newState)
    {
        if (_isInit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
        _anim.SetInteger("CurrentState", (int)_currentState);
    }

    public void SetDeadState()
    {
        _dead = true;
        SetState(AIState.Dead);
    }

    public bool SoundCheck()
    {
        return _soundCount >= 3;
    }

    public void ResetSound()
    {
        _soundPos = nullSoundPos;
        _soundCount = 0;
        _soundResetTime = _soundResetTimer;
    }

    public void HearSound()
    {
        _soundCount++;
        if (SoundCheck())
            _soundPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        _soundResetTime = _soundResetTimer;
    }

    public void TargetCheck()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterController cc = player.GetComponent<CharacterController>();

        bool found = false;

        if (cc)
        {
            Plane[] ps = GeometryUtility.CalculateFrustumPlanes(_sight);
            if (GeometryUtility.TestPlanesAABB(ps, cc.bounds))
            {
                RaycastHit hit;
                Vector3 direction = player.transform.position + Vector3.up * 1.5f;
                direction = direction - _sight.transform.position;
                Physics.Raycast(_sight.transform.position, direction, out hit);

                Debug.DrawRay(_sight.transform.position, direction);
                if (hit.transform == player.transform)
                {
                    FSMManager playerManager = player.GetComponent<FSMManager>();
                    if (playerManager.CurrentState != PlayerState.Hide && playerManager.CurrentState != PlayerState.Dead)
                        found = true;
                }
            }
        }
        else if (Vector3.Distance(transform.position, player.transform.position) <= 1.0f)
            found = true;

        if (found)
            _target = player;
        else
            _target = null;
    }

    private void OnDrawGizmos()
    {
        if (_sight != null)
        {
            Gizmos.color = Color.blue;
            Matrix4x4 temp = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(
                _sight.transform.position,
                _sight.transform.rotation,
                Vector3.one
                );

            DrawFrustum(_sight);

            Gizmos.matrix = temp;
        }
    }

    // https://forum.unity.com/threads/drawfrustum-is-drawing-incorrectly.208081/
    void DrawFrustum(Camera cam)
    {
        Vector3[] nearCorners = new Vector3[4];
        Vector3[] farCorners = new Vector3[4];
        Plane[] camPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        Plane temp = camPlanes[1]; camPlanes[1] = camPlanes[2]; camPlanes[2] = temp;

        for (int i = 0; i < 4; i++)
        {
            nearCorners[i] = Plane3Intersect(camPlanes[4], camPlanes[i], camPlanes[(i + 1) % 4]);
            farCorners[i] = Plane3Intersect(camPlanes[5], camPlanes[i], camPlanes[(i + 1) % 4]);
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(nearCorners[i], nearCorners[(i + 1) % 4], Color.red, Time.deltaTime, true);
            Debug.DrawLine(farCorners[i], farCorners[(i + 1) % 4], Color.blue, Time.deltaTime, true);
            Debug.DrawLine(nearCorners[i], farCorners[i], Color.green, Time.deltaTime, true);
        }
    }

    Vector3 Plane3Intersect(Plane p1, Plane p2, Plane p3)
    {
        return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
                (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
                (-p3.distance * Vector3.Cross(p1.normal, p2.normal))) /
            (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
    }
}