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
    [Space]
    [SerializeField] protected bool canInterrupt = false; //����� �� �������� ��������� �� ����� ������
    [SerializeField] protected bool canRepeated = false; //����� �����������
    [SerializeField] protected bool isDynamicState; //������������ �� ���������
    protected StateConditions stateCondition = StateConditions.DontWork; //������� ��������� ��������� :\
    //������
    [HideInInspector] public UnityEvent onEnter = new UnityEvent();
    [HideInInspector] public UnityEvent onFinish = new UnityEvent();
    [HideInInspector] public UnityEvent onExit = new UnityEvent();
    [HideInInspector] public UnityEvent onUpdate = new UnityEvent();

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