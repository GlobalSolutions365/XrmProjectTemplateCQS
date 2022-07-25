/// <reference path="../node_modules/@types/jquery/index.d.ts" />

namespace XRM.Libs {
    export class HtmlHelper {
        public static addOptionToSelect(selectId: string, label: string, value: string, isSelected: boolean = false) {
            $(`#${selectId}`).append($('<option>', {
                value: value,
                text: label,
                select: isSelected
            }));
        }
    }
}