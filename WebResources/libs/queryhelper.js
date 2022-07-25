var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var XRM;
(function (XRM) {
    var Libs;
    (function (Libs) {
        class QueryHelper {
            static valueOrDefault(attr, defaultValue = null) {
                return attr ? attr : defaultValue;
            }
            static nullableNumberToString(nr) {
                if (!nr) {
                    return "";
                }
                else
                    return nr.toString();
            }
            static nullableStringToString(str) {
                if (!str) {
                    return "";
                }
                else
                    return str;
            }
            static nullableStringToNumber(str) {
                if (!str) {
                    return null;
                }
                else
                    return parseInt(str);
            }
            static jsonDateToDateString(dateStr) {
                if (!dateStr) {
                    return "";
                }
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
            static ensureResponse(result, callback) {
                if (!result || !callback) {
                    return;
                }
                if (result.responseText) {
                    callback(JSON.parse(result.responseText));
                }
                result.json().then((response) => {
                    callback(response);
                });
            }
            static ensureResponseAsync(result) {
                return __awaiter(this, void 0, void 0, function* () {
                    if (!result) {
                        return null;
                    }
                    if (result.responseText) {
                        return JSON.parse(result.responseText);
                    }
                    return result.json();
                });
            }
            static callGenericActionAsync(type, request) {
                return __awaiter(this, void 0, void 0, function* () {
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
                    let result = yield Xrm.WebApi.online.execute(action);
                    if (result.ok) {
                        let response = yield this.ensureResponseAsync(result);
                        return response.Response;
                    }
                    return null;
                });
            }
            static callGenericAction(type, request, callback) {
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
                Xrm.WebApi.online.execute(action).then((result) => {
                    if (result.ok) {
                        XRM.Libs.QueryHelper.ensureResponse(result, (response) => {
                            callback(response.Response);
                        });
                    }
                }, (error) => {
                    Xrm.Utility.alertDialog(error.message, null);
                });
            }
        }
        Libs.QueryHelper = QueryHelper;
    })(Libs = XRM.Libs || (XRM.Libs = {}));
})(XRM || (XRM = {}));
//# sourceMappingURL=queryhelper.js.map