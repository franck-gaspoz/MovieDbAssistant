using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

public interface IDataProvider
{
    MoviesModel Get(string source);
}
