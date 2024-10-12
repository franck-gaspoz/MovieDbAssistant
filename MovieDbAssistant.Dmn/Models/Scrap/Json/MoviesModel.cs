﻿namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

#pragma warning disable CD1606 // The property must have a documentation header.

/// <summary>
/// The movies model.
/// </summary>
public sealed partial class MoviesModel
{
    /// <summary>
    /// movies models
    /// </summary>
    public List<MovieModel> Movies { get; set; } = [];

}
