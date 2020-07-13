namespace Cubra.Heplers
{
    public interface IPoolable
    {
        /// <summary>
        /// Активация объекта из пула
        /// </summary>
        void ActivateObject();

        /// <summary>
        /// Деактивация объекта при возвращении в пул
        /// </summary>
        void DeactivateObject();
    }
}