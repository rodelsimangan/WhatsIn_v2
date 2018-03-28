﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using WhatsIn.Models;
using WhatsIn.AppServices;
using System.IO;
using System.Web.Helpers;

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
            int BlockSize = 9;
            List<ServiceModel> model = _services.GetServices(location, type, 1, BlockSize);
            ViewBag.Message = string.Format("{0} Result(s) in {1}", model.Count(), location);
            ViewBag.FoodAndDiningCount = _services.CountServices(location, "Food And Dining");
            ViewBag.HotelAndAccomodationsCount = _services.CountServices(location, "Hotel and Accomodations");
            ViewBag.TouristSpots = _services.CountServices(location, "Tourist Spots");
            ViewBag.BeautyAndRelaxation = _services.CountServices(location, "Beauty and Relaxation");
            ViewBag.SportsAndRecreation = _services.CountServices(location, "Sports and Recreation");
            ViewBag.ShoppingAndSouvenirs = _services.CountServices(location, "Shopping and Souvenirs");


            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ServiceList(List<ServiceModel> Model)
        {
            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult InfinateScroll(string location, string type, int BlockNumber)
        {
            //////////////// THis line of code only for demo. Needs to be removed ///////////////
            System.Threading.Thread.Sleep(3000);
            ////////////////////////////////////////////////////////////////////////////////////////

            int BlockSize = 9;
            var books = _services.GetServices(location, type, BlockNumber, BlockSize);

            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = books.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ServiceList", books);

            return Json(jsonModel);
        }

        protected string RenderPartialViewToString(string viewName, object model)
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
        
        public JsonResult GetMyFirstServiceImage(int ServiceId)
        {
            var list = _serviceImages.GetServiceImages(ServiceId);
            var model = list.FirstOrDefault();
            return Json(Convert.ToString(model.ImagePath), JsonRequestBehavior.AllowGet);
        }
      
        [HttpGet]
        public PartialViewResult DisplayServiceImages(int ServiceId)
        {
           // int ServiceId = int.Parse(TempData["ServiceId"].ToString());
            var model = _serviceImages.GetServiceImages(ServiceId);
            ViewBag.Message = "My Service Images.";
            return PartialView(model);
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
            ResultViewModel model = new ResultViewModel();
            var service = _services.GetService(s);
            var user = _membership.GetUser(service.UserId);
            var ratings = _ratings.GetServiceRatings(s);


            if (service!=null && user !=null)
            {
                model.ServiceId = service.Id;
                model.ServiceName = service.Name;
                model.ServiceType = service.Type;
                model.ServiceProvides = service.ServiceProvides;
                model.ServiceLocation = service.Location;
                model.ServicePhoneNumber = service.PhoneNumber;
                model.ServiceSchedule = service.Schedule;
                model.ServiceFacebookPage = service.FacebookPage;
                model.ServiceTwitterPage = service.TwitterPage;
                model.ServiceInstagramPage = service.InstagramPage;
                model.ServiceWebsiteAddress = service.WebsiteAddress;

                model.UserId = user.Id;
                model.UserName = user.Name;
                model.UserLocation = user.Location;
                model.UserContactNumber = user.ContactNumber;
                model.UserNameIdentifier = user.NameIdentifier;

                model.PositiveRatings = ratings.Where(r => r.Rating == true).Count();
                model.NegativeRatings = ratings.Where(r => r.Rating == false).Count();
                if (User.Identity.IsAuthenticated)
                {
                    var loginId = _membership.GetUserId(User.Identity.Name);
                    var hasRatings = ratings.Where(r => r.LoginId == loginId).Count();
                    if (hasRatings > 0)
                        model.RatingGiven = true;

                    model.LoginId = loginId;
                }
            }
            return View(model);
        }

        public ActionResult RateService(RatingModel model)
        {
            _ratings.UpsertServiceRating(model);
            return RedirectToAction("Details", new { s = model.ServiceId });
        }

        public ActionResult ReportService(ReportModel model)
        {
            _report.UpsertServiceReport(model);
            return RedirectToAction("Details", new { s = model.ServiceId });
        }

        public PartialViewResult ShowNegativeComments(int serviceId)
        {
            List<RatingViewModel> model = new List<RatingViewModel>();
            var ratings = _ratings.GetServiceRatings(serviceId).Where(q=> q.Rating == false).ToList();
            for(int i = 0; i< ratings.Count(); i++)
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

        public PartialViewResult ShowPositiveComments(int serviceId)
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

    }
}