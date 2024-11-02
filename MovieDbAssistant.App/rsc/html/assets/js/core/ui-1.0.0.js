﻿/**
 * UI
 * ------------------
 * dependencies:
 *      util-1.0.0
 *      template-1.0.0
 *      layout-1.0.0
 */

/**
 * ui engine
 * @class
*/
class UI {

    constructor() {
        window.ui = this
        
    }

    /**
     * pre setup
     */
    preSetup() {
        // apply zoom scale
        this.#applyZoomScale()

        // add front props
        this.#setupVariables()
    }

    /**
     * setup (on document ready)
     */
    setup() {
        // activate dialogs 'closer' buttons
        this.#setupClosersButtons()

        // activate draggables (TODO: drag.js)
        this.#setupDraggables()

        // setup zoom for windowed mode
        this.#setupZoom()
    }

    /**
     * close current window
     * @param {HTMLElement} e
     */
    closeWindow(e) {

    }

    /**
     * mawimize current window
     * @param {HTMLElement} e
     */
    mawimizeWindow(e) {

    }

    /**
     * minimize current window
     * @param {HTMLElement} e
     */
    minimizeWindow(e) {

    }

    /**
     * restore current window
     * @param {HTMLElement} e
     */
    restoreWindow(e) {

    }

    /**
     * open a dialog
     * @param {HTMLElement} this caller
     * @param {string} tplEId id of the dialog tpl instance */
    openDialog(e, tplEId) {
        var $tpl = $(Query_Prefix_Id + tplEId)
        $tpl.fadeIn(Dialog_FadeIn_Time)
    }

    /**
     * close a dialog
     * @param {HTMLElement} this caller
     * @param {string} tplEId id of the dialog tpl instance */
    closeDialog(e, tplEId) {
        var $tpl = $(Query_Prefix_Id + tplEId)
        $tpl.fadeOut(Dialog_FadeOut_Time)
    }

    /* #region ----- drag ----- */

    activateDrag(elem, container) {
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;

        elem.onmousedown = dragMouseDown;

        const $container = $(container)
        const $e = $(elem)

        function dragMouseDown(ev) {
            //e = e || window.event;
            if (!ev) return
            ev.preventDefault();

            const e = ev.target

            // get the mouse cursor position at startup:
            pos3 = ev.clientX;
            pos4 = ev.clientY;
            $container.on(Event_MouseUp, () => {
                closeDragElement(e);
            })

            // call a function whenever the cursor moves:
            $container.on(Event_MouseMove, ev => {
                elementDrag(ev)
            })
            $e.addClass(Class_Dragging)
        }

        function elementDrag(e) {
            e.preventDefault();
            // calculate the new cursor position:
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            // set the element's new position:
            elem.style.top = (elem.offsetTop - pos2) + Unit_Px;
            elem.style.left = (elem.offsetLeft - pos1) + Unit_Px;
        }

        function closeDragElement(e) {
            // stop moving when mouse button is released:
            $container.off(Event_MouseUp)
            $container.off(Event_MouseMove)
            $e.removeClass(Class_Dragging)
        }
    }

    /* #endregion */

    /**
     * setup variables
     */
    #setupVariables() {
        _tpl().setVar(Var_InDesktopMode,inDesktopMode())
    }

    /**
     * setup draggables
     */
    #setupDraggables() {
        var t = this
        $(Query_Prefix_Class + Class_Draggable).each((i, e) => {
            t.activateDrag(e, $(Tag_Body)[0])
        })
    }

    /**
     * activate closers buttons
     */
    #setupClosersButtons() {
        var t = this
        $(Query_Prefix_Class + Class_Dialog_Button_Closer).each((i, e) => {
            const $e = $(e)
            const id = $e.attr(Data_Dialog_Id)
            $e.on(Event_Click, ev => {
                t.closeDialog(e, id)
            })
        })
    }

    /**
     * activate zoom for windowed mode
     */
    #setupZoom() {
        $(window).on(Event_Resize, ev => {
            this.#applyZoomScale()
        });
    }

    /**
     * apply zoom scale
     */
    #applyZoomScale() {
        var $w = $(window)
        const winWidth = $w.width()
        const refWidth = 1920
        const z = winWidth / refWidth;
        $(Tag_Html)
            .css(Attr_Zoom, z);
        if (z > 0)
            $(Query_Prefix_Class + Class_Page_Container_App_Region)
                .css(Attr_Zoom, 1 / z)
    }
}
