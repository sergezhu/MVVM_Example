namespace Core
{
    public struct OperationsLockedData
    {
        public bool AddLocked;
        public bool SubtractLocked;
        public bool MultiplyLocked;
        public bool DivideLocked;

        public OperationsLockedData(bool addLocked, bool subtractLocked, bool multiplyLocked, bool divideLocked)
        {
            AddLocked = addLocked;
            SubtractLocked = subtractLocked;
            MultiplyLocked = multiplyLocked;
            DivideLocked = divideLocked;
        }
    }
}