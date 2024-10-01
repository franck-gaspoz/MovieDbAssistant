const ID_Model_Item = 'ItemModel';
const ID_Model_Class_Movie_List = 'movie-list';
const Tag_Body = 'body';
const Class_Prefx_If = 'if-'

/**
 * front sode template engine
 * @class
*/
class Template {

    constructor() {
        window.tpl = this
    }

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
        "Actors": (o, value) => o.hseps(value, x => o.actorSimple(x)),
        "PicsUrls": null,
        "PicsSizes": null
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

    /** @param {MoviesModel} data movies set */
    buildItems(data) {

        data.Movies.forEach((e, i) => {
            this.addItem(e)
        })
        this.removeItemModel()
    }

    /**@param {MovieModel} data movie */
    addItem(data) {
        const $it = $('#ItemModel').clone()
        var p = {}
        Object.assign(p, data)
        Object.assign(p, props)
        var src = $it[0].outerHTML
        src = this.parseVars(src, p)

        var $container = $('.movie-list')
        var $e = $(src)
        $container.append($e)

        this.setStates($e, p)

        $e.show()
    }

    removeItemModel() {
        const $it = $('#ItemModel')
        $it.remove()
    }

    /** @param {MovieModel} data movie */
    buildDetails(data) {
        var $src = $(Tag_Body)
        var html = $src[0].outerHTML
        html = this.parseVars(html, data)
        $src.html(html)
        this.setStates(null, data)
    }

    setStates($from, data) {
        for (var p in data) {
            var val = data[p]
            if (!val || val == '') {
                var cl = x => '.' + x
                $(cl(Class_Prefx_If) + this.getVarname(p), $from)
                    .each((i, e) => {
                        $(e).addClass('hidden')
                    });
            }
        }
    }

    /**
     * parse and set vars
     * @param {string} tpl html source
     * @param {object} data 
     */
    parseVars(tpl, data) {
        for (var p in data) {
            tpl = tpl.replaceAll(
                this.getVar(p),
                this.props[p] ?
                    this.props[p](this, data[p])
                    : data[p]
            )
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
        return this.firstLower(name)
            .replaceAll('.', '-')
    }
}

function handleBackImgLoaded(img) {
    var $i = $('#Image_Background')
    var w = img.naturalWidth
    var h = img.naturalHeight
    var $c = $('.movie-page-detail-background-container')
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

    //$('#Image_BackgroundIdle').fadeOut(1000);

    $i[0].src = img.src
    $i.fadeIn(1000)
}

function addBackImgLoadedHandler(src) {
    var img = new Image();
    img.addEventListener('load', () => handleBackImgLoaded(img), false);
    img.src = src;
}