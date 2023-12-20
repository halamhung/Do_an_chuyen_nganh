using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
    public static class checkingStartLocation
    {
        public static bool checkStartLocationName(ApplicationDbContext data, string name)
        {
            return data.startLocations.Count(u => u.StartLocationName == name) > 0;
        }

        public static bool checkStartLocationNameWhenUpdate(ApplicationDbContext data, string name)
        {
            return data.startLocations.Count(u => u.StartLocationName == name) > 1;
        }
    }
}
