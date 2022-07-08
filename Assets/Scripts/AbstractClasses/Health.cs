using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class EffectStats
{
    public EffectStats(float rate, int strength)
    {
        effectRate = rate;
        effectStrength = strength; 
    }
    public float effectRate;
    public int effectStrength;
    [HideInInspector] public float nextTime = 0f;
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
    [SerializeField] private string destroySound; // ���� ������
    [SerializeField] private string hitSound; // ���� ��������� �����

    [Header("�������")]
    public List<EffectsList> effectsCanUse;
    [HideInInspector] public EffectStats burn;
    /*[HideInInspector] */public EffectStats poison;
    [HideInInspector] public EffectStats bleed;
    [HideInInspector] public EffectStats regeneration;

    //�������
    [Header("�������")]
    public float destroyOffset = 0f;
    public UnityEvent onDie = new UnityEvent();  //������ ������� ���������� ��� ����������� �������
    public UnityEvent<int, int> onHealthChange = new UnityEvent<int, int>();
    public UnityEvent<float> stun = new UnityEvent<float>();
    public UnityEvent effects = new UnityEvent();

    //������
    private Coroutine damageInd = null;
    [HideInInspector] public EffectsInfo effectManager;
    public void DefaultOnDie() => Destroy(gameObject, destroyOffset);

    //������ ����������� �� ���������
    public abstract void TakeHit(int damage, float stunDuration = 0f);
    public abstract void TakeAwayHealth(int takeAwayMaxHealth, int takeAwayHealth);
    public abstract void Heal(int heal);
    public abstract void SetHealth(int newMaxHealth, int newHealth);
    public abstract void PlusNewHealth(int newMaxHealth, int newHealth);

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
                    burn.nextTime = 0f;
                    break;
                case EffectsList.Bleed:
                    //���������� �������
                    effectIndicator.sprite = effectManager.bleedndicator;
                    bleed = effectStats;
                    effects.AddListener(Bleed);

                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Bleed);
                    bleed.nextTime = 0f;
                    break;
                case EffectsList.Poison:
                    //���������� �������
                    effectIndicator.sprite = effectManager.poisonIndicator;
                    poison = effectStats;
                    effects.AddListener(Poison);

                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Poison);
                    poison.nextTime = 0f;
                    Debug.Log("[Test]: Poisoned active on - " + gameObject.name);
                    break;
                case EffectsList.Regeneration:
                    //���������� �������
                    effectIndicator.sprite = effectManager.regenerationIndicator;
                    regeneration = effectStats;
                    effects.AddListener(Regeneration);
                    
                    //����� �������
                    yield return new WaitForSeconds(duration);
                    effects.RemoveListener(Regeneration);
                    regeneration.nextTime = 0f;
                    break;
            }
        }
    }
    public void GetEffect(float duration, EffectStats effectStats, EffectsList effect) => StartCoroutine(EffectActive(duration, effectStats, effect));

    //�������
    public void Burn() 
    {
        if(burn.nextTime <= Time.time)
        {
            burn.nextTime = Time.time + burn.effectRate;
            TakeHit(burn.effectStrength); 
        }
    }
    public void Poison()
    {
        if (Time.time >= poison.nextTime)
        {
            Debug.Log("[Test]: Poison use on " + gameObject.name);
            poison.nextTime = Time.time + poison.effectRate;
            TakeHit(poison.effectStrength);
        }
        Debug.Log("[Test]: Poison hasn't been use on " + gameObject.name);
    }
    public void Bleed()
    {
        if (Time.time >= bleed.nextTime)
        {
            bleed.nextTime = Time.time + bleed.effectRate;
            TakeHit(bleed.effectStrength);
        }
    }
    public void Regeneration()
    {
        if (Time.time >= regeneration.nextTime)
        {
            regeneration.nextTime = Time.time + regeneration.effectRate;
            TakeHit(regeneration.effectStrength);
        }
    }
}