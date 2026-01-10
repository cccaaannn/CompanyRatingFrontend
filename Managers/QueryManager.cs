namespace CompanyRatingFrontend.Managers;

public static class QueryManager
{
    public static async Task<ApiResult<T>> Query<T>(Func<Task<T>> queryFunc)
    {
        var response = ApiResult<T>.Empty();
        try
        {
            response.IsLoading = true;
            response.Data = await queryFunc();
            response.IsLoading = false;
            response.IsError = false;
        }
        catch
        {
            response.IsError = true;
        }
        finally
        {
            response.IsLoading = false;
        }

        return response;
    }
}

public class ApiResult<T>
{
    public T? Data { get; set; }
    public bool IsLoading { get; set; }
    public bool IsError { get; set; }

    public static ApiResult<T> Empty() => new()
    {
        Data = default,
        IsLoading = false,
        IsError = false
    };
}