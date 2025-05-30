﻿using CollegeApi.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<StudentProfile> Students { get; set; } = new();
}
