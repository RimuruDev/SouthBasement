using Creature.CombatSkills;
using Creature.Moving;
using Creature.Other;
using UnityEngine;

namespace Creature
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState; //������� ��������
        private string currentStateName;

        [Header("����������")] //��� ���������� ������� ����� ������������
        [SerializeField] protected Animator animator;
        [SerializeField] protected Sleeping sleeping;
        [SerializeField] protected Health health;
        [SerializeField] protected Move moving;
        [SerializeField] protected DynamicPathfinding dynamicPathFinding;
        [SerializeField] protected TargetSelection targetSelection;
        [SerializeField] protected Combat combat;

        //�������
        public State CurrentState => currentState;
        public Animator Animator => animator;
        public Sleeping Sleeping => sleeping;
        public Health Health => health;
        public Move Move => moving;
        public DynamicPathfinding DynamicPathFinding => dynamicPathFinding;
        public TargetSelection TargetSelection => targetSelection;
        public Combat Combat => combat;

        //������ 
        public void ChangeState(State newState)
        {
            if (currentState != null && currentState.StateCondition == State.StateConditions.Working) currentState.ExitState(this); //������� �� �������� ���������
            currentState = newState; //������ ����� ��������� ��� �������
            currentStateName = currentState.StateName;
            currentState.onFinish.AddListener(ChooseState); //��������� ����� ������ ��������� �� ����� ���� ��� �� ����� ������������ ���� �� ����������
            currentState.EnterState(this); //������ � ����� ���������
        }
        public virtual void UpdateStates()
        {
            if (CurrentState != null)
            {
                if (CurrentState.IsDynamicState)
                    CurrentState.UpdateState(this);
                if (CurrentState.MustBeFinished)
                    ChooseState();
            }
        }
        public abstract void ChooseState(); //����� ������ ���������
        public void PlayAnimation(string animation, float animationSpeed)
        {
            if (Animator != null)
            {
                Animator.Play(animation);
                Animator.speed = animationSpeed;
            }
            else Debug.LogWarning("Failed trying to play " + animation + ". Animator is null");
        }
    }
}
