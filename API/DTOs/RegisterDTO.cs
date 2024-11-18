using System;
using System.ComponentModel.DataAnnotations;

namespace API.Data;

public class RegisterDTO
{

[Required]
public required string UserName { get; set; }

[Required]
public required string Password { get; set; }
}
