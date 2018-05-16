using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace hdcore.Utils
{
    public class TextHelper
    {
        public static string CREAT_SUCCESSFULL = "Thêm mới thành công";

        public static string EDIT_SUCCESSFULL = "Cập nhật thành công";

        public static string DELETE_SUCCESSFULL = "Xóa thành công";

        public static string ERROR_SYSTEM = "Lỗi hệ thống, vui lòng thử lại sau";

        private const string chars = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890qwertyuiopasdfghjklzxcvbnm@";

        public static string GenerateRandomText(int textLength)
        {
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, textLength)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        public static string IgnoreVietnameseCharacters(string keyword)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = keyword.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}