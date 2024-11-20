
using Microsoft.EntityFrameworkCore;

public class AppConfig
{
    public static string DatabaseName = "plagacero";

    public static MySqlServerVersion MySqlServerVersion = new(new Version(8, 0, 32));

    public static string DbUserKey = "erick";

    public static string DbPasswordKey = "12917erick";

}