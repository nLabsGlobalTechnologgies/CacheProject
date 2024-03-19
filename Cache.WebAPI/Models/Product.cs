﻿namespace Cache.WebAPI.Models;

public sealed class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}
