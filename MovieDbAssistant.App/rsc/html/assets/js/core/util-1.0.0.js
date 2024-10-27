﻿/*#region ----- consts ----- */

/*#region global */

const Dot = '.'
const Space = ' '
const Slash = '/'
const Dash = '-'

const Text_Null = 'null'
const Text_Empty = ''

const Type_Name_Object = 'object'
const Type_Name_Array = 'Array'

const Path_Current = './'

const Event_Load = 'load'
const Event_Error = 'error'
const Event_Click = 'click'
const Event_MouseUp = 'mouseup'
const Event_MouseMove = 'mousemove'

/*#endregion*/

/*#region kernel */

const Var_System = "system";
const Var_Clock = Var_System + ".now";

/*#endregion*/

/*#region ui */

const HRef_Id_Separator = '#'

const Dialog_FadeIn_Time = 100
const Dialog_FadeOut_Time = 200

const Data_HRef = 'data-href'
const Data_Target = 'data-target'
const Data_Dialog_Id = 'data-dialog-id'

const Attr_Src = 'src'
const Attr_Id = 'id'
const Attr_Class = 'class'

const Class_Hidden = 'hidden'
const Class_Clock_With = 'with-clock'
const Class_Date_With = 'with-date'
const Class_Alternate_Pic_List_Enabled = 'alternate-pic-list-enabled'
const Class_Movie_Page_Detail = 'movie-page-detail'
const Class_Movie_Page_List = 'movie-page-list'
const Class_Movie_List = 'movie-list'
const Class_Movie_List_Item = 'movie-list-item'
const Class_Alternate_Pic_List = 'alternate-pic-list'
const Class_Dialog_Hidden = "dialog-hidden"
const Class_Dialog_Visible = "dialog-visible"
const Class_Dialog_Button_Closer = 'dialog-button-closer'
const Class_Draggable = 'draggable'
const Class_Dragging = 'dragging'

const Id_Item_Model = 'ItemModel';

const Tag_Body = 'body';
const Tag_Html = 'html';

const Query_Prefix_Class = '.'
const Query_Prefix_Id = '#'
const Query_Select_Prefix = '['
const Query_Select_Postfix = ']'
const Query_Contains_Class_Prefix = "[class*='"
const Query_Selector_Postfix = "']"
const Query_Equals_Id_Prefix = "[id='"

const Unit_Px = 'px'

/*#endregion*/

/*#endregion ----- */

/*#region ----- kernel ----- */

/**
 * gets the template object 
 * @returns {Template} object template
 */
function _tpl() {
    return window.tpl
}

/**
 * gets the layout object
 * @returns {Layout} object layout
 */
function _layout() {
    return window.layout
}

/**
 * gets the ui object
 * @returns {UI} ui object
 */
function _ui() {
    return window.ui
}

/*#endregion ----- */

/**
 * first letter in lower case
 * @param {string} txt text
 * @returns text
 */
function firstLower(txt) {
    return txt.charAt(0).toLowerCase() + txt.slice(1);
}
