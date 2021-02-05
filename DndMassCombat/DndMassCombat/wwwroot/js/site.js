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
