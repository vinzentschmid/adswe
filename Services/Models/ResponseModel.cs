namespace Services.Models;

public class ResponseModel
{
    public Dictionary<String, String> ErrorMessages { get; set; } = new();

    public Boolean HasError
    {
        get
        {
            if(ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return true;
            }

            return false;
        }
        
    }
}