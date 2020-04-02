using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportCompassRestApi.Models;

/// <summary>
/// handles the blog end point
/// </summary>
namespace SportCompassRestApi.Controllers
{
    [Route("SportCompass/v1")]
    public class BlogController : Controller
    {
        private dbContext db = new dbContext();
        private string uploads_folder = "Uploads";
        private HostingEnvironment host;

        public BlogController(HostingEnvironment host)
        {
            this.host = host;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }

        /*posts*/

        [HttpPost("CreatePost")]
        public async Task<JsonResult> CreatePost(IFormFile image, PostModel postModel)
        {
            try
            {
                var post = new MPosts();
                post.PostBody = postModel.PostBody;
                post.UserEmail = postModel.UserEmail;
                post.Title = postModel.Title;

                if (image != null)
                {
                    var filename = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(image.FileName)}";
                    var filepath = $"{host.WebRootPath}/{uploads_folder}/{filename}";
                    using (var stream = System.IO.File.Create(filepath))
                    {
                        await image.CopyToAsync(stream);
                        stream.Dispose();
                    }
                    post.ImageUrl = filename;
                }
                db.MPosts.Add(post);
                db.SaveChanges();
                return Json(new
                {
                    res = "ok",
                    data = "Post saved"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }

        [HttpDelete("DeletePost")]
        public async Task<JsonResult> DeletePost(int post_id)
        {
            try
            {

                var post = db.MPosts.Find(post_id);
                if (post == null)
                {
                    return Json(new
                    {
                        res = "err",
                        msg = $"Post with id={post_id} does not exist"
                    });
                }
                db.MPosts.Remove(post);
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Post Removed"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }


        [HttpPatch("UpDatePost")]
        public async Task<JsonResult> UpDatePost(PostModel postModel)
        {
            try
            {
                var post = new MPosts();
                post.PostBody = postModel.PostBody;
                post.UserEmail = postModel.UserEmail;
                post.Id = postModel.Id;
                post.Title = postModel.Title;

                var existingPost = db.MPosts.Find(post.Id);
                if (existingPost == null)
                {
                    return Json(new
                    {
                        res = "err",
                        msg = $"Post with id={post.Id} does not exist"
                    });
                }
                //update the fields that can change
                existingPost.Title = post.Title;
                existingPost.PostBody = post.PostBody;
                db.Entry(existingPost).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Post updated"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }


        [HttpPut("UpDatePost")]
        public async Task<JsonResult> UpDatePost(PostModel postModel, IFormFile image)
        {
            try
            {
                var post = new MPosts();
                post.PostBody = postModel.PostBody;
                post.UserEmail = postModel.UserEmail;
                post.Title = postModel.Title;
                post.Id = postModel.Id;

                var existingPost = db.MPosts.Find(post.Id);
                if (existingPost == null)
                {
                    return Json(new
                    {
                        res = "err",
                        msg = $"Post with id={post.Id} does not exist"
                    });
                }
                //delete old picture
                if (!string.IsNullOrEmpty(existingPost.ImageUrl))
                {
                    System.IO.File.Delete(existingPost.ImageUrl);
                }
                if (image != null)
                {
                    var filename = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(image.FileName)}";
                    var filepath = $"{host.WebRootPath}/{uploads_folder}/{filename}";
                    using (var stream = System.IO.File.Create(filepath))
                    {
                        await image.CopyToAsync(stream);
                        stream.Dispose();
                    }
                    existingPost.ImageUrl = filename;
                }
                //update all the fields
                existingPost.Title = post.Title;
                existingPost.PostBody = post.PostBody;
                existingPost.UserEmail = post.UserEmail;
                db.Entry(existingPost).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Post updated"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }

        [HttpGet("GetPosts")]
        public JsonResult GetPosts(int post_id = 0)
        {
            var posts = db.MPosts.AsQueryable();
            var posts_ = new List<MPosts>();

            if (post_id == 0)
            {
                posts_ = posts.Where(i => i.Id >= 0).Include(i => i.MComments).ToList();
            }
            else
            {
                posts_ = posts.Where(i => i.Id == post_id).Include(i => i.MComments).ToList();
            }

            foreach (var post in posts_)
            {
                //place-holder image incase one does not exist
                if (string.IsNullOrEmpty(post.ImageUrl))
                {
                    post.ImageUrl = "https://www.dubaiautodrome.com/wp-content/uploads/2016/08/placeholder.png";
                }
                else
                {
                    post.ImageUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}/{uploads_folder}/{post.ImageUrl}";
                }
            }
            return Json(new
            {
                res = "ok",
                data = posts_
            },
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        }



        /*comments*/

        [HttpPost("CreateComment")]
        public async Task<JsonResult> CreateComment([FromBody]CommentModel commentModel)
        {
            try
            {
                var comment = new MComments();
                comment.Id = commentModel.Id;
                comment.PostId = commentModel.PostId;
                comment.CommentBody = commentModel.CommentBody;
                comment.UserEmail = commentModel.UserEmail;


                db.MComments.Add(comment);
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Comment saved"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }

        [HttpDelete("DeleteComment")]
        public async Task<JsonResult> DeleteComment(int comment_id)
        {
            try
            {

                var comment = db.MComments.Find(comment_id);
                if (comment == null)
                {
                    return Json(new
                    {
                        res = "err",
                        msg = $"Comment with id={comment_id} does not exist"
                    });
                }
                db.MComments.Remove(comment);
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Comment Removed"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }


        [HttpPatch("UpDateComment")]
        public async Task<JsonResult> UpDateComment([FromBody]CommentModel commentModel)
        {
            try
            {
                var comment = new MComments();
                comment.Id = commentModel.Id;
                comment.PostId = commentModel.PostId;
                comment.CommentBody = commentModel.CommentBody;
                comment.UserEmail = commentModel.UserEmail;

                var existingComment = db.MComments.Find(comment.Id);
                if (existingComment == null)
                {
                    return Json(new
                    {
                        res = "err",
                        msg = $"Comment with id={comment.Id} does not exist"
                    });
                }
                //update the fields that can change
                existingComment.CommentBody = comment.CommentBody;
                db.Entry(existingComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Comment updated"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }


        [HttpPut("UpDateComment")]
        public async Task<JsonResult> UpDateComment_([FromBody]CommentModel commentModel)
        {
            try
            {
                var comment = new MComments();
                comment.Id = commentModel.Id;
                comment.PostId = commentModel.PostId;
                comment.CommentBody = commentModel.CommentBody;
                comment.UserEmail = commentModel.UserEmail;

                var existingComment = db.MComments.Find(comment.Id);
                if (existingComment == null)
                {
                    return Json(new
                    {
                        res = "err",
                        msg = $"Comment with id={comment.Id} does not exist"
                    });
                }
                //update all the fields
                existingComment.UserEmail = comment.UserEmail;
                existingComment.CommentBody = comment.CommentBody;
                db.Entry(existingComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new
                {
                    res = "ok",
                    data = "Comment updated"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                });
            }

        }

        [HttpGet("GetComments")]
        public JsonResult GetComments(int post_id)
        {
            var comments = db.MComments.Where(i => i.PostId == post_id).ToList();

            return Json(new
            {
                res = "ok",
                data = Json(comments).Value
            }, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

    }
}
