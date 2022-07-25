using UnityEngine;

namespace EnemysAI
{
    public class AngryRatPyrotechnicAI : EnemyAI
    {
        private Animator animator;
        private Health health;

        //���� ������� � �������
        public override void GoSleep()
        { 
        }
        public override void WakeUp()
        {
        }
        public override void ResetStun(bool stopChange, bool blockChange)
        {
        }
        public override void SetStun(bool stopChange, bool blockChange)
        {
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            moving = GetComponent<Move>();
            health = GetComponent<EnemyHealth>();
            moving.speed = walkSpeed;

            //�������
            targetSelection.onSetTarget.AddListener(CheckTarget);
            targetSelection.onResetTarget.AddListener(CheckTarget);
            GoSleep();
        }
        private void Update()
        {
            if (!isSleep && !isStopped)
            {
                if (animator != null && moving != null)//��������
                {
                    //�������� ����
                    if (moving.isNowWalk && !animator.GetBool("isRun")) animator.SetBool("isRun", true);
                    if ((!moving.isNowWalk || moving.GetStop()) && animator.GetBool("isRun")) animator.SetBool("isRun", false);
                }
            }
        }
    }
}