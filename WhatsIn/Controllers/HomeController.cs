using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Web.Helpers;

using WhatsIn.Models;
using WhatsIn.AppServices;
using System.Configuration;

namespace WhatsIn.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        readonly IServicesAppService _services;
        readonly IServiceImagesAppService _serviceImages;
        readonly IMembershipAppService _membership;
        readonly IMessagesAppService _message;
        readonly IWhatsInDBContext _context;

        public HomeController()
        {
            _context = new WhatsInDBContext();
            _services = new ServicesAppService(_context);
            _serviceImages = new ServiceImagesAppService(_context);
            _membership = new MembershipAppService();
            _message = new MessagesAppService(_context);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ServiceModel model)
        {
            return RedirectToAction("", "Results", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Privacy Policy";

            return View();
        }

        public ActionResult TermsOfUse()
        {
            ViewBag.Message = "Terms Of Use.";

            return View();
        }

        public ActionResult Disclaimer()
        {
            ViewBag.Message = "Terms Of Use.";

            return View();
        }

        public ActionResult MyServices()
        {
            ViewBag.Message = "My Services.";
            var userId = _membership.GetUserId(User.Identity.Name);
            List<ServiceModel> model = _services.GetServices(userId);
            return View(model);
        }

        public ActionResult AddNewService(ServiceModel model)
        {
            var userId = _membership.GetUserId(User.Identity.Name);
            model.UserId = userId;
            _services.UpsertService(model);
            return RedirectToAction("MyServices");
        }

        [HttpGet]
        public JsonResult GetMyService(int ServiceId)
        {
            var model = _services.GetService(ServiceId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveService(int ServiceId)
        {
            _services.RemoveService(ServiceId);
            return RedirectToAction("MyServices");
        }
        public PartialViewResult MyServiceImages(int ServiceId)
        {
            TempData["ServiceId"] = ServiceId;
            //var model = _serviceImages.GetServiceImages(ServiceId);
            //ViewBag.Message = "My Service Images.";
            //return PartialView(model);
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult GetMyServiceImages()
        {
            int ServiceId = int.Parse(TempData["ServiceId"].ToString());
            var model = _serviceImages.GetServiceImages(ServiceId);
            ViewBag.Message = "My Service Images.";
            return PartialView(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadServiceImage()
        {
            string _imgname = string.Empty;
            string _comPath = string.Empty;

            TempData.Keep("ServiceId");

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    _comPath = string.Concat(Server.MapPath("/Uploads/ServiceImages/"), TempData["ServiceId"], "_", _imgname, _ext);
                    _imgname = string.Concat(TempData["ServiceId"], "_", _imgname, _ext);

                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    //if (img.Width > 200)
                    //    img.Resize(200, 200);
                    img.Save(_comPath);
                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewServiceImage(ServiceImageModel model)
        {
            _serviceImages.UpsertServiceImage(model);
            return RedirectToAction("MyServiceImages", new { ServiceId = model.ServiceId });
        }

        public ActionResult DeleteServiceImage(int ServiceImageId)
        {
            var model = _serviceImages.GetServiceImage(ServiceImageId);
            model.IsDeleted = true;
            _serviceImages.UpsertServiceImage(model);
            return RedirectToAction("MyServiceImages", new { ServiceId = model.ServiceId });
        }

        public JsonResult GetMyFirstServiceImage(int ServiceId)
        {
            try
            {
                var list = _serviceImages.GetServiceImages(ServiceId);
                var model = list.FirstOrDefault();
                if (model != null)
                    return Json(Convert.ToString(model.ImagePath), JsonRequestBehavior.AllowGet);
                else
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SendMessage(MessageModel model)
        {            
            try
            {
                WebMail.SmtpServer = ConfigurationManager.AppSettings["mailHost"];
                WebMail.From = ConfigurationManager.AppSettings["mailAccount"];
                WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["mailPort"]);
                WebMail.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSsl"]);
                WebMail.UserName = ConfigurationManager.AppSettings["mailAccount"];
                WebMail.Password = ConfigurationManager.AppSettings["mailPassword"];

                WebMail.Send(
                 to: ConfigurationManager.AppSettings["mailAccount"],
                 subject: string.Concat(model.Name, " - ", model.EmailAddress),
                 body: model.Content,
                 isBodyHtml: true
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("");
        }

        [ChildActionOnly]
        public ActionResult ServiceList(List<ServiceModel> Model)
        {
            return PartialView(Model);
        }
    }
}