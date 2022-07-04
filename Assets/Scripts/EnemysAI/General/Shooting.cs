using System.Collections.Generic;
using UnityEngine;

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
        public int bulletCount; //����� ����(��� ����������� ���-�� ���� �� �����������)
    }
    [System.Serializable] public class Pattern
    {
        public float patternChance = 100f;
        [SerializeField] public ShootingPattern pattern;
    }

    [Header("���������")]
    [SerializeField] private Transform firePoint;
    public UsageParameters shootingController = UsageParameters.Independently;
    public ForceMode2D forceMode = ForceMode2D.Impulse;
    public Patterns patternsUsage = Patterns.UsePatterns;

    [Header("")]
    //��� ���������
    private float fireRate = 1f;
    private List<Bullet> bullets = new List<Bullet>();
    //C ���������
    public List<Pattern> patternsList = new List<Pattern>();
    public float patternWaitTime = 0.5f;
    public Pattern currentPattern;

    //������ �� ������ �������
    private PointRotation pointRotation;

    //��������
    private Pattern FindNewPattern()
    {
        float chance = Random.Range(0f, 100f);
        List<Pattern> patternsInChance = new List<Pattern>();
        
        foreach(Pattern pattern in patternsList)
        {
            if (pattern.patternChance >= chance)
                patternsInChance.Add(pattern);
        }

        if(patternsInChance.Count == 0) Debug.Log(chance);
        return patternsInChance[Random.Range(0, patternsInChance.Count)];
    }
    private void SetNewPattern()
    {
        if (currentPattern != null && currentPattern.pattern.isWork) currentPattern.pattern.StopPattern(this); 
        currentPattern = FindNewPattern();
        currentPattern.pattern = Instantiate(currentPattern.pattern);
        currentPattern.pattern.onExit.AddListener(SetNewPattern);
        currentPattern.pattern.StartPattern(this, patternWaitTime);
    }

    //�������� ������
    public void Shoot(GameObject projectile, float offset, float speed)
    {
        pointRotation.offset = offset;
        GameObject _projectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = _projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * speed, forceMode);
    }

    private void Start()
    {
        SetNewPattern();
        pointRotation = GetComponent<PointRotation>();
    }
    private void Update()
    {
    }
}