namespace MinhaBiblioteca.Domain.Utils;

public static class Helpers
{
    public static string UnificarStrings(this IEnumerable<string> fonte, string delimitador = ",", char separador = '\'') => 
        string.Join(delimitador, fonte.Select(item => $"{separador}{item}{separador}"));
}
