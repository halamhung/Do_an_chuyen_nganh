using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
    public static class checkingDiscounts
    {
        public static bool checkDiscountsName(ApplicationDbContext data, string name)
        {
            return data.discounts.Count(u => u.DiscountName == name) > 0;
        }

        public static bool checkDiscountsNameWhenUpdate(ApplicationDbContext data, string name)
        {
            return data.discounts.Count(u => u.DiscountName == name) > 1;
        }
    }
}
