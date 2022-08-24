using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.CombatSkills
{
    [RequireComponent(typeof(PointRotation))]
    [AddComponentMenu("Creature/General/CombatSkills/Combat")]
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
        public UnityEvent onAttack = new UnityEvent(); // ��� �����
        public UnityEvent beforeAttack = new UnityEvent(); // �� attackTimeOffset �� �����
        public UnityEvent afterAttack = new UnityEvent();
        public UnityEvent onEnterArea = new UnityEvent(); // ����� ����� � ������ ��������� �����

        [Header("����������� ����")]
        [SerializeField] private LayerMask damageLayer; // �������� ����
        [SerializeField] private List<string> enterTags = new List<string>(); // ��� �� �������� � �������

        private bool isOnTrigger = false;
        private bool isStopped = false;
        private Transform attackTarget;

        //������ ���������� ��������
        public void Attack() => StartCoroutine(StartAttack(attackTimeOffset));

        //������ �����
        private IEnumerator StartAttack(float waitTime)
        {
            while(!isStopped && isOnTrigger)
            {
                beforeAttack.Invoke();
                yield return new WaitForSeconds(attackTimeOffset);
                Hit();
                afterAttack.Invoke();
                yield return new WaitForSeconds(attackRate);
            }
        }
        private bool Hit()
        {
            //���������� ��� ������� �������� � ������ �����
            bool hasHitted = false;
            Collider2D[] hitObj = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damageLayer);
            pointRotation.StopRotating(true, 0.8f);
            onAttack.Invoke();

            //��������� ������ �� ��� �� ������� ���������� Health
            foreach (Collider2D obj in hitObj)
            {
                hasHitted = true;
                if (obj.TryGetComponent(typeof(PlayerHealth), out Component comp))
                    obj.GetComponent<PlayerHealth>().TakeHit(Random.Range(minDamage, maxDamage + 1));
            }
            
            return hasHitted;
        }

        //���� ������� � �������
        public void SetStop(bool active) { isStopped = active; }
        public bool GetStop() { return isStopped; }
        public bool IsOnTrigger => isOnTrigger;

        //���������� ������
        private void Awake() 
        {
            pointRotation = GetComponent<PointRotation>();
            if(enterTags.Count == 0) enterTags.Add("Player");
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (enterTags.Contains(collision.tag) && attackTarget != collision.transform)
            {
                isOnTrigger = true;
                attackTarget = collision.transform;
                onEnterArea.Invoke();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (attackTarget == collision.transform)
            {
                isOnTrigger = false;
                attackTarget = null;
            }
        }
        void OnDrawGizmosSelected()//��������� ������� �����
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}