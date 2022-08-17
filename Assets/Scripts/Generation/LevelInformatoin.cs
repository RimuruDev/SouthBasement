using UnityEngine;
using UnityEngine.UI;

public class LevelInformatoin : MonoBehaviour
{
    [Header("����������")]
    [SerializeField] private string levelName = "FirstLevelBasement"; //�������� ������
    [SerializeField] private string locationName = "Basement"; //�������� �������
    [SerializeField] private Sprite levelIcon; //������ �������
    [SerializeField] private string[] music; //������ ������� ����� ������ �� ���� ������
    [Header("���������")]
    [SerializeField] private string outPutLevelName; //�������� ������ ������� ����� ��������� � ������
    [SerializeField] private string outPutLocationName; //�������� ������ ����� ��������� � ������

    //������� ��� ����� ������ � �������
    public string LevelName => levelName;
    public string LocationName => locationName;
    public void UpdateUIInformationAboutLevel(Image levelUIIcon, Text informationUIText)
    {
        informationUIText.text =
            outPutLocationName + "\n" +
            outPutLevelName;
        levelUIIcon.sprite = levelIcon;
    }

    //������ ������ ������
    private void Start()  { if (music.Length > 0) ManagerList.AudioManager.SetToMain(music[Random.Range(0, music.Length)]); }
}