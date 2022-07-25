namespace XRM.Libs {
    export class UrlHelper {
        public static getUrlParameter(name, search = location.search) {
            if (!search.startsWith('?')) {
                search = `?${search}`;
            }

            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };

        public static getUrlParameterFromData(name): string {
            let queryString = window.location.search.substring(1);
            let realQueryString = decodeURIComponent(queryString.replace("data=", ""));

            return UrlHelper.getUrlParameter(name, realQueryString);
        }

        public static cleanupAttributeForApiRequest(value: string) : string {
            if (!value) { return value; }

            value = value.split("'").join("''");
            value = encodeURIComponent(value);
            value = value.split("%20").join(" ");

            return value;
        };

        public static replaceSpecialSymbol(value: string): string {
            return value.split('&').join('%26');
        }
    }
}

