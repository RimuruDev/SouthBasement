using UnityEngine;
using UnityEngine.UI;

public class ActiveItemsSlots : MonoBehaviour
{
    public ActiveItem activeItem;   // Предмет который лежит в этом слоте
    [SerializeField] private Image slotIcon;   // Иконка предмета который лежит в этом слоте
    [SerializeField] private Slider slider;   // Иконка предмета который лежит в этом слоте
    public GameObject objectOfItem;   // гейм обжект этого предмета

    public bool isEmpty; // Пустой ли этот слот
    public bool isActiveSlot; // Используется ли этот слот сейчас

    private InventoryManager inventoryManager;
    private Player playerController;

    //Использовние активки на зажатую клавишу
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float startTime;
    [SerializeField] private bool waitTimeSetted;
                    
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        playerController = FindObjectOfType<Player>();
    }

    private void Update() //Использование предмета
    {
        if (!isEmpty & isActiveSlot & objectOfItem.GetComponent<ItemInfo>().uses > 0)
        {
            //При зарядке
            if (activeItem.useMode == UseMode.Charge & !playerController.isSprinting)
            {
                if (!activeItem.isItemCharged)
                {
                    if (Input.GetKey(KeyCode.Space) & Time.time >= activeItem.GetNextTime())
                    {
                        SetNewWaitTime();
                        waitTime = Time.time - startTime;
                        inventoryManager.activeItemChargeSlider.value = waitTime;

                        if (activeItem.chargeTime <= waitTime) // Если активка зарядилась
                            activeItem.isItemCharged = true;
                    }
                    else //Сброс зарядки если кнопка была отпущена
                        ResetWaitTime();
                }

                //Использование активки после того как она зарядилась
                if (Input.GetKeyUp(KeyCode.Space) & activeItem.isItemCharged)
                    UseActiveItem();
            }
            else if (playerController.isSprinting)
                ResetWaitTime();

            //При простом нажатии
            else if(activeItem.useMode == UseMode.UseImmedialty)// Если заряжать активку не надо
            {
                if (Input.GetKey(KeyCode.Space) & Time.time >= activeItem.GetNextTime() & !playerController.isSprinting)
                    UseActiveItem();
            }

            // Визуализация перезарядки активки
            slider.value = activeItem.GetNextTime() - Time.time;
        }
        //Выкидываем если 0 использований
        else if(!isEmpty && objectOfItem.GetComponent<ItemInfo>().uses > 0)
            Drop();
    }

    public void Add(ActiveItem newActiveItem, GameObject _objectOfItem) // Добавеление предмета
    {
        if(activeItem != null)
            Drop();

        activeItem = newActiveItem;
        objectOfItem = _objectOfItem;
        isEmpty = false;
        slotIcon.sprite = newActiveItem.sprite;
    }

    public void Drop() // Выброс предмета в игре
    {
        if(objectOfItem != null)
        {
            objectOfItem.SetActive(true);
            objectOfItem.transform.position = FindObjectOfType<Player>().GetComponent<Transform>().position;
        }
        Remove();
    }
    
    public void Remove() // Удаление предмета из слота
    {
        ResetWaitTime();
        activeItem = null;
        objectOfItem.GetComponent<DontDestroyOnLoadNextScene>().Disable();
        objectOfItem.GetComponent<ItemInfo>().SetActive(true);//Этот метод отключает спрайт и триггер прелмета
        objectOfItem = null;
        isEmpty = true;
        slotIcon.sprite = FindObjectOfType<GameManager>().hollowSprite;
    }

    private void SetNewWaitTime() // 
    {
        if(!waitTimeSetted)
        {
            inventoryManager.activeItemChargeSlider.maxValue = activeItem.chargeTime;
            waitTime = 0f;
            startTime = Time.time;
            waitTimeSetted = true;
        }
    }
    private void ResetWaitTime() // Сброс всех показателей зарядки
    {
        waitTime = 0f;
        startTime = 0f;
        waitTimeSetted = false;
        activeItem.isItemCharged = false;
        inventoryManager.activeItemChargeSlider.value = 0f;
    }


    private void UseActiveItem()
    {
        activeItem.itemAction.Invoke();
        activeItem.SetNextTime();
        objectOfItem.GetComponent<ItemInfo>().uses--;
        Debug.Log("Use active item");
        slider.maxValue = activeItem.GetNextTime() - Time.time;
        ResetWaitTime();    
    }
}
