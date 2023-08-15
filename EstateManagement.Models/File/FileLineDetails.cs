namespace EstateManagement.Models.File;

using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class FileLineDetails{
    #region Properties

    public Int32 FileLineNumber { get; set; }
    public String FileLineData{ get; set; }
    public String Status{ get; set; }
    public Transaction Transaction{ get; set; }

    #endregion
}