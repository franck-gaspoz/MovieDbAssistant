﻿const ID_Model_Item = 'ItemModel';
const ID_Model_Class_Movie_List = 'movie-list';

const Tag_Body = 'body';
const Tag_Html = 'html';

const Class_Prefx_If = 'if-'
const Class_Prefx_If_No = 'if_no-'
const Separator_ClassCondition_ClassResult = '--'

const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

/**
 * front sode template engine
 * @class
*/
class Template {

    constructor(enableAvoidNextItemClick) {
        window.tpl = this
        this.enableAvoidNextItemClick = enableAvoidNextItemClick
    }

    /** @type {boolean} avoid next item click in case overlapped click */
    avoidNextItemClick = false

    /** @type {boolean} enabed/disable feature 'avoid next item click' to prevent possible overlapped clicks */
    enableAvoidNextItemClick = false

    /**
     * @typedef MoviesModel movies model
     * @type {object}
     * @property {MovieModel[]} Movies movies models
     */

    /**
     * @typedef MovieModel movie model
     * @type {object}
     * @property {string} Id provider id
     * @property {string} Key title hash
     * @property {string} Url provider web page url
     * @property {string} Title title
     * @property {string} Summary summary
     * @property {string[]} Interests interests
     * @property {string} Rating rating
     * @property {string} RatingCount rating count
     * @property {string} Duration duration
     * @property {string} ReleaseDate releasedate
     * @property {string} Year year
     * @property {string} Vote vote
     * @property {string} Director director
     * @property {string[]} Writers writers
     * @property {string[]} Stars stars 
     * @property {ActorModel[]} Actors actors 
     * @property {string} Anecdotes anecdotes
     * @property {string} MinPicUrl min pic url
     * @property {string} MinPicAlt min pic alt
     * @property {string} MinPicWidth min pic width
     * @property {string[]} PicsUrls pics urls
     * @property {string} PicFullUrl pic full size url
     * @property {string[]} PicsSizes pics sizes 
     */

    props = {
        "Interests": (o, value) => o.hseps(value),
        "Stars": (o, value) => o.hseps(value),
        "Actors": (o, value) => o.hseps(value, x => o.actorSimple(x))
    };

    hseps(t, tr) {
        if (!t) return null
        if (!tr) tr = x => x
        return t.map(x => tr(x))
            .join(this.hsep())
    }

    hsep() {
        return '<span class="hsep"></span>';
    }

    /**
     * actor simple html
     * @param {ActorModel} actor Actor
     * @returns actor simple html
     */
    actorSimple(actor) {
        return actor.Actor
    }

    /**
     * @typedef ActorModel actor model
     * @property {string} Actor actor
     * @property {string} PicUrl pic url
     * @property {string[]} Characters characters
     */

    /**
     * build page movies list
     * @param {MoviesModel} data movies set
     */
    buildItems(data) {
        data.Movies.forEach((e, i) => {
            this.addItem(e)
        })
        this.removeItemModel()
        this.postInitCommon()
    }

    /**
     * build page movie detail
     * @param {MovieModel} data movie
     */
    buildDetails(data) {
        var $src = $(Tag_Body)
        var html = $src[0].outerHTML
        html = this.parseVars(html, data)
        $src.html(html)
        this.setStates(null, data)
        this.setLinks($src)
        this.postInitCommon()
    }

    postInitCommon() {
        this.setAlternatePics()
        this.enableClock()
        this.enableDate()
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
        const seconds = now.getSeconds().toString().padStart(2, '0');
        //const str = hours + " : " + minutes + " : " + seconds;
        const str = hours + " : " + minutes;
        props['clock'] = str
        $('.with-clock').html(str)
        this.clockUpdating = false
    }

    setAlternatePics() {
        var $pics = $('.movie-page-list .alternate-pic-list')
        var altUrl = props['listMoviePicNotAvailable']
        var altnfUrl = props['listMoviePicNotFound']
        this.setupAlternatePic($pics, altUrl, altnfUrl)
        $pics = $('.movie-page-detail .alternate-pic-list')
        altUrl = props['detailMoviePicNotAvailable']
        altnfUrl = props['detailMoviePicNotFound']
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

    /**@param {MovieModel} data movie */
    addItem(data) {
        const $it = $('#ItemModel').clone()
        $it.removeAttr('id')
        $it.removeClass('hidden')
        $it.attr('id', data.Key);

        var p = {}
        Object.assign(p, data)
        Object.assign(p, props)
        var src = $it[0].outerHTML
        window.pvars = []
        src = this.parseVars(src, p)

        var $container = $('.movie-list')
        var $e = $(src)
        $container.append($e)
        $e.find('.movie-list-item')
            .on('click', () => {
                if (this.avoidNextItemClick) {
                    this.avoidNextItemClick = false
                    return
                }
                window.location =
                    './'
                    + props['output.pages'/*Template_Var_OutputPages*/]
                    + '/'
                    + data.Filename
            })
        this.setStates($e, p)
        this.setLinks($e)

        $e.show()
    }

    removeItemModel() {
        const $it = $('#ItemModel')
        $it.remove()
    }

    setLinks($from) {
        var $t = $("[data-href]", $from)
        $t.each((i, e) => {
            var $e = $(e)
            var href = $e.attr('data-href')
            var target = $e.attr('data-target')
            $e.on('click', e => {
                if (this.enableAvoidNextItemClick) {
                    this.avoidNextItemClick = true;
                }
                if (!target)
                    window.location = href;
                else {
                    window.open(href, target)
                }
            })
        })
    }

    setStates($from, data, prefix) {

        var cl = x => '.' + x

        for (var p in data) {

            var val = data[p]
            var varnp = this.getVarname(p)

            if (typeof val == 'object'
                && val && val.constructor.name != 'Array'
            ) {
                this.setStates(
                    $from,
                    val,
                    prefix ?
                        prefix + '.' + varnp
                        : varnp)
            }
            else {

                if (prefix)
                    p = prefix + '.' + varnp

                if (!val || val == '') {

                    // if- : show if not null and no empty
                    var cn = cl(Class_Prefx_If) + this.getVarnameForClass(p)
                    $(cn, $from)
                        .each((i, e) => {
                            $(e).addClass('hidden')
                        });

                    // if_no- : show if null or emptpy
                    cn = cl(Class_Prefx_If_No) + this.getVarnameForClass(p)
                    $(cn, $from)
                        .each((i, e) => {
                            var $e = $(e)
                            var classList = $e.attr("class");
                            var classArr = classList.split(/\s+/);
                            $.each(classArr, (i, v) => {
                                if (!v.includes(Separator_ClassCondition_ClassResult)) {
                                    if (v.startsWith(cn)) {
                                        $(e).removeClass('hidden')
                                    }
                                }
                            });
                        });

                    // if_no-prop--cn : enable class cn if null or empty
                    cn = Class_Prefx_If_No + this.getVarnameForClass(p)
                        + Separator_ClassCondition_ClassResult
                    var cns = "[class*='" + cn + "']";
                    $(cns, $from)
                        .each((i, e) => {
                            var $e = $(e)
                            var classList = $e.attr("class");
                            var classArr = classList.split(/\s+/);
                            $.each(classArr, (i, v) => {
                                if (v.includes(cn)) {
                                    var cn2 = v.split(Separator_ClassCondition_ClassResult)[1]
                                    $e.removeClass(v)
                                    $e.addClass(cn2)
                                }
                            });
                        });
                }

                if (val && val != '') {

                    // if_no- : hide coz if null or emptpy
                    cn = cl(Class_Prefx_If_No) + this.getVarnameForClass(p)
                    $(cn, $from)
                        .each((i, e) => {
                            var $e = $(e)
                            var classList = $e.attr("class");
                            var classArr = classList.split(/\s+/);
                            $.each(classArr, (i, v) => {
                                if (!v.includes(Separator_ClassCondition_ClassResult)) {
                                    if (v.startsWith(cn)) {
                                        $(e).addClass('hidden')
                                    }
                                }
                            });
                        });
                }
            }
        }
    }

    /**
     * parse and set vars
     * @param {string} tpl html source
     * @param {object} data 
     * @param {?string} prefix var prefix (default null)
     */
    parseVars(tpl, data, prefix) {

        for (var p in data) {

            var val = data[p]
            var varnp = this.getVarname(p)

            if (typeof val == 'object'
                && val && val.constructor.name != 'Array'
            ) {
                // sub object is ignored if null

                tpl = this.parseVars(
                    tpl,
                    val,
                    prefix ?
                        prefix + '.' + varnp
                        : varnp)
            }
            else {
                if (prefix)
                    varnp = prefix + '.' + varnp

                const srcVarName = this.getVar(varnp)
                tpl = tpl.replaceAll(
                    srcVarName,
                    this.props[p] ?
                        this.props[p](this, val)
                        : data[p]
                )

                // store as array for debug
                if (!window.pvars) window.pvars = []
                window.pvars.push(srcVarName)
            }
        }
        return tpl
    }

    firstLower(txt) {
        return txt.charAt(0).toLowerCase() + txt.slice(1);
    }

    getVar(name) {
        return '{{' + this.getVarname(name) + '}}';
    }

    getVarname(name) {
        return this.firstLower(name);
        //.replaceAll('.', '-')
    }

    getVarnameForClass(name) {
        return this.firstLower(name)
            .replaceAll('.', '-')
    }
}

function handleBackImgLoaded(img) {
    var $i = $('#Image_Background')
    var w = img.naturalWidth
    var h = img.naturalHeight
    var $c = $('.movie-page-background-container')
    var wc = $c.width()
    var hc = $c.height()
    var maxw = w >= h
    var aw = w, ah = h

    var w0 = w
    var h0 = h
    while (w > wc && h > hc) {
        aw = w
        ah = h
        w /= 1.2
        h /= 1.2
    }
    w = aw
    h = ah
    var zoom = w / w0;

    var setwh = false
    if (w < wc) {
        var z = wc / w
        w *= z
        h *= z
        setwh = true
    }

    if (h < hc) {
        var z = hc / h
        w *= z
        h *= z
        setwh = true
    }

    var cl = maxw ?
        'width100p' : 'height100p'
    var left = w >= wc ?
        -(maxw ? w0 : w - wc) / 2 : (wc - w0) / 2
    var top = h >= hc ?
        -(!maxw ? h0 : h - hc) / 2 : (hc - h0) / 2
    $i.addClass(cl)
    $i.css('left', left + 'px')
    $i.css('top', top + 'px')
    $i.css('zoom', zoom)
    if (setwh) {
        $i.css('width', w + 'px')
        $i.css('height', h + 'px')
    }

    $i[0].src = img.src
    $i.fadeIn(1000)
}

function addBackImgLoadedHandler(src) {
    var img = new Image();
    img.addEventListener('load', () => handleBackImgLoaded(img), false);
    img.src = src;
}

function setupItemsLinkId() {
    var t = window.location.href.split('#')
    if (t.length == 2) {
        var $it = $("[id='" + t[1] + "']")
        $('.movie-list').scrollTop(
            $it.offset().top
            - $it.height()
        )
    }
}