﻿namespace BudgetManagement.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
    }
}
