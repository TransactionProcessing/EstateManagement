namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IPDFGenerator
    {
        #region Methods

        /// <summary>
        /// Creates the PDF.
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<String> CreatePDF(String htmlString,
                               CancellationToken cancellationToken);

        #endregion
    }
}