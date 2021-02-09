using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class bill_ys_benefitpro
    {
        /// <summary>
        ///procode
        /// </summary>		
        public string procode { get; set; }

        /// <summary>
        ///proname
        /// </summary>		
        public string proname { get; set; }

        /// <summary>
        ///calculatype
        /// </summary>		
        public string calculatype { get; set; }

        /// <summary>
        ///fillintype
        /// </summary>		
        public string fillintype { get; set; }

        /// <summary>
        ///sortcode
        /// </summary>		
        public string sortcode { get; set; }

        /// <summary>
        ///status
        /// </summary>		
        public string status { get; set; }

        /// <summary>
        ///adduser
        /// </summary>		
        public string adduser { get; set; }

        /// <summary>
        ///adddate
        /// </summary>		
        public DateTime? adddate { get; set; }

        /// <summary>
        ///modifyuser
        /// </summary>		
        public string modifyuser { get; set; }

        /// <summary>
        ///modifydate
        /// </summary>		
        public DateTime? modifydate { get; set; }

        /// <summary>
        ///annual
        /// </summary>		
        public string annual { get; set; }

        /// <summary>
        ///je
        /// </summary>		
        public decimal? je { get; set; }
    }
}
