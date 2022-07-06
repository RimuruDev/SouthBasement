using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public float patternChance = 100f;
        [SerializeField] public ShootingPattern pattern;
    }

    [Header("���������")]
    public UsageParameters shootingController = UsageParameters.Independently; //������ ����� ������������� ��������(���/� ������ �������)
    public ForceMode2D forceMode = ForceMode2D.Impulse; //���� ��� ��� �������
    public Patterns patternsUsage = Patterns.UsePatterns; //������ �������� ��� ������������ �������� ����

    //��� ���������
    [Header("��������� ������� ��������")]
    [SerializeField] private List<Bullet> bulletsList = new List<Bullet>();
    [SerializeField] private float fireRate = 1f; //�������� ��������
    private float nextTime = 0f; //���� ����� ��� ��������

    //C ���������
    [Header("��������� ���������")]
    public List<Pattern> patternsList = new List<Pattern>(); //���� ���������
    public float patternUseRate = 0.5f; //������� ������������� ���������
    [HideInInspector] public ShootingPattern currentPattern; //������� ������� ������ �������

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
            if (pattern.patternChance >= chance)
                patternsInChance.Add(pattern);
        }

        if (patternsInChance.Count == 0) Debug.Log(chance);
        return patternsInChance[Random.Range(0, patternsInChance.Count)];
    }
    private void SetNewPattern()
    {
        if (currentPattern != null && currentPattern.isWork) currentPattern.StopPattern(this);
        currentPattern = FindNewPattern().pattern;
        currentPattern = Instantiate(currentPattern);
        currentPattern.onExit.AddListener(SetNewPattern);
        UnityAction<Shooting> startMethod = currentPattern.StartPattern;
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
        pointRotation.offset = offset;
        if (forceMode == ForceMode2D.Force) speed *= 30;
        GameObject _projectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = _projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * speed, forceMode);
    }
    public void StopCurrentPattern() { if (currentPattern != null) currentPattern.StopPattern(this); }
    
    //���������� ������
    private void Start()
    {
        if(patternsUsage == Patterns.UsePatterns && shootingController == UsageParameters.Independently) SetNewPattern();
        pointRotation = GetComponent<PointRotation>();
    }
    private void Update()
    {
        if(shootingController == UsageParameters.Independently)
        {
            if(patternsUsage == Patterns.DontUsePatterns && Time.time >= nextTime)
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