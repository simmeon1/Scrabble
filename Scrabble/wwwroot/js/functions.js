$(document).ready(function () {
    var originalRack = $("#rack").text();
    var activeTile = null;
    $('#wordInput').on('input', function (e) {
        var inputLetter = $(this).val();
        inputLetter = inputLetter[inputLetter.length - 1];
        if (inputLetter != undefined) {
            inputLetter = inputLetter.toUpperCase();
        } else {
            inputLetter = " ";
        }

        /*
         Change board updates to completely refresh on text input, click on active tile to change direction
         
         
         
         */
        activeTile.text(inputLetter);
        activeTile = activeTile.next("div .grid-item");
        activeTile.trigger("click");
        var rack  = $("#rack").text();
        if (rack.includes(inputLetter)) {
            $("#rack").text(rack.replace(inputLetter, "_"));
        }
        $(this).val($(this).val().toUpperCase());
        var wordInput = $("#wordInput").val();
        for (var i = 0; i < originalRack.length; i++) {
            if (rack[i] == "_" && !wordInput.includes(originalRack[i])) {
                $("#rack").text(rack.replace("_", originalRack[i]));
                rack = $("#rack").text();
            }
        }
    });

    $('.grid-item').on('click', function (e) {
        activeTile = $(this);
        $(".grid-item").removeClass("currently-selected-tile");
        $(".grid-item").removeClass("currently-foreseen-tile");
        $(this).toggleClass("currently-selected-tile");
        $(this).next("div .grid-item").toggleClass("currently-foreseen-tile");
    });
});