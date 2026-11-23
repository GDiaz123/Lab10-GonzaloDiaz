using System;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class AssignRoleDto
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
}