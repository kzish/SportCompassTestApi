using System;
using System.Collections.Generic;

namespace SportCompassRestApi.Models
{
    public partial class MPosts
    {
        public MPosts()
        {
            MComments = new HashSet<MComments>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string PostBody { get; set; }
        public string UserEmail { get; set; }

        public virtual ICollection<MComments> MComments { get; set; }
    }
}
