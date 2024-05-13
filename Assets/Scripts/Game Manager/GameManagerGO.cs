using System;
using UnityEngine;

public class GameManagerGO : MonoBehaviour {

    public static GameManagerGO Instance { get; private set; }


    [SerializeField]
    private GameObject wonMenu;

    [SerializeField]
    private GameObject lostMenu;


    private CounterController counterController;

    private int _counter;
    public int Counter {
        get => _counter;
        set {
            _counter = value;

            counterController.UpdateCounter(
                value
            );
        }
    }



    private TimerController timerController;

    private float _timer;
    public float Timer {
        get => _timer;
        set {
            _timer = value;

            timerController.UpdateTimer(
                value
            );
        }
    }


    private GameState _gameState;

    public GameState gameState {
        get => _gameState;
        set {
            if (value != _gameState) {
                switch (value) {
                    case GameState.Won:
                        wonMenu.SetActive(true);
                        break;

                    case GameState.Lost:
                        lostMenu.SetActive(true);
                        break;
                }
            }

            _gameState = value;
        }
    }


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        timerController = FindAnyObjectByType<TimerController>();
        counterController = FindAnyObjectByType<CounterController>();
    }
}
