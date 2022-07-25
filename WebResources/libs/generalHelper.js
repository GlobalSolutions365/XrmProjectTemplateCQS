var XRM;
(function (XRM) {
    var Libs;
    (function (Libs) {
        class GeneralHelper {
            static newGuid() {
                return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
                    return v.toString(16);
                });
            }
            static isGuid(value) {
                var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
                var match = regex.exec(value);
                return match != null;
            }
        }
        Libs.GeneralHelper = GeneralHelper;
    })(Libs = XRM.Libs || (XRM.Libs = {}));
})(XRM || (XRM = {}));
//# sourceMappingURL=generalHelper.js.map