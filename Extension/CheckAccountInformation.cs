﻿using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using HKQTravellingAuthenication.Models;
using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Extension
{
    public static class CheckAccountInformation
    {
        public static bool checkUsername(ApplicationDbContext data, string username)
        {
            return data.Users.Count(u => u.UserName == username) > 0;
        }
        public static bool checkEmail(ApplicationDbContext data, string email)
        {
            return data.Users.Count(u => u.Email == email) > 0;
        }
        public static bool IsEmailValid(string email)
        {
            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool checkSurname(string surname)
        {
            if (surname == null)
            {
                return false;
            }
            return true;
        }
        public static bool checkName(string name)
        {
            if (name == null)
            {
                return false;
            }
            return true;
        }
        public static bool existetPhoneNumber(ApplicationDbContext data, string phoneNumber)
        {
            return data.Users.Count(u => u.PhoneNumber == phoneNumber) > 0;
        }
        public static bool IsVietnamesePhoneNumber(string phoneNumber)
        {
            // Sử dụng biểu thức chính quy cho số điện thoại Việt Nam
            // Biểu thức này kiểm tra số điện thoại di động Việt Nam bắt đầu bằng 0, sau đó là 9 số.
            string pattern = @"^0\d{9}$";

            // Kiểm tra số điện thoại sử dụng biểu thức chính quy
            return Regex.IsMatch(phoneNumber, pattern);
        }
        /*public static bool existetNINumber(ApplicationDbContext data, string niNumber)
        {
            return data.Users.Count(u => u. == niNumber) > 0;
        }
        public static bool numberOfNINumber(string niNumber)
        {
            int count = niNumber.Length;

            if (count == 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
        public static bool ValidateBirthdate(DateTime birthdate)
        {
       
            // Kiểm tra xem ngày sinh có hợp lệ hay không
            DateTime now = DateTime.Now;
            TimeSpan age = now - birthdate;
            if (age.Days < 365 * 5 || age.Days > 365 * 90)
            {
                return false;
            }

            return true;
        }
    }
}
