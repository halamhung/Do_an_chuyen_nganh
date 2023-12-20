using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
    public static class checkingEndLocation
    {
        public static bool checkEndLocationName(ApplicationDbContext data, string name)
        {
            return data.endLocations.Count(u => u.EndLocationName == name) > 0;
        }

        public static bool checkEndLocationNameWhenUpdate(ApplicationDbContext data, string name)
        {
            return data.endLocations.Count(u => u.EndLocationName == name) > 1;
        }
    }
}
