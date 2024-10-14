___

# Movie Db Assistant : Templates
version: 1.0.0
___

The template engine transforms movies data into html web pages. A **template** is constitued of
a set of files (js,img,css;...) and by **pages** and **parts** of pages. A specific
language inside `html` allows to integrate fragments of content and some **values** from
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

the default theme **`dark`** contains these files/folders:

```
📁 templates
    📁 dark
	📄 template.json
	📁 pages
	   📄 page-details.tpl.html
	   📄 page-list.tpl.html
	📁 parts
	   📄 head.tpl.html
	   📄 head-links.tpl.html
	   📄 head-metas.tpl.html
	   📄 head-scripts.tpl.html
	   📄 html.tpl.html
	📁 css
	   📄 styles.css
```

<a name="tfs"></a>
## Application resources folder

Templates can rely on resources provided by the application.
These resources can be copied on template demand at build time.

```
# path rsc/html/assets/

📁 assets
    📁 fonts
	📁 icons
	📁 img
	📁 js
	   📁 core
          # the mandatory template engine js
	      📄 template-1.0.0.js
	   📁 ext
	📁 movie-page-list-wallpapers
```

<a name="tps"></a>
## Template specification

A template is specified in the file named `template.json` at the root of the template folder.

```json
{
  // template id
  "Id": "dark",
  // template display name
  "Name": "Dark",
  "Description": "minimalist with movies pictures and dark panes"
  "Version": "1.0.0",
  "VersionDate": "2024/10/03",
  "Author": "F. G.",

  // template files

  "Templates": {
	// list page template
    "List": "page-list.tpl.html",
	// details page template
    "Details": "page-details.tpl.html"
  },

  // template options

  "Options": {
    "Paths": {
	  // folder of pages templates
      "Pages": "pages",
	  // folder of parts templates
      "Parts": "parts"
    },
	// page list options
    "PageList": {
      "Background": "cinema-wallpaper-16.jpg",
      "Title": "My Movies",
      "PageTitle": "My Movies",
      "Filename": "index"
    },
	// page detail options
    "PageDetail": {
      "Background": "../img/cinema-wallpaper-16.jpg",
      "BackgroundIdle": "../img/cinema-wallpaper-40.jpg",
      "PageTitle": "My Movies"
    },
	// common options
    "ListMoviePicNotAvailable": "./img/image-not-availble-300x175.jpg",
    "DetailMoviePicNotAvailable": "../img/image-not-availble-300x175.jpg",
    "ListMoviePicNotFound": "./img/image-404-630x630.jpg",
    "DetailMoviePicNotFound": "../img/image-404-630x630.jpg",
    "RepoLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
    "HelpLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
    "AuthorLink": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md"
  },
  // template to produce an horizontal separator for string arrays values
  "HSep": "<span class=\"hsep\"></span>",

  // back-end functions mapped to variables that must be transformed

  "Transforms": [
    {
      "Target": "Interests",
      "Operation": "Transform_Array"
    },
    {
      "Target": "Stars",
      "Operation": "Transform_Array"
    },
    {
      "Target": "Actors",
      "Operation": "Transform_Array"
    },
    {
      "Target": "ActorModel",
      "Operation": "Transform_ActorSimple"
    }
  ],

  // copy files from the resources folder

  "Resources": [
    "/fonts/ComicNeue-Light.ttf:/fonts",
    "/fonts/MomTypewritter.ttf:/fonts",
    "/fonts/Play Chickens.ttf:/fonts",
    "/fonts/Quesha.ttf:/fonts",
    "/fonts/Vonique 92_D.otf:/fonts",
    "/fonts/Open 24 Display St.ttf:/fonts",
    "/fonts/Renegade Master.ttf:/fonts",
    "/icons/back.png:/img",
    "/icons/download.png:/img",
    "/icons/favicon.ico:/img",
    "/icons/house.png:/img",
    "/icons/left-arrow.png:/img",
    "/icons/play.png:/img",
    "/movie-page-list-wallpapers/cinema-wallpaper-16.jpg:/img",
    "/movie-page-list-wallpapers/cinema-wallpaper-40.jpg:/img",
    "/js/ext/jquery-3.7.1.min.js:/js/ext",
    "/js/core/template-1.0.0.js:/js/core",
    "/img/image-not-availble-300x175.jpg:/img",
    "/img/image-404-630x630.jpg:/img"
  ],
  
  // copy folders & files from the template folder

  "Files": [
    "/img",
    "/css",
    "/fonts",
    "/js"
  ]
}
```

<a name="tc"></a>
## Template configuration

Some sections of the application settings concern the template engine.

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
	"RscHtmlAssetsFonts": "fonts",
	"RscHtmlAssetsMoviePageListWallpapers": "movie-page-list-wallpapers"
},

  /* ... */

// --------------
//  Html builder
// --------------

