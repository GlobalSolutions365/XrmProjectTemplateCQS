var XRM;
(function (XRM) {
    var Libs;
    (function (Libs) {
        class UrlHelper {
            static getUrlParameter(name, search = location.search) {
                if (!search.startsWith('?')) {
                    search = `?${search}`;
                }
                name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
                var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
                var results = regex.exec(search);
                return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
            }
            ;
            static getUrlParameterFromData(name) {
                let queryString = window.location.search.substring(1);
                let realQueryString = decodeURIComponent(queryString.replace("data=", ""));
                return UrlHelper.getUrlParameter(name, realQueryString);
            }
            static cleanupAttributeForApiRequest(value) {
                if (!value) {
                    return value;
                }
                value = value.split("'").join("''");
                value = encodeURIComponent(value);
                value = value.split("%20").join(" ");
                return value;
            }
            ;
            static replaceSpecialSymbol(value) {
                return value.split('&').join('%26');
            }
        }
        Libs.UrlHelper = UrlHelper;
    })(Libs = XRM.Libs || (XRM.Libs = {}));
})(XRM || (XRM = {}));
//# sourceMappingURL=urlHelper.js.map