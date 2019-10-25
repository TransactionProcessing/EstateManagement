namespace EstateManagement.Factories
{
    using DataTransferObjects.Responses;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.Factories.IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <returns></returns>
        public EstateResponse ConvertFrom(Estate estate)
        {
            if (estate == null)
            {
                return null;
            }

            EstateResponse estateResponse = new EstateResponse
                                            {
                                                EstateName = estate.Name,
                                                EstateId = estate.EstateId
                                            };

            return estateResponse;
        }
    }
}