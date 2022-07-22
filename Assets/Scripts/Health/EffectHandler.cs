using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectHandler : MonoBehaviour
{
    [Header("��������� ��������")]
    public List<EffectsList> effectsCanUse;
    private EffectStats burn;
    private EffectStats poison;
    private EffectStats bleed;
    private EffectStats regeneration;

    [Header("������")]
    public Health health;
    [SerializeField] private SpriteRenderer effectIndicator;
    [HideInInspector] public UnityEvent effects = new UnityEvent();
    
    //������ �� ������ �������
    private EffectsInfo effectManager;
    private GameManager gameManager;

    //������� ������� ����� ���������� �� �����    
    private IEnumerator EffectActive(float duration, EffectStats effectStats, EffectsList effect)
    {
        if (effectsCanUse.Contains(effect))
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

                    if (gameManager != null)
                        effectIndicator.sprite = gameManager.hollowSprite;
                    break;
            }
        }
    }
    public void GetEffect(float duration, EffectStats effectStats, EffectsList effect) => StartCoroutine(EffectActive(duration, effectStats, effect));
 
    //�������
    public void Burn()
    {
        if (Time.time >= burn.GetNextTime())
        {
            burn.SetNextTime(burn.effectRate);
            health.TakeHit(burn.effectStrength);
        }
    }
    public void Poison()
    {
        if (Time.time >= poison.GetNextTime())
        {
            poison.SetNextTime(poison.effectRate);
            health.TakeHit(poison.effectStrength);
        }
    }
    public void Bleed()
    {
        if (Time.time >= bleed.GetNextTime())
        {
            bleed.SetNextTime(bleed.effectRate);
            health.TakeHit(bleed.effectStrength);
        }
    }
    public void Regeneration()
    {
        if (Time.time >= regeneration.GetNextTime())
        {
            regeneration.SetNextTime(regeneration.effectRate);
            health.Heal(regeneration.effectStrength);
        }
    }

    private void Update() { effects.Invoke(); }
}