﻿/**
 * open a dialog
 * @param {HTMLElement} this caller
 * @param {string} tplEId id of the dialog tpl instance */
function openDialog(e, tplEId) {
    var $tpl = $(Query_Prefix_Id + tplEId)
    $tpl.fadeIn(Dialog_FadeIn_Time)
}

/**
 * close a dialog
 * @param {HTMLElement} this caller
 * @param {string} tplEId id of the dialog tpl instance */
function closeDialog(e, tplEId) {
    var $tpl = $(Query_Prefix_Id + tplEId)
    $tpl.fadeOut(Dialog_FadeOut_Time)
}

/* #region ----- drag ----- */

function activateDrag(elem, container) {
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
 * ui engine
 * @class
*/
class UI {

    constructor() {
        window.ui = this
    }

    /**
     * setup
     */
    setup() {

        // activate dialogs 'closer' buttons
        $(Query_Prefix_Class+Class_Dialog_Button_Closer).each((i, e) => {
            const $e = $(e)
            const id = $e.attr(Data_Dialog_Id)
            $e.on(Event_Click, ev => {
                closeDialog(e, id)
            })
        })

        // activate draggables (drag.js)
        $(Query_Prefix_Class+ Class_Draggable).each((i, e) => {
            activateDrag(e, $(Tag_Body)[0])
        })
    }
}
