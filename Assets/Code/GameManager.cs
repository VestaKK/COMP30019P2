using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] DungeonSpawner _dungeonSpawner;
    [SerializeField] Player _playerPrefab;

    DungeonController _currentDungeon = null;
    [SerializeField] Player _currentPlayer = null;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            NextLevel();
        }
        else if (_instance != null)
        {
            Destroy(this);
        }
    }

    void NextLevel() 
    {
        if (_currentDungeon != null) 
        {
            while (_currentDungeon.gameObject.transform.childCount > 0)
            {
                DestroyImmediate(_currentDungeon.gameObject.transform.GetChild(0).gameObject);
            }
            DestroyImmediate(_currentDungeon.gameObject);
        }

        SetUpDungeon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            NextLevel();
        }
    }

    private static void SetUpDungeon() { 
        if (_instance._currentPlayer == null)
        {
            _instance._currentDungeon = _instance._dungeonSpawner.SpawnDungeon();
            Player p = Instantiate(_instance._playerPrefab,
            new Vector3(_instance._currentDungeon.spawnRoom.SpawnPoint.x, 0.1f, _instance._currentDungeon.spawnRoom.SpawnPoint.y),
            Quaternion.identity);
            _instance._currentPlayer = p;
        }
        else 
        {
            _instance._currentPlayer.GetComponent<CharacterController>().enabled = false;
            
            _instance._currentPlayer.gameObject.transform.position = new Vector3(0, 100, 0);

            _instance._currentDungeon = _instance._dungeonSpawner.SpawnDungeon();

            _instance._currentPlayer.gameObject.transform.position = 
                new Vector3(_instance._currentDungeon.spawnRoom.SpawnPoint.x, 0.1f, _instance._currentDungeon.spawnRoom.SpawnPoint.y);

            _instance._currentPlayer.GetComponent<CharacterController>().enabled = true;
        }

        _instance._currentPlayer.CurrentDungeon = _instance._currentDungeon;
        _instance._currentPlayer.CurrentRoom = _instance._currentDungeon.spawnRoom;
    }

    public static Player CurrentPlayer { get => _instance._currentPlayer; set => _instance._currentPlayer = value; }
    public static DungeonController CurrentDungeon { get => _instance._currentDungeon; set => _instance._currentDungeon = value; }
}
