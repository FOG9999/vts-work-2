using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class FILE_UPLOAD
    {
        public decimal ID_FILE { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> ID { get; set; }
        public string CTYPE { get; set; }
    }
}
