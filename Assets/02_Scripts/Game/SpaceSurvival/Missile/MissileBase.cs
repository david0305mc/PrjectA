using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class MissileBase : MonoBehaviour
    {
        private TrailRenderer trailRenderer;
        protected Rigidbody2D rigidBody2d;

        protected float speed;
        protected AttackData2 attackData;
        protected BaseObj targetObj;

        protected Vector2 srcPos;
        protected Vector2 dstPos;
        protected Vector2 lastMoveVector;
        protected Vector2 prevPos;

        protected float dist;
        private float moveDist;
        protected float elapse;
        private bool isDisposed;
        private int targetAttackCount;
        private long targetUID;

        protected virtual void Awake()
        {
            rigidBody2d = GetComponent<Rigidbody2D>();
            trailRenderer = GetComponent<TrailRenderer>();
        }

        public virtual void Shoot(AttackData2 _attackData, BaseObj _targetObj, float _speed)
        {
            attackData = _attackData;
            targetObj = _targetObj;
            targetUID = targetObj.UnitUID;
            var unitGradeInfo = DataManager.Instance.GetUnitGrade(attackData.attackerTID, attackData.grade);
            var projectileInfo = DataManager.Instance.GetProjectileInfoData(unitGradeInfo.projectileid);

            dstPos = targetObj.transform.position;
            srcPos = transform.position;
            dist = Vector2.Distance(srcPos, dstPos);
            moveDist = Vector2.Distance(srcPos, dstPos);
            elapse = 0f;
            speed = _speed;
            isDisposed = false;
            targetAttackCount = 1;
            if (trailRenderer != null)
            {
                trailRenderer.Clear();
            }
        }

        private void Update()
        {
            UpdateMissile();
        }

        protected virtual bool UpdateMissile()
        {
            if (targetObj != null && targetObj.UnitUID == targetUID)
            {
                dstPos = targetObj.transform.position;
            }

            if (elapse >= 1)
            {
                float addElapse = (Time.fixedDeltaTime / moveDist) * speed;
                var moveDelta = lastMoveVector.normalized * addElapse * moveDist;

                //rigidBody2d.transform.position = new Vector2(transform.position.x, transform.position.y) + moveDelta;
                rigidBody2d.MovePosition(new Vector2(transform.position.x, transform.position.y) + moveDelta);
                prevPos = transform.position;
                return false;
            }
            return true;
        }

        protected void Dispose()
        {
            if (!isDisposed)
            {
                Lean.Pool.LeanPool.Despawn(gameObject);
                elapse = 0f;
                isDisposed = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                if (damagable.IsEnemy())
                {
                    if (!targetObj.IsHero)
                    {
                        Hit(collision, damagable);
                    }
                }
                else
                {
                    if (targetObj.IsHero)
                    {
                        Hit(collision, damagable);
                    }
                }
            }
        }

        private void Hit(Collider2D collision, IDamagable damagable)
        {
            GameManager.Instance.ShowBoomEffect(attackData, collision.ClosestPoint(transform.position));
            //MGameManager.Instance.DoAreaAttack(attackData, collision.ClosestPoint(transform.position));
            damagable.GetDamaged(attackData);

            targetAttackCount--;
            if (targetAttackCount <= 0)
            {
                Dispose();
            }
        }
    }

}
