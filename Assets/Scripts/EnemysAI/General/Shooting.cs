using System.Collections.Generic;
using UnityEngine;

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
    public Pattern currentPattern;
    
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
    public void Shoot(GameObject projectile)
    {
        GameObject _projectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = _projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * 500, forceMode);
    }
    private void SetNewPattern()
    {
        if (currentPattern != null && currentPattern.pattern.isWork) currentPattern.pattern.StopPattern(this); 
        currentPattern = FindNewPattern();
        currentPattern.pattern = Instantiate(currentPattern.pattern);
        currentPattern.pattern.onExit.AddListener(SetNewPattern);
        currentPattern.pattern.StartPattern(this);
    }


    private void Start() => SetNewPattern();
    private void Update()
    {
    }
}