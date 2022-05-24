using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Food", menuName = "Items/FoodItem")]
public class FoodItem : ScriptableObject
{
    //О предмете
    new public string name;
    [TextArea(3,5)]
    public string Dicription;
    public int uses;
    public int usesInGame;
    public UnityEvent itemAction;
    public int Cost;
    public bool CanRise;
    public int ChanceOfDrop;
    
    //Другие переменные
    public Sprite sprite;
    public Sprite WhiteSprite;
    public Sprite[] extraSprites;

    //Ссылки на другие скрипты
    private Health playerHealth;
    [HideInInspector] public FoodSlots slot;
    private Player plaeyrController;

    public void ActiveItem() // Скрипт для активации предмета
    {
        playerHealth =  GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();;
        plaeyrController = FindObjectOfType<Player>();
        usesInGame = 0;
    }

    public void PowerDrink()
    {
        playerHealth.TakeAwayHealth(1,1);
        plaeyrController.BoostSpeed(0.1f);
    }

    public void Cookie()
    {
        if(playerHealth.health != playerHealth.maxHealth)
        {
            playerHealth.Heal(1);
            SetSprite(extraSprites[0], null, slot.slotIcon);
        }
    }

    public void CannedCockroach()
    {
        if(playerHealth.health != playerHealth.maxHealth)
        {
            playerHealth.Heal(1);
            SetSprite(extraSprites[usesInGame], null, slot.slotIcon);
            usesInGame++;
        }
    }
    public void GlassOfMilk(){playerHealth.SetBonusHealth(1,0);}

    public void Blueberry()
    {
        if(playerHealth.health != playerHealth.maxHealth)
            playerHealth.Heal(1);
    }

    public void CheeseSnack()
    {
        if(playerHealth.health != playerHealth.maxHealth)
        {
            playerHealth.TakeAwayHealth(1,1);
            playerHealth.Heal(3);
        }
    }    
    public void BakedCockroach()
    {
        if(playerHealth.health != playerHealth.maxHealth)
            playerHealth.Heal(4);
    }

    public void SetSprite(Sprite newSprite, SpriteRenderer spriteRend = null, Image image = null)
    {
        if(spriteRend != null)
            spriteRend.sprite = newSprite;

        if(image != null)
            image.sprite = newSprite;
    }
}