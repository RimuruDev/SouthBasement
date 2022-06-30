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
        public ShootingPattern pattern;
    }

    [Header("���������")]
    public UsageParameters shootingController = UsageParameters.Independently;
    public Patterns patternsUsage = Patterns.UsePatterns;

    [Header("")]
    //��� ���������
    private float fireRate = 1f;
    private List<Bullet> bullets = new List<Bullet>();
    //C ���������
    public List<Pattern> patternsList = new List<Pattern>();

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}