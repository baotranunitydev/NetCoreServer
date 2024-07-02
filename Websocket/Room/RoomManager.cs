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




        public void OnCreateRoom(CreateRoomRequest createRoomRequest)
        {
            var room = new Room();
            var roomModel = new RoomModel()
            {
                name = createRoomRequest.roomName,
                id = RoomIDGenerator.GenerateRoomID(),
                priceRoom = createRoomRequest.priceRoom,
                owner = createRoomRequest.playerSessionModel,
                password = createRoomRequest.password,
                maxMember = 2,
                stateRoom = TypeStateRoom.Wait,
                amountMemember = 1,
                lstPlayerOther = new List<PlayerSessionModel>()
            };
            room.SetRoomModel(roomModel);
            AddRoomToDics(roomModel.id, room);
            room.DebugModelRoom(out string jsonRoomModel);
            Console.WriteLine($"Model: \n{jsonRoomModel}");
        }

        public void OnFindRoom(FindRoomRequest findRoomRequest)
        {
            var room = GetRoomById(findRoomRequest.roomId);
            if (room != null)
            {
                if (room.IsHavePassword())
                {
                    Console.WriteLine($"Room Id: {findRoomRequest.roomId} - Have password");
                    return;
                }
                OnJoinRoom(findRoomRequest.roomId, findRoomRequest.playerSessionModel, room);
                room.DebugModelRoom(out string jsonRoomModel);
                Console.WriteLine($"Model: \n{jsonRoomModel}");
            }
            else
            {
                Console.WriteLine($"Room not found {findRoomRequest.roomId}");
            }
        }

        private void OnJoinRoom(int roomId, PlayerSessionModel playerSessionModel, Room room)
        {
            var isSuccess = room.TryAddMemberInRoom(playerSessionModel);
            if (isSuccess)
            {
                Console.WriteLine($"Player Id Join room: {playerSessionModel.id} - Room Id: {roomId}");
                room.DebugModelRoom(out string jsonRoomModel);
                Console.WriteLine($"Model: \n{jsonRoomModel}");
            }
            else
            {
                switch (room.GetRoomModel().stateRoom)
                {
                    case TypeStateRoom.None:
                        break;
                    case TypeStateRoom.Wait:
                        break;
                    case TypeStateRoom.Full:
                        Console.WriteLine($"Room Id: {roomId} - Full");
                        break;
                    case TypeStateRoom.Play:
                        Console.WriteLine($"Room Id: {roomId} - IsPlay");
                        break;
                }
                room.DebugModelRoom(out string jsonRoomModel);
                Console.WriteLine($"Model: \n{jsonRoomModel}");
            }
        }

        public void OnJoinRoomWithPassword(JoinRoomPassowrdRequest joinRoomPassowrdRequest)
        {
            var room = GetRoomById(joinRoomPassowrdRequest.roomId);
            if (room != null)
            {
                if (room.GetRoomModel().password != joinRoomPassowrdRequest.password)
                {
                    Console.WriteLine($"Room Id: {joinRoomPassowrdRequest.roomId} - Password error");
                    return;
                }
                OnJoinRoom(joinRoomPassowrdRequest.roomId, joinRoomPassowrdRequest.playerSessionModel, room);
            }
            else
            {
                Console.WriteLine($"Room not found {joinRoomPassowrdRequest.roomId}");
            }
        }

        public void OnChangeStatusPlayer(ChangeStatusRequest changeStatusRequest)
        {
            var room = GetRoomById(changeStatusRequest.roomId);
            if (room != null)
            {
                room.ChangeStatusPlayer(changeStatusRequest.playerSessionModel);
                room.CheckPlay();
                room.DebugModelRoom(out string jsonRoomModel);
                Console.WriteLine($"Model: \n{jsonRoomModel}");
            }
            else
            {
                Console.WriteLine($"Room not found {changeStatusRequest.roomId}");
            }
        }



        public void OnChangePriceRoom(ChangePriceRoomRequest changePriceRoomRequest)
        {
            var room = GetRoomById(changePriceRoomRequest.roomId);
            if (room != null)
            {
                var canChange = room.TryChangePriceRoom(changePriceRoomRequest.priceRoom);
                if (canChange)
                {
                    Console.WriteLine($"Channge Done");
                }
                else
                {
                    Console.WriteLine($"Not enough point to change price");
                }
                room.DebugModelRoom(out string jsonRoomModel);
                Console.WriteLine($"Model: \n{jsonRoomModel}");
            }
            else
            {
                Console.WriteLine($"Room not found {changePriceRoomRequest.roomId}");
            }
        }


        public void OnPlayerQuitRoom(PlayerQuitRoomRequest playerQuitRoomRequest)
        {
            var room = GetRoomById(playerQuitRoomRequest.roomId);
            if (room != null)
            {
                var isRemovePlayer = room.TryRemovePlayer(playerQuitRoomRequest.playerSessionModel);
                if (isRemovePlayer)
                {
                    Console.WriteLine($"Channge Done");
                    room.DebugModelRoom(out string jsonRoomModel);
                    Console.WriteLine($"Model: \n{jsonRoomModel}");
                }
                else
                {
                    RemoveRoomFormDics(playerQuitRoomRequest.roomId);
                }
            }
            else
            {
                Console.WriteLine($"Room not found {playerQuitRoomRequest.roomId}");
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
