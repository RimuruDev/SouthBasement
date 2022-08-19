using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EnemysAI.CombatSkills
{
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
        [SerializeField] private bool controlCombatFromHere = true; //����� �� ����� �������� ������

        [Header("�������")]
        public UnityEvent onAttack = new UnityEvent(); // ��� �����
        public UnityEvent onBeforeAttack = new UnityEvent(); // �� attackTimeOffset �� �����
        public UnityEvent onEnterArea = new UnityEvent(); // ����� ����� � ������ ��������� �����

        [Header("����������� ����")]
        [SerializeField] private LayerMask damageLayer; // �������� ����
        [SerializeField] private List<string> enterTags = new List<string>(); // ��� �� �������� � �������

        private float nextTime = 0f;
        private bool onTrigger = false;
        private bool isStopped = false;

        //������ ���������� ��������
        public void Attack()
        {
            if (!isStopped && onTrigger && Time.time >= nextTime - attackTimeOffset)
            {
                StartCoroutine(StartAttack(attackTimeOffset));
                SetNextAttackTime(attackRate + attackTimeOffset);
            }
        }

        //������ �����
        private IEnumerator StartAttack(float waitTime)
        {
            onBeforeAttack.Invoke();
            yield return new WaitForSeconds(waitTime);
            Hit();
        }
        private void Hit()
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
        private void Awake() 
        {
            pointRotation = GetComponent<PointRotation>();
            if(enterTags.Count == 0) enterTags.Add("Player");
        }
        private void Update()
        {
            if (controlCombatFromHere)
            {
                //��������� ����� �� �� ���������
                if (!isStopped && onTrigger && Time.time >= nextTime - attackTimeOffset)
                    Attack();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (enterTags.Contains(collision.tag))
            {
                onTrigger = true;
                pointRotation.SetTarget(collision.transform);
                onEnterArea.Invoke();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (pointRotation.Target == collision.transform)
                onTrigger = false;
        }
        void OnDrawGizmosSelected()//��������� ������� �����
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}