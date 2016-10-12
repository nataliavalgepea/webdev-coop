using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }


            MailMessage message = new MailMessage();
            message.To.Add("eaanava.student@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message + "\n my email is: " + model.Email;

            using (SmtpClient smtp = new SmtpClient())
            {
                try
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com"; smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("eaanava.student@gmail.com", "eaanaval");

                    //send mail 
                    smtp.Send(message);
                }
                catch (Exception e)
                {

                }
            }
            TempData["success"] = true;



            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Comment");

            comment.SetValue("commentName", model.Name);
            comment.SetValue("commentEmail", model.Email);
            comment.SetValue("commentSubject", model.Subject);
            comment.SetValue("commentMessage", model.Message);

            Services.ContentService.Save(comment);

            return RedirectToCurrentUmbracoPage();

        }

    }


}

