using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
    public enum RoomSpawnerState
    {
        Open,
        Close,
        StaticOpen,
        StaticClose
    }
    [System.Serializable] public class RoomObject
    {
        public GameObject room;
        public int chance;
    }
    public enum Rooms
    {
        Default,
        NPC,
        Trader,
        Box,
        MustSpawn,
        Exit
    }
    
    public class RoomsLists : MonoBehaviour
    {
        [SerializeField] private List<RoomObject> simpleRooms = new List<RoomObject>();
        [SerializeField] private List<RoomObject> npcRooms = new List<RoomObject>();
        [SerializeField] private List<RoomObject> traderRooms = new List<RoomObject>();
        [SerializeField] private List<RoomObject> boxRooms = new List<RoomObject>();
        [SerializeField] private List<RoomObject> mustSpawnRooms = new List<RoomObject>();
        [SerializeField] private List<RoomObject> exitRooms = new List<RoomObject>();

        //�������

        //�� �����
        public List<RoomObject> GetRoomsList(Rooms roomType)
        {
            switch (roomType)
            {
                case Rooms.Default:
                    return simpleRooms;
                case Rooms.NPC:
                    return npcRooms;
                case Rooms.Trader:
                    return traderRooms;
                case Rooms.Box:
                    return boxRooms;
                case Rooms.MustSpawn:
                    return mustSpawnRooms;
                case Rooms.Exit:
                    return exitRooms;
            }
            return new List<RoomObject>();
        }
        public ref List<RoomObject> GetRefOnRoomsList(Rooms roomType)
        {
            switch (roomType)
            {
                case Rooms.Default:
                    return ref simpleRooms;
                case Rooms.NPC:
                    return ref npcRooms;
                case Rooms.Trader:
                    return ref traderRooms;
                case Rooms.Box:
                    return ref boxRooms;
                case Rooms.MustSpawn:
                    return ref mustSpawnRooms;
                case Rooms.Exit:
                    return ref exitRooms;
            }
            return ref simpleRooms; 
        }

        //�� ��������� �������
        public GameObject GetRandomRoomInChance(Rooms roomType, int chance, bool remove)
        {
            List<RoomObject> roomsInChance = new List<RoomObject>();
            ref List<RoomObject> roomsList = ref GetRefOnRoomsList(roomType);

            foreach (RoomObject roomToCheck in roomsList)
                if (roomToCheck.chance >= chance) roomsInChance.Add(roomToCheck);
            
            if(roomsInChance.Count != 0)
            {
                int roomIndex = Random.Range(0, roomsInChance.Count);
                if(remove) roomsList.Remove(roomsInChance[roomIndex]);

                return roomsInChance[roomIndex].room;
            }
            return null;
        }
    }
}
