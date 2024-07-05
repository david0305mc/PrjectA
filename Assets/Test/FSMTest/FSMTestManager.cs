using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTestManager : MonoBehaviour
{
    protected StateMachine<UnitStates> fsm;
    private void Awake()
    {
        fsm = StateMachine<UnitStates>.Initialize(this, UnitStates.Idle);
        //fsm = new StateMachine<UnitStates, StateDriverUnity>(this);
        //fsm.ChangeState(UnitStates.Idle);


    }

    //private void Update()
    //{
    //    fsm.Driver.Update.Invoke();
    //}

    void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
    }
    void Idle_Update()
    {
        Debug.Log("Idle_Update");
    }

    void Idle_FixedUpdate()
    {
        Debug.Log("Idle_FixedUpdate");
    }
    void Move_Enter()
    {
        Debug.Log("Move_Enter");
    }
    void Move_Update()
    {
        Debug.Log("Move_Update");
    }
}
