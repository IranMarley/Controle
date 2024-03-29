using System.ComponentModel.DataAnnotations;

namespace Imarley.Controle.UI.Web.ViewModels.Identity
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome da Role")]
        public string Name { get; set; }
    }
}