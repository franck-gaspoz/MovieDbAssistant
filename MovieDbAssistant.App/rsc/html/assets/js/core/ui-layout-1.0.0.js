/**
 * ui layout
 * uses window.props provided by template.js
 * @class
*/
class Layout {

    constructor() {
        window.layout = this
    }

    enableDate() {
        this.dateUpdate()
        setTimeout(() => this.enableDate(), 1000 * 30)
    }

    dateUpdate() {
        const now = new Date()
        const day = now.getDay();
        const date = now.getDate();
        const month = now.getMonth();
        const year = now.getFullYear();
        const str = `${dayNames[day].substring(0, 3)} ${date} ${monthNames[month].substring(0, 3)}`;
        props['date'] = str
        $('.with-date').html(str)
    }

    enableClock() {
        this.clockUpdate()
        setTimeout(() => this.enableClock(), 500)
    }

    clockUpdate() {
        this.clockUpdating = true
        const now = new Date()
        const hours = now.getHours().toString().padStart(2, '0');
        const minutes = now.getMinutes().toString().padStart(2, '0');
        //const seconds = now.getSeconds().toString().padStart(2, '0');
        //const str = hours + " : " + minutes + " : " + seconds;
        const str = hours + " : " + minutes;
        props['clock'] = str
        $('.with-clock').html(str)
        this.clockUpdating = false
    }

    setAlternatePics() {
        var $pics = $('.movie-page-list .alternate-pic-list')
        var altUrl = props.tpl.listMoviePicNotAvailable;
        var altnfUrl = props.tpl.listMoviePicNotFound;
        this.setupAlternatePic($pics, altUrl, altnfUrl)
        $pics = $('.movie-page-detail .alternate-pic-list')
        altUrl = props.tpl.detailMoviePicNotAvailable;
        altnfUrl = props.tpl.detailMoviePicNotFound;
        this.setupAlternatePic($pics, altUrl, altnfUrl)
    }

    setupAlternatePic($set, altUrl, altnfUrl) {
        $set.each((i, e) => {
            var $e = $(e)
            $e.on('error', () => {
                const src = $e.attr('src')
                const url = (!src || src == '' || src == 'null') ?
                    altUrl : altnfUrl
                $e.attr('src', url)
                $e.addClass('alternate-pic-list-enabled')
            })
        });
    }
}
