const ID_Model_Item = 'ItemModel';
const ID_Model_Class_Movie_List = 'movie-list';

/**
 * front sode template engine
 * @class
*/
class Template {

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
     * @property {string} PicFullUrls pic full size url
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
        if (!tr) tr = x => x;
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

        for (var e in data) {
            //console.debug(e);
        }
    }

    /** @param {MovieModel} data movie */
    buildDetails(data) {
        var $src = $('.' + ID_Model_Class_Movie_List)
        var html = $src[0].outerHTML
        html = this.parseVars(html, data)
        $src.html(html)
        console.debug(html)
    }

    /**@param {MovieModel} data movie */
    addItem(data) {

    }

    /**
     * 
     * @param {string} tpl html source
     * @param {MovieModel} data 
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
        return '{{' + this.firstLower(name) + '}}';
    }
}