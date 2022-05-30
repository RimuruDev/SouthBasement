﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    private struct Effect{
        public float startTime;
        public float durationTime;
    };
    //Показатели здоровья
    public int health;
    public int maxHealth;

    private GameManager gameManager;
    [SerializeField] private AngryRatAI angryRatAi;
    private EffectsManager effectsManager;
    private AudioManager audioManager;
    public SpriteRenderer effectIndicator;
    [SerializeField] private Color damageColor;

    [SerializeField] int minCheese = 0;
    [SerializeField] int maxCheese = 4;
    [SerializeField] private string destroySound; // Звук смерти
    [SerializeField] private string hitSound; // Звук получения урона
    public RoomCloser roomCloser;

    //Время действия еффектов
    private Effect burn;
    private Effect bleed;
    private Effect poisoned;
    private Effect regeneration;
    private Coroutine damageInd = null;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        effectsManager = FindObjectOfType<EffectsManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void TakeHit(int damage, float stunTime = 0f)
    {
        health -= damage;
        if (stunTime != 0f) angryRatAi.Stun(stunTime);

        if(hitSound != "")audioManager.PlayClip(hitSound);
        
        if(damageInd != null)
            StopCoroutine(damageInd); 
            
        damageInd = StartCoroutine(TakeHitVizualization());

        if (health <= 0)
        {
            int cheeseCount = Random.Range(minCheese,maxCheese);
            Debug.Log("CheeseInEnemy" + cheeseCount);
            if(maxCheese != 0) gameManager.SpawnCheese(transform.position, cheeseCount);
            if(destroySound != "") audioManager.PlayClip(destroySound);
            Destroy(gameObject);
        }         
    }
    private IEnumerator TakeHitVizualization()
    {
        gameObject.GetComponent<SpriteRenderer>().color = damageColor;
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(100,100,100,100);
    }
    public void Heal(int bonusHealth)
    {
        health += bonusHealth;

        if(health > maxHealth)
            health = maxHealth;     
    }
    public void SetHealth(int NewMaxHealth, int NewHealth)
    {
        maxHealth = NewMaxHealth;
        health = NewHealth;
        
        if(health > maxHealth)
            health = maxHealth;
    }
    public void TakeAwayHealth(int TakeAwayMaxHealth, int TakeAwayHealth)
    {
        maxHealth -= TakeAwayMaxHealth;
        health -= TakeAwayHealth;
       
        if(health > maxHealth)
            health = maxHealth;
    }
    public void SetBonusHealth(int NewMaxHealth, int NewHealth)
    {
        Debug.Log("New Health");
        maxHealth += NewMaxHealth;
        health += NewHealth;

        if(health > maxHealth)
            health = maxHealth;
    }
    private void OnDestroy()
    {
        if (roomCloser != null)
            roomCloser.enemyesCount--;
    }

    //Еффекты которые могут наложиться на врага    
    public void ResetBurn() { effectsManager.Burn.listeners.RemoveListener(Burn); burn.durationTime = 0f; burn.startTime = 0f;effectIndicator.sprite = gameManager.hollowSprite;}
    public void ResetPoisoned() { effectsManager.Poisoned.listeners.RemoveListener(Poisoned); poisoned.durationTime = 0f; poisoned.startTime = 0f;effectIndicator.sprite = gameManager.hollowSprite;}
    public void ResetBleed() { effectsManager.Bleed.listeners.RemoveListener(Bleed); bleed.durationTime = 0f; bleed.startTime = 0f; effectIndicator.sprite = gameManager.hollowSprite;}

    public void Burn(){TakeHit(1);}
    public void Poisoned(){TakeHit(1);}
    public void Bleed(){TakeHit(2);}
    public void Regeneration(){Heal(1);}
}