using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;

using WhatsIn.Models;
using WhatsIn.AppServices;
using WhatsIn.Util;

namespace WhatsIn.Controllers
{
    [RequireHttps]
    public class ResultsController : Controller
    {
        readonly IServicesAppService _services;
        readonly IServiceImagesAppService _serviceImages;
        readonly IMembershipAppService _membership;
        readonly IServiceRatingsAppService _ratings;
        readonly IServiceReportsAppService _report;
        readonly IWhatsInDBContext _context;

        public ResultsController()
        {
            _context = new WhatsInDBContext();
            _services = new ServicesAppService(_context);
            _serviceImages = new ServiceImagesAppService(_context);
            _membership = new MembershipAppService();
            _ratings = new ServiceRatingsAppService(_context);
            _report = new ServiceReportsAppService(_context);
        }

        public ActionResult Index(string location, string type)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = _membership.GetUser(userId);

                if (user != null && user.IsServiceExists.HasValue)
                    TempData["IsServiceExists"] = user.IsServiceExists.Value;

                int BlockSize = 9;
                List<ServiceModel> list = new List<ServiceModel>();

                ViewBag.All = _services.CountServices(location, "");
                ViewBag.Message = string.Format("{0} Result(s) in {1}", list.Count(), location);
                ViewBag.FoodAndDiningCount = _services.CountServices(location, "Food And Dining");
                ViewBag.HotelAndAccomodationsCount = _services.CountServices(location, "Hotel and Accomodations");
                ViewBag.TouristSpots = _services.CountServices(location, "Tourist Spots");
                ViewBag.BeautyAndRelaxation = _services.CountServices(location, "Beauty and Relaxation");
                ViewBag.SportsAndRecreation = _services.CountServices(location, "Sports and Recreation");
                ViewBag.ShoppingAndSouvenirs = _services.CountServices(location, "Shopping and Souvenirs");


                if (type == "All" || type == null)
                {
                    list = _services.GetServices(location, "", 1, BlockSize);
                    type = "All";
                }
                else
                    list = _services.GetServices(location, type, 1, BlockSize);
                //TempData["Type"] = type;
                ViewBag.Type = type;
                ViewBag.Location = location;

