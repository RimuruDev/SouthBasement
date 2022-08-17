using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Generation
{
    public abstract class RoomTemplate : MonoBehaviour
    {
        public enum Directions //������ �����������
        {
            Up,
            Down,
            Left,
            Right
        }

        [Header("���������")]
        [SerializeField] protected int passagesCountMin = 2; //�����������, ������������ ���-�� �������� ��� �� �������
        [SerializeField] public int passagesCountMax = 3;
        [SerializeField] protected bool randomizePassagesOnAwake = false; //����� �� ���������� ������� ��� ������ Awake()
        [SerializeField] protected bool isStartRoom = false; //��������� �� �������
        [SerializeField] protected List<Transform> pointsForSomething; //����� ��� ������ ��������� �����
        [Header("�������")] //�������
        [SerializeField] protected RoomSpawner upPassage; //������� ������
        [SerializeField] protected RoomSpawner downPassage; //������ ������
        [SerializeField] protected RoomSpawner leftPassage; //����� ������
        [SerializeField] protected RoomSpawner rightPassage; //������ ������

        [Header("��������� ������")] //������� ��� ����� ������
        [SerializeField] protected Vector2 instantiatePositionUp = new Vector2(0f, -18f);
        [SerializeField] protected Vector2 instantiatePositionDown = new Vector2(0f, 18f);
        [SerializeField] protected Vector2 instantiatePositionLeft = new Vector2(-18f, 0f);
        [SerializeField] protected Vector2 instantiatePositionRight = new Vector2(18f, 0f);

        //������ �� ������ ����
        protected List<RoomTemplate> spawnedRooms = new List<RoomTemplate>(); // ������������ ����� �������
        protected RoomSpawner startingSpawnPoint; // ����� ������ ���� ���������� ��� �������

        //������� � �������
        public List<RoomTemplate> SpawnedRooms => spawnedRooms; //���-�� ����������� ����� ������
        public bool IsStartRoom => isStartRoom; //��������� �� �������
        public RoomSpawner StartingSpawnPoint => startingSpawnPoint; // ����� ������ ���� ���������� ��� �������
        public void AddSpawnedRoom(RoomTemplate newRoom) => spawnedRooms.Add(newRoom);  //�������� ������������ �������
        public void SetStartingSpawnPoint(RoomSpawner point) //��������� ��������� �����
        {
            startingSpawnPoint = point;
            startingSpawnPoint.onClose.AddListener(startingSpawnPoint.DestroyRoom);
        }
        public Vector2 GetInstantiatePosition(Directions direction) //�������� ������� ������ �� �����������
        {
            switch (direction)
            {
                case Directions.Up:
                    return instantiatePositionUp;
                case Directions.Down:
                    return instantiatePositionDown;
                case Directions.Left:
                    return instantiatePositionLeft;
                case Directions.Right:
                    return instantiatePositionRight;
            }
            return new Vector2(18f, 18f);
        }
        public RoomSpawner GetPassage(Directions direction) //�������� ������ �� �����������
        {
            switch (direction)
            {
                case Directions.Up:
                    return upPassage;
                case Directions.Down:
                    return downPassage;
                case Directions.Left:
                    return leftPassage;
                case Directions.Right:
                    return rightPassage;
            }
            return null;
        }

        //������
        //��������� ������ ��� ������������ ���-�� ����������� ����� �������
        protected void CheckSpawnedRoomsCount(int roomsCount, UnityAction action) { if (spawnedRooms.Count == roomsCount) action.Invoke(); }
        public void SpawnSomething(GameObject something) //����� ����-�� � �������
        {
            int randomPlace = Random.Range(0, pointsForSomething.Count);
            Instantiate(something, pointsForSomething[randomPlace].position, Quaternion.identity, pointsForSomething[randomPlace]);
            pointsForSomething.RemoveAt(randomPlace);
        }
        public void RandomizePassages() //������������ ��������
        {

            //�������� ���-�� ��������
            if (passagesCountMin <= 0) passagesCountMin = 1;
            if (passagesCountMax > 4) passagesCountMax = 4;

            int passagesCount = Random.Range(passagesCountMin, passagesCountMax + 1);

            for (int i = 0; i < 4 - passagesCount; i++)
            {
                int index = Random.Range(0, 4);
                if (index == 0 && upPassage.State == RoomSpawnerState.Open)
                {
                    bool isClosed = upPassage.Close(true);
                    if (isClosed) continue;
                }
                if (index == 1 && downPassage.State == RoomSpawnerState.Open)
                {
                    bool isClosed = downPassage.Close(true);
                    if (isClosed) continue;
                }
                if (index == 2 && leftPassage.State == RoomSpawnerState.Open)
                {
                    bool isClosed = leftPassage.Close(true);
                    if (isClosed) continue;
                }
                if (index == 3 && rightPassage.State == RoomSpawnerState.Open)
                {
                    bool isClosed = rightPassage.Close(true);
                    if (isClosed) continue;
                }
                i--;
            }
        }
        protected void DefaultOnDestroy()
        {
            //����������� ������������ ����� ������ � �������� ���������� �������
            if (upPassage != null && upPassage.SpawnedRoom != null)
                upPassage.DestroyRoom();
            if (downPassage != null && downPassage.SpawnedRoom != null)
                downPassage.DestroyRoom();
            if (leftPassage != null && leftPassage.SpawnedRoom != null)
                leftPassage.DestroyRoom();
            if (rightPassage != null && rightPassage.SpawnedRoom != null)
                rightPassage.DestroyRoom();
            if (StartingSpawnPoint != null) StartingSpawnPoint.ForcedClose();
        }

        //����������� ������
        protected abstract void OnSpawned();
        protected abstract void StartSpawning();
    }
}