# ![icon](./assets/multimedia-small.png) Movie Db Assistant

[![GPLv3 license](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://github.com/GetPublii/Publii/blob/master/LICENSE)
![.net](https://img.shields.io/static/v1?label=&message=.NET%208&color=307639&style=plastic&logo=.net) 
![csharp](https://img.shields.io/static/v1?label=&message=C%20&sharp;&color=cdf998&style=plastic&logo=csharp&logoColor=dodgerblue) 
![json](https://img.shields.io/static/v1?label=&message=JSON&color=cdf998&style=plastic&logo=javascript&logoColor=darkgreen)
![html5](https://img.shields.io/static/v1?label=&message=HTML5&color=cdf998&style=plastic&logo=html5) ![css3](https://img.shields.io/static/v1?label=&message=CSS3&color=cdf998&style=plastic&logo=css3&logoColor=black)
![linux](https://img.shields.io/static/v1?label=&message=Linux&color=285fdd&style=plastic&logo=linux) ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF) ![osx](https://img.shields.io/static/v1?label=&message=OSX&color=285fdd&style=plastic&logo=apple&logoColor=AAFFAA)

🚧 under construction 🚧

___

Generates **Movie catalogs documents** (HTML, stand alone ZIP,...) using **Web Crawlers** and a **templating system**.
Can scrap data from [IMDb](http://www.imdb.com)

User & Developer manual: [manual.md](doc/manual.md)
___

## Usage

___

### ![icon](./assets/multimedia-small.png) Windows system tray

Available on `Windows 10.0.22621.0` and more. This application runs as a tray icon.

:arrow_right: build `MovieDbAssistant.app` and run 🗔 `MovieDbAssistant.exe`

:arrow_right: select an action from the tray menu

:point_right: consult the **manual** here : [manual.md](/doc/manual.md)

____

## Dependencies

- **App (GUI)**

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - framework `net8.0-windows10.0.22621.0` (`Microsoft.Windows.Desktop.App.WindowsForms`)
    - `OS`: Windows 10.0.22621.0 and +

- **Dmn**

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - `MovieDbSpiders`: [/MovieDbScraper/blob/master/README.md](https://github.com/franck-gaspoz/MovieDbSpiders/blob/master/README.md)
    - `NewtonSoft.Json`: [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
    - `OS`: Linux, Windows, 

- **Lib**

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - `OS`: Linux, Windows, OSX

- **Web Api (*coming soon*)**

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - `OS`: Linux, Windows, OSX

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

2024/../10 - 1.0
initial version

functionalities:

- build html movie catalogs documents from:
    - use [MovieDbSpiders](https://github.com/franck-gaspoz/MovieDbSpiders/blob/master/README.md) `json` output
    - direct scrap from queries given in files (several formats are supported)
    - multi format queries
    - direct scrap from a query given in last text clipboard entry
    - html catalog templates
    - html catalog resources : backgrounds, fonts, css, js template engine
- Windows setup for System Tray GUI

