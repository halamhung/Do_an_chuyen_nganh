using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HKQTravellingAuthenication.Models.Tour;

namespace HKQTravellingAuthenication.Areas.Tour.Models
{
    public class TourExtraViewModel
    {
        public Tours? Tours { get; set; }
        public List<TourImages>? TourImagesList { get; set; }
    }
}
