using System;
using System.Globalization;
using System.Text;
using AirQualityService.Helpers.@interface;

namespace AirQualityService.Helpers
{
    public class FullTextSearchHelper : IFullTextSearchHelper
    {
        public FullTextSearchHelper()
        {
        }

        public Guid searchFullText(string text)
        {
            throw new NotImplementedException();
        }

        private string convertToUnsign(string strInput)
        {
            string stFormD = strInput.Normalize(System.Text.NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < stFormD.Length; i++)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');

            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
    }
}
