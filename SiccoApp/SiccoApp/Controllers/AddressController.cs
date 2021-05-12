using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resources;

namespace SiccoApp.Controllers
{
    public class AddressController : Controller //BaseController
    {
        
        public JsonResult GetStatesByCountryId2(string id)
        {

            List<SelectListItem> states = new List<SelectListItem>();

            switch (id)
            {
                case "1012":
                    states.Clear();
                    states.Add(new SelectListItem { Text = Resources.Resources.Select, Value = "" });

                    states.Add(new SelectListItem { Text = "Mendoza", Value = "1" });
                    states.Add(new SelectListItem { Text = "Cordoba", Value = "2" });
                    states.Add(new SelectListItem { Text = "Santa Fe", Value = "3" });
                    states.Add(new SelectListItem { Text = "Buenos Aires", Value = "66" });
                    states.Add(new SelectListItem { Text = "Catamarca", Value = "67" });
                    states.Add(new SelectListItem { Text = "Chaco", Value = "68" });
                    states.Add(new SelectListItem { Text = "Chubut", Value = "69" });
                    states.Add(new SelectListItem { Text = "Ciudad Autónoma de", Value = "70" });
                    states.Add(new SelectListItem { Text = "Buenos Aires", Value = "71" });
                    states.Add(new SelectListItem { Text = "Córdoba", Value = "72" });
                    states.Add(new SelectListItem { Text = "Corrientes", Value = "73" });
                    states.Add(new SelectListItem { Text = "Entre Ríos", Value = "74" });
                    states.Add(new SelectListItem { Text = "Formosa", Value = "75" });
                    states.Add(new SelectListItem { Text = "Jujuy", Value = "76" });
                    states.Add(new SelectListItem { Text = "La Pampa", Value = "77" });
                    states.Add(new SelectListItem { Text = "La Rioja", Value = "78" });
                    states.Add(new SelectListItem { Text = "Mendoza", Value = "79" });
                    states.Add(new SelectListItem { Text = "Misiones", Value = "80" });
                    states.Add(new SelectListItem { Text = "Neuquén", Value = "81" });
                    states.Add(new SelectListItem { Text = "Río Negro", Value = "82" });
                    states.Add(new SelectListItem { Text = "Salta", Value = "83" });
                    states.Add(new SelectListItem { Text = "San Juan", Value = "84" });
                    states.Add(new SelectListItem { Text = "San Luis", Value = "85" });
                    states.Add(new SelectListItem { Text = "Santa Cruz", Value = "86" });
                    states.Add(new SelectListItem { Text = "Santa Fe", Value = "87" });
                    states.Add(new SelectListItem { Text = "Santiago del Estero", Value = "88" });
                    states.Add(new SelectListItem { Text = "Tierra del Fuego", Value = "89" });
                    states.Add(new SelectListItem { Text = "Tucumán", Value = "90" });

                    break;

                case "1231":
                    states.Clear();
                    states.Add(new SelectListItem { Text = Resources.Resources.Select, Value = "" });

                    states.Add(new SelectListItem { Text = "Alabama", Value = "7" });
                    states.Add(new SelectListItem { Text = "Alaska", Value = "8" });
                    states.Add(new SelectListItem { Text = "American Samoa", Value = "9" });
                    states.Add(new SelectListItem { Text = "Arizona", Value = "10" });
                    states.Add(new SelectListItem { Text = "Arkansas", Value = "11" });
                    states.Add(new SelectListItem { Text = "California", Value = "12" });
                    states.Add(new SelectListItem { Text = "Colorado", Value = "13" });
                    states.Add(new SelectListItem { Text = "Connecticut", Value = "14" });
                    states.Add(new SelectListItem { Text = "Delaware", Value = "15" });
                    states.Add(new SelectListItem { Text = "District Of Columbia", Value = "16" });
                    states.Add(new SelectListItem { Text = "Federated States Of Micronesia", Value = "17" });
                    states.Add(new SelectListItem { Text = "Florida", Value = "18" });
                    states.Add(new SelectListItem { Text = "Georgia", Value = "19" });
                    states.Add(new SelectListItem { Text = "Guam", Value = "20" });
                    states.Add(new SelectListItem { Text = "Hawaii", Value = "21" });
                    states.Add(new SelectListItem { Text = "Idaho", Value = "22" });
                    states.Add(new SelectListItem { Text = "Illinois", Value = "23" });
                    states.Add(new SelectListItem { Text = "Indiana", Value = "24" });
                    states.Add(new SelectListItem { Text = "Iowa", Value = "25" });
                    states.Add(new SelectListItem { Text = "Kansas", Value = "26" });
                    states.Add(new SelectListItem { Text = "Kentucky", Value = "27" });
                    states.Add(new SelectListItem { Text = "Louisiana", Value = "28" });
                    states.Add(new SelectListItem { Text = "Maine", Value = "29" });
                    states.Add(new SelectListItem { Text = "Marshall Islands", Value = "30" });
                    states.Add(new SelectListItem { Text = "Maryland", Value = "31" });
                    states.Add(new SelectListItem { Text = "Massachusetts", Value = "32" });
                    states.Add(new SelectListItem { Text = "Michigan", Value = "33" });
                    states.Add(new SelectListItem { Text = "Minnesota", Value = "34" });
                    states.Add(new SelectListItem { Text = "Mississippi", Value = "35" });
                    states.Add(new SelectListItem { Text = "Missouri", Value = "36" });
                    states.Add(new SelectListItem { Text = "Montana", Value = "37" });
                    states.Add(new SelectListItem { Text = "Nebraska", Value = "38" });
                    states.Add(new SelectListItem { Text = "Nevada", Value = "39" });
                    states.Add(new SelectListItem { Text = "New Hampshire", Value = "40" });
                    states.Add(new SelectListItem { Text = "New Jersey", Value = "41" });
                    states.Add(new SelectListItem { Text = "New Mexico", Value = "42" });
                    states.Add(new SelectListItem { Text = "New York", Value = "43" });
                    states.Add(new SelectListItem { Text = "North Carolina", Value = "44" });
                    states.Add(new SelectListItem { Text = "North Dakota", Value = "45" });
                    states.Add(new SelectListItem { Text = "Northern Mariana Islands", Value = "46" });
                    states.Add(new SelectListItem { Text = "Ohio", Value = "47" });
                    states.Add(new SelectListItem { Text = "Oklahoma", Value = "48" });
                    states.Add(new SelectListItem { Text = "Oregon", Value = "49" });
                    states.Add(new SelectListItem { Text = "Palau", Value = "50" });
                    states.Add(new SelectListItem { Text = "Pennsylvania", Value = "51" });
                    states.Add(new SelectListItem { Text = "Puerto Rico", Value = "52" });
                    states.Add(new SelectListItem { Text = "Rhode Island", Value = "53" });
                    states.Add(new SelectListItem { Text = "South Carolina", Value = "54" });
                    states.Add(new SelectListItem { Text = "South Dakota", Value = "55" });
                    states.Add(new SelectListItem { Text = "Tennessee", Value = "56" });
                    states.Add(new SelectListItem { Text = "Texas", Value = "57" });
                    states.Add(new SelectListItem { Text = "Utah", Value = "58" });
                    states.Add(new SelectListItem { Text = "Vermont", Value = "59" });
                    states.Add(new SelectListItem { Text = "Virgin Islands", Value = "60" });
                    states.Add(new SelectListItem { Text = "Virginia", Value = "61" });
                    states.Add(new SelectListItem { Text = "Washington", Value = "62" });
                    states.Add(new SelectListItem { Text = "West Virginia", Value = "63" });
                    states.Add(new SelectListItem { Text = "Wisconsin", Value = "64" });
                    states.Add(new SelectListItem { Text = "Wyoming", Value = "65" });

                    break;
            }

            return Json(new SelectList(states, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        // GET: Address
        public ActionResult Index()
        {
            return View();
        }

        // GET: Address/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Address/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Address/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Address/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
