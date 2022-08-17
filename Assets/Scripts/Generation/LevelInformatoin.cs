using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformatoin : MonoBehaviour
{
    [SerializeField] private string levelName = "FirstLevelBasement"; //�������� ������
    [SerializeField] private string locationName = "Basement"; //�������� �������
    [SerializeField] private Sprite levelIcon; //������ �������
    [SerializeField] private string[] music; //������ ������� ����� ������ �� ���� ������

    //������� ��� ����� ������ � �������
    public string LevelName => levelName;
    public string LocationName => locationName;

    //������ ������ ������
    private void Start() { if(music.Length > 0) ManagerList.AudioManager.SetToMain(music[Random.Range(0, music.Length)]); }
}