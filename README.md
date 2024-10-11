# ![icon](./assets/multimedia-small.png) Movie Db Assistant

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

## Dependencies

- **APP**

    - `MovieDbSpiders`: [/MovieDbScraper/blob/master/README.md](https://github.com/franck-gaspoz/MovieDbSpiders/blob/master/README.md)
    - `NewtonSoft.Json`: [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
    - `OS`: Linux, Windows, OSX

- **CLI**

    - `CommandLine.NetCore`: [/CommandLine.NetCore/blob/main/README.md](https://github.com/franck-gaspoz/CommandLine.NetCore/blob/main/README.md)
    - `OS`: Linux, Windows, OSX

- **GUI**

    - framework `net8.0-windows10.0.22621.0`
    - `OS`: Windows 10.0.22621.0 and +
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

