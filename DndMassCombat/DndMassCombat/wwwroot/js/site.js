// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function setTrueAndSubmit(id) {
    const $form = $("#simulateForm");

    $.validator.unobtrusive.parse($form);
    $form.validate();

    if ($form.valid()) 
    {
        const button = document.getElementById(id);
        const hidden = button.previousElementSibling;
        hidden.value = true;
        
        $form.submit();
    }
}

// UnitCount = Group hit point / Unit hit point
function assignUnitCount(id){
    const groupHitpointString = document.getElementById(id).value;

    if (!isInt(groupHitpointString))
        return;

    const groupNumberIdentifier = id.split("_");
    const groupNumber = groupNumberIdentifier[0] === "Group1" ? 1 : 2;
    const unitHitPointFieldId = "UnitDescription" + groupNumber + "_HitPoint";
    const unitHitpointString = document.getElementById(unitHitPointFieldId).value;

    if (!isInt(unitHitpointString))
        return;

    const groupHitpoint = parseInt(groupHitpointString);
    const unitHitpoint = parseInt(unitHitpointString);
    const unitCount = Math.floor(groupHitpoint / unitHitpoint);

    let unitCountField = document.getElementById(groupNumberIdentifier[0] + "_UnitCount");
    
    if (unitCount > 0)
         unitCountField.value = unitCount;
    else
        unitCountField.value = null;
    
}

function isInt(value) {
    return !isNaN(value) &&
        parseInt(Number(value)) == value &&
        !isNaN(parseInt(value, 10));
}