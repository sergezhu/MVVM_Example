namespace Core.Storage
{
    public interface IStorage
    {
        public PlayerProgress Progress { get; }

        public void Save();
        public void Load();
    }
}