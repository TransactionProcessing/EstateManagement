﻿namespace EstateManagement.Models.File;

using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Transaction{
    public Guid TransactionId{ get; set; }
    public String AuthCode { get; set; }
    public Boolean IsAuthorised { get; set; }
    public Boolean IsCompleted { get; set; }
    public String ResponseCode { get; set; }
    public String ResponseMessage { get; set; }
    public String TransactionNumber { get; set; }
}