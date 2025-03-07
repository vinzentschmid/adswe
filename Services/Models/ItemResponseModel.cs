namespace Services.Models;

public class ItemResponseModel<T> : ResponseModel where T : class
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public ItemResponseModel() 
    {
        Success = true; // Standardmäßig als erfolgreich setzen
    }
    public ItemResponseModel(T data)
    {
        Success = true;
        Data = data;
    }

    public ItemResponseModel(string errorMessage)
    {
        Success = false;
        Message = errorMessage;
    }

}