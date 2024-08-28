using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SS
{
    public class BuildingObj : BaseObj
    {
        private CompositeDisposable compositeDisposable;
        private StateMachine<UnitStates, Driver> fsm;
        protected override void Awake()
        {
            base.Awake();
            fsm = new StateMachine<UnitStates, Driver>(this);
        }
        protected override void Update()
        {
            if (fsm == null)
                return;
            fsm.Driver.Update.Invoke();
        }

        protected override void FixedUpdate()
        {
            if (fsm == null)
                return;
            fsm.Driver.FixedUpdate.Invoke();
        }

        protected override void ChangeIdleState()
        {
            base.ChangeIdleState();
            fsm.ChangeState(UnitStates.Idle);
        }

        protected void Idle_Enter()
        {
            //Debug.Log("Idle_Enter");
            PlayAni("Walk");
        }
        protected void Idle_Update()
        {
            TargetObj = SearchNearestOpponent(false);
            if (TargetObj != null)
            {
                fsm.ChangeState(UnitStates.Attack);
            }
        }

        protected override void Attack_Enter()
        {
            base.Attack_Enter();
        }

        protected override void DoAttack()
        {
            base.DoAttack();
        }

        protected override void Attack_Exit()
        {
            base.Attack_Exit();
            TargetObj = null;
        }
    }

}
