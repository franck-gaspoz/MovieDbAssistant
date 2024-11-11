# movie-db-scrapper-windows-64bit-intel-1.1.1.exe %*
# example:
# cls ; .\scrap-imdb.ps1 "Conjuring : Sous l'emprise du Diable" "release_date=2019-01-01,2019-12-31&title_type=feature,tv_series,tv_miniseries,video,tv_special,tv_movie,tv_episode&count=10,country_of_origin=FR"

param([string]$title="",[string]$year="",[string]$country="") 

cls
echo title=$title
echo year=$year
echo country=$country
#pause
#del ./test.json
#./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json $title $filters
./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json $title "release_date=$year-01-01,$year-12-31&title_type=feature,tv_series,tv_miniseries,video,tv_special,tv_movie,tv_episode&count=10&country_of_origin=$country&user_rating=6.2,6.2"
code ./test.json
