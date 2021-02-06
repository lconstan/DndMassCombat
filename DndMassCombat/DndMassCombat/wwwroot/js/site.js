// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function setTrueAndSubmit(id) {
    var $form = $("#simulateForm");
    
    $.validator.unobtrusive.parse($form);
    $form.validate();

    if ($form.valid()) 
    {
        var button = document.getElementById(id);
        var hidden = button.previousElementSibling;
        hidden.value = true;
        
        $form.submit();
    }
}

// UnitCount = Group hit point / Unit hit point
function assignUnitCount(id){
    var groupHitpointString = document.getElementById(id).value;
    
    if (!isInt(groupHitpointString))
        return;
    
    var groupNumberIdentifier = id.split("_");
    var groupNumber = groupNumberIdentifier[0] === "Group1" ? 1 : 2;
    var unitHitPointFieldId = "UnitDescription"+groupNumber+"_HitPoint";
    var unitHitpointString = document.getElementById(unitHitPointFieldId).value;
        
    if (!isInt(unitHitpointString))
        return;
    
    var groupHitpoint = parseInt(groupHitpointString);
    var unitHitpoint = parseInt(unitHitpointString);
    var unitCount = groupHitpoint / unitHitpoint;
    
    if (unitCount !== 0)
        document.getElementById(groupNumberIdentifier[0]+"_UnitCount").value = unitCount;
}

function isInt(value) {
    return !isNaN(value) &&
        parseInt(Number(value)) == value &&
        !isNaN(parseInt(value, 10));
}