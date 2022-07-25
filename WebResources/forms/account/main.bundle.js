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
/// <reference path="../node_modules/@types/xrm/index.d.ts" />
/// <reference path="../typings/domain.ts" />
var XRM;
(function (XRM) {
    var Libs;
    (function (Libs) {
        class FormHelper {
            constructor(formContext) {
                this.formContext = formContext;
            }
            attributeIsDirty(attributeId) {
                let attribute = this.formContext.getAttribute(attributeId);
                return attribute && attribute.getIsDirty();
            }
            getValueOrNull(attributeId) {
                var attribute = this.formContext.getAttribute(attributeId);
                if (!attribute) {
                    return null;
                }
                return attribute.getValue();
            }
            getLookupValue(attributeId) {
                var lookup = this.formContext.getAttribute(attributeId);
                if (!lookup) {
                    return null;
                }
                var lookupValue = lookup.getValue();
                if (!lookupValue || !lookupValue[0] || !lookupValue[0].id) {
                    return null;
                }
                else {
                    return {
                        entityType: lookupValue[0].entityType,
                        id: this.cleanupGuid(lookupValue[0].id),
                        name: lookupValue[0].name,
                    };
                }
            }
            getIdFromLookup(attributeId) {
                var lookup = this.formContext.getAttribute(attributeId);
                if (!lookup) {
                    return null;
                }
                var lookupValue = lookup.getValue();
                if (!lookupValue || !lookupValue[0] || !lookupValue[0].id) {
                    return null;
                }
                else {
                    return this.cleanupGuid(lookupValue[0].id);
                }
            }
            setIdInLookup(attributeId, entityType, name, recordId, fireOnChange = false) {
                var _a, _b;
                var lookupValue = new Array();
                lookupValue[0] = new Object();
                lookupValue[0].id = this.cleanupGuid(recordId);
                lookupValue[0].name = name;
                lookupValue[0].entityType = entityType;
                (_a = this.formContext.getAttribute(attributeId)) === null || _a === void 0 ? void 0 : _a.setValue(lookupValue);
                if (fireOnChange) {
                    (_b = this.formContext.getAttribute(attributeId)) === null || _b === void 0 ? void 0 : _b.fireOnChange();
                }
            }
            setValue(attributeId, value, fireOnChange = false) {
                var _a, _b;
                (_a = this.formContext.getAttribute(attributeId)) === null || _a === void 0 ? void 0 : _a.setValue(value);
                if (fireOnChange) {
                    (_b = this.formContext.getAttribute(attributeId)) === null || _b === void 0 ? void 0 : _b.fireOnChange();
                }
            }
            isCreateForm() {
                return this.formContext.ui.getFormType() === 1;
            }
            isUpdateForm() {
                return this.formContext.ui.getFormType() === 2;
            }
            openFormByName(name) {
                if (this.formContext.data.entity.getIsDirty()) {
                    this.formContext.data.save().then(() => {
                        this.openFormByNameActual(name);
                    }, () => { });
                }
                else {
                    this.openFormByNameActual(name);
                }
            }
            openFormByNameActual(name) {
                for (let formItem of this.formContext.ui.formSelector.items.get()) {
                    if (formItem.getLabel() == name) {
                        formItem.navigate();
                        return;
                    }
                }
            }
            getCurrentRecordId() {
                return this.cleanupGuid(this.formContext.data.entity.getId());
            }
            cleanupGuid(guid) {
                if (!guid) {
                    return "";
                }
                return guid.toLowerCase().replace("{", "").replace("}", "");
            }
        }
        Libs.FormHelper = FormHelper;
    })(Libs = XRM.Libs || (XRM.Libs = {}));
})(XRM || (XRM = {}));
//# sourceMappingURL=formhelper.js.map
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