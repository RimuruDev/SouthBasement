using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EnemysAI
{
    [RequireComponent(typeof(PointRotation))]
    public class Shooting : MonoBehaviour
    {
        //����� ��� ������������ � ���������� ����������
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
            [SerializeField] private ShootingPattern shootingPattern; //��� �������
            
            [Header("��������� ������������� ��������")]
            [SerializeField] private int chance; //���� ������������� ����� ��������
            [SerializeField] private float useRate; //����� �������� ����� ������������� ����� ��������
            
            [Header("�������")]
            public UnityEvent onEnter; //��� ��������� ����� ��������
            public UnityEvent onExit; //�� ����� ���������� ����� ��������

            //������� � ������ �������������� � ���������
            public int GetChance() { return chance; }
            public float GetUseRate() { return useRate; }
            public bool GetIsWork() { if (shootingPattern != null) return shootingPattern.isWork; else return false; }

            public void StartPattern(Shooting shooting) { if (shootingPattern) shootingPattern.StartPattern(shooting); }
            public void StopPattern(Shooting shooting) { if (shootingPattern) shootingPattern.StartPattern(shooting); }
            public void ActivePattern()
            {
                shootingPattern = Instantiate(shootingPattern);

                shootingPattern.onEnter = onEnter;
                shootingPattern.onExit = onExit;
            }
        
        }

        //����������
        [Header("���������")]
        private bool isStopped = false;
        public UsageParameters shootingController = UsageParameters.Independently; //������ ����� ������������� ��������(���/� ������ �������)
        public ForceMode2D forceMode = ForceMode2D.Impulse; //���� ��� ��� �������
        public Patterns patternsUsage = Patterns.UsePatterns; //������ �������� ��� ������������ �������� ����

        //��� ���������
        [Header("��������� ������� ��������")]
        [SerializeField] private List<Bullet> bulletsList = new List<Bullet>();
        public UnityEvent onFire = new UnityEvent();
        private float nextTime = 0f; //���� ����� ��� ��������
        [SerializeField] private float fireRate;

        //C ���������
        [Header("��������� ���������")]
        public List<Pattern> patternsList = new List<Pattern>(); //���� ���������
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
                if (pattern.GetChance() >= chance)
                    patternsInChance.Add(pattern);
            }

            if (patternsInChance.Count == 0) Debug.Log(chance);
            return patternsInChance[Random.Range(0, patternsInChance.Count)];
        }
        private void SetNewPattern()
        {
            if (currentPattern != null && currentPattern.GetIsWork()) currentPattern.StopPattern(this);
            
            currentPattern = FindNewPattern();
            currentPattern.ActivePattern();
        }
        
        //�������� ������ ��������
        public void StartWorking()
        {
            if (patternsUsage == Patterns.UsePatterns && shootingController == UsageParameters.Independently)
                StartCoroutine(ActivatePatterns());
            else if (shootingController == UsageParameters.Independently)
                StartCoroutine(ActivateShooting(fireRate));
        }
        private IEnumerator ActivatePatterns()
        {
            float patternUseRate = 2f;
            
            while(true)
            {
                Debug.Log("[Shooting] - Working");
                if(!isStopped)
                {
                    SetNewPattern();
                    patternUseRate = currentPattern.GetUseRate();
                    currentPattern.StartPattern(this);
                }
                yield return new WaitForSeconds(patternUseRate);
            }
        }
        private IEnumerator ActivateShooting(float fireRate)
        {   
            while(true)
            {
                if(!isStopped)
                {
                    int bulletInd = FindBullet();
                    Shoot(bulletsList[bulletInd].projectile, 0f, bulletsList[bulletInd].bulletSpeed);
                    if (bulletsList[bulletInd].bulletsType == BulletsType.Limited)
                    {
                        bulletsList[bulletInd].bulletCount--;
                        if (bulletsList[bulletInd].bulletCount <= 0) bulletsList.RemoveAt(bulletInd);
                    }
                }
                yield return new WaitForSeconds(fireRate);
            }
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

        //������ �������������� �� ��������
        public void SetStop(bool active) => isStopped = active;
        public bool GetStop() { return isStopped; }

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
        public void StopCurrentPattern() { if (currentPattern != null) currentPattern.StopPattern(this); }

        //���������� ������
        private void Start()
        {
            pointRotation = GetComponent<PointRotation>();
            SetStop(true);
        }
        private void OnEnable()
        {
            if(pointRotation == null)
                pointRotation = GetComponent<PointRotation>();
            StartWorking();
            Debug.Log("[Info]: Shooting component has been enabled");
        }
    }
}