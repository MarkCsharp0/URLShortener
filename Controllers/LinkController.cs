using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortener.Model;
using URLShortener.Util;

namespace URLShortener.Controllers
{
    [ApiController]
    //
    public class LinkController : Controller
    {

        LinksContext db;
        // Конструктор контроллера
        public LinkController(LinksContext context)
        {
            db = context;
            // Заполним БД данными для демонстрации
            
        }
        [HttpGet]
        [Route("[controller]")]
        public JsonResult Index()
        {
            return Json("Hello");
        }

        [HttpPost]

        [Route("/generate")]
        public async Task<JsonResult> GenerateShortLink([FromBody] OriginalLinkDTO originalLink)
        {
            var link = await db.Links?.FirstOrDefaultAsync(x => x.OriginalLink == originalLink.OriginalLink);

          
            if (link == null)
            {
                int maxID = 0;
                if (db.Links.Count() != 0)
                {
                    maxID = db.Links.Max(u => u.Id);
                }
               
                string shortLink = UtilLinks.GetShortLink(maxID + 1);
                var newLink = new Link();
                newLink.OriginalLink = originalLink.OriginalLink;
                newLink.ShortLink = shortLink;
                newLink.DateOfCreation = DateTime.Now;
                db.Links.Add(newLink);
                await db.SaveChangesAsync();
             
                return Json(new ShortLinkDTO { ShortLink = newLink.ShortLink });
            }

            bool isExpired = UtilLinks.GetDateDifference(DateTime.Now, link.DateOfCreation) > 10 ? true : false;

            if (isExpired)
            {
                link.DateOfCreation = DateTime.Now;  
                await db.SaveChangesAsync();
            }
            return Json(new ShortLinkDTO { ShortLink = link.ShortLink });

        }



        [HttpGet]
        [Route("/get/{shortLink}")]
        public async Task<JsonResult> GetOriginalLink(string shortLink)
        {
            //return Json(shortLink);
            if (shortLink.Length < 3)
            {
                return Json(new URLShortener.Model.Exception { Message = "Too short link" });
            }

            int ID = UtilLinks.Decode(shortLink.Substring(0, shortLink.Length - 3));
            var link = await db.Links.FirstOrDefaultAsync(x => x.Id == ID);
            if (link == null)
            {
                return Json(new URLShortener.Model.Exception { Message = "Original link not found" });
            }
            bool isExpired = UtilLinks.GetDateDifference(DateTime.Now, link.DateOfCreation) > 10 ? true : false;
            if (link != null && isExpired)
            {
                return Json(new URLShortener.Model.Exception { Message = "This is expired link - " + 
                    link.ShortLink + ". Update link lifetime using post request. Full link - " + link.OriginalLink });
            }
            return Json(new OriginalLinkDTO { OriginalLink = link.OriginalLink});
        
        }

        [HttpGet]
        [Route("/notFound")]
        public new JsonResult NotFound()
        {
            return Json(new URLShortener.Model.Exception { Message = "Original link not found" });
        }

        [HttpGet]
        [Route("/expiredLink")]
        public JsonResult ExpiredLink()
        {
            return Json(new URLShortener.Model.Exception { Message = "This is expired link" });
        }

        [HttpGet]
        [Route("/l/{shortLink}")]
        public async Task<RedirectResult> RedirectToOriginalLink(string shortLink)
        {
            if (shortLink.Length < 3)
            {
                return Redirect("/notFound");
            }
            int ID = UtilLinks.Decode(shortLink.Substring(0, shortLink.Length - 3));
            var link = await db.Links.FirstOrDefaultAsync(x => x.Id == ID);
            if (link == null)
            {

                return Redirect("/notFound");
            }
            bool isExpired = UtilLinks.GetDateDifference(DateTime.Now, link.DateOfCreation) > 10 ? true : false;
            if (link != null && isExpired)
            {
                return Redirect("/expiredLink");
            }
           
            return Redirect(link.OriginalLink);
        }


    }
}
