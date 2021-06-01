using RM_WithKafka.Models;
using RM_WithKafka.WebMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RM_WithKafka.Controllers
{
    public class ResourceInfoController : Controller
    {
        List<Models.Status> statusList = new List<Models.Status>();
        List<Models.Type> typeList = new List<Models.Type>();

        // GET: ResourceInfo/GetAllResourceDetails
        public ActionResult GetAllResourceDetails()
        {
            ModelState.Clear();
            List<ResourceWithValue> resourceList = ServiceRepository.GetAllResource();
            return View(resourceList);
        }

        // GET: ResourceInfo/AddResource
        public ActionResult AddResource()
        {
            IEnumerable<SelectListItem> statusCollection = ServiceRepository.GetStatusList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            });
            IEnumerable<SelectListItem> typeCollection = ServiceRepository.GetTypeList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            });
            ViewData["StatusList"] = statusCollection;
            ViewData["TypeList"] = typeCollection;
            return View();
        }

        // POST: ResourceInfo/AddEmployee    
        [HttpPost]
        public ActionResult AddResource(ResourceWithValue res, FormCollection fobj)
        {
            IEnumerable<SelectListItem> statusCollection = ServiceRepository.GetStatusList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            });
            IEnumerable<SelectListItem> typeCollection = ServiceRepository.GetTypeList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            });
            ViewData["StatusList"] = statusCollection;
            ViewData["TypeList"] = typeCollection;
            try
            {
                if (ModelState.IsValid)
                {
                    res.TypeId = Convert.ToInt32(fobj["DropDownTypeList"]);
                    res.StatusId = Convert.ToInt32(fobj["DropDownStatusList"]);
                    ServiceRepository.SaveResource(res);
                    ViewBag.Message = "Resource details added successfully";
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/EditEmpDetails/5    
        public ActionResult EditResourceDetails(long id)
        {
            ResourceWithValue selectedResourceForEdit = new ResourceWithValue();
            selectedResourceForEdit = ServiceRepository.GetAllResource().Find(x => x.Id == id);

            List<SelectListItem> statusList = ServiceRepository.GetStatusList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            }).ToList<SelectListItem>();
            statusList.Find(x => x.Value == selectedResourceForEdit.StatusId.ToString()).Selected = true;

            List<SelectListItem> typeList = ServiceRepository.GetTypeList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            }).ToList<SelectListItem>();
            typeList.Find(x => x.Value == selectedResourceForEdit.TypeId.ToString()).Selected = true;

            ViewData["StatusList"] = statusList;
            ViewData["TypeList"] = typeList;
            List<ResourceWithValue> resList = ServiceRepository.GetAllResource();
            return View(resList.Find(Res => Res.Id == selectedResourceForEdit.Id));
        }

        // POST: Employee/EditEmpDetails/5    
        [System.Web.Mvc.HttpPost]
        public ActionResult EditResourceDetails(long id, ResourceWithValue model, FormCollection fobj)
        {
            IEnumerable<SelectListItem> statusCollection = ServiceRepository.GetStatusList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            });
            IEnumerable<SelectListItem> typeCollection = ServiceRepository.GetTypeList().Select(i => new SelectListItem()
            {
                Text = i.Name.ToString(),
                Value = i.Id.ToString()
            });
            statusCollection.Where(x => x.Value == model.StatusId.ToString()).Select(x => x.Selected = true);
            typeCollection.Where(x => x.Value == model.TypeId.ToString()).Select(x => x.Selected = true);

            ViewData["StatusList"] = statusCollection;
            ViewData["TypeList"] = typeCollection;
            try
            {
                model.TypeId = Convert.ToInt32(fobj["DropDownTypeList"]);
                model.StatusId = Convert.ToInt32(fobj["DropDownStatusList"]);
                ServiceRepository.UpdateResource(model);
                return RedirectToAction("GetAllResourceDetails");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/DeleteEmp/5    
        public ActionResult DeleteResource(long id)
        {
            try
            {
                ServiceRepository.DeleteResource(id);
                ViewBag.AlertMsg = "Resource details deleted successfully";
                return RedirectToAction("GetAllResourceDetails");
            }
            catch
            {
                return View();
            }
        }
    }
}