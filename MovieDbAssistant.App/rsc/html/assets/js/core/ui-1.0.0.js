const Class_Dialog_Hidden = "dialog-hidden"
const Class_Dialog_Visible = "dialog-visible"

const Dialog_FadeIn_Time = 100
const Dialog_FadeOut_Time = 200

/**
 * open a dialog
 * @param {HTMLElement} this caller
 * @param {string} tplEId id of the dialog tpl instance */
function openDialog(e, tplEId) {
    var $tpl = $('#' + tplEId)
    $tpl.fadeIn(Dialog_FadeIn_Time)
}

/**
 * close a dialog
 * @param {HTMLElement} this caller
 * @param {string} tplEId id of the dialog tpl instance */
function closeDialog(e, tplEId) {
    var $tpl = $('#' + tplEId)
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
        $container.on('mouseup', () => {
            closeDragElement(e);
        })

        // call a function whenever the cursor moves:
        $container.on('mousemove', ev => {
            elementDrag(ev)
        })
        $e.addClass("dragging")
    }

    function elementDrag(e) {
        e.preventDefault();
        // calculate the new cursor position:
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        // set the element's new position:
        elem.style.top = (elem.offsetTop - pos2) + "px";
        elem.style.left = (elem.offsetLeft - pos1) + "px";
    }

    function closeDragElement(e) {
        // stop moving when mouse button is released:
        $container.off('onmouseup')
        $container.off('mousemove')
        $e.removeClass("dragging")
    }
}

/* #endregion */

/**
 * front ui engine
 * @class
*/
class UI {

    constructor() {
        window.ui = this
    }

    /** setup */
    setup() {

        // activate dialogs 'closer' buttons
        $('.dialog-button-closer').each((i, e) => {
            const $e = $(e)
            const id = $e.attr('data-dialog-id')
            $e.on('click', ev => {
                closeDialog(e, id)
            })
        })

        // activate draggables (drag.js)
        $('.draggable').each((i, e) => {
            activateDrag(e, $('body')[0])
        })
    }
}
