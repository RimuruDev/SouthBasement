using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EnemysAI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [Header("��������� ��������")]
        [SerializeField] protected float walkSpeed = 2f; //�������� ��� ������
        [SerializeField] protected float runSpeed = 3.3f; //�������� ��� ����
        protected EnemyTarget target; //��������� �� ������

        //������ ��������� ����
        protected bool isStopped = false;

        //������ �� ������ ������
        [Header("������")]
        [SerializeField] private bool wakeUpOnSettingTarget = false;
        [SerializeField] protected UnityEvent onSleep = new UnityEvent();
        [SerializeField] protected UnityEvent onWakeUp = new UnityEvent();

        [Header("������ ����������")]
        [SerializeField] protected TargetSelection targetSelection;
        protected Move moving;

        protected IEnumerator ChangeSpeed(TargetType moveType) //������� ������� ��������
        {
            float nextSpeed;
            if (moveType == TargetType.Static) nextSpeed = walkSpeed;
            else nextSpeed = runSpeed;

            float k = (nextSpeed - moving.speed) / 20;
            int n = (int)((nextSpeed - moving.speed) / k);
            if (n < 0f) n *= -1;

            for (int i = 0; i < n; i++)
            {
                yield return new WaitForSeconds(0.25f);
                moving.speed += k;
            }
        }
        protected void CheckTarget(EnemyTarget newTarget)
        {
            if (targetSelection.Targets.Count > 0)
            {
                if (target == newTarget) return;
                else if (target == null || target.targetMoveType != newTarget.targetMoveType) StartCoroutine(ChangeSpeed(newTarget.targetMoveType));
                target = newTarget;
            }
        }

        //���������
        public IEnumerator Stun(float duration)
        {
            SetStun(true, true);
            yield return new WaitForSeconds(duration);
            ResetStun(true, true);
        }
        public void GetStunned(float duration) => StartCoroutine(Stun(duration));
        public abstract void SetStun(bool stopChange, bool blockChange);
        public abstract void ResetStun(bool stopChange, bool blockChange);

        //���� ������� � �������
        public void SetStop(bool active) { isStopped = active; }
        public bool GetStop => isStopped; 
    }
}
