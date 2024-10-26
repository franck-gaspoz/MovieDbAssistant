﻿namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The scrap settings.
/// </summary>
public sealed class ScrapSettings
{
    /// <summary>
    /// Gets or sets the tool path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string ToolPath { get; set; }

    /// <summary>
    /// output model prefix
    /// </summary>
    public required string OutputModelPrefix { get; set; }

    /// <summary>
    /// output model postfix
    /// </summary>
    public required string OutputModelPostfix { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether skip if temp output file already exists.
    /// </summary>
    /// <value>A <see cref="bool"/></value>
    public bool SkipIfTempOutputFileAlreadyExists { get; set; }

    /// <summary>
    /// if true, keep query title when enter a new movie model for a query, thus replace the remote title    
    /// </summary>
    public bool KeepQueryTitle { get; set; }

    /// <summary>
    /// if provided in query, the year replaces the scraped data year
    /// </summary>
    public bool KeepQueryYear { get; set; }

    /// <summary>
    /// Gets or sets the default filters.
    /// </summary>
    /// <value>A <see cref="FiltersSettings"/></value>
    public required FiltersSettings DefaultFilters { get; set; }

}
