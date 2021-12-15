namespace EstateManagement.BusinessLogic.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Converts to datetime.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Guid guid)
        {
            var bytes = guid.ToByteArray();

            Array.Resize(ref bytes, 8);

            return new DateTime(BitConverter.ToInt64(bytes));
        }

        /// <summary>
        /// Converts to guid.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static Guid ToGuid(this DateTime dt)
        {
            var bytes = BitConverter.GetBytes(dt.Ticks);

            Array.Resize(ref bytes, 16);

            return new Guid(bytes);
        }

        #endregion
    }
}