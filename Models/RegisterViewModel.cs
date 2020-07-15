using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class RegisterViewModel{

    [Required]
    [EmailAddress]
    public string Email {get;set;}
    
    [Required]
    [DataType(DataType.Password)]
    public string PassWord{get;set;}
    
    [DataType(DataType.Password)]
    [Display(Name="Confirm PassWord")]
    [Compare("PassWord",ErrorMessage="Password and Confirmation password doesnt match")]
    public string ConfirmPassWord{get;set;}

}