using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCompassRestApi.Models
{
    /// <summary>
    /// viewmodel for the comments
    /// </summary>
    public class CommentModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string CommentBody { get; set; }
        public string UserEmail { get; set; }
    }
}
