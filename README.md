﻿# ![icon](./assets/multimedia-small.png) Movie Db Assistant

🚧 under construction 🚧

___

Generates **Movie documents** (HTML, stand alone ZIP,...) using **Web Crawlers** and a **templating system**.

Can scrap data from:
- [IMDb](http://www.imdb.com)

<br>

![.net](https://img.shields.io/static/v1?label=&message=.NET%208&color=307639&style=plastic&logo=.net) 
![csharp](https://img.shields.io/static/v1?label=&message=C%20&sharp;&color=cdf998&style=plastic&logo=csharp&logoColor=dodgerblue) 
![json](https://img.shields.io/static/v1?label=&message=JSON&color=cdf998&style=plastic&logo=javascript&logoColor=darkgreen)
![html5](https://img.shields.io/static/v1?label=&message=HTML5&color=cdf998&style=plastic&logo=html5) ![css3](https://img.shields.io/static/v1?label=&message=CSS3&color=cdf998&style=plastic&logo=css3&logoColor=black)
![linux](https://img.shields.io/static/v1?label=&message=Linux&color=285fdd&style=plastic&logo=linux) ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF) ![osx](https://img.shields.io/static/v1?label=&message=OSX&color=285fdd&style=plastic&logo=apple&logoColor=AAFFAA)

## dependencies

- `MovieDbScrapper`: [/MovieDbScraper/blob/master/README.md](https://github.com/franck-gaspoz/MovieDbScraper/blob/master/README.md)
- `NewtonSoft.Json`: [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

- **CLI**

    - `CommandLine.NetCore`: [/CommandLine.NetCore/blob/main/README.md](https://github.com/franck-gaspoz/CommandLine.NetCore/blob/main/README.md)

- **GUI**

    - `PInvoke.User32`: [https://github.com/dotnet/pinvoke](https://github.com/dotnet/pinvoke)
    - framework `net8.0-windows10.0.22621.0`

___

## Usage

___

### ![icon](./assets/multimedia-small.png) Windows system tray

Available on `Windows 10.0.22621.0` and more. This application runs as a tray icon.

:arrow_right: run `MovieDbAssistant.exe`

:arrow_right: select an action from the tray menu

___

### 🗔 Any plateform command line

Available on Windows, Linux, OSX

run `MovieDbAssistantCLI.exe` from a `shell`

___

## Credits

- <a target="_blank" href="https://www.flaticon.com/free-icons/movie-theater" title="movie theater icons">Movie theater icons created by Freepik - Flaticon</a>
- <a target="_blank" href="https://www.flaticon.com/free-icons/backward" title="backward icons">Backward icons created by Md Tanvirul Haque - Flaticon</a>
- <a target="_blank" href="https://www.flaticon.com/free-icons/house-agent" title="house-agent icons">House-agent icons created by Ihdizein - Flaticon</a>
- <a target="_blank" href="https://icons8.com/icon/11511/reply-arrow">Back</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/60449/play-button-circled">Play</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/23353/downloading-updates">Download</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>

___

## Releases History

/10/2024 - 1.0
initial version

functionalities:

- build html movie documents from:
    - scrapper [MovieDbScraper](https://github.com/franck-gaspoz/MovieDbScraper/blob/master/README.md) `json` output
    - direct scrap from queries given in files (several formats are supported)
    - direct scrap from a query given in last text clipboard entry


