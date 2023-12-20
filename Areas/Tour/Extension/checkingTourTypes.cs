using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
    public static class checkingTourTypes
    {
        public static bool checkTourTypeName(ApplicationDbContext data, string name)
        {
            return data.tourTypes.Count(u => u.TourTypeName == name) > 0;
        }

        public static bool checkTourTypeNameWhenUpdate(ApplicationDbContext data, string name)
        {
            return data.tourTypes.Count(u => u.TourTypeName == name) > 1;
        }
    }
}
