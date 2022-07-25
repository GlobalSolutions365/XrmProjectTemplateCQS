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