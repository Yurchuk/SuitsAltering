namespace SuitsAltering.BL.Results;

public class Error
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }

    public string FieldName { get; set; }

    public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

    public Error WithFieldName(string fieldName)
    {
        FieldName = fieldName;
        return this;
    }

    public Error WithParameters(params (string, string)[] parameters)
    {
        foreach (var (item1, item2) in parameters)
        {
            Parameters.Add(item1, item2);
        }

        return this;
    }

    public Error WithParameter(string key, string value)
    {
        Parameters.Add(key, value);
        return this;
    }

    public Error WithParameter(string key, Guid value)
    {
        Parameters.Add(key, value.ToString());
        return this;
    }

    public Error WithParameter(string key, int value)
    {
        Parameters.Add(key, value.ToString());
        return this;
    }

    public Error WithParameter(string key, long value)
    {
        Parameters.Add(key, value.ToString());
        return this;
    }
}