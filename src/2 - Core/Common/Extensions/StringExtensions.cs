namespace MinhaBiblioteca.Core.Common.Extensions;

public static class StringExtensions
{
    public static string GetDigits(this string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return phone;

        return new string(phone.Where(char.IsDigit).ToArray());
    }
}