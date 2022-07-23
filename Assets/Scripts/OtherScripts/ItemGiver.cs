using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemGiver : MonoBehaviour
{
    [Header("��������� ������ ��������")]
    [SerializeField] private GameObject item; //��� �������
    public bool changeSpriteAfterGive = false;
    [SerializeField] private Sprite spriteAfterGive;
    [SerializeField] private int giveCount = 1; //��� �������
    [SerializeField] ItemClass itemClass = ItemClass.Food; //����� ��������
    [SerializeField] public bool giveRightAway = false; //���� ����� � ���������/���������� � �����
    [SerializeField] private Transform itemPos; //����� ������ ��������(giveRightAway == true)
    [Header("��������� ��������")]
    [SerializeField] private string triggerTag = "Player"; //��� �� ������� ����� ������������ 
    [SerializeField] public bool changeSpriteOnTrigger = true; //����� �� �������� ������
    [SerializeField] private Sprite defaultSprite; //������� ������
    [SerializeField] private Sprite triggerSprite; //������ ��� ��������
    [SerializeField] public bool checkFromTriggerChecker = false; //��������� �� ������� ����� TriggerChecker
    
    //������ 
    private SpriteRenderer spriteRenderer;
    private InventoryManager inventoryManager;

    //���� ������ ��������
    private void GiveItem()
    {
        if(giveCount > 0)
        {
            if (!giveRightAway)
                Instantiate(item, itemPos.position, Quaternion.identity, itemPos);
            else
            {
                GameObject newItem = Instantiate(item, itemPos.position, Quaternion.identity, itemPos);
                newItem.GetComponent<ItemInfo>().pickUp.Invoke();                    
            }
            giveCount--;
            if (giveCount <= 0)
            {
                if(changeSpriteAfterGive) spriteRenderer.sprite = spriteAfterGive;
                else spriteRenderer.sprite = defaultSprite;
            }
        }
    }

    //�������� ������
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //�������� ���������
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!checkFromTriggerChecker && giveCount > 0 && collision.CompareTag(triggerTag))
        {
            spriteRenderer.sprite = triggerSprite;
            if (Input.GetKeyDown(KeyCode.E)) GiveItem();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!checkFromTriggerChecker && giveCount > 0 && collision.CompareTag(triggerTag))
            spriteRenderer.sprite = defaultSprite;
    }
}