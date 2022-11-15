namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("responsecodes")]
    public class ResponseCodes
    {
        public Int32 ResponseCode { get; set; }

        public String Description { get; set; }
    }
}
