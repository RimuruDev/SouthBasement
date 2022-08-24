using Creature;
using UnityEngine.Events;

[System.Serializable]
public abstract class State
{
    public enum StateConditions
    {
        DontWork,
        Working,
        Finished
    }

    protected string stateName;
    protected bool canInterrupt = false; //����� �� �������� ��������� �� ����� ������
    protected StateConditions stateCondition = StateConditions.DontWork; //������� ��������� ��������� :\
    //������
    public UnityEvent onEnter = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();
    public UnityEvent onUpdate = new UnityEvent();

    public bool CanInterrupt => canInterrupt;
    public StateConditions StateCondition => stateCondition;
    public string StateName => stateName;

    public virtual void Enter(StateMachine stateMachine) => onEnter.Invoke();
    public virtual void Exit(StateMachine stateMachine) => onExit.Invoke();
    public virtual void Update(StateMachine stateMachine) => onUpdate.Invoke();
}