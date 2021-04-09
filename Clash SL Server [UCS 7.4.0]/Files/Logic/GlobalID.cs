namespace CSS.Files.Logic
{
    public static class GlobalID
    {
        public static int CreateGlobalID(int index, int count) => count + 1000000 * index;

        public static int GetClassID(int commandType)
        {
            var r1 = 1125899907;
            commandType = (int) ((r1 * (long) commandType) >> 32);
            return (commandType >> 18) + (commandType >> 31);
        }

        public static int GetInstanceID(int globalID)
        {
            var r1 = 1125899907;
            r1 = (int) ((r1 * (long) globalID) >> 32);
            return globalID - 1000000 * ((r1 >> 18) + (r1 >> 31));
        }
    }
}