"Html": {
    "Extension": ".html",
    "TemplateFilename": "template.json",
    "DataFilename": "js/data/data.js",
    "TemplateId": "dark",
}
```

<a name="tpl"></a>
## Template language

```html
<html>

<!-- includes a part -->
{{{part}}}

<!-- includes the value of a variable -->
{{variable}}

<!-- replace by the text identified by textId in the user locale -->
{{(textId)}}

<!-- set visible if 'variable' is not null and not empty -->
<div class="if-varname"></div>

<!-- set visible if 'variable' is null or empty -->
<div class="if_no-varname"></div>

<!-- add class 'classname' if 'variable' is null or empty -->
<div class="if_no-varname--classname"></div>/
</html>
```
<br>

**definitions:**

- a `part` is the source code of a template (including template language, html, ...)
- a `part` can includes `parts`
- a `varname` is the name of a property (allowing sub objects paths) in the objects `data` and `props`
- special class names `if-...` are parsed by the template engine and lead to a transforms of html elements depending of the value

### naming conventions

variable name are:
- camel case (first letter minus, words with first letter upper, words are concatenated)
- use `.` to build paths across object values

exemple: 

<a name="tdp"></a>
### template data & properties

Templates must by convention integrates these properties in two javascript objects:
- `data` : movie or movie list data
- `props` : a subset of template & application settings (what is really needed by the template)

#### template `data`

example of a movie detail page. Aggregates some properties from:
- the movie data
- the data of the query that provided the movie data (if any)

```js
const data = {
	"Url": "https://www.imdb.com/title/tt13655120/",
	"MetaData": {
		"Query": {
			"Spiders": [
				0
			],
			"Metadata": {
				"Source": "https://abcd/video/6844335721192",
				"Download": null,
				"QueryFile": "C:\\Users\\franc\\source\\repos\\MovieDbAssistant\\MovieDbAssistant.App\\bin\\Debug\\net8.0-windows10.0.22621.0\\input\\abcd.txt",
				"QueryFileLine": 9,
				"QueryCacheFiles": [
					"C:\\Users\\franc\\source\\repos\\MovieDbAssistant\\MovieDbAssistant.App\\bin\\Debug\\net8.0-windows10.0.22621.0\\temp\\imdb-SS5TLlMu2023.json"
				],
				"InstanceId": {
					"Value": 3
				}
			},
			"HashKey": "SS5TLlMu2023",
			"Title": "I.S.S.",
			"Count": null,
			"Year": "2023",
			"Languages": null,
			"Countries": null,
			"UserRating": null,
			"TitleTypes": null,
			"Genres": null
		},
		"ScraperTool": "movie-db-scrapper-windows-64bit-intel-1.1.1.exe",
		"ScraperToolVersion": "1.1.1",
		"SpiderId": "imdb",
		"SearchScore": {
			"Affinity": 1.0833333333333333,
			"DataCompletion": 1,
			"Value": 2.083333333333333
		}
	},
	"Sources": {
		"Download": null,
		"Play": "https://abcd/video/6844335721192"
	},
	"Id": "tt13655120",
	"Title": "I.S.S.",
	"OriginalTitle": "I.S.S.",
	"QueryTitle": "I.S.S.",
	"Summary": "Lorsqu'une guerre mondiale éclate sur Terre, entre l'Amérique et la Russie, les deux nations contactent secrètement leurs astronautes à bord de l'ISS et leur donnent des instructions pour pr...",
	"Interests": [
		"Psychological Thriller",
		"Space Sci-Fi",
		"Sci-Fi",
		"Thriller"
	],
	"Rating": "5,3",
	"RatingCount": "10",
	"Duration": "1h 35min",
	"ReleaseDate": "10 févr. 2024",
	"Year": "2024",
	"Vote": "10 k",
	"Director": "Gabriela Cowperthwaite",
	"Writers": [
		"Nick Shafir"
	],
	"Stars": [
		"Ariana DeBose",
		"Chris Messina",
		"John Gallagher Jr."
	],
	"Actors": [
		{
			"Actor": "Ariana DeBose",
			"PicUrl": [
				"https://m.media-amazon.com/images/M/MV5BNzk1NTNiYzgtZjgzZS00NTk1LWExMDYtYWRhNTVhZDRkNTczXkEyXkFqcGc@._V1_QL75_UX140_CR0,12,140,140_.jpg"
			],
			"Characters": [
				"Dr. Kira Foster"
			]
		},
		/* ... */
		{
			"Actor": "Pilou Asbæk",
			"PicUrl": [
				"https://m.media-amazon.com/images/M/MV5BZmU4NmE5NGYtYzk0Mi00MDQyLTgxZWUtNGEyMGRkYjQ2N2E3XkEyXkFqcGc@._V1_QL75_UX140_CR0,12,140,140_.jpg"
			],
			"Characters": [
				"Alexey Pulov"
			]
		}
	],
	"Anecdotes": "In December 2020, Nick Shafir's screenplay I.S.S. was included on that year's \"Black List\" of most-liked unproduced screenplays.@1:09, the word \"Canada\" on the Canadarm is reversed, showing that the filmmakers flipped the image.Referenced in Film Threat: I.S.S. + FOUNDERS DAY + MORE JANUARY GARBAGE | Film Threat Livecast (2024)Wind of ChangeWritten by Klaus MeinePerformed by ScorpionsPublished by BMG Platinum Songs US (BMI)All Rights Administered by BMG Rights Management (US) LLCLicensed by Sony Music Entertainment UK Limited",
	"MinPicUrl": "https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY281_CR2,0,190,281_.jpg",
	"MinPicWidth": "190",
	"MinPicAlt": "I.S.S. (2023)",
	"PicsUrls": [
		"https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY281_CR2,0,190,281_.jpg",
		"https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY422_CR3,0,285,422_.jpg",
		"https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY562_CR4,0,380,562_.jpg"
	],
	"MedPicUrl": "https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY562_CR4,0,380,562_.jpg",
	"PicFullUrl": "https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY281_CR2",
	"PicsSizes": [
		"50vw",
		" (min-width: 480px) 34vw",
		" (min-width: 600px) 26vw",
		" (min-width: 1024px) 16vw",
		" (min-width: 1280px) 16vw"
	],
	"Key": "SS5TLlMudHQxMzY1NTEyMA",
	"Filename": "SS5TLlMudHQxMzY1NTEyMA.html",
	"ListIndex": 28,
	"output.pages": "pages",
	"output.ext": ".html",
	"background": "https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY281_CR2",
	"backgroundIdle": "../img/cinema-wallpaper-40.jpg",
	"movies.index": 29,
	"movies.total": 98,
	"movies.home": "../index.html",
	"movies.previous": "../pages\\SGlwcG9jcmF0ZQdHQyODkxMDcw.html",
	"movies.next": "../pages\\SWJpemEdHQ3OTQyOTM2.html",
	"titleList": "My Movies",
	"pageTitleList": "My Movies",
	"pageTitleDetails": "My Movies",
	"templateId": "dark",
	"templateVersion": "1.0",
	"templateVersionDate": "2024/10/03",
	"softwareId": "MovieDbAssistant",
	"software": "Movie Db Assistant",
	"softwareVersion": "1.0.0.0",
	"softwareVersionDate": "2024/10/03",
	"builtAt": "13/10/2024 03:19:53",
	"lang": "en",
	"linkRepo": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
	"linkHelp": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
	"linkAuthor": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
	"listMoviePicNotAvailable": "./img/image-not-availble-300x175.jpg",
	"detailMoviePicNotAvailable": "../img/image-not-availble-300x175.jpg",
	"listMoviePicNotFound": "./img/image-404-630x630.jpg",
	"detailMoviePicNotFound": "../img/image-404-630x630.jpg",
	"subTitleList": "abcd"
}
```

#### template `props`

example of a movie detail page. Aggregates some properties from:

- the template settings
- the application settings (paths, description,...)
- some dynamic properties generated server side
- some dynamic properties generated client side by the template engine (eg: clock, date)
- some properties can be specific to a template theme
- the template engine and any of its modules can add and setup any properties in this set

```js
const props = {
	"output.pages": "pages",
	"output.ext": ".html",
	"background": "https://m.media-amazon.com/images/M/MV5BNzJkMDhkZTQtMzhkYi00YmI4LWE5ODctYjczZDA5NDUyZWQ0XkEyXkFqcGc@._V1_QL75_UY281_CR2",
	"backgroundIdle": "../img/cinema-wallpaper-40.jpg",
	"movies.index": 29,
	"movies.total": 98,
	"movies.home": "../index.html",
	"movies.previous": "../pages\\SGlwcG9jcmF0ZQdHQyODkxMDcw.html",
	"movies.next": "../pages\\SWJpemEdHQ3OTQyOTM2.html",
	"titleList": "My Movies",
	"pageTitleList": "My Movies",
	"pageTitleDetails": "My Movies",
	"templateId": "dark",
	"templateVersion": "1.0",
	"templateVersionDate": "2024/10/03",
	"softwareId": "MovieDbAssistant",
	"software": "Movie Db Assistant",
	"softwareVersion": "1.0.0.0",
	"softwareVersionDate": "2024/10/03",
	"builtAt": "13/10/2024 03:19:53",
	"lang": "en",
	"linkRepo": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
	"linkHelp": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
	"linkAuthor": "https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/README.md",
	"listMoviePicNotAvailable": "./img/image-not-availble-300x175.jpg",
	"detailMoviePicNotAvailable": "../img/image-not-availble-300x175.jpg",
	"listMoviePicNotFound": "./img/image-404-630x630.jpg",
	"detailMoviePicNotFound": "../img/image-404-630x630.jpg",
	"subTitleList": "My Catalog",
	"clock": "16 : 06",
	"date": "Sun 13 Oct"
}
```
