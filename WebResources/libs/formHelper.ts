/// <reference path="../node_modules/@types/xrm/index.d.ts" />
/// <reference path="../typings/domain.ts" />


namespace XRM.Libs {
    export class FormHelper {
        formContext: Xrm.FormContext;

        constructor(formContext: Xrm.FormContext) {
            this.formContext = formContext;
        }

        public attributeIsDirty(attributeId: string): boolean {
            let attribute = this.formContext.getAttribute(attributeId);
            return attribute && attribute.getIsDirty();
        }

        public getValueOrNull<TValue>(attributeId) : TValue {
            var attribute = this.formContext.getAttribute(attributeId);
            if (!attribute) { return null; }

            return <TValue> attribute.getValue();
        }

        public getLookupValue(attributeId): LookupValue {
            var lookup = this.formContext.getAttribute(attributeId);
            if (!lookup) {
                return null;
            }
            var lookupValue = lookup.getValue();

            if (!lookupValue || !lookupValue[0] || !lookupValue[0].id) {
                return null;
            } else {
                return {
                    entityType: lookupValue[0].entityType,
                    id: this.cleanupGuid(lookupValue[0].id),
                    name: lookupValue[0].name,
                }
            }
        }

        public getIdFromLookup(attributeId): string {
            var lookup = this.formContext.getAttribute(attributeId);
            if (!lookup) {
                return null;
            }
            var lookupValue = lookup.getValue();

            if (!lookupValue || !lookupValue[0] || !lookupValue[0].id) {
                return null;
            } else {
                return this.cleanupGuid(lookupValue[0].id);
            }
        }

        public setIdInLookup(attributeId: string, entityType: string, name: string, recordId: string, fireOnChange = false)
        {
            var lookupValue = new Array();
            lookupValue[0] = new Object();
            lookupValue[0].id = this.cleanupGuid(recordId);
            lookupValue[0].name = name;
            lookupValue[0].entityType = entityType
            this.formContext.getAttribute(attributeId)?.setValue(lookupValue);

            if (fireOnChange) {
                this.formContext.getAttribute(attributeId)?.fireOnChange();
            }
        }        
        
        public setValue(attributeId: string, value: any, fireOnChange = false) {
            this.formContext.getAttribute(attributeId)?.setValue(value);

            if (fireOnChange) {
                this.formContext.getAttribute(attributeId)?.fireOnChange();
            }
        }

        public isCreateForm() {
            return this.formContext.ui.getFormType() === 1;
        }

        public isUpdateForm() {
            return this.formContext.ui.getFormType() === 2;
        }

        public openFormByName(name: string) {
            if (this.formContext.data.entity.getIsDirty()) {
                this.formContext.data.save().then(
                    () => {
                        this.openFormByNameActual(name);
                    },
                    () => { });
            }
            else {
                this.openFormByNameActual(name);
            }
        }

        private openFormByNameActual(name: string) {
            for (let formItem of this.formContext.ui.formSelector.items.get()) {
                if (formItem.getLabel() == name) {
                    formItem.navigate();
                    return;
                }
            }
        }

        public getCurrentRecordId() {
            return this.cleanupGuid(this.formContext.data.entity.getId());
        }

        public cleanupGuid(guid: string): string {
            if (!guid) {
                return "";
            }

            return guid.toLowerCase().replace("{", "").replace("}", "");
        }
    }
}
