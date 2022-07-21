using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EffectStats
{
    public EffectStats(float rate, int strength)
    {
        effectRate = rate;
        effectStrength = strength;
    }
    public float effectRate;
    public int effectStrength;
    private float nextTime = 0f;
    public void SetNextTime(float time) => nextTime += time;
    public void ResetToZeroNextTime() => nextTime = 0f;
    public float GetNextTime() { return nextTime; }
};
public abstract class Health : MonoBehaviour
{
    //���������� ��������
    [Header("��������")]
    public int health;
    public int maxHealth;

    [SerializeField] private Color damageColor;
    public SpriteRenderer effectIndicator;

    [Header("�����")]
    [SerializeField] protected string destroySound; // ���� ������
    [SerializeField] protected string hitSound; // ���� ��������� �����

    [Header("�������")]
    public List<EffectsList> effectsCanUse;
    [HideInInspector] public EffectStats burn;
    [HideInInspector] public EffectStats poison;
    [HideInInspector] public EffectStats bleed;
    [HideInInspector] public EffectStats regeneration;

    //�������
    [Header("�������")]
    [SerializeField] protected bool destroyOnDie = true;
    [SerializeField] protected float destroyOffset = 0f;
    public UnityEvent onDie = new UnityEvent();  //������ ������� ���������� ��� ����������� �������
    public UnityEvent<int, int> onHealthChange = new UnityEvent<int, int>();
    public UnityEvent<float> stun = new UnityEvent<float>();
    public UnityEvent effects = new UnityEvent();

    //������
    private Coroutine damageInd = null;
    protected EffectsInfo effectManager;
    protected GameManager gameManager;

    //������ ����������� �� ���������
    public abstract void TakeHit(int damage, float stunDuration = 0f);
    public abstract void Heal(int heal);
    public abstract void SetHealth(int newMaxHealth, int newHealth);

    public IEnumerator TakeHitVizualization()
    {
        gameObject.GetComponent<SpriteRenderer>().color = damageColor;
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 100);
    }

    //������� ������� ����� ���������� �� �����    
    private IEnumerator EffectActive(float duration, EffectStats effectStats, EffectsList effect)
    {
        if(effectsCanUse.Contains(effect))
        {
            switch (effect)
            {
                case EffectsList.Burn:
                    //���������� �������
                    effectIndicator.sprite = effectManager.burnIndicator;
                    burn = effectStats;
                    effects.AddListener(Burn);

                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Burn);
                    burn.ResetToZeroNextTime();
                    effectIndicator.sprite = gameManager.hollowSprite;
                    break;
                case EffectsList.Bleed:
                    //���������� �������
                    effectIndicator.sprite = effectManager.bleedndicator;
                    bleed = effectStats;
                    effects.AddListener(Bleed);

                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Bleed);
                    bleed.ResetToZeroNextTime();
                    effectIndicator.sprite = gameManager.hollowSprite;
                    break;
                case EffectsList.Poison:
                    //���������� �������
                    effectIndicator.sprite = effectManager.poisonIndicator;
                    poison = effectStats;
                    effects.AddListener(Poison);

                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Poison);
                    poison.ResetToZeroNextTime();
                    effectIndicator.sprite = gameManager.hollowSprite;
                    break;
                case EffectsList.Regeneration:
                    //���������� �������
                    effectIndicator.sprite = effectManager.regenerationIndicator;
                    regeneration = effectStats;
                    effects.AddListener(Regeneration);
                    
                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Regeneration);
                    regeneration.ResetToZeroNextTime();
                    effectIndicator.sprite = gameManager.hollowSprite;
                    break;
            }
        }
    }
    public void GetEffect(float duration, EffectStats effectStats, EffectsList effect) => StartCoroutine(EffectActive(duration, effectStats, effect));

    //�������
    public void Burn() 
    {
        if(Time.time >= burn.GetNextTime())
        {
            burn.SetNextTime(burn.effectRate);
            TakeHit(burn.effectStrength); 
        }
    }
    public void Poison()
    {
        if (Time.time >= poison.GetNextTime())
        {
            poison.SetNextTime(poison.effectRate);
            TakeHit(poison.effectStrength);
        }
    }
    public void Bleed()
    {
        if (Time.time >= bleed.GetNextTime())
        {
            bleed.SetNextTime(bleed.effectRate);
            TakeHit(bleed.effectStrength);
        }
    }
    public void Regeneration()
    {
        if (Time.time >= regeneration.GetNextTime())
        {
            regeneration.SetNextTime(regeneration.effectRate);
            TakeHit(regeneration.effectStrength);
        }
    }
}