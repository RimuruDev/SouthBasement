using Creature.CombatSkills;
using Creature.Moving;
using Creature.Other;
using UnityEngine;

namespace Creature.Controllers
{
    [RequireComponent(typeof(Sleeping))]
    [RequireComponent(typeof(DynamicPathfinding))]
    [RequireComponent(typeof(Move))]
    [AddComponentMenu("Creature/Controllers/Angry Rat State Machine")]
    public sealed class AngryRatStateMachine : StateMachine
    {
        [Header("���������")]
        [SerializeField] private State idleState;
        [SerializeField] private State movingState;
        [SerializeField] private State attackState;
        [SerializeField] private State healingState;

        public override void ChooseState()
        {
            State nextState = idleState;
            if (DynamicPathFinding.Target != null) { nextState = movingState; }
            if (Combat.IsOnTrigger) nextState = attackState;
            if (Health.CurrentHealth < 10) nextState = healingState;

            Debug.Log(nextState.StateName);
            if (CurrentState != nextState)
                ChangeState(nextState);
            else if (CurrentState.CanRepeated && CurrentState.StateCondition == State.StateConditions.Finished)
                ChangeState(nextState);
        }

        private void Start()
        {
            //������������� ����������� ������� ��������� �� ���� �������
            moving = GetComponent<Move>();
            dynamicPathFinding = GetComponent<DynamicPathfinding>();
            sleeping = GetComponent<Sleeping>();
            //��������� ��������� � ������
            targetSelection.onSetTarget.AddListener(dynamicPathFinding.SetNewTarget);
            dynamicPathFinding.whenANewPathIsFound.AddListener(moving.SetPath);
            //������ idle ���������
            ChangeState(idleState);
        }
        private void Update()
        {
            if (CurrentState != null)
            {
                if (CurrentState.IsDynamicState) 
                    CurrentState.UpdateState(this);
            }
        }
    }
}