using Microsoft.AspNetCore.Mvc;
using Thox.Models;

namespace Thox.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            var personCardModelList = new PersonCardModelList
            {
                personCardModelList = new List<PersonCardModel>
                {
                    new PersonCardModel
                    {
                        Title = "2 Personen",
                        Image = "images/PersonCardImage/2persons.png",
                        PersonCount = 2,
                        Price = 99,
                        SuccessPercentage = 37
                    },
                    new PersonCardModel
                    {
                        Title = "3 Personen",
                        Image = "images/PersonCardImage/3persons.png",
                        PersonCount = 3,
                        Price = 105,
                        SuccessPercentage = 55
                    },
                    new PersonCardModel
                    {
                        Title = "4 Personen",
                        Image = "images/PersonCardImage/4persons.png",
                        PersonCount = 4,
                        Price = 110,
                        SuccessPercentage = 64
                    },
                    new PersonCardModel
                    {
                        Title = "5 Personen",
                        Image = "images/PersonCardImage/5persons.png",
                        PersonCount = 5,
                        Price = 115,
                        SuccessPercentage = 73,
                    },
                    new PersonCardModel
                    {
                        Title = "6 personen",
                        Image = "images/PersonCardImage/6persons.png",
                        PersonCount = 6,
                        Price = 130,
                        SuccessPercentage = 80
                    }
                }
            };
            return View(personCardModelList);
        }


        public IActionResult DateSelection([FromQuery(Name = "PersonCount")] int personCount)
        {
            //check if personCount is valid (2-6) and is set
            if (personCount < 2 || personCount > 6)
            {
                return RedirectToAction("Index");
            }
            return View("DateSelection", personCount);
        }

        public IActionResult ReservationComplete()
        {
            return View();
        }

    }
}


