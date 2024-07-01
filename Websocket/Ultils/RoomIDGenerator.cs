
namespace WebSocket.Ultils
{
    internal static class RoomIDGenerator
    {
        private static HashSet<int> generatedIDs = new HashSet<int>();
        private static Random random = new Random();
        public static int GenerateRoomID()
        {
            int newID;
            do
            {
                int randomPart = random.Next(100, 1000);
                int timePart = DateTime.Now.Millisecond % 1000;
                newID = (randomPart * 1000) + (timePart % 1000);

                if (newID < 100000)
                {
                    newID += 100000;
                }
                else if (newID > 999999)
                {
                    newID -= 100000;
                }
            } while (generatedIDs.Contains(newID));

            generatedIDs.Add(newID);
            return newID;
        }
    }
}