                return View(list);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult GetResult(string location, string type)
        {
            try
            {
                int BlockSize = 9;
                List<ServiceModel> list = new List<ServiceModel>();

                ViewBag.All = _services.CountServices(location, "");
                ViewBag.Message = string.Format("{0} Result(s) in {1}", list.Count(), location);
                ViewBag.FoodAndDiningCount = _services.CountServices(location, "Food And Dining");
                ViewBag.HotelAndAccomodationsCount = _services.CountServices(location, "Hotel and Accomodations");
                ViewBag.TouristSpots = _services.CountServices(location, "Tourist Spots");
                ViewBag.BeautyAndRelaxation = _services.CountServices(location, "Beauty and Relaxation");
                ViewBag.SportsAndRecreation = _services.CountServices(location, "Sports and Recreation");
                ViewBag.ShoppingAndSouvenirs = _services.CountServices(location, "Shopping and Souvenirs");


                if (type == "All" || type == null)
                {
                    list = _services.GetServices(location, "", 1, BlockSize);
                    type = "All";
                }
                else
                    list = _services.GetServices(location, type, 1, BlockSize);
                //TempData["Type"] = type;
                ViewBag.Type = type;
                ViewBag.Location = location;
                JsonModel jsonModel = new JsonModel();
                jsonModel.HTMLString = RenderPartialViewToString("ServiceList", list);

                return Json(jsonModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult InfinateScroll(string location, string type, int BlockNumber)
        {
            try
            {      //////////////// THis line of code only for demo. Needs to be removed ///////////////
                System.Threading.Thread.Sleep(3000);
                ////////////////////////////////////////////////////////////////////////////////////////

                int BlockSize = 9;
                var books = _services.GetServices(location, type, BlockNumber, BlockSize);

                JsonModel jsonModel = new JsonModel();
                jsonModel.NoMoreData = books.Count < BlockSize;
                jsonModel.HTMLString = RenderPartialViewToString("ServiceList", books);

                return Json(jsonModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult InfinateScrollMobile(string location, string type, int BlockNumber)
        {
            try
            {  //////////////// THis line of code only for demo. Needs to be removed ///////////////
                System.Threading.Thread.Sleep(3000);
                ////////////////////////////////////////////////////////////////////////////////////////

                int BlockSize = 1;
                var books = _services.GetServices(location, type, BlockNumber, BlockSize);

                JsonModel jsonModel = new JsonModel();
                jsonModel.NoMoreData = books.Count < BlockSize;
                jsonModel.HTMLString = RenderPartialViewToString("ServiceList", books);

                return Json(jsonModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult Search(ServiceModel model)
        {
            return PartialView(model);
        }
        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ControllerContext.RouteData.GetRequiredString("action");

                ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
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
                return Json(Convert.ToString(model.ImagePath), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult DisplayServiceImages(int ServiceId)
        {
            try
            {         // int ServiceId = int.Parse(TempData["ServiceId"].ToString());
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

        [HttpGet]
        public JsonResult Search()
        {
            ServiceModel model = new ServiceModel();
            ViewBag.Message = "My Service Images.";
            return Json(model);
        }

        public ActionResult Details(int s)
        {
            try
            {
                var loggeduserId = User.Identity.GetUserId();
                var loggeduser = _membership.GetUser(loggeduserId);

                if (loggeduser != null && loggeduser.IsServiceExists.HasValue)
                    TempData["IsServiceExists"] = loggeduser.IsServiceExists.Value;

                ResultViewModel model = new ResultViewModel();
                var service = _services.GetService(s);
                var user = _membership.GetUser(service.UserId);
                var ratings = _ratings.GetServiceRatings(s);


                if (service != null && user != null)
                {
                    model.ServiceId = service.Id;
                    model.ServiceName = service.Name;
                    model.ServiceType = service.Type;
                    model.ServiceProvides = service.ServiceProvides;
                    model.ServiceAddress = service.Address;
                    model.ServiceLocation = service.Location;
                    model.ServicePhoneNumber = service.PhoneNumber;
                    model.ServiceSchedule = service.Schedule;
                    model.ServiceFacebookPage = string.IsNullOrEmpty(service.FacebookPage) ? "" : service.FacebookPage.Substring(3);
                    model.ServiceTwitterPage = string.IsNullOrEmpty(service.TwitterPage) ? "": service.TwitterPage.Substring(8);
                    model.ServiceInstagramPage = string.IsNullOrEmpty(service.InstagramPage) ? "" : service.InstagramPage.Substring(10);
                    model.ServiceWebsiteAddress = string.IsNullOrEmpty(service.WebsiteAddress) ? "" : service.WebsiteAddress;

                    model.UserId = user.Id;
                    model.UserName = user.Name;
                    model.UserLocation = user.Location;
                    model.UserContactNumber = user.ContactNumber;
                    model.UserNameIdentifier = user.NameIdentifier;

                    model.PositiveRatings = ratings.Where(r => r.Rating == true).Count();
                    model.NegativeRatings = ratings.Where(r => r.Rating == false).Count();
                    if (User.Identity.IsAuthenticated)
                    {
                        //var loginId = _membership.GetUserId(User.Identity.Name);
                        var loginId = User.Identity.GetUserId();
                        var hasRatings = ratings.Where(r => r.LoginId == loginId).Count();
                        if (hasRatings > 0)
                            model.RatingGiven = true;

                        model.LoginId = loginId;
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult RateService(RatingModel model)
        {
            try
            {
                _ratings.UpsertServiceRating(model);
                return RedirectToAction("Details", new { s = model.ServiceId });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public ActionResult ReportService(ReportModel model)
        {
            try
            {
                _report.UpsertServiceReport(model);
                return RedirectToAction("Details", new { s = model.ServiceId });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public PartialViewResult ShowNegativeComments(int serviceId)
        {
            try
            {
                List<RatingViewModel> model = new List<RatingViewModel>();
                var ratings = _ratings.GetServiceRatings(serviceId).Where(q => q.Rating == false).ToList();
                for (int i = 0; i < ratings.Count(); i++)
                {
                    var item = ratings[i];
                    RatingViewModel comment = new RatingViewModel();
                    comment.Id = item.Id;
                    comment.LoginName = _membership.GetUser(item.LoginId).Name;
                    comment.Comment = item.Comment;
                    model.Add(comment);
                }
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

        public PartialViewResult ShowPositiveComments(int serviceId)
        {
            try
            {
                List<RatingViewModel> model = new List<RatingViewModel>();
                var ratings = _ratings.GetServiceRatings(serviceId).Where(q => q.Rating == true).ToList();
                for (int i = 0; i < ratings.Count(); i++)
                {
                    var item = ratings[i];
                    RatingViewModel comment = new RatingViewModel();
                    comment.Id = item.Id;
                    comment.LoginName = _membership.GetUser(item.LoginId).Name;
                    comment.Comment = item.Comment;
                    model.Add(comment);
                }
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType(), ex);
                throw ex;
            }
        }

    }
}