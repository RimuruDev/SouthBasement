using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelleWeaponPickUp : MonoBehaviour
{
    public MelleRangeWeapon melleWeapon; // Предмет
    public ItemInfo itemInfo;
    private InventoryManager inventory; 
    private GameManager gameManager; 
    public Trader trader; 
    private InputManager inputManager;

    public bool canDestoring = true; // Будет ли уничтожаться при старте если предмет пустой
    private bool isOnTrigger = false; // Если стоит на триггере
    private bool isWhiteSprite = false; // Стоит ли уже спрайт с обводкой
    public bool isForTrade = false; // Продается ли этот предмет


    private void Awake()
    {
        if(melleWeapon == null & canDestoring)
            Destroy(gameObject);
        else if(!canDestoring) // Если canDestroing == false, то просто спрайт прдмета будет становиться прозрачным
            gameObject.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<GameManager>().hollowSprite;      
        if(melleWeapon != null)
        {
            // Ставим спрайт предмета и ищем инвентарь
            gameObject.GetComponent<SpriteRenderer>().sprite = melleWeapon.sprite;
            itemInfo = FindObjectOfType<ItemInfo>();

            //Записываем всю информацию о предмете в ItemInfo
            itemInfo.itemTipe = "MelleRangeWeapon";
            itemInfo.itemName = melleWeapon.name;
            itemInfo.discription = melleWeapon.Dicription;
            itemInfo.cost = melleWeapon.Cost;
            itemInfo.chanceOfDrop = melleWeapon.ChanceOfDrop;
        }
        inventory = FindObjectOfType<InventoryManager>();
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();
    } 

    private void OnTriggerEnter2D(Collider2D coll) 
    {
        if(coll.tag == "Player")
        {
            isOnTrigger = true;
            itemInfo.isOnTrigger = isOnTrigger;
        }
    }
    
    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            isOnTrigger = false;
            itemInfo.isOnTrigger = isOnTrigger;
        }
    }


    private void Update()
    {
        //Смена обычного спрайта на спрайт с обводкой и наоборот
        if(isOnTrigger & !isWhiteSprite)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = melleWeapon.WhiteSprite;
            isWhiteSprite = true;
        }
        else if(!isOnTrigger & isWhiteSprite)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = melleWeapon.sprite;
            isWhiteSprite = false;
        }


        //Поднимание прдмета
        if(isOnTrigger & Input.GetKeyDown(inputManager.PickUpButton))
        {
            if(isForTrade)
                Trade();

            else // Поднимаем предмет
            {
                inventory.AddMelleWeapon(melleWeapon, gameObject);
                gameObject.SetActive(false);
            }
        }
        if(isForTrade && trader == null)
            isForTrade = false;
    }

    private void Trade() // Продаем предмет
    {
        if(gameManager.playerCheese >= melleWeapon.Cost)
        {
            gameManager.CheeseScore(-melleWeapon.Cost);
            isForTrade = false;
            inventory.AddMelleWeapon(melleWeapon, gameObject);
            gameObject.SetActive(false);
        }
        else // Если не хватает сыра
            trader.DisplayFrase(trader.SentenceNotEnoghtCheese, 5f);
    }
}
