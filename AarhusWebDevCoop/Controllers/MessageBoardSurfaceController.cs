using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using Umbraco.Core.Models;


namespace AarhusWebDevCoop.Controllers
{
    public class MessageBoardSurfaceController : SurfaceController
    {
        // GET: MessageBoardSurface
        public ActionResult Index()
        {
            return PartialView("MessageForm", new MessageForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(MessageForm model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            IContent message = Services.ContentService.CreateContent(model.Name, CurrentPage.Id, "Message");

            message.SetValue("messageName", model.Name);
            message.SetValue("messageMessage", model.Message);

            Services.ContentService.SaveAndPublishWithStatus(message);
            TempData["success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}



