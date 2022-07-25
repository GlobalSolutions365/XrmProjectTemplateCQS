namespace XRM.Libs {
    export class QueryHelper {
        public static valueOrDefault(attr, defaultValue = null) {
            return attr ? attr : defaultValue;
        }

        public static nullableNumberToString(nr?: number): string {
            if (!nr) { return ""; }
            else return nr.toString();
        }

        public static nullableStringToString (str?: string): string {
            if (!str) { return ""; }
            else return str;
        }

        public static nullableStringToNumber(str?: string): number {
            if (!str) { return null; }
            else return parseInt(str);
        }

        public static jsonDateToDateString(dateStr?: string): string {
            if (!dateStr) { return ""; }

            // Sample value: "/Date(1584023400000)/"
            try {
                dateStr = dateStr.replace(/\D/g, '');

                let date = new Date(parseInt(dateStr));

                return date.toLocaleDateString();
            }
            catch (err) {
                return "";
            }
        }

        public static ensureResponse(result, callback: (response: any) => void) {
            if (!result || !callback) { return; }

            if (result.responseText) {
                callback(JSON.parse(result.responseText));
            }

            result.json().then(
                (response) => {
                    callback(response);
                }
            );
        }

        public static async ensureResponseAsync(result) : Promise<any> {
            if (!result) { return null; }

            if (result.responseText) {
                return JSON.parse(result.responseText);
            }

            return result.json();
        }

        public static async callGenericActionAsync(type: string, request: string): Promise<string> {
            let action = {
                Type: type,
                Request: request,

                getMetadata: function () {
                    return {
                        boundParameter: null,
                        parameterTypes: {
                            "Type": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            },
                            "Request": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            }
                        },
                        operationType: 0,
                        operationName: "dss_GenericAction"
                    };
                }
            };

            let result = await Xrm.WebApi.online.execute(action);
            if (result.ok) {
                let response = await this.ensureResponseAsync(result);

                return response.Response;
            }

            return null;
        }

        public static callGenericAction(type: string, request: string, callback: (response: string) => void) {
            let action = {
                Type: type,
                Request: request,

                getMetadata: function () {
                    return {
                        boundParameter: null,
                        parameterTypes: {
                            "Type": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            },
                            "Request": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            }
                        },
                        operationType: 0,
                        operationName: "dss_GenericAction"
                    };
                }
            };

            Xrm.WebApi.online.execute(action).then(
                (result) => {
                    if (result.ok) {
                        XRM.Libs.QueryHelper.ensureResponse(result, (response: any) => {
                            callback(response.Response);
                        });

                    }
                },
                (error) => {
                    Xrm.Utility.alertDialog(error.message, null);
                }
            );
        }
    }
}