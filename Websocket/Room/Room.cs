using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Player;
using WebSocket.Ultils;

namespace WebSocket.Room
{
    internal class Room
    {
        private RoomModel roomModel;
        public RoomModel GetRoomModel() => roomModel;

        public void SetRoomModel(RoomModel roomModel)
        {
            this.roomModel = roomModel;
        }

        public void CheckPlay()
        {
            var isCanPlay = IsCanPlay();
            if (isCanPlay)
            {
                roomModel.stateRoom = TypeStateRoom.Play;
                Console.WriteLine("Play");
            }
        }

        public void ChangeStatusPlayer(PlayerSessionModel playerSessionModel)
        {
            if (playerSessionModel.id == roomModel.owner.id)
            {
                roomModel.owner.statePlayer = playerSessionModel.statePlayer;
            }
            else
            {
                var playerModel = roomModel.lstPlayerOther.Find(ex => Equals(ex.id, playerSessionModel.id));
                playerModel.statePlayer = playerSessionModel.statePlayer;
            }
        }

        public bool TryChangePriceRoom(int priceRoom)
        {
            var isCanChangePriceOwner = false;
            var isCanChangePrice = false;
            if (roomModel.owner.point >= priceRoom)
            {
                roomModel.priceRoom = priceRoom;
                isCanChangePriceOwner = true;
            }
            var isEnoughAll = roomModel.lstPlayerOther.TrueForAll(ex => ex.point >= priceRoom);
            isCanChangePrice = isCanChangePriceOwner && isEnoughAll;
            return isCanChangePrice;
        }

        public bool IsCanPlay()
        {
            var isMaxPlayer = roomModel.amountMemember == roomModel.maxMember;
            var isOwnerReady = roomModel.owner.statePlayer == TypeStatePlayer.Ready;
            var isAllOthersReady = roomModel.lstPlayerOther.TrueForAll(player => player.statePlayer == TypeStatePlayer.Ready);
            var isCanPlay = isOwnerReady && isAllOthersReady && isMaxPlayer;
            return isCanPlay;
        }

        public TypeResponse TryAddMemberInRoom(PlayerSessionModel playerSessionModel)
        {
            var typeRoomResponse = TypeResponse.None;
            if (roomModel.amountMemember < roomModel.maxMember)
            {
                if (playerSessionModel.point >= roomModel.priceRoom)
                {
                    var isContain = roomModel.lstPlayerOther.Contains(playerSessionModel);
                    if (!isContain)
                    {
                        roomModel.lstPlayerOther.Add(playerSessionModel);
                    }
                    typeRoomResponse = TypeResponse.Success;
                    roomModel.amountMemember++;
                }
                else
                {
                    typeRoomResponse = TypeResponse.NotEnoughPointToJoin;
                }
            }
            else
            {
                typeRoomResponse = TypeResponse.RoomFull;
            }
            return typeRoomResponse;
        }

        public bool TryRemovePlayer(PlayerSessionModel playerSessionModel)
        {
            var canRemove = false;
            if (roomModel.amountMemember > 1)
            {
                canRemove = true;
                if (playerSessionModel.id == roomModel.owner.id)
                {
                    var countLst = roomModel.lstPlayerOther.Count();
                    if (countLst > 0)
                    {
                        var firstOther = roomModel.lstPlayerOther[0];
                        var playerModel = new PlayerSessionModel()
                        {
                            id = firstOther.id,
                            name = firstOther.name,
                            statePlayer = firstOther.statePlayer,
                            point = firstOther.point,
                        };
                        roomModel.owner = playerModel;
                        roomModel.lstPlayerOther.RemoveAt(0);
                    }
                }
                else
                {
                    roomModel.lstPlayerOther.Remove(playerSessionModel);
                }
                roomModel.amountMemember--;
            }
            else
            {
                Console.WriteLine($"Remove Room");
            }
            return canRemove;
        }

        public bool IsHavePassword()
        {
            var isHavePassword = roomModel.password >= 0;
            return isHavePassword;
        }

        public void DebugModelRoom(out string jsonRoomModel)
        {
            jsonRoomModel = JsonConvert.SerializeObject(roomModel);
        }
    }

    internal struct RoomModel
    {
        public TypeStateRoom stateRoom;
        public string name;
        public int id;
        public int priceRoom;
        public int maxMember;
        public int amountMemember;
        public int password;
        public PlayerSessionModel owner;
        public List<PlayerSessionModel> lstPlayerOther;
    }
}
