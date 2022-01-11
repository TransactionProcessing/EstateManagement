namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Shared.General;
    using Syncfusion.HtmlConverter;
    using Syncfusion.Pdf;
    using Syncfusion.Pdf.Graphics;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IPDFGenerator" />
    public class PDFGenerator : IPDFGenerator
    {
        #region Fields

        /// <summary>
        /// The file system
        /// </summary>
        private readonly IFileSystem FileSystem;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PDFGenerator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public PDFGenerator(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the PDF.
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<String> CreatePDF(String htmlString,
                                            CancellationToken cancellationToken)
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings settings = new WebKitConverterSettings
            {
                                                  Margin = new PdfMargins
                                                           {
                                                               All = 50
                                                           }
                                              };

            //Set WebKit path
            settings.WebKitPath = ConfigurationReader.GetValue("AppSettings", "PDFGenerateBinariesPath");
            settings.TempPath = "/home/txnproc/pdftemp";

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            IDirectoryInfo path = this.FileSystem.Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            String basePath = $"{path}/Templates/Email/";

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert(htmlString, basePath);

            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);
            document.Close();

            String base64 = Convert.ToBase64String(stream.ToArray());

            return base64;
        }

        #endregion
    }
}