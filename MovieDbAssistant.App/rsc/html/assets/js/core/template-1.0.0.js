const Separator_ClassCondition_ClassResult = '--'

const Tpl_Var_Prefix = '{{'
const Tpl_Var_Postfix = '}}'

const Class_Prefx_If = 'if-'
const Class_Prefx_If_No = 'if_no-'


// TODO: culture this
const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

/**
 * front template engine
 * @class
*/
class Template {

    /**
     * @type {UILayout} layout
     */
    layout = null

    constructor(enableAvoidNextItemClick) {
        window.tpl = this
        this.enableAvoidNextItemClick = enableAvoidNextItemClick
        this.layout = new UILayout()
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
     * @property {string} id provider id
     * @property {string} key title hash
     * @property {string} url provider web page url
     * @property {string} title title
     * @property {string} summary summary
     * @property {string[]} interests interests
     * @property {string} rating rating
     * @property {string} ratingCount rating count
     * @property {string} duration duration
     * @property {string} releaseDate releasedate
     * @property {string} year year
     * @property {string} vote vote
     * @property {string} director director
     * @property {string[]} writers writers
     * @property {string[]} stars stars 
     * @property {ActorModel[]} actors actors 
     * @property {string} anecdotes anecdotes
     * @property {string} minPicUrl min pic url
     * @property {string} minPicAlt min pic alt
     * @property {string} minPicWidth min pic width
     * @property {string[]} picsUrls pics urls
     * @property {string} picFullUrl pic full size url
     * @property {string[]} picsSizes pics sizes 
     */

    transforms = {
        "interests": (o, value) => o.hseps(value),
        "stars": (o, value) => o.hseps(value),
        "actors": (o, value) => o.hseps(value, x => o.actorSimple(x))
    };

    hseps(t, tr) {
        if (!t) return null
        if (!tr) tr = x => x
        return t.map(x => tr(x))
            .join(this.hsep())
    }

    /**
     * gets the horizontal separator from the template
     * @returns
     */
    hsep() {
        return props.tpl.props.hSep;
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
     * @property {string} actor actor
     * @property {string} picUrl pic url
     * @property {string[]} characters characters
     */

    /**
     * build page movies list
     * @param {MoviesModel} data movies set
     */
    buildItems(data) {
        data.movies.forEach((e, i) => {
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
        this.layout.setLinks($src)
        this.postInitCommon()
    }

    postInitCommon() {
        this.layout.setAlternatePics()
        this.layout.enableClock()
        this.layout.enableDate()
    }

    /**
     * set a prop variable value
     * @param {string} path var path in props.vars (separator point, first letter lower)
     * @param {any} value value
     */
    setVar(path, value) {
        const t = path.split(Dot)
        var vars = props.vars
        var i = 0        
        var k = ''
        while (i <= t.length - 1) {
            k = t[i]
            if (i == t.length - 1) {
                vars[t[i]] = value
                return
            } else {
                if (!vars[k])
                    vars[k] = {}
                vars = vars[k]
                i++
            }
        }        
    }

    /**
     * get a var value
     * @param {any} path var path (in props.vars)
     * @returns value at path
     */
    getVar(path) {
        const t = path.split('.')
        var vars = props.vars
        var i = 0
        var k = ''
        while (i <= t.length - 1)
        {
            k = t[i]
            if (i == t.length - 1) {
                return vars[t[i]]
            } else {
                vars = vars[k]
                i++
            }
        }
    }
   
    /**
     * add a new movie item in list
     * @param {MovieModel} data movie
     */
    addItem(data) {
        const $it = $(Query_Prefix_Id+Id_Item_Model).clone()
        $it.removeAttr(Attr_Id)
        $it.removeClass(Class_Hidden)
        $it.attr(Attr_Id, data.key);

        var p = {}
        Object.assign(p, data)
        Object.assign(p, props)
        var src = $it[0].outerHTML
        window.pvars = []
        src = this.parseVars(src, p)

        var $container = $(Query_Prefix_Class + Class_Movie_List)
        var $e = $(src)
        $container.append($e)
        $e.find(Query_Prefix_Class + Class_Movie_List_Item)
            .on(Event_Click, () => {
                if (this.avoidNextItemClick) {
                    this.avoidNextItemClick = false
                    return
                }
                window.location =
                    Path_Current
                    + props.output.pages
                    + Slash
                    + data.filename
            })
        this.setStates($e, p)
        this.layout.setLinks($e)

        $e.show()
    }

    removeItemModel() {
        const $it = $(Query_Prefix_Id + Id_Item_Model)
        $it.remove()
    }

    setStates($from, data, prefix) {

        var cl = x => '.' + x

        for (var p in data) {

            var val = data[p]
            var varnp = this.getVarname(p)

            if (typeof val == Type_Name_Object
                && val && val.constructor.name != Type_Name_Array
            ) {
                this.setStates(
                    $from,
                    val,
                    prefix ?
                        prefix + Dot + varnp
                        : varnp)
            }
            else {

                if (prefix)
                    p = prefix + Dot + varnp

                if (!val || val == Text_Empty) {

                    // if- : show if not null and no empty
                    var cn = cl(Class_Prefx_If) + this.getVarnameForClass(p)
                    $(cn, $from)
                        .each((i, e) => {
                            $(e).addClass(Class_Hidden)
                        });

                    // if_no- : show if null or emptpy
                    cn = cl(Class_Prefx_If_No) + this.getVarnameForClass(p)
                    $(cn, $from)
                        .each((i, e) => {
                            var $e = $(e)
                            var classList = $e.attr(Attr_Class);
                            var classArr = classList.split(/\s+/);
                            $.each(classArr, (i, v) => {
                                if (!v.includes(Separator_ClassCondition_ClassResult)) {
                                    if (v.startsWith(cn)) {
                                        $(e).removeClass(Class_Hidden)
                                    }
                                }
                            });
                        });

                    // if_no-prop--cn : enable class cn if null or empty
                    cn = Class_Prefx_If_No + this.getVarnameForClass(p)
                        + Separator_ClassCondition_ClassResult
                    var cns = Query_Contains_Class_Prefix + cn + Query_Selector_Postfix;
                    $(cns, $from)
                        .each((i, e) => {
                            var $e = $(e)
                            var classList = $e.attr(Attr_Class);
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

                if (val && val != Text_Empty) {

                    // if_no- : hide coz if null or emptpy
                    cn = cl(Class_Prefx_If_No) + this.getVarnameForClass(p)
                    $(cn, $from)
                        .each((i, e) => {
                            var $e = $(e)
                            var classList = $e.attr(Attr_Class);
                            var classArr = classList.split(/\s+/);
                            $.each(classArr, (i, v) => {
                                if (!v.includes(Separator_ClassCondition_ClassResult)) {
                                    if (v.startsWith(cn)) {
                                        $(e).addClass(Class_Hidden)
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

            if (typeof val == Type_Name_Object
                && val && val.constructor.name != Type_Name_Array
            ) {
                // sub object is ignored if null

                tpl = this.parseVars(
                    tpl,
                    val,
                    prefix ?
                        prefix + Dot + varnp
                        : varnp)
            }
            else {
                if (prefix)
                    varnp = prefix + Dot + varnp

                const srcVarName = this.getVarTag(varnp)
                tpl = tpl.replaceAll(
                    srcVarName,
                    this.transforms[p] ?
                        this.transforms[p](this, val)
                        : data[p]
                )

                // store as array for debug
                if (!window.pvars) window.pvars = []
                window.pvars.push(srcVarName)
            }
        }
        return tpl
    }

    getVarTag(name) {
        return Tpl_Var_Prefix + this.getVarname(name) + Tpl_Var_Postfix;
    }

    getVarname(name) {
        return firstLower(name);
    }

    getVarnameForClass(name) {
        return firstLower(name)
            .replaceAll(Dot, Dash)
    }
}
