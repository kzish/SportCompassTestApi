using System;
using System.Collections.Generic;

namespace SportCompassRestApi.Models
{
    public partial class MComments
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string CommentBody { get; set; }
        public string UserEmail { get; set; }

        public virtual MPosts Post { get; set; }
    }
}
