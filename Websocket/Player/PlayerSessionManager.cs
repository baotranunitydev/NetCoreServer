using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Player
{
    internal class PlayerSessionManager
    {
        private Dictionary<string, PlayerSession> dicPlayerSeesion = new Dictionary<string, PlayerSession>();
        private int maxPlayerSessionInSever;

        public PlayerSession? GetPlayerSessionById(string playerId)
        {
            dicPlayerSeesion.TryGetValue(playerId, out PlayerSession? playerSession);
            return playerSession;
        }

        public void SetMaxPlayerSessionInSever(int maxPlayerSessionInSever)
        {
            this.maxPlayerSessionInSever = maxPlayerSessionInSever;
        }

        public bool TryAddPlayerSessionToDic(PlayerSession playerSession)
        {
            var isCanAdd = false;
            if (dicPlayerSeesion.Count < maxPlayerSessionInSever)
            {
                isCanAdd = true;
                var playerSessionModel = playerSession.GetPlayerSessionModel();
                var isContain = dicPlayerSeesion.ContainsKey(playerSessionModel.id);
                if (!isContain)
                {
                    dicPlayerSeesion.Add(playerSessionModel.id, playerSession);
                }
                else
                {
                    dicPlayerSeesion[playerSessionModel.id].Close();
                    dicPlayerSeesion.Add(playerSessionModel.id, playerSession);
                }
            }
            return isCanAdd;
        }

        public void RemovePlayerSessionFormDics(string playerId)
        {
            dicPlayerSeesion.Remove(playerId);
        }
    }
}
