using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.Models.Estate
{
    [ExcludeFromCodeCoverage]
    public class Operator
    {
        #region Properties
        
        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }
        public String Name{ get; set; }
        public Boolean RequireCustomMerchantNumber { get; set; }
        public Boolean RequireCustomTerminalNumber { get; set; }

        #endregion
    }
}
