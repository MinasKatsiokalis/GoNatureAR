using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action<State> OnChangeState;

    private State state;
    public State CurrentState 
    {
        get => state;
    }

    private int currentStateIndex;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {   
        //First state == 0
        currentStateIndex = -1;
        GoToNextState();
    }

    private void GoToNextState()
    {
        currentStateIndex++;

        State[] states = (State[])Enum.GetValues(typeof(State));
        if (currentStateIndex >= states.Length)
            return;

        // Set the current state to the next state
        state = states[currentStateIndex];

        OnChangeState?.Invoke(state);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}