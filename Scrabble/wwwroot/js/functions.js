$(document).ready(function () {

    $('#wordInput').on("cut copy paste", function (e) {
        e.preventDefault();
    });

    var originalRack = $("#rack").text();
    var activeTile = null;
    var inputLetters = "";
    $('#wordInput').on('keydown', function (e) {
        var originalRackTemp = originalRack;
        $('#inputErrors').text("");
        if (activeTile == null) {
            $('#inputErrors').text("Please select tile");
            e.preventDefault();
            return;
        }
        if (e.keyCode == 8) {
            $(this).val("");
            $("#rack").text(originalRack);
            return;
        }
        if (e.keyCode < 65 || e.keyCode > 90) {
            $('#inputErrors').text("Please use only letters");
            e.preventDefault();
            return;
        }
        var rack = $("#rack").text();
        inputLetters = $(this).val() + String.fromCharCode(e.which);
        inputLetters = inputLetters.toUpperCase();
        for (var i = 0; i < inputLetters.length; i++) {
            if (originalRackTemp.includes(inputLetters[i])) {
                originalRackTemp = originalRackTemp.replace(inputLetters[i], "_");
            } else if (e.keyCode != 8 && e.keyCode != 32) {
                $('#inputErrors').text("Original rack does not contain input letter.");
                e.preventDefault();
                return;
            }
        }
        $("#rack").text(originalRackTemp);
        /*var inputLetter = inputLetters[inputLetters.length - 1];
        if (inputLetter != undefined) {
            inputLetter = inputLetter.toUpperCase();
        } else {
            inputLetter = " ";
        }*/

        /*
         Change board updates to completely refresh on text input, click on active tile to change direction
         
         
         
         */
        activeTile.text(inputLetter);
        activeTile = activeTile.next("div .grid-item");
        activeTile.trigger("click");
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