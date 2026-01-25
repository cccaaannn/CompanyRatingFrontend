using CompanyRatingFrontend.Data.Company;
using Microsoft.AspNetCore.Components;

namespace CompanyRatingFrontend.Pages.Company.Components;

public partial class CommentCard
{
    [Parameter] public required CompanyCommentDto Comment { get; set; }
    private string FormattedDate => Comment.CreatedAt.ToLocalTime().ToString("g");
    private string UserFullName => $"{Comment.UserName} {Comment.UserSurname}";
}