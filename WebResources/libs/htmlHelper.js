/// <reference path="../node_modules/@types/jquery/index.d.ts" />
var XRM;
(function (XRM) {
    var Libs;
    (function (Libs) {
        class HtmlHelper {
            static addOptionToSelect(selectId, label, value, isSelected = false) {
                $(`#${selectId}`).append($('<option>', {
                    value: value,
                    text: label,
                    select: isSelected
                }));
            }
        }
        Libs.HtmlHelper = HtmlHelper;
    })(Libs = XRM.Libs || (XRM.Libs = {}));
})(XRM || (XRM = {}));
//# sourceMappingURL=htmlHelper.js.map