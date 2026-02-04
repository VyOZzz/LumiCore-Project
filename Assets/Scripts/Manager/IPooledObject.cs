namespace Manager
{
    public interface IPooledObject
    {
        /// <summary>
        /// Called when object is spawned from pool
        /// Use this to reset object state
        /// </summary>
        void OnObjectSpawn();
    }
}