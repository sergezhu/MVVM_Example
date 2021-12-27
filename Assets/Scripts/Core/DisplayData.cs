namespace Core
{
    public readonly struct DisplayData
    {
        public readonly long CurrentNumber;
        public readonly long MemoryNumber;
        public readonly OperationType OperationType;

        public DisplayData(long currentNumber, long memoryNumber, OperationType operationType)
        {
            CurrentNumber = currentNumber;
            MemoryNumber = memoryNumber;
            OperationType = operationType;
        }
    }
}