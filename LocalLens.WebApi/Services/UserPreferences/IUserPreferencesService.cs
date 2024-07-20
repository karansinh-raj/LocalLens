﻿using LocalLens.WebApi.Contracts.UserPreferences;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.UserPreferences;

public interface IUserPreferencesService
{
    Task<ResultT<string>> CreateUserPreferencesAsync(
        CreateUserPreferecesRequest request,
        Guid userId,
        CancellationToken ct);
}