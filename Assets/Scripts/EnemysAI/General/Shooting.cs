using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EnemysAI
{
    [RequireComponent(typeof(PointRotation))]
    public class Shooting : MonoBehaviour
    {
        //�����
        public enum UsageParameters
        {
            FromOtherScript,
            Independently
        }
        public enum BulletsType
        {
            Limited,
            Unlimited
        }
        public enum Patterns
        {
            UsePatterns,
            DontUsePatterns
        }

        //������
        [System.Serializable] public class Bullet
        {
            public BulletsType bulletsType = BulletsType.Limited; //������������ ���-�� ���� ��� ���
            public GameObject projectile; //���� ����
            public float bulletSpeed; //�������� ������ ����
            public float bulletChance = 100f; //���� ������ ����(��� ����� ���� � ����� �� �����������)
            public int bulletCount; //���-�� ����(��� ����������� ���-�� ���� �� �����������)
        }
        [System.Serializable] public class Pattern
        {
            public ShootingPattern pattern;
            public int chance;
            public UnityEvent onEnter;
            public UnityEvent onExit;
        }


        [Header("���������")]
        public UsageParameters shootingController = UsageParameters.Independently; //������ ����� ������������� ��������(���/� ������ �������)
        public ForceMode2D forceMode = ForceMode2D.Impulse; //���� ��� ��� �������
        public Patterns patternsUsage = Patterns.UsePatterns; //������ �������� ��� ������������ �������� ����

        //��� ���������
        [Header("��������� ������� ��������")]
        [SerializeField] private List<Bullet> bulletsList = new List<Bullet>();
        public UnityEvent onFire = new UnityEvent();
        [SerializeField] private float fireRate = 1f; //�������� ��������
        private float nextTime = 0f; //���� ����� ��� ��������

        //C ���������
        [Header("��������� ���������")]
        public List<Pattern> patternsList = new List<Pattern>(); //���� ���������
        public float patternUseRate = 0.5f; //������� ������������� ���������
        [HideInInspector] public Pattern currentPattern; //������� ������� ������ �������

        [Header("������")]
        [SerializeField] private Transform firePoint; //����� ������ ����
                                                      
        //������ �� ������ �������
        private PointRotation pointRotation;

        //��������
        private Pattern FindNewPattern()
        {
            float chance = Random.Range(0f, 100f);
            List<Pattern> patternsInChance = new List<Pattern>();

            foreach (Pattern pattern in patternsList)
            {
                if (pattern.chance >= chance)
                    patternsInChance.Add(pattern);
            }

            if (patternsInChance.Count == 0) Debug.Log(chance);
            return patternsInChance[Random.Range(0, patternsInChance.Count)];
        }
        private void SetNewPattern()
        {
            if (currentPattern.pattern != null && currentPattern.pattern.isWork) currentPattern.pattern.StopPattern(this);
            currentPattern = FindNewPattern();
            currentPattern.pattern = Instantiate(currentPattern.pattern);
            currentPattern.onExit.AddListener(SetNewPattern);
            currentPattern.pattern.onEnter = currentPattern.onEnter;
            currentPattern.pattern.onExit = currentPattern.onExit;
            UnityAction<Shooting> startMethod = currentPattern.pattern.StartPattern;
            Utility.InvokeMethod<Shooting>(startMethod, this, patternUseRate);
        }

        //����
        private int FindBullet()
        {
            float chance = Random.Range(0f, 100f);
            List<Bullet> bulletsInChance = new List<Bullet>();

            foreach (Bullet bullet in bulletsList)
            {
                if (bullet.bulletChance >= chance)
                    bulletsInChance.Add(bullet);
            }

            if (bulletsInChance.Count == 0) Debug.Log(chance);
            return Random.Range(0, bulletsList.Count);
        }

        //�������� ������
        public void Shoot(GameObject projectile, float offset, float speed)
        {
            if (projectile != null && firePoint != null)
            {
                pointRotation.offset = offset;
                if (forceMode == ForceMode2D.Force) speed *= 30;
                GameObject _projectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = _projectile.GetComponent<Rigidbody2D>();
                rb.AddForce(firePoint.up * speed, forceMode);
                onFire.Invoke();
            }
        }
        public void StopCurrentPattern() { if (currentPattern != null) currentPattern.pattern.StopPattern(this); }

        //���������� ������
        private void Start()
        {
            if (patternsUsage == Patterns.UsePatterns && shootingController == UsageParameters.Independently) SetNewPattern();
            pointRotation = GetComponent<PointRotation>();
        }
        private void Update()
        {
            if (shootingController == UsageParameters.Independently)
            {
                if (patternsUsage == Patterns.DontUsePatterns && Time.time >= nextTime)
                {
                    int bulletInd = FindBullet();
                    Shoot(bulletsList[bulletInd].projectile, 0f, bulletsList[bulletInd].bulletSpeed);
                    if (bulletsList[bulletInd].bulletsType == BulletsType.Limited)
                    {
                        bulletsList[bulletInd].bulletCount--;
                        if (bulletsList[bulletInd].bulletCount <= 0) bulletsList.RemoveAt(bulletInd);
                    }
                    nextTime = Time.time + fireRate;
                }
            }
        }
    }
}