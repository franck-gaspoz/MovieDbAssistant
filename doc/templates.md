___

# Movie Db Assistant : Templates
version: 1.0.0
___

The template engine transforms movies data into a catalog of html web pages. A **template** is constitued of
a set of files (js,img,css;...) and by **pages** and **parts** of pages. A specific
language inside `sources files (html,css,js,...)` allows to integrate fragments of content and **values** from
the **movies data** and from the **properties** of the **template**, the **engine** and the **application settings**.

___

Previous level: [manual](manual.md)

Index

- [Template folder structure](#tfs)
- [Application resources folder](#arf)
- [Template specification](#tps)
- [Template configuration](#tc)
- [Template language](#tpl)
- [Template data & properties](#tdp)

___

<a name="tfs"></a>
## Template folder structure

```
# path /rsc/html/templates/

📁 templates
    📁 <templateId>
	📄 template.json
	📁 pages
	    📄 page-details.tpl.html
	    📄 page-list.tpl.html
	📁 parts
	    # any tpl file
	    📄 ...
	 # any specific folder and/or files
	📁 css
	📁 js
	📁 img	
	📁 ...
	📄 ...
```

this is the minimalistic folder structure of a template with id **`templateId`**,
defining the mandatory two pages *list* and *detail*, including any **`part`** and any files for doing this.

The elements names indicated here may change since they are inflected throught the template configuration.

### exemple

the default theme **`cine-static-1.0.0`** contains these files/folders:

```
📁 templates
    📁 cine-static-1.0.0
	📄 template.json
	📁 pages
	   📄 page-details.tpl.html
	   📄 page-list.tpl.html
	📁 parts
	   📁 backgrounds
	      📄 background-container.tpl.html
	      📄 background-idle.tpl.html
	      📄 background-vignette.tpl.html
	   📁 page
	      📄 head-links.tpl.html
	      📄 head-metas.tpl.html
	      📄 head-scripts.tpl.html
	      📄 html.tpl.html
	   📁 page-details
	      📁 dialogs
		  📄 movie-details-settings.tpl.html
	      📄 content-container.tpl.html
	      📄 navbar-top.tpl.html
	   📁 page-list
	      📁 dialogs
		  📄 movie-list-settings.tpl.html
	      📄 item-container.tpl.html
	      📄 navbar-top.tpl.html
	   📁 panels
	      📄 navbar-bottom.tpl.html
	      📄 navbar-top.tpl.html	   
	📁 css
	   📄 styles.css
```

<a name="tfs"></a>
## Application resources folder

Templates can rely on resources provided by the application.
These resources can be copied on template demand at build time.

Resources files are classified in 3 categories:

- `static files`: images,icons,sounds,js,css,html,...
- `themes`:
    themes are styles and templates dedicated to the UI. A theme is composed of:
    - `static files`: images,icons,sounds,js,css,html,...
	- `templates`
	- `buttons` styles
	- `icons` styles
	- `ui` styles
	- `ui fonts`
- `templates`: any kind of text file that can be processed as a template: `*.tpl.html`,`*.tpl.js`,`*.tpl.css`, ...

### static files

```
# path: rsc/html/assets/

📁 assets
    📁 fonts
    📁 icons
    📁 img
    📁 js
       📁 core
          # the template & app engine js files
          📄 template-1.0.0.js
          📄 ui-1.0.0.js
          📄 ui-layout-1.0.0.js
          📄 util-1.0.0.js
       📁 ext
          📄 jquery-3.7.1.min.js
    📁 movie-page-list-wallpapers
```

### themes files
this is the default UI theme 'style part' : (buttons,icons,ui,fonts,...)

```
# path: rsc/html/assets/themes

📁 themes
    📁 blue-neon-1.0.0
       📁 css
          📄 buttons-1.0.0.css
          📄 icons-1.0.0.css
          📄 ui-1.0.0.tpl.css
          📄 ui-fonts-1.0.0.css
       📁 fonts
           ...
```
this is the default UI theme 'structure part' : (dialogs,controls,buttons,...)
```
# path: rsc/html/assets/themes

📁 themes
    📁 core-1.0.0
       📁 tpl
          📁 ui
             📁 controls
                📁 buttons
                   📄 button.tpl.html
                   📄 dialog-button-closing.tpl.html
                   📄 frame-button-close.tpl.html
                📁 icons
                   📄 frame-icon.tpl.html
                   📄 frame-icon-clickable.tpl.html
                   📄 nav-icon.tpl.html
             📁 dialogs
                📄 message-box.tpl.html
```

<a name="tps"></a>
## Template specification

A template is specified in the file named `template.json` at the root of the template folder.

```json
{
  // global informations

  // template id
  "id": "cine-static",
  // template display name
  "name": "Cine Static",
  "description": "template for static html catalogs with a minimalistic front-end UI",
  "version": "1.0.0",
  "versionDate": "2024/10/03",
  "author": "Movie Db Assistant",

  // theme: refers to engine resources themes

  "theme": {
    "kernel": {
      "id": "core",
      "ver": "1.0.0"
    },
    "ui": {
      "id": "blue-neon",
      "ver": "1.0.0"
    },
    "fonts": {
      "id": "blue-neon",
      "ver": "1.0.0"
    },
    "icons": {
      "id": "blue-neon",
      "ver": "1.0.0"
    },
    "buttons": {
      "id": "blue-neon",
      "ver": "1.0.0"
    }
  },

  // spec of paths

"paths": {
    "pages": "pages",
    "parts": "parts",
    "handleExtensions": [
      ".tpl.html",
      ".tpl.css",
      ".tpl.js"
    ]
  },

  // template files
  
  // description of templates pages

  "pages": [
    {
      "id": "list",
      "layout": "List",
      "tpl": "page-list.tpl.html",
      "background": "cinema-wallpaper-16.jpg",
      "title": "My Movies",
      "filename": "index"
    },
    {
      "id": "detail",
      "layout": "Detail",
      "tpl": "page-details.tpl.html",
      "backgroundIdle": "../img/cinema-wallpaper-40.jpg",
      "title": "My Movies"
    }
  ],

  // template properties

  "props": {
    "listMoviePicNotAvailable": "./img/image-not-availble-300x175.jpg",
    "detailMoviePicNotAvailable": "../img/image-not-availble-300x175.jpg",
    "listMoviePicNotFound": "./img/image-404-630x630.jpg",
    "detailMoviePicNotFound": "../img/image-404-630x630.jpg",
    "repoLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
    "helpLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
    "authorLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
    "hSep": "<span class=\"hsep\"></span>"
  },

  // back-end functions mapped to variables that must be transformed

  "transforms": [
    {
      "target": "interests",
      "operation": "Transform_Array"
    },
    {
      "target": "stars",
      "operation": "Transform_Array"
    },
    {
      "target": "actors",
      "operation": "Transform_Array"
    },
    {
      "target": "ActorModel",
      "operation": "Transform_ActorSimple"
    }
  ],

  // files and folders that will be copied (and eventually tpl interpreted) from the resources folder

  resources": [
    "/fonts/ComicNeue-Light.ttf:/fonts",
    "/fonts/MomTypewritter.ttf:/fonts",
    "/fonts/Play Chickens.ttf:/fonts",
    "/fonts/Quesha.ttf:/fonts",
    "/fonts/Vonique 92_D.otf:/fonts",
    "/fonts/Open 24 Display St.ttf:/fonts",
    "/fonts/Renegade Master.ttf:/fonts",
    "/fonts/BrunoAce-Regular.ttf:/fonts",

    "/icons/back.png:/img",
    "/icons/download.png:/img",
    "/icons/favicon.ico:/img",
    "/icons/house.png:/img",
    "/icons/left-arrow.png:/img",
    "/icons/play.png:/img",
    "/icons/settings.png:/img",
    "/icons/close-window.png:/img",

    "/img/image-not-availble-300x175.jpg:/img",
    "/img/image-404-630x630.jpg:/img",

    "/movie-page-list-wallpapers/cinema-wallpaper-16.jpg:/img",
    "/movie-page-list-wallpapers/cinema-wallpaper-40.jpg:/img",

    "/js/ext/jquery-3.7.1.min.js:/js/ext",
    "/js/core/template-1.0.0.js:/js/core",
    "/js/core/ui-1.0.0.js:/js/core",
    "/js/core/ui-layout-1.0.0.js:/js/core",
    "/js/core/util-1.0.0.js:/js/core",

    "/themes/blue-neon-1.0.0:/theme"
  ],
  
  // files that will be simply copied from the template folder

  "files": [
    "/img",
    "/css",
    "/fonts",
    "/js"
  ]
}
```

<a name="tc"></a>
## Template configuration

Some sections of the application settings are related to the template engine.

```json
// -----------
//  Paths
// -----------

  "Paths": {
    "Assets": "assets",
    "Temp": "temp",
    "Output": "output",
    "OutputPages": "pages",
    "Input": "input",
    // Rsc & sub paths
    "Resources": "rsc",
    "RscHtml": "html",
    "RscHtmlTemplates": "templates",
    "RscHtmlAssets": "assets",
    "RscHtmlAssetsThemes": "themes",
    "RscHtmlAssetsTpl": "tpl"
  },

  /* ... */

// --------------
//  Html builder
// --------------

"Html": {
      "Extension": ".html",
      "TemplateFilename": "template.json",
      "DataFilename": "js/data/data.js",
      "TemplateId": "cine-static",
      "TemplateVersion": "1.0.0",
      "Assets": {
        "ListWallpaper": "/movie-page-list-wallpapers"
      }
    }
```

<a name="tpl"></a>
## Template language

```html
<html>

<!-- includes a part -->
```

```md
{{{part}}}
```

```html
<!-- includes a part with props values -->
```

```html
{{{part(prop1=propValue_1,..propn=propValue_n)}}}
```

```html
<!-- includes a part with props -->
```

```html
{{{part--}}}
	{{--propName_1--}}
	<!-- ...value prop name 1... -->
	...
	<!-- ...end of value prop name 1... -->
	{{--propName_n--}}
	<!-- ......value prop name n... -->
	...
	<!-- ...end of value prop name n... -->
{{{--part}}}
```

```html
<!-- includes the value of a variable -->
```

```html
{{variable}}
```

```html
<!-- setup the defaut value of a variable: provided in replacement of null in the current 
 part context, for any value include of the variable) -->
```

```html
{{variable:default=...=}}
```

```html
<!-- setup the value of a variable: replace value if already in the context. 
  Is applied in the current part scope before variables are expanded -->
```

```html
{{variable:set=...=}}
```

```html
<!-- replace by the text identified by textId in the user locale -->
```

```html
{{(textId)}}
```

```html
<!-- set visible if 'variable' is not null and not empty -->
```

```html
<div class="if-varname"></div>
```

```html
<!-- set visible if 'variable' is null or empty -->
```

```html
<div class="if_no-varname"></div>
```

```html
<!-- add class 'classname' if 'variable' is null or empty -->
```

```html
<div class="if_no-varname--classname"></div>
```

```html
</html>
```
<br>

**definitions:**

- a `part` is the source code of a template (including template language, html, ...)
- a `part` can includes `parts`
- a `varname` is the name of a property (allowing sub objects paths) in the objects `data` and `props`, and in `part included props` in a part context
- special class names `if-...` are parsed by the template engine and lead to changes on the elements: `visibility` and/or `css classes` depending of the value
- props values included in parts, with `{{{part(prop1=..)}}}` or `{{--propName_1--}}`, **can't** be handled with the special classes `if-`,`if_no-` beacause they don't belong to the global props scope
- escaped characters are handled in syntax `prop1=propValue_1,..propn=propValue_n` from syntax `{{{part(prop1=propValue_1,..propn=propValue_n)}}}`. 
  For example `\,` allowes to have `,` in values from the value list

### naming conventions

variable name are:
- camel case (first letter minus, words with first letter upper, words are concatenated)
- use `.` to build paths across object values

exemple: 

<a name="tdp"></a>
### template data & properties

Templates have the responsability by convention to integrates these properties in two javascript objects
(done by the template engine thorught the html layout builders)
- `data` : movie or movie list data
- `props` : a subset of `template settings`, `application settings` (what is really needed by the template), and `application variables`

#### template `data`

example of a movie detail page. Aggregates some properties from:
- the movie data
- the data of the query that provided the movie data (if any). if it a simple movie item or a list of movie items depending of the page layout (detail,list)

```js
const data = {
    "url": "https://www.imdb.com/title/tt0343818/",
    "metaData": {
        "query": {
            "spiders": [0],
            "metadata": {
                "source": "https://ok.ru/video/8211840567985",
                "download": null,
                "queryFile": "C:\\\\Users\\\\franc\\\\source\\\\repos\\\\MovieDbAssistant\\\\MovieDbAssistant.App\\\\bin\\\\Debug\\\\net8.0-windows10.0.22621.0\\\\input\\\\query2.txt",
                "queryFileLine": 17,
                "queryCacheFiles": ["C:\\\\Users\\\\franc\\\\source\\\\repos\\\\MovieDbAssistant\\\\MovieDbAssistant.App\\\\bin\\\\Debug\\\\net8.0-windows10.0.22621.0\\\\temp\\\\imdb-aSByb2JvdA.json"],
                "instanceId": {
                    "value": 2
                }
            },
            "hashKey": "aSByb2JvdA",
            "title": "i robot",
            "count": null,
            "year": null,
            "languages": null,
            "countries": null,
            "userRating": null,
            "titleTypes": null,
            "genres": null
        },
        "scraperTool": "movie-db-scrapper-windows-64bit-intel-1.1.1.exe",
        "scraperToolVersion": "1.1.1",
        "spiderId": "imdb",
        "searchScore": {
            "affinity": 0.4166666666666667,
            "dataCompletion": 1,
            "value": 1.4166666666666667
        }
    },
    "sources": {
        "download": null,
        "play": "https://ok.ru/video/8211840567985"
    },
    "id": "tt0343818",
    "title": "I, Robot",
    "originalTitle": "I, Robot",
    "queryTitle": "i robot",
    "summary": "En 2035, un policier méfiant à l'égard des robots enquête sur un crime susceptible d'avoir été commis par un robot, ce qui mène à une plus grande menace pour l'humanité.",
    "interests": ["Artificial Intelligence", "Cyber Thriller", "Cyberpunk", "Action", "Mystery", "Sci-Fi", "Thriller"],
    "rating": "7,1",
    "ratingCount": "10",
    "duration": "1h 55min",
    "releaseDate": "10 août 2004",
    "year": "2004",
    "vote": "585 k",
    "director": "Alex Proyas",
    "writers": ["Jeff Vintar", "Akiva Goldsman", "Isaac Asimov"],
    "stars": ["Will Smith", "Bridget Moynahan", "Bruce Greenwood"],
    "actors": [{
        "actor": "Will Smith",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMGY5MWVlOGUtOWQ1NC00OWJlLWJmNjQtYWJlYzQwOTYxYzY5XkEyXkFqcGdeQXVyNzU1NzE3NTg@._V1_QL75_UX500_CR0,47,500,281_.jpg"],
        "characters": null
    }, {
        "actor": "Bridget Moynahan",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BZGVlZmQxNTctOGRhMi00ZTZjLTliYmMtYTliOWMxZGRjMDIwXkEyXkFqcGdeQWFybm8@._V1_QL75_UX500_CR0,0,500,281_.jpg"],
        "characters": null
    }, {
        "actor": "Bruce Greenwood",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMDZiMGFlYjktNGM2OS00YzVjLWJmNTItMGYyMDBhN2YzMzNkXkEyXkFqcGdeQXVyNzU1NzE3NTg@._V1_QL75_UX500_CR0,47,500,281_.jpg"],
        "characters": null
    }, {
        "actor": "Alan Tudyk",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMTBlNWRmMjEtYzFhNS00MDczLWE4NDktMDc4N2ViZmE5MjNlXkEyXkFqcGdeQXVyNzU1NzE3NTg@._V1_QL75_UX500_CR0,47,500,281_.jpg"],
        "characters": null
    }, {
        "actor": "James Cromwell",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BNTczMzk1MjU1MV5BMl5BanBnXkFtZTcwNDk2MzAyMg@@._V1_QL75_UX140_CR0,9,140,140_.jpg"],
        "characters": null
    }, {
        "actor": "Adrian Ricard",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMTc0Nzc3ODE3OF5BMl5BanBnXkFtZTcwNDQxOTU3MQ@@._V1_QL75_UX140_CR0,13,140,140_.jpg"],
        "characters": null
    }, {
        "actor": "Chi McBride",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BODYwMDI5MDM1Nl5BMl5BanBnXkFtZTYwODUyNjc4._V1_QL75_UX140_CR0,3,140,140_.jpg"],
        "characters": null
    }, {
        "actor": "Jerry Wasserman",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMjE0MzEwNDUxNV5BMl5BanBnXkFtZTcwNTE1MDAzOQ@@._V1_QL75_UX140_CR0,9,140,140_.jpg"],
        "characters": null
    }, {
        "actor": "Fiona Hogan",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BNzU1NTEzMzMxM15BMl5BanBnXkFtZTgwMzY0MjAzMTE@._V1_QL75_UX140_CR0,12,140,140_.jpg"],
        "characters": ["Del Spooner"]
    }, {
        "actor": "Peter Shinkoda",
        "picUrl": null,
        "characters": ["Susan Calvin"]
    }, {
        "actor": "Terry Chen",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMTY2NDcyNTI1Ml5BMl5BanBnXkFtZTcwNTYyOTI4Mw@@._V1_QL75_UX140_CR0,1,140,140_.jpg"],
        "characters": ["Lawrence Robertson"]
    }, {
        "actor": "David Haysom",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BYzY4ZjZiZTAtNTk4ZC00YzlmLTk1NTgtOGNmMDU5MDk5NjFlXkEyXkFqcGc@._V1_QL75_UX140_CR0,0,140,140_.jpg"],
        "characters": ["Sonny"]
    }, {
        "actor": "Scott Heindl",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BM2MzZDc2ZTAtYTNjNC00ZDRhLTg2YzEtOGUwYjAxOTU4Y2M3XkEyXkFqcGc@._V1_CR1,1,1212,1818_QL75_UX140_CR0,12,140,140_.jpg"],
        "characters": ["Dr. Alfred Lanning"]
    }, {
        "actor": "Sharon Wilkins",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BYzIwZWU0MGMtOTBlZi00MmE2LWE5NDgtOWM2NTE5OTU2Yjk4XkEyXkFqcGc@._V1_QL75_UX140_CR0,6,140,140_.jpg"],
        "characters": ["Granny", "(as Adrian L. Ricard)"]
    }, {
        "actor": "Craig March",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMmI1ZWZjNTgtMmJhZS00Y2FjLWIzZWQtYjg2OGQwNjJjYzEwXkEyXkFqcGc@._V1_QL75_UX140_CR0,12,140,140_.jpg"],
        "characters": ["Lt. John Bergin"]
    }, {
        "actor": "Kyanna Cox",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BOTY1N2MwNmUtYWY3Ni00MDk5LWI2N2YtMTFkMzMyMWRlNGE5XkEyXkFqcGc@._V1_CR0,0,1199,1799_QL75_UX140_CR0,12,140,140_.jpg"],
        "characters": ["Baldez"]
    }, {
        "actor": "Darren Moore",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BMTMxODQ5MjgxMF5BMl5BanBnXkFtZTcwMzA3MDYyMQ@@._V1_QL75_UX140_CR0,2,140,140_.jpg"],
        "characters": ["V.I.K.I."]
    }, {
        "actor": "Aaron Douglas",
        "picUrl": ["https://m.media-amazon.com/images/M/MV5BZTNiMmFjYjQtZDRkOS00N2E0LTg1NzktYjY5NjdiMThiYzRkXkEyXkFqcGc@._V1_QL75_UX140_CR0,0,140,140_.jpg"],
        "characters": ["Chin"]
    }],
    "anecdotes": "The car used by Will Smith's character is a concept car called Audi RSQ, which was designed exclusively for the film and includes special features suggested by director Alex Proyas.When Spooner is fighting off the NS-5s while Calvin attempts to gain access to V.I.K.I.'s positronic brain, Spooner drops a large gun with a shoulder strap which then appears to spontaneously attach itself around a large guide wire. The gun-strap actually swings over and snags onto the guns clip. If you slow down the shot you can see it easier. Its also explains why the straps length is shortened by half.Detective Del Spooner: Human beings have dreams. Even dogs have dreams, but not you, you are just a machine. An imitation of life. Can a robot write a symphony? Can a robot turn a... canvas into a beautiful masterpiece?Sonny: Can *you*?Instead of opening credits, the beginning of the movie features Isaac Asimov's 3 Laws of Robotics:\\nLAW I. A robot may not injure a human being or, through inaction, allow a human being to come to harm.\\nLAW II. A robot must obey orders given it by human beings except where such orders would conflict with the First Law.\\nLAW III. A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.Post-converted to 3D for Blu-Ray release in 2012.Edited into 2004 MLB All-Star Game (2004)SuperstitionWritten and Performed by Stevie WonderCourtesy of Motown RecordsUnder license from Universal Music Enterprises",
    "minPicUrl": "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY281_CR11,0,190,281_.jpg",
    "minPicWidth": "190",
    "minPicAlt": "Alan Tudyk in I, Robot (2004)",
    "picsUrls": ["https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY281_CR11,0,190,281_.jpg", "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY422_CR16,0,285,422_.jpg", "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY562_CR21,0,380,562_.jpg"],
    "medPicUrl": "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY562_CR21,0,380,562_.jpg",
    "picFullUrl": "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY281_CR11",
    "picsSizes": ["50vw", " (min-width: 480px) 34vw", " (min-width: 600px) 26vw", " (min-width: 1024px) 16vw", " (min-width: 1280px) 16vw"],
    "key": "SSwgUm9ib3QdHQwMzQzODE4",
    "filename": "SSwgUm9ib3QdHQwMzQzODE4.html",
    "listIndex": 1
}
```

#### template `props`

Aggregates some properties from:

- the template settings
- the application settings (paths, description,...)
- some dynamic properties generated server side
- some dynamic properties generated client side by the template engine (eg: clock, date)
- some properties can be specific to a template theme
- the template engine and any of its modules can add and setup any properties in this set

principales properties are:

- `tpl`: tpl settings (from template.json)
- `page`: related to current page
- `output`: about tpl output
- `build`: report about the build
- `navigation`: informations about the current page related to the owning collection (if any one)
- `app`: about the application
- `basePath`: path of the current page relative to the template output root
- `vars`: runtime variables created on front or back side. it is the place where any routine can add and share variables 

```js
const props = {
    "tpl": {
        "name": "Cine Static",
        "version": "1.0.0",
        "versionDate": "2024/10/03",
        "id": "cine-static",
        "theme": {
            "kernel": {
                "id": "core",
                "ver": "1.0.0"
            },
            "ui": {
                "id": "blue-neon",
                "ver": "1.0.0"
            },
            "fonts": {
                "id": "blue-neon",
                "ver": "1.0.0"
            },
            "icons": {
                "id": "blue-neon",
                "ver": "1.0.0"
            },
            "buttons": {
                "id": "blue-neon",
                "ver": "1.0.0"
            }
        },
        "pages": [{
            "id": "list",
            "layout": "List",
            "tpl": "page-list.tpl.html",
            "background": "cinema-wallpaper-16.jpg",
            "backgroundIdle": null,
            "title": "My Movies",
            "subTitle": "Query2",
            "filename": "index"
        }, {
            "id": "detail",
            "layout": "Detail",
            "tpl": "page-details.tpl.html",
            "background": "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY281_CR11",
            "backgroundIdle": "../img/cinema-wallpaper-40.jpg",
            "title": "My Movies",
            "subTitle": "Query2",
            "filename": null
        }],
        "props": {
            "listMoviePicNotAvailable": "./img/image-not-availble-300x175.jpg",
            "detailMoviePicNotAvailable": "../img/image-not-availble-300x175.jpg",
            "listMoviePicNotFound": "./img/image-404-630x630.jpg",
            "detailMoviePicNotFound": "../img/image-404-630x630.jpg",
            "repoLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
            "helpLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
            "authorLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
            "hSep": "<span class=\"hsep\"></span>"
        },
        "files": ["/img", "/css", "/fonts", "/js"],
        "resources": ["/fonts/ComicNeue-Light.ttf:/fonts", "/fonts/MomTypewritter.ttf:/fonts", "/fonts/Play Chickens.ttf:/fonts", "/fonts/Quesha.ttf:/fonts", "/fonts/Vonique 92_D.otf:/fonts", "/fonts/Open 24 Display St.ttf:/fonts", "/fonts/Renegade Master.ttf:/fonts", "/fonts/BrunoAce-Regular.ttf:/fonts", "/icons/back.png:/img", "/icons/download.png:/img", "/icons/favicon.ico:/img", "/icons/house.png:/img", "/icons/left-arrow.png:/img", "/icons/play.png:/img", "/icons/settings.png:/img", "/icons/close-window.png:/img", "/img/image-not-availble-300x175.jpg:/img", "/img/image-404-630x630.jpg:/img", "/movie-page-list-wallpapers/cinema-wallpaper-16.jpg:/img", "/movie-page-list-wallpapers/cinema-wallpaper-40.jpg:/img", "/js/ext/jquery-3.7.1.min.js:/js/ext", "/js/core/template-1.0.0.js:/js/core", "/js/core/ui-1.0.0.js:/js/core", "/js/core/ui-layout-1.0.0.js:/js/core", "/js/core/util-1.0.0.js:/js/core", "/themes/blue-neon-1.0.0:/theme"],
        "path": null,
        "paths": {
            "pages": "pages",
            "parts": "parts",
            "handleExtensions": [".tpl.html", ".tpl.css", ".tpl.js"]
        },
        "transforms": [{
            "target": "interests",
            "operation": "Transform_Array"
        }, {
            "target": "stars",
            "operation": "Transform_Array"
        }, {
            "target": "actors",
            "operation": "Transform_Array"
        }, {
            "target": "ActorModel",
            "operation": "Transform_ActorSimple"
        }]
    },
    "page": {
        "id": "detail",
        "layout": "Detail",
        "tpl": "page-details.tpl.html",
        "background": "https://m.media-amazon.com/images/M/MV5BNDU2MjkzZWYtNGY5MS00MTNhLWIwZGMtYjY1OWM0YTAxMDVlXkEyXkFqcGc@._V1_QL75_UY281_CR11",
        "backgroundIdle": "../img/cinema-wallpaper-40.jpg",
        "title": "My Movies",
        "subTitle": "Query2",
        "filename": null
    },
    "output": {
        "ext": ".html",
        "pages": "pages"
    },
    "build": {
        "startedAt": "2024-10-27T22:13:18.0450789Z",
        "finishedAt": "2024-10-27T22:13:18.0781515Z",
        "duration": 33.0726,
        "layout": 1
    },
    "navigation": {
        "home": "../index.html",
        "index": 2,
        "next": "../pages\\TGEgcGxhbsOodGUgZGVzIHNpbmdlcwdHQwMDYzNDQy.html",
        "previous": "../pages\\QnJhemlsdHQwMDg4ODQ2.html",
        "total": 6
    },
    "app": {
        "name": "Movie Db Assistant",
        "id": "MovieDbAssistant",
        "version": "1.0.0.0",
        "versionDate": "2024/10/03",
        "lang": "en"
    },
    "basePath": "../",
    "vars": {
        "system": {
            "now": {
                "date": "Mon 28 Oct",
                "clock": "01 : 12"
            }
        }
    }
}
```
