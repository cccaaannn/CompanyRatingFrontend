using CompanyRatingFrontend.Data.Company;
using CompanyRatingFrontend.Managers;
using CompanyRatingFrontend.Services;

namespace CompanyRatingFrontend.ViewModels;

public class CompanyViewModel(CompanyService companyService)
{
    public ApiResult<CompanyDetailDto> CompanyResponse { get; private set; } = ApiResult<CompanyDetailDto>.Empty();
    public bool CommentErrorVisible { get; set; }
    public string NewCommentText { get; set; } = string.Empty;
    public bool CommentButtonDisabled => IsCommenting || string.IsNullOrWhiteSpace(NewCommentText);
    private bool IsCommenting { get; set; }

    public async Task LoadCompanyAsync(Guid companyId)
    {
        CompanyResponse = await QueryManager.Query(async () => await companyService.GetAsync(companyId));
    }

    public async Task SubmitCommentAsync(Guid companyId)
    {
        if (IsCommenting || string.IsNullOrWhiteSpace(NewCommentText)) return;

        IsCommenting = true;
        CommentErrorVisible = false;

        try
        {
            var request = new CommentAddRequest { Content = NewCommentText };
            await companyService.AddCommentAsync(companyId, request);
            NewCommentText = string.Empty;
            await LoadCompanyAsync(companyId);
        }
        catch (Exception)
        {
            CommentErrorVisible = true;
        }
        finally
        {
            IsCommenting = false;
        }
    }
}