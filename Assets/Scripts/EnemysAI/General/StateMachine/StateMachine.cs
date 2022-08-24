using EnemysAI.CombatSkills;
using EnemysAI.Moving;
using EnemysAI.Other;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EnemysAI
{ 
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState; //������� ��������
            
        [Header("����������")] //��� ���������� ������� ����� ������������
        [SerializeField] protected Animator animator; 
        [SerializeField] protected Sleeping sleeping;
        [SerializeField] protected Health health;
        [SerializeField] protected Move moving;
        [SerializeField] protected DynamicPathFinding dynamicPathFinding;
        [SerializeField] protected TargetSelection targetSelection;
        [SerializeField] protected Combat combat;

        public State CurrentState => currentState;
        public Animator Animator => animator;
        public Sleeping Sleeping => sleeping;
        public Health Health => health;
        public Move Move => moving;
        public DynamicPathFinding DynamicPathFinding => dynamicPathFinding;
        public TargetSelection TargetSelection => targetSelection;
        public Combat Combat => combat;

        public void ChangeState(State newState)
        {
            if (currentState != null && currentState.StateCondition == State.StateConditions.Working) currentState.Exit(this); //������� �� �������� ���������
            currentState = newState; //������ ����� ��������� ��� �������
            if(!currentState.CanInterrupt) currentState.onExit.AddListener(ChooseState); //��������� ����� ������ ��������� �� ����� ���� ��� �� ����� ������������ ���� �� ����������
            currentState.Enter(this); //������ � ����� ���������
        }
        public abstract void ChooseState(); //����� ������ ���������
    }
}
