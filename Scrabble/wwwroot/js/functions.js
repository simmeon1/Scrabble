$(document).ready(function () {

    $('#wordInput').on("cut copy paste", function (e) {
        e.preventDefault();
    });

    var originalRack = $("#rack").text();
    var originalActiveTile = null;
    var activeTile = null;
    var inputLetters = "";
    var wordLimit = 0;
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
            originalActiveTile.trigger("click");
            return;
        }

        if (wordLimit < 0) {
            $('#inputErrors').text("Cannot put more characters.");
            e.preventDefault();
            return;
        }

        if (e.keyCode < 65 || e.keyCode > 90) {
            $('#inputErrors').text("Please use only letters");
            e.preventDefault();
            return;
        }
        var inputLetter = String.fromCharCode(e.which);
        var rack = $("#rack").text();
        inputLetters = $(this).val() + inputLetter;
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
         
        activeTile.text(inputLetter);
        activeTile.toggleClass("filled");
        wordLimit = wordLimit - 1;
        activeTile = activeTile.next("div .grid-item");
        activeTile.trigger("selectNextTile");
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

    $('.grid-item').on('selectNextTile', function (e) {
        activeTile = $(this);       
        $(".grid-item").removeClass("currently-selected-tile");
        $(".grid-item").removeClass("currently-foreseen-tile");
        $(this).toggleClass("currently-selected-tile");
        $(this).next("div .grid-item").toggleClass("currently-foreseen-tile");
    });

    $('.grid-item').on('click', function (e) {
        originalActiveTile = $(this);
        activeTile = $(this);
        wordLimit = $(this).nextAll().length;
        $('#wordInput').val("");
        $("#rack").text(originalRack);
        $(".grid-item").html('&nbsp;&nbsp;');
        $(".grid-item").removeClass("currently-selected-tile");
        $(".grid-item").removeClass("currently-foreseen-tile");
        $(".grid-item").removeClass("filled");
        $(this).toggleClass("currently-selected-tile");
        $(this).next("div .grid-item").toggleClass("currently-foreseen-tile");
    });

});