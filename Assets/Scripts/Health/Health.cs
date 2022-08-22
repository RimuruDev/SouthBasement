using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    //���������� ��������
    [Header("���������� ��������")]
    [SerializeField] protected int currentHealth = 60;
    [SerializeField] protected int maxHealth = 60;

    [Header("�������")]
    //������������ ���� 
    [SerializeField] protected float destroyOffset = 0f;
    //�������
    public UnityEvent<int, int> onHealthChange = new UnityEvent<int, int>();
    public UnityEvent<int, int> onTakeHit = new UnityEvent<int, int>();
    public UnityEvent<int, int> onHeal = new UnityEvent<int, int>();
    public UnityEvent onDie = new UnityEvent();  //������ ������� ���������� ��� ����������� �������

    //������
    protected EffectHandler effectHandler;
    private Coroutine damageInd = null;

    //�������
    public EffectHandler EffectHandler => effectHandler;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    //������ ����������� �� ���������
    public abstract void TakeHit(int damage, float stunDuration = 0f);
    public abstract void Heal(int heal);
    public abstract void SetHealth(int newMaxHealth, int newHealth);
}