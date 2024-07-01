using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Player;
using WebSocket.Ultils;

namespace WebSocket.Room
{
    internal class RoomManager
    {
        private Dictionary<int, Room> dicsRoom = new Dictionary<int, Room>();

        public Dictionary<int, Room> GetDicsRomFillterByPrice(int price)
        {
            var dicsRoomFilter = new Dictionary<int, Room>();
            foreach (var room in dicsRoom.Values)
            {
                if (room.GetRoomModel().priceRoom == price)
                {
                    dicsRoomFilter.Add(room.GetRoomModel().id, room);
                }
            }
            return dicsRoomFilter;
        }

        public void OnCreateRoom(string roomName, int password, int priceRoom, PlayerSessionModel playerSessionModel)
        {
            var room = new Room();
            var roomModel = new RoomModel()
            {
                name = roomName,
                id = RoomIDGenerator.GenerateRoomID(),
                priceRoom = priceRoom,
                owner = playerSessionModel,
                password = password,
                maxMember = 2,
                stateRoom = TypeStateRoom.Wait,
                lstPlayerOther = new List<PlayerSessionModel>()
            };
            room.SetRoomModel(roomModel);
            AddRoomToDics(roomModel.id, room);
        }

        public void OnFindRoom(int roomId, PlayerSessionModel playerSessionModel)
        {
            var room = GetRoomById(roomId);
            if (room != null)
            {
                if (room.IsHavePassword())
                {
                    Console.WriteLine($"Room Id: {roomId} - Have password");
                    return;
                }
                OnJoinRoom(roomId, playerSessionModel, room);
            }
            else
            {
                Console.WriteLine($"Room not found {roomId}");
            }
        }


        private void OnJoinRoom(int roomId, PlayerSessionModel playerSessionModel, Room room)
        {
            var isSuccess = room.TryAddMemberInRoom(playerSessionModel);
            if (isSuccess)
            {
                Console.WriteLine($"Player Id Join room: {playerSessionModel.id} - Room Id: {roomId}");
            }
            else
            {
                switch (room.GetRoomModel().stateRoom)
                {
                    case TypeStateRoom.None:
                        break;
                    case TypeStateRoom.Wait:
                        Console.WriteLine($"Room Id: {roomId} - Full");
                        break;
                    case TypeStateRoom.Play:
                        Console.WriteLine($"Room Id: {roomId} - IsPlay");
                        break;
                }
            }
        }

        public void OnJoinRoomWithPassword(int roomId, int password, PlayerSessionModel playerSessionModel)
        {
            var room = GetRoomById(roomId);
            if (room != null)
            {
                if (room.GetRoomModel().password != password)
                {
                    Console.WriteLine($"Room Id: {roomId} - Password error");
                    return;
                }
                OnJoinRoom(roomId, playerSessionModel, room);
            }
            else
            {
                Console.WriteLine($"Room not found {roomId}");
            }
        }

        public void OnChangeStatusPlayer(int roomId, PlayerSessionModel playerSessionModel)
        {
            var room = GetRoomById(roomId);
            if (room != null)
            {
                room.ChangeStatusPlayer(playerSessionModel);
                room.CheckPlay();
            }
            else
            {
                Console.WriteLine($"Room not found {roomId}");
            }
        }

        public void OnChangePriceRoom(int roomId, int priceRoom)
        {
            var room = GetRoomById(roomId);
            if (room != null)
            {
                var canChange = room.TryChangePriceRoom(priceRoom);
                if (canChange)
                {
                    Console.WriteLine($"Channge Done");
                }
                else
                {
                    Console.WriteLine($"Not enough point to change price");
                }
            }
            else
            {
                Console.WriteLine($"Room not found {roomId}");
            }
        }

        public void OnPlayerQuitRoom(int roomId, PlayerSessionModel playerSessionModel)
        {
            var room = GetRoomById(roomId);
            if (room != null)
            {
                var isRemovePlayer = room.TryRemovePlayer(playerSessionModel);
                if (isRemovePlayer)
                {
                    Console.WriteLine($"Channge Done");
                }
                else
                {
                    RemoveRoomFormDics(roomId);
                }
            }
            else
            {
                Console.WriteLine($"Room not found {roomId}");
            }
        }


        private Room? GetRoomById(int roomId)
        {
            dicsRoom.TryGetValue(roomId, out Room? room);
            return room;
        }

        private void AddRoomToDics(int roomId, Room room)
        {
            var isContain = dicsRoom.ContainsKey(roomId);
            if (!isContain)
            {
                dicsRoom.Add(roomId, room);
            }
        }

        private void RemoveRoomFormDics(int roomId)
        {
            dicsRoom.Remove(roomId);
        }
    }
}
