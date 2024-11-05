
using Microsoft.EntityFrameworkCore;

public class AppConfig
{
    public static string DatabaseName = "Plaga_Cero";

    public static MySqlServerVersion MySqlServerVersion = new(new Version(8, 0, 32));

    public static string DbUserKey = "PLAGA_CERO_USER";

    public static string DbPasswordKey = "PLAGA_CERO_PASSWORD";

}