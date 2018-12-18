using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager _singleton;
    public static GameManager Singleton { get { if (_singleton == null) return _singleton = new GameManager(); return _singleton; } }

    private GameManager() { }

    public GameObject _aiPrefab;

    int _timer;
    int _score;

    public Text timerText;
    public Text scoreText;

    List<AIFSMManager> ais = new List<AIFSMManager>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _timer = 120;
        _score = 0;
    }

    private void Start()
    {
        StartCoroutine(TimeDecreaseEverySecond());
        StartCoroutine(SpawnAI());
    }

    private void Update()
    {
        if (_timer <= 0.0f)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (ais.Count > 0)
        {
            foreach (var ai in ais)
            {
                if (ai.HowLongNearByPlayer >= 1.0f)
                {
                    int time = (int)ai.HowLongNearByPlayer / 1;
                    _score += time * 10;
                    ai.DecreaseTimerForAddScore(time);
                }
            }
        }
        if (_timer % 60 >= 10)
            timerText.text = (_timer / 60).ToString() + ":" + (_timer % 60).ToString();
        else
            timerText.text = (_timer / 60).ToString() + ":0" + (_timer % 60).ToString();
        scoreText.text = _score.ToString();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator TimeDecreaseEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _timer = Mathf.Clamp(_timer - 1, 0, int.MaxValue);
        }
    }

    IEnumerator SpawnAI()
    {
        yield return new WaitForSeconds(5.0f);
        ais.Add(Instantiate(_aiPrefab, Vector3.up, Quaternion.identity).GetComponent<AIFSMManager>());
        yield return new WaitForSeconds(25.0f);
        ais.Add(Instantiate(_aiPrefab, Vector3.up, Quaternion.identity).GetComponent<AIFSMManager>());
        yield return new WaitForSeconds(30.0f);
        ais.Add(Instantiate(_aiPrefab, Vector3.up, Quaternion.identity).GetComponent<AIFSMManager>());
        yield return new WaitForSeconds(30.0f);
        ais.Add(Instantiate(_aiPrefab, Vector3.up, Quaternion.identity).GetComponent<AIFSMManager>());
    }
}