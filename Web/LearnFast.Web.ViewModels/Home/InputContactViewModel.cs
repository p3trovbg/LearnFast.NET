namespace LearnFast.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    using LearnFast.Common;

    public class InputContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxConctactMessage, MinimumLength = GlobalConstants.MinConctactMessage)]
        public string Message { get; set; }

        public bool IsSuccessfully { get; set; }
    }
}
