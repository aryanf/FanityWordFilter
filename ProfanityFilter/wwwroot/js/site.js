// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    const fileSelector = document.getElementById('file-selector');
    const reader = new FileReader();

    fileSelector.addEventListener('change', (event) => {
        var file = event.target.files[0];
        reader.readAsText(file);
    });

    function callApiUpdateDom($requestType, $requestUrl, $requestData, $domIdToUpdate) {
        $.ajax({
            type: $requestType,
            url: $requestUrl,
            crossDomain: true,
            dataType: "text",
            data: $requestData,
            success: function (data) {
                $($domIdToUpdate).html(
                    '<div class="alert alert-success"><i class="fa fa-thumbs-up"><div id="result"></div></i></div>'
                );
                $.each(JSON.parse(data), function (k, v) {
                    $($domIdToUpdate + " #result").append('<li>' + k + ' : ' + v + '</li>');
                });
            },
            error: function (data) {
                $($domIdToUpdate).html(
                    '<div class="alert alert-danger"><i class="fa fa-exclamation-triangle"></i> There is some thing wrong.</div><div class="alert alert-danger"><i class="fa fa-exclamation-triangle"></i><div id="result"></div></div>'
                );
                $.each(JSON.parse(data.responseText), function (k, v) {
                    $($domIdToUpdate + " #result").append('<li>' + k + ' : ' + v + '</li>');
                });
            }
        });
    }

    $("#uploadBtn").click(function () {
        callApiUpdateDom("POST", "http://localhost:55418/api/profanityfilter", reader.result, "#msg1");
    });

    $("#listBtn").click(function () {
        callApiUpdateDom("GET", "http://localhost:55418/api/profanityfilter/listwords", null, "#msg2");
    });

    $("#addBtn").click(function () {
        callApiUpdateDom("POST", "http://localhost:55418/api/profanityfilter/addword", $("#word").val(), "#msg2");
    });

    $("#removeBtn").click(function () {
        callApiUpdateDom("POST", "http://localhost:55418/api/profanityfilter/removeword", $("#word").val(), "#msg2");
    });


});