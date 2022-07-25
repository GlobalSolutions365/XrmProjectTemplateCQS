/// <reference path="../../node_modules/@types/xrm/index.d.ts" />
/// <reference path="../../libs/formhelper.ts" />
/// <reference path="../../libs/queryhelper.ts" />

namespace KAS.Account {
    let formContext: Xrm.FormContext;
    let formHelper: XRM.Libs.FormHelper;

    export function onLoad(eventContext: Xrm.Events.EventContext) {
        formContext = eventContext.getFormContext();
        formHelper = new XRM.Libs.FormHelper(formContext);
        
        initEvents();
    }

    function initEvents() {
        formContext.getAttribute("name")?.addOnChange(nameChanged);     
        formContext.data.entity.addOnSave(onSave);
    }

    function nameChanged() {
        const name = formContext.getAttribute("name")?.getValue();

        alert(`Name changed to ${name}`);
    }

    function onSave(eventContext: Xrm.Events.SaveEventContext)
    {       
        // On save logic goes here
    }
}