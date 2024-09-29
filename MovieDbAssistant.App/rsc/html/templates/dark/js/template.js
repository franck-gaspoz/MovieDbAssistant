/**
 * front sode template engine
 * @class
*/
class Template {

    /** @param {Object[]} data movies list */
    buildItems(data) {

        for (var e in data) {
            console.debug(e);
        }
    }

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
     * @property {string[]} Starts stars 
     * @property {ActorModel[]} Actors actors 
     * @property {string} Anecdotes anecdotes
     * @property {string} MinPicUrl min pic url
     * @property {string} MinPicAlt min pic alt
     * @property {string} MinPicWidth min pic width
     * @property {string[]} PicsUrls pics urls
     * @property {string} PicFullUrls pic full size url
     * @property {string[]} PicsSizes pics sizes 
     */

    /**
     * @typedef ActorModel actor model
     * @property {string} Actor actor
     * @property {string} PicUrl pic url
     * @property {string[]} Characters characters
     */

    /**@param {MovieModel} data movie object */
    addItem(data) {
        data.string
    }
}