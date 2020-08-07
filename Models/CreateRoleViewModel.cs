using System.ComponentModel.DataAnnotations;

public class CreateRoleViewModel
{
    public CreateRoleViewModel()
    {

    }


    [Required]
    public string RoleName { get; set; }

}