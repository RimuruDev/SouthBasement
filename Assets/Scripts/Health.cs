using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour 
{
    //���������� ��������
    [Header("��������")]
    public int health;
    public static int maxHealth;

    public SpriteRenderer effectIndicator;
    [SerializeField] private Color damageColor;

    [Header("�����")]
    [SerializeField] private string destroySound; // ���� ������
    [SerializeField] private string hitSound; // ���� ��������� �����

    //�������
    [Header("�������")]
    public UnityEvent onDestroy = new UnityEvent();  //������ ������� ���������� ��� ����������� �������
    public UnityEvent<float> stun = new UnityEvent<float>();
    private UnityEvent effects = new UnityEvent();

    //������
    private Coroutine damageInd = null;

    //������ �� ������ ������
    public RoomCloser roomCloser;
    private GameManager gameManager;
    private AudioManager audioManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        if (roomCloser != null)
        {
            roomCloser.EnemyCounterTunUp();
            onDestroy.AddListener(roomCloser.EnemyCounterTunDown);
        }
    }
    private void OnDestroy() { onDestroy.Invoke(); }

    //������ ����������� �� ���������
    public abstract void TakeHit(int damage);
    private IEnumerator TakeHitVizualization()
    {
        gameObject.GetComponent<SpriteRenderer>().color = damageColor;
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 100);
    }
    public abstract void Heal(int heal);
    public void SetHealth(int NewMaxHealth, int NewHealth)
    {
        maxHealth = NewMaxHealth;
        health = NewHealth;

        if (health > maxHealth)
            health = maxHealth;
    }
    public void TakeAwayHealth(int TakeAwayMaxHealth, int TakeAwayHealth)
    {
        maxHealth -= TakeAwayMaxHealth;
        health -= TakeAwayHealth;

        if (health > maxHealth)
            health = maxHealth;
    }
    public void SetBonusHealth(int NewMaxHealth, int NewHealth)
    {
        Debug.Log("New Health");
        maxHealth += NewMaxHealth;
        health += NewHealth;

        if (health > maxHealth)
            health = maxHealth;
    }

    //������� ������� ����� ���������� �� �����    
    private IEnumerator EffectActive(float duration, EffectsList effect)
    {
        switch (effect)
        {
            case EffectsList.Burn:
                effects.AddListener(Burn);
                break;
            case EffectsList.Bleed:
                effects.AddListener(Burn);
                break;
            case EffectsList.Poisoned:
                effects.AddListener(Burn);
                break;
                effects.AddListener(Burn);
                break;
        }
        yield return new WaitForSeconds(duration);
    }

    public void Burn() { TakeHit(9); }
    public void Poisoned() { TakeHit(5); }
    public void Bleed() { TakeHit(14); }
    public void Regeneration() { Heal(10); }
}
