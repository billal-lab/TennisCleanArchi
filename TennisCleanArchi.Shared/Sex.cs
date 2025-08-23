namespace TennisCleanArchi.Shared;

public class Sex
{
    public string Value { get; private set; }

    public static Sex Male => new("M");
    public static Sex Female => new("F");


    private Sex(string value)
    {
        Value = value;
    }

    public static Sex FromValue(string value)
    {
        return value switch
        {
            "M" => Male,
            "F" => Female,
            _ => throw new ArgumentException(value + " is not a valid value")
        };
    }

    public static bool IsValidValue(string value)
    {
        return value == Male.Value || value == Female.Value;
    }
}
