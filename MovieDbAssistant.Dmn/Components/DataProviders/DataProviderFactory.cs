﻿namespace MovieDbAssistant.Dmn.Components.DataProviders;

/// <summary>
/// data provider factory.
/// </summary>
public sealed class DataProviderFactory
{
    /// <summary>
    /// Creates data provider.
    /// </summary>
    /// <param name="dataProvider">The data provider.</param>
    /// <returns>An <see cref="IDataProvider"/></returns>
    public static IDataProvider CreateDataProvider(Type dataProvider)
        => (IDataProvider)Activator.CreateInstance(dataProvider)!;
}