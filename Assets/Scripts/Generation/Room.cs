using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Generation
{
    public class Room : MonoBehaviour
    {
        public enum Directions //������ �����������
        {
            Up,
            Down,
            Left,
            Right
        }

        [Header("���������")]
        [SerializeField] private int passagesCountMin = 2; //�����������, ������������ ���-�� �������� ��� �� �������
        [SerializeField] public int passagesCountMax = 3;
        [SerializeField] private bool randomizePassagesOnAwake = false; //����� �� ���������� ������� ��� ������ Awake()
        [SerializeField] private bool isStartRoom = false; //��������� �� �������
        [SerializeField] private bool isPassageRoom = false; //���� ��� �������-�������
        [SerializeField] private List<Transform> pointsForSomething; //����� ��� ������ ��������� �����
        [Header("�������")] //�������
        [SerializeField] private RoomSpawner upPassage; //������� ������
        [SerializeField] private RoomSpawner downPassage; //������ ������
        [SerializeField] private RoomSpawner leftPassage; //����� ������
        [SerializeField] private RoomSpawner rightPassage; //������ ������
        [Header("��������� ������")] //������� ��� ����� ������
        [SerializeField] private Vector2 instantiatePositionUp = new Vector2(0f, 18f);
        [SerializeField] private Vector2 instantiatePositionDown = new Vector2(0f, -18f);
        [SerializeField] private Vector2 instantiatePositionLeft = new Vector2(-18f, 0f);
        [SerializeField] private Vector2 instantiatePositionRight = new Vector2(18f, 0f);

        //������ �� ������ ����
        private List<Room> spawnedRooms = new List<Room>(); // ������������ ����� �������
        private RoomSpawner startingSpawnPoint; // ����� ������ ���� ���������� ��� �������

        //������� � �������
        public List<Room> SpawnedRooms => spawnedRooms; //���-�� ����������� ����� ������
        public bool IsStartRoom => isStartRoom; //��������� �� �������
        public bool IsPassageRoom => isPassageRoom; //�������-�������
        public RoomSpawner StartingSpawnPoint => startingSpawnPoint; // ����� ������ ���� ���������� ��� �������
        public void AddSpawnedRoom(Room newRoom) => spawnedRooms.Add(newRoom); //�������� ������������ �������
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
        private void CheckSpawnedRoomsCount(int roomsCount, UnityAction action) { if (spawnedRooms.Count == roomsCount) action.Invoke(); }
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

        //���������� ������
        private void Start()
        { 
            if (randomizePassagesOnAwake) RandomizePassages();

            //����� ������
            if (upPassage != null)
            {
                upPassage.SetOwnRoom(this);
                upPassage.StartSpawnningRoom(ManagerList.GenerationManager.RoomsSpawnOffset + 0.05f);
                if(IsPassageRoom) upPassage.onSpawn.AddListener( () => { if (SpawnedRooms.Count == 0) startingSpawnPoint.DestroyRoom(); });
            }
            if (downPassage != null)
            {
                downPassage.SetOwnRoom(this);
                downPassage.StartSpawnningRoom(ManagerList.GenerationManager.RoomsSpawnOffset + 0.07f);
                if(IsPassageRoom) downPassage.onSpawn.AddListener(() => { if (SpawnedRooms.Count == 0) startingSpawnPoint.DestroyRoom(); });
            }
            if (leftPassage != null)
            {
                leftPassage.SetOwnRoom(this);
                leftPassage.StartSpawnningRoom(ManagerList.GenerationManager.RoomsSpawnOffset + 0.09f);
                if (IsPassageRoom) leftPassage.onSpawn.AddListener(() => { if (SpawnedRooms.Count == 0) startingSpawnPoint.DestroyRoom(); });
            }
            if (rightPassage != null)
            {
                rightPassage.SetOwnRoom(this);
                rightPassage.StartSpawnningRoom(ManagerList.GenerationManager.RoomsSpawnOffset + 0.11f);
                if (IsPassageRoom) rightPassage.onSpawn.AddListener(() => { if (SpawnedRooms.Count == 0) startingSpawnPoint.DestroyRoom(); });
            }
        }
        private void OnDestroy()
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
    }
}