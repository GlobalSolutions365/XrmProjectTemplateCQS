/// <reference path="../../node_modules/@types/xrm/index.d.ts" />
/// <reference path="../../libs/formhelper.ts" />
/// <reference path="../../libs/queryhelper.ts" />
var KAS;
(function (KAS) {
    var Account;
    (function (Account) {
        let formContext;
        let formHelper;
        function onLoad(eventContext) {
            formContext = eventContext.getFormContext();
            formHelper = new XRM.Libs.FormHelper(formContext);
            initEvents();
        }
        Account.onLoad = onLoad;
        function initEvents() {
            var _a;
            (_a = formContext.getAttribute("name")) === null || _a === void 0 ? void 0 : _a.addOnChange(nameChanged);
            formContext.data.entity.addOnSave(onSave);
        }
        function nameChanged() {
            var _a;
            const name = (_a = formContext.getAttribute("name")) === null || _a === void 0 ? void 0 : _a.getValue();
            alert(`Name changed to ${name}`);
        }
        function onSave(eventContext) {
            // On save logic goes here
        }
    })(Account = KAS.Account || (KAS.Account = {}));
})(KAS || (KAS = {}));
//# sourceMappingURL=accountMain.js.map