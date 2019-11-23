// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




$(document).ready(function () {
    $('#UserList').DataTable({
        "lengthMenu": [[15, 25, 50, 100, -1], [15, 25, 50, 100, "All"]]
    });
    $('#anime-list').DataTable({
        "order": [[3, "desc"]],
        "lengthMenu": [[15, 25, 50, 100, -1], [15, 25, 50, 100, "All"]]
    });
    $(function () {
        $('[data-toggle="popover"]').popover()
    });
    $('#logindropdown > .dropdown-header').click(function (e) {
        e.stopPropagation();
    });
    

    var prevScrollpos = window.pageYOffset;
    var topamount = 0;

    window.onscroll = function () {
        var currentScrollPos = window.pageYOffset;

        if (prevScrollpos > currentScrollPos) {
            topamount = 0;
            $('#navbar').css('top', 0 + 'px');

            //document.getElementById("navbar").style.top = "0";
        }
        else {
            topamount -= 5;
            $('#navbar').css('top', clamp(topamount, -100, 0) + 'px');
            //document.getElementById("navbar").style.top = document.getElementById("navbar").style.top "-100px";
        }

        prevScrollpos = currentScrollPos;
    }



});


function clamp(num, min, max) {
    return num <= min ? min : num >= max ? max : num;
}

