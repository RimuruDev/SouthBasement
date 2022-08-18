using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [Header("��������� �����������")]
    [SerializeField] protected string minimapLayer = "Blocks"; //���� ������� ������ ���������
    [SerializeField] private GameObject[] objectsToChangeLayer; //������� ������� ���� �������� ����
    private bool layerChanged = false; //��� �� ��� ������� ����

    //���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !layerChanged)
        {
            foreach (GameObject nextObject in objectsToChangeLayer)
            {
                nextObject.layer = LayerMask.NameToLayer(minimapLayer);
                Transform[] children = nextObject.GetComponentsInChildren<Transform>();
                
                foreach(Transform nextChild in children)
                    nextChild.gameObject.layer = LayerMask.NameToLayer(minimapLayer);
                
                layerChanged = true;
            }
        }
    }
}
