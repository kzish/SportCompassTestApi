using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCompassRestApi.Models
{
    /// <summary>
    /// viewmodel for the post
    /// </summary>
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostBody { get; set; }
        public string UserEmail { get; set; }
    }
}
