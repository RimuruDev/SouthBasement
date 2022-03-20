using UnityEngine;
using UnityEngine.UI;

public class FoodSlots : MonoBehaviour
{
    public FoodItem food;   // Предмет который лежит в этом слоте
    public GameObject objectOfItem;    // Гейм обжект этого предмета
    public Image slotIcon;   // Иконка предмета который лежит в этом слоте
    public bool isEmpty; // Используется ли этот слот сейчас
    public bool isActiveSlot; // Используется ли этот слот сейчас

    private void Update()
    {
        if(!isEmpty)
        {
            if(food.GetUses() <= 0)
            {
                Destroy(objectOfItem);
                Remove();
            }    
            if(isActiveSlot & !isEmpty & Input.GetKeyDown(KeyCode.Space))
            {       
                food.itemAction.Invoke();
                Debug.Log("useItem");
            }
        }
    }

    public void Add(FoodItem newFood, GameObject _objectOfItem) // Добавеление предмета
    {
        if(food != null)
            Drop();
            
        food = newFood;
        isEmpty = false;
        objectOfItem = _objectOfItem;
        slotIcon.sprite = newFood.sprite;
        food.slot = gameObject.GetComponent<FoodSlots>();  
    }
    public void Drop() // Выброс предмета в игре
    {
        objectOfItem.SetActive(true);
        objectOfItem.transform.position = FindObjectOfType<Player>().GetComponent<Transform>().position;
        Remove();
    }
    public void Remove() // Удаление предмета из слота
    {
        food.slot = null;  
        food = null;
        objectOfItem = null;
        isEmpty = true;
        slotIcon.sprite = FindObjectOfType<GameManager>().hollowSprite;
    }
}
