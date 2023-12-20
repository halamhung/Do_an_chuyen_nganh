using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
    public static class CheckingTours
    {
        public static bool checkTourName(ApplicationDbContext data, string name)
        {
            return data.tours.Count(u => u.TourName == name) > 0;
        }

        //Khi chính sửa thì nếu thông tin trùng với thông tin cũ thì không sao nhưng trùng hơn 1 thì sai
        public static bool checkTourNameWhenUpdate(ApplicationDbContext data, string name)
        {
            return data.tours.Count(u => u.TourName == name) > 1;
        }
    }
}
