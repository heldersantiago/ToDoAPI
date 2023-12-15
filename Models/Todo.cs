using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models;

public class TodoItem
{
    public int Id { get; set; }
    public required string Task { get; set; }
    public bool Complete { get; set; }
    public required  int Owner { get; set; }
}