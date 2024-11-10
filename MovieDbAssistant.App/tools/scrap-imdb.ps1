# movie-db-scrapper-windows-64bit-intel-1.1.1.exe %*
# example:
# cls ; .\scrap-imdb.ps1 "Conjuring : Sous l'emprise du Diable" "release_date=2021-01-01,2021-12-31&title_type=feature,tv_series,tv_miniseries,video,tv_special,tv_movie,tv_episode&count=10,country_of_origin=FR"

param([string]$title="",[string]$filters="") 

cls
echo $title
echo $filters
#pause
del ./test.json
./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json $title $filters
code ./test.json
