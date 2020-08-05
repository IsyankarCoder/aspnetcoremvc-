using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class RegisterViewModel{

    [Required]
    [EmailAddress] 
    [ValidEmailDomain(allowedDomain:"gmail.com",ErrorMessage="Email domain gmail olmalı bebişim...")]
    [Remote(action:"IsEmailInUse",controller:"Account")]
    public string Email {get;set;}
    
    [Required]
    [DataType(DataType.Password)]
    public string PassWord{get;set;}
    
    [DataType(DataType.Password)]
    [Display(Name="Confirm PassWord")]
    [Compare("PassWord",ErrorMessage="Password and Confirmation password doesnt match")]
    public string ConfirmPassWord{get;set;}


    public string City{get;set;}

}