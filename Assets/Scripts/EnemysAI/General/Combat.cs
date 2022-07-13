using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PointRotation))]
public class Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint; // ����� �����
    private PointRotation pointRotation;

    [Header("��������� �����")]
    [SerializeField] private int minDamage = 10;
    [SerializeField] private int maxDamage = 10;
    [SerializeField] private float attackRange = 0.5f; // ������ �����
    [SerializeField] private float attackRate = 3f; // ������������� �����
    [SerializeField] private float attackTimeOffset = 0.6f; // ����� ����� ��������� ��������

    [Header("�������")]
    [SerializeField] private UnityEvent onAttack = new UnityEvent(); // ��� �����
    [SerializeField] private UnityEvent onBeforeAttack = new UnityEvent(); // �� attackTimeOffset �� �����
    [SerializeField] private UnityEvent onEnterArea = new UnityEvent(); // ����� ����� � ������ ��������� �����

    [Header("����������� ����")]
    [SerializeField] private LayerMask damageLayer; // �������� ����
    [SerializeField] private string enterTag = "Player"; // ��� �� �������� � �������

    private float nextTime = 0f;
    private bool onTrigger = false;
    private bool isStopped = false;

    //������ �����
    private IEnumerator StartAttack(float waitTime)
    {
        onBeforeAttack.Invoke();
        yield return new WaitForSeconds(waitTime);
        Attack();
    }
    private void Attack()
    {
        //���������� ��� ������� �������� � ������ �����
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damageLayer);
        pointRotation.StopRotating(true, 0.8f);
        onAttack.Invoke();

        //��������� ������ �� ��� �� ������� ���������� Health
        foreach (Collider2D obj in hitObj)
        {
            if (obj.TryGetComponent(typeof(PlayerHealth), out Component comp))
                obj.GetComponent<PlayerHealth>().TakeHit(Random.Range(minDamage, maxDamage + 1));
        }
    }
    private void SetNextAttackTime(float value) { nextTime = Time.time + value; }

    //���� ������� � �������
    public void SetStop(bool active) { isStopped = active; }
    public bool GetStop() { return isStopped; }

    //���������� ������
    private void Start()
    {
        pointRotation = GetComponent<PointRotation>();
    }
    private void Update()
    {
        //��������� ����� �� �� ���������
        if (!isStopped && onTrigger && Time.time >= nextTime - attackTimeOffset)
        {
            StartCoroutine(StartAttack(attackTimeOffset));
            SetNextAttackTime(attackRate + attackTimeOffset);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == enterTag)
        {
            onTrigger = true;
            onEnterArea.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            onTrigger = false;
    }
    void OnDrawGizmosSelected()//��������� ������� �����
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
