// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.truncated-description').hover(function () {
        var fullDescription = $(this).data('full-description');
        $('.description-tooltip .full-description').text(fullDescription);
        $('.description-tooltip').show();
    }, function () {
        $('.description-tooltip').hide();
    });
});
