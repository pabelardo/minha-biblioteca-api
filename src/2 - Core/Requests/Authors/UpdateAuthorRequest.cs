﻿namespace MinhaBiblioteca.Core.Requests.Authors;

public class UpdateAuthorRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
