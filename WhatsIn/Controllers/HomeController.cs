using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Web.Helpers;
using System.Configuration;
using Microsoft.AspNet.Identity;
using WhatsIn.Models;
using WhatsIn.AppServices;
using WhatsIn.Util;



namespace WhatsIn.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        readonly IServicesAppService _services;
        readonly IServiceImagesAppService _serviceImages;
        readonly IMembershipAppService _membership;
        readonly IMessagesAppService _message;
        readonly IItinerariesAppService _itineraries;
        readonly IWhatsInDBContext _context;
        

        public HomeController()
        {
            _context = new WhatsInDBContext();
            _services = new ServicesAppService(_context);
            _serviceImages = new ServiceImagesAppService(_context);
            _membership = new MembershipAppService();
            _message = new MessagesAppService(_context);
            _itineraries = new ItinerariesAppService(_context);
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _membership.GetUser(userId);

            if (user !=null && user.IsServiceExists.HasValue)
                TempData["IsServiceExists"] = user.IsServiceExists.Value;
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

        [Authorize]
        public ActionResult MyServices()
        {
            try
            {
                ViewBag.Message = "My Services.";
                //var userId = _membership.GetUserId(User.Identity.Name);
                var userId = User.Identity.GetUserId();
                var user = _membership.GetUser(userId);

                if (user != null && user.IsServiceExists.HasValue)
                    TempData["IsServiceExists"] = user.IsServiceExists.Value;

                List<ServiceModel> model = _services.GetServices(userId);
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [ChildActionOnly]
        public ActionResult ServiceList(List<ServiceModel> Model)
        {
            return PartialView(Model);
        }

        public ActionResult UpdateIsServiceExistsTag()
        {
            var userId = User.Identity.GetUserId();
            var user = _membership.GetUser(userId);
            if (!user.IsServiceExists.Value)
            {
                user.IsServiceExists = true;
                _membership.UpdateUser(user);
            }
            return RedirectToAction("MyServices");
        }

        public ActionResult AddNewService(ServiceModel model)
        {
            try
            {
                //var userId = _membership.GetUserId(User.Identity.Name);
                var userId = User.Identity.GetUserId();
                model.UserId = userId;
                _services.UpsertService(model);
                return RedirectToAction("MyServices");
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetMyService(int ServiceId)
        {
            try
            {
                var model = _services.GetService(ServiceId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult RemoveService(int ServiceId)
        {
            try
            {
                _services.RemoveService(ServiceId);
                return RedirectToAction("MyServices");
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
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
            try
            {
                int ServiceId = int.Parse(TempData["ServiceId"].ToString());
                var model = _serviceImages.GetServiceImages(ServiceId);
                ViewBag.Message = "My Service Images.";
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadServiceImage()
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult AddNewServiceImage(ServiceImageModel model)
        {
            try
            {
                _serviceImages.UpsertServiceImage(model);
                return RedirectToAction("MyServiceImages", new { ServiceId = model.ServiceId });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult DeleteServiceImage(int ServiceImageId)
        {
            try
            {
                var model = _serviceImages.GetServiceImage(ServiceImageId);
                model.IsDeleted = true;
                _serviceImages.UpsertServiceImage(model);
                return RedirectToAction("MyServiceImages", new { ServiceId = model.ServiceId });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
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
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SendMessage(MessageModel model)
        {
            try
            {
                WebMail.SmtpServer = ConfigurationManager.AppSettings["mailHost"];
                WebMail.From = ConfigurationManager.AppSettings["mailFrom"];
                WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["mailPort"]);
                //WebMail.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSsl"]);
                WebMail.UserName = ConfigurationManager.AppSettings["mailFrom"];
                WebMail.Password = ConfigurationManager.AppSettings["mailPassword"];

                WebMail.Send(
                 to: ConfigurationManager.AppSettings["mailTo"],
                 subject: string.Concat(model.Name, " - ", model.EmailAddress),
                 body: model.Content,
                 isBodyHtml: true
                );

                return RedirectToAction("");
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }

        }

        [Authorize]
        public ActionResult MyItineraries()
        {
            try
            {
                ViewBag.Message = "My Itineraries.";
                //var userId = _membership.GetUserId(User.Identity.Name);
                var userId = User.Identity.GetUserId();
                var user = _membership.GetUser(userId);

                List<ItineraryViewModel> model = _itineraries.GetItineraries(userId);
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [ChildActionOnly]
        public ActionResult ItineraryList(List<ItineraryViewModel> Model)
        {
            return PartialView(Model);
        }

        public PartialViewResult ItineraryService(int ServiceId)
        {
            var model = _services.GetService(ServiceId);
            //ViewBag.Message = "My Service Images.";
            return PartialView(model);
            //return PartialView();
        }

        public ActionResult CompleteItinerary(int itineraryId)
        {
            try
            {
                var model =  _itineraries.GetItinerary(itineraryId);
                model.IsCompleted = true;
                _itineraries.UpsertItinerary(model);
                return RedirectToAction("MyItineraries");
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult RemoveItinerary(int ItineraryId)
        {
            try
            {
                _itineraries.RemoveItinerary(ItineraryId);
                return RedirectToAction("MyItineraries");
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public PartialViewResult MyItineraryCount(string loginId)
        {
            int? iTcount = null;
            var list = _itineraries.GetItineraries(loginId);
            if (list.Count() > 0)
                iTcount = list.Count();
            //ViewBag.Message = "My Service Images.";
            return PartialView(iTcount);
            //return PartialView();
        }

        [Authorize]
        public PartialViewResult GetNameOfUser(string loginId)
        {            
            var user = _membership.GetUser(loginId);          
            return PartialView(user);
        }

    }
}