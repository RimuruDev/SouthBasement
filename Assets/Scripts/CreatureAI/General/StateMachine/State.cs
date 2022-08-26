using Creature;
using UnityEngine.Events;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public enum StateConditions
    {
        DontWork,
        Working,
        Finished
    }

    [SerializeField] protected string stateName = "Some state"; //��� ���������. �� ������������ ����, ������ ������ �������� � ������
    [SerializeField] protected bool canInterrupt = false; //����� �� �������� ��������� �� ����� ������
    [SerializeField] protected bool canRepeated = false; //����� �����������
    [SerializeField] protected bool isDynamicState; //������������ �� ���������
    protected StateConditions stateCondition = StateConditions.DontWork; //������� ��������� ��������� :\
    //������
    public UnityEvent onEnter = new UnityEvent();
    public UnityEvent onFinish = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();
    public UnityEvent onUpdate = new UnityEvent();

    //�������
    public bool CanInterrupt => canInterrupt;
    public bool CanRepeated => canRepeated;
    public bool IsDynamicState => isDynamicState;
    public StateConditions StateCondition => stateCondition;
    public string StateName => stateName;

    //������ � ������� ����������� ��������� ���������
    public virtual void EnterState(StateMachine stateMachine) => onEnter.Invoke();
    public virtual void FinishState(StateMachine stateMachine) { stateCondition = StateConditions.Finished; onFinish.Invoke(); } 
    public virtual void ExitState(StateMachine stateMachine) => onExit.Invoke();
    public virtual void UpdateState(StateMachine stateMachine) => onUpdate.Invoke();
}