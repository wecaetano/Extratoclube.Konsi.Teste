using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Domain.Helpers.v1;
public static class Validate
{
    public static bool Cpf(string cpf)
    {
        // Remove any non-digit characters
        cpf = Regex.Replace(cpf, @"\D", "");

        // CPF must have 11 digits
        if (cpf.Length != 11)
        {
            return false;
        }

        // Calculate the first verification digit
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }
        int firstDigit = 11 - (sum % 11);
        if (firstDigit > 9)
        {
            firstDigit = 0;
        }

        // Calculate the second verification digit
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }
        int secondDigit = 11 - (sum % 11);
        if (secondDigit > 9)
        {
            secondDigit = 0;
        }

        // Check if the verification digits match
        if (int.Parse(cpf[9].ToString()) != firstDigit || int.Parse(cpf[10].ToString()) != secondDigit)
        {
            return false;
        }

        return true;
    }
}
