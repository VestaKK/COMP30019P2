using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private bool _isPaused = false;
    private bool _GameOver = false;

    [SerializeField] DungeonSpawner _dungeonSpawner;
    [SerializeField] Player _playerPrefab;

    [SerializeField] private int _score = 0;

    private int _levelCount = 0;

    private float _timer = 0;

    [SerializeField] private Transform _scoreText;
    [SerializeField] private TextDisplay _textDisplay;

    DungeonController _currentDungeon = null;
    [SerializeField] Player _currentPlayer = null;
    [SerializeField] private bool _inExitRoom = false;

    public delegate void GameEvent();
    public static event GameEvent OnEnterExitRoom;
    public static event GameEvent OnExitExitRoom;
    public static event GameEvent OnPlayerFinishLevel;
    public static event GameEvent OnPlayerStartLevel;
    public static event GameEvent OnPlayerDeath;

    public static void AddToScore(int scoreAmount) 
    {
        _instance._score += scoreAmount;
        _instance._scoreText.GetComponent<TMP_Text>().text = "SCORE: " + _instance._score.ToString();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            StartCoroutine(NextLevel());
        }
        else if (_instance != null)
        {
            Destroy(this);
        }
    }

    IEnumerator NextLevel() 
    {
        if (_instance.LevelCount > 0)
            AddToScore(500);

        PauseGame();
        UIManager.instance.Show<LOADING>(false);
        yield return new WaitForSecondsRealtime(0.2f);

        if (_instance._currentDungeon != null) 
        {
            while (_instance._currentDungeon.gameObject.transform.childCount > 0)
            {
                DestroyImmediate(_instance._currentDungeon.gameObject.transform.GetChild(0).gameObject);
            }
            DestroyImmediate(_instance._currentDungeon.gameObject);
        }

        SetUpDungeon();
        yield return new WaitForSecondsRealtime(0.2f);
        _currentPlayer.Heal(60);

        Camera.main.GetComponent<CameraLookAt>().Target(_instance._currentPlayer);
        UIManager.instance.Show<PlayerHUD>();
        UnpauseGame();
        if (_instance.LevelCount == 0)
            _textDisplay.DisplayFadingText("PRESS ESC FOR INSTRUCTIONS", 10f);

        yield return null;
    }

    private void Update()
    {
        if (_instance._GameOver)
            return;

        if (!_instance._isPaused)
            _timer += Time.deltaTime;
        
        if (_instance._currentPlayer == null) return;

        if (_instance._currentPlayer.CurrentRoom == _instance._currentDungeon.exitRoom)
        {
            if (_inExitRoom == false)
            {
                _instance._inExitRoom = true;
                OnEnterExitRoom.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _instance.LevelCount++;
                StartCoroutine(NextLevel());
            }
        }
        else 
        {
            if (_instance._inExitRoom == true) 
            {
                _instance._inExitRoom = false;
                OnExitExitRoom.Invoke();
            } 
        }
    }

    public static float GetHealthDifficultyScalar() 
    {
        if (_instance._levelCount == 0) return 0;
        Debug.Log(Mathf.Sqrt((_instance._levelCount) / 4.0f).ToString());
        return Mathf.Sqrt((_instance._levelCount) / 9.0f);
    }

    private static void SetUpDungeon() {
        if (_instance._currentPlayer == null)
        {
            _instance._currentDungeon = _instance._dungeonSpawner.SpawnDungeon();

            Player p = Instantiate(_instance._playerPrefab,
            new Vector3(_instance._currentDungeon.spawnRoom.SpawnPoint.x, 0.1f, _instance._currentDungeon.spawnRoom.SpawnPoint.y),
            Quaternion.identity);

            _instance._currentPlayer = p;
            _instance._currentPlayer.OnPlayerInstanceDeath += _instance.GameOverFunc;

            UIManager.instance.Get<PlayerHUD>().LinkPlayer(_instance._currentPlayer);
        }
        else 
        {
            _instance._currentPlayer.GetComponent<CharacterController>().enabled = false;
            
            _instance._currentPlayer.gameObject.transform.position = new Vector3(0, 100, 0);

            // Kinda Temp, But We'll see
            _instance._dungeonSpawner.GetComponent<Spawner<Enemy>>().MaxSpawnsPerRoom++;
            _instance._dungeonSpawner.GetComponent<Spawner<Enemy>>().MinSpawnsPerRoom++;
            
            _instance._currentDungeon = _instance._dungeonSpawner.SpawnDungeon();

            _instance._currentPlayer.gameObject.transform.position = 
                new Vector3(_instance._currentDungeon.spawnRoom.SpawnPoint.x, 0.1f, _instance._currentDungeon.spawnRoom.SpawnPoint.y);

            _instance._currentPlayer.GetComponent<CharacterController>().enabled = true;
        }

        _instance._currentPlayer.CurrentDungeon = _instance._currentDungeon;
        _instance._currentPlayer.CurrentRoom = _instance._currentDungeon.spawnRoom;
        
    }

    public static void PauseGame() 
    {
        Time.timeScale = 0;
        _instance._isPaused = true;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
        _instance._isPaused = false;
    }

    private void GameOverFunc() 
    {
        _instance._GameOver = true;
        GameOverScreen gameOverScreen = (GameOverScreen)UIManager.instance.Get<GameOverScreen>();

        if (gameOverScreen != null) 
        {
            gameOverScreen.SetStats(_instance._score, _instance.LevelCount, _instance._timer);
            UIManager.instance.Show(gameOverScreen, false);
        }
    }

    public static bool GameOver { get => _instance._GameOver; }
    public static Player CurrentPlayer { get => _instance._currentPlayer; set => _instance._currentPlayer = value; }
    public static DungeonController CurrentDungeon { get => _instance._currentDungeon; set => _instance._currentDungeon = value; }
    public static bool isPaused { get => _instance._isPaused; }
    public static bool inExitRoom { get => _instance._inExitRoom; }
    public int LevelCount { get => _levelCount; set => _levelCount = value; }
}
