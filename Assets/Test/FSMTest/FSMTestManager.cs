using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTestManager : MonoBehaviour
{
    public class Driver
    {
        public StateEvent Update;
        public StateEvent FixedUpdate;
    }

    public enum States
    {
        Init,
        Play,
        Win,
        Lose
    }

    StateMachine<States, Driver> fsm;

    void Awake()
    {
        fsm = new StateMachine<States, Driver>(this);

        fsm.ChangeState(States.Init); //3. Easily trigger state transitions
    }

    private void Update()
    {
        fsm.Driver.Update.Invoke();
    }

    private void FixedUpdate()
    {
        fsm.Driver.FixedUpdate.Invoke();
    }
    public void Init_Enter()
    {
        Debug.Log("Ready");
    }

    public void Play_Enter()
    {
        Debug.Log("Spawning Player");
    }

    public void Play_FixedUpdate()
    {
        Debug.Log("Doing Physics stuff");
    }

    public void Play_Update()
    {
        Debug.Log("Play_Update");
    }

    public void Play_Exit()
    {
        Debug.Log("Despawning Player");
    }

    public void Win_Enter()
    {
        Debug.Log("Game Over - you won!");
    }

    void Lose_Enter()
    {
        Debug.Log("Game Over - you lost!");
    }

    public void OnClickPlay()
    {
        fsm.ChangeState(States.Play);
    }
}