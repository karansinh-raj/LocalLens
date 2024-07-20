﻿namespace LocalLens.WebApi.Contracts.Auth;

public class GoogleUserDetailsResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfileUrl { get; set; }
}
