using Lab.Chat.Models.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;
using System;
using System.Net.Mime;

namespace Lab.Chat.Controllers
{
    [Route("messages")]
    public class MessagesController : Controller
    {
        /// <summary>
        /// Get messages
        /// </summary>
        /// <remarks>
        /// Direct messages sent from one user to another user or group.
        /// </remarks>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListMessageResponse), StatusCodes.Status200OK)]
        public ActionResult List(){
            return Ok(new ListMessageResponse{
                Messages = new[]{
                    new MessageResponse{
                        Id = Ulid.NewUlid().ToString(),
                        Content = "Content Bla",
                        SentOn = DateTime.Now
                    }
                }
            });
        }
    }
}
