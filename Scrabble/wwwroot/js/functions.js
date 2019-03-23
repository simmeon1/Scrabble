$(document).ready(function () {

    /*$('#wordInput').on("cut copy paste", function (e) {
        e.preventDefault();
    });*/

    var activeTile = "";
    var foreseenTile = "";
    var directionOfPlay = "right";

    $(document).on("click", ".rack_chartile", function () {
        if (!$(this).hasClass("btn-secondary")) {
            if (activeTile == "") {
                alert("Please select a tile");
            } else {
                if (activeTile.html().includes("button")) {
                    var rack_chartile = activeTile.children(":first").attr("id");
                    rack_chartile = rack_chartile.replace("board_", "");
                    $("#" + rack_chartile).toggleClass("btn-default");
                    $("#" + rack_chartile).toggleClass("btn-secondary");
                }
                activeTile.html("<button id=board_" + this.id + " class='btn btn-warning board_rack_chartile'><span class='board_rack_chartile_letter'>" + $(this).find('.rack_charTile_letter:first').text() + "</span><span class='board_rack_chartile_score small'>" + $(this).find('.rack_charTile_score:first').text() + "</span></button>");
                $(this).toggleClass("btn-default");
                $(this).toggleClass("btn-secondary");
            }
        } else {
            var parentId = $("#board_" + this.id).parent().attr("id");
            $("#board_" + this.id).remove();
            $("#" + parentId).html("&nbsp; &nbsp;");
            $(this).toggleClass("btn-secondary");
            $(this).toggleClass("btn-default");
        }
    });

    $(document).on("click", ".changeDirection", function () {
        var direction = $(this).children(":first").attr("class");
        switch (direction) {
            case "glyphicon glyphicon-arrow-right":
                $(this).children(":first").removeClass();
                $(this).children(":first").addClass("glyphicon glyphicon-arrow-up");
                directionOfPlay = "up";
                break;
            case "glyphicon glyphicon-arrow-up":
                $(this).children(":first").removeClass();
                $(this).children(":first").addClass("glyphicon glyphicon-arrow-left");
                directionOfPlay = "left";
                break;
            case "glyphicon glyphicon-arrow-left":
                $(this).children(":first").removeClass();
                $(this).children(":first").addClass("glyphicon glyphicon-arrow-down");
                directionOfPlay = "down";
                break;
            case "glyphicon glyphicon-arrow-down":
                $(this).children(":first").removeClass();
                $(this).children(":first").addClass("glyphicon glyphicon-arrow-right");
                directionOfPlay = "right";
                break;
        }
        $(activeTile).trigger("click");
    });

    $(document).on("click", ".grid-item", function () {
        if ($(this).hasClass("locked")) {
            return;
        }
        activeTile = $(this);
        $(".grid-item").removeClass("currently-selected-tile");
        $(this).toggleClass("currently-selected-tile");
        //console.log(activeTile.attr("id"));
        $(".grid-item").removeClass("currently-foreseen-tile");
        var tileCoordinates = activeTile.attr("id").split("_");
        var x = parseInt(tileCoordinates[1]);
        var y = parseInt(tileCoordinates[2]);
        //console.log(tileCoordinates);
        switch (directionOfPlay) {
            case "up":
                foreseenTile = $('#tile_' + (x - 1) + "_" + y);
                break;
            case "left":
                foreseenTile = $('#tile_' + x + "_" + (y - 1));
                break;
            case "down":
                foreseenTile = $('#tile_' + (x + 1) + "_" + y);
                break;
            case "right":
                foreseenTile = $('#tile_' + x + "_" + (y + 1));
                break;
        }
        //console.log(directionOfPlay);
        //console.log(foreseenTile.attr("id"));
        foreseenTile.toggleClass("currently-foreseen-tile");
        //alert(activeTile);
        //if (activeTile.html().length == 1) {
        //    availableBoardTile = availableBoardTile + activeTile.html();
        //}
        /*console.log("Used tiles: " + usedBoardTiles);
        $(".grid-item").removeClass("currently-selected-tile");
        $(".grid-item").removeClass("currently-foreseen-tile");
        $(this).toggleClass("currently-selected-tile");
        if (typeOfPlay == "horizontalPlay") {
            $(this).next("div .grid-item").toggleClass("currently-foreseen-tile");
        } else {
            var index = $(this).index();
            $(this).parent().next("div").children("div .grid-item").eq(index).toggleClass("currently-foreseen-tile");
        }*/
    });

    $(document).on("click", "#submit", function () {
        var startTile = $('div[class*="Start"]');
        if (startTile.length == 1) {
            if (!startTile.hasClass("locked")) {
                var startTileCoordinates = $("body").find(".Start").first().attr("id").split("_");
                var startTileX = parseInt(startTileCoordinates[1]);
                var startTileY = parseInt(startTileCoordinates[2]);
                if (startTile.has('.board_rack_chartile').length == 0 || $("#tile_" + (startTileX - 1) + "_" + startTileY).has('.board_rack_chartile').length == 1 || $("#tile_" + startTileX + "_" + (startTileY - 1)).has('.board_rack_chartile').length == 1) {
                    console.log("#tile_" + (startTileX - 1) + "_" + startTileY);
                    console.log("#tile_" + startTileX + "_" + (startTileY - 1));
                    $("#statusMessage").html("Invalid starting move.");
                    return;
                }
            } else {
                startTile.removeClass("Start");
            }
        }

        var rowsUsed = [];
        var columnsUsed = [];
        var submission = [];
        $('*[id*=board_rack_chartile]:visible').each(function () {
            var tileCoordinates = $(this).parent().closest('div').attr('id').split("_");
            var tileX = parseInt(tileCoordinates[1]);
            var tileY = parseInt(tileCoordinates[2]);
            if (!columnsUsed.includes(tileX)) {
                columnsUsed.push(tileX);
            }
            if (!rowsUsed.includes(tileY)) {
                rowsUsed.push(tileY);
            }
            var charTilesDetails = $(this).attr("id").split("_");
            var charTileId = charTilesDetails[4];
            //console.log($(this).attr("id"));
            //console.log($(this).parent().closest('div').attr('id'));
            submission.push(tileX + "_" + tileY + "_" + charTileId);
        });
        //console.log(columnsUsed.length);
        //console.log(rowsUsed.length);

        if ((columnsUsed.length == 1 && rowsUsed.length >= 1) || (columnsUsed.length >= 1 && rowsUsed.length == 1)) {
            //console.log(submission);
        } else {
            $("#statusMessage").html("Please use only one row or column.");
            return;
        }

        var filledTilesCoordinates = columnsUsed.length > 1 ? columnsUsed : rowsUsed;
        var typeOfPlay = columnsUsed.length > 1 ? "vertical" : "horizontal";
        var secondaryCoordinate = columnsUsed.length == 1 ? columnsUsed[0] : rowsUsed[0];
        var playIsConnected = true;
        filledTilesCoordinates.sort();
        var startOfPlay = filledTilesCoordinates[0];
        var endOfPlay = filledTilesCoordinates[filledTilesCoordinates.length - 1];
        for (var i = startOfPlay; i <= endOfPlay; i++) {
            var tile = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + i) : $("#tile_" + i + "_" + secondaryCoordinate);
            if (!tile.has('.board_rack_chartile').length == 1) {
                if (!tile.hasClass("locked")) {
                    playIsConnected = false;
                    break;
                }
            }
        }

        if (!playIsConnected) {
            $("#statusMessage").html("Play is not connected.");
            return;
        }

        var anchorUsed = false;
        $("#showAnchors").trigger("click");
        $('*[class*=anchor]:visible').each(function () {
            if ($(this).has('.board_rack_chartile').length == 1) {
                anchorUsed = true;
            }
        });

        if (!anchorUsed) {
            if (!startTile.hasClass("Start")) {
                $("#statusMessage").html("Anchor not used.");
                return;
            }
        }

        //var filledTiles = $(".grid-item").has('.board_rack_chartile');
        //var filledXs = [];
        //var filledYs = [];
        //filledTiles.each(function () {
        //    var tileCoordinates = $(this).attr('id').split("_");
        //    var tileX = parseInt(tileCoordinates[1]);
        //    var tileY = parseInt(tileCoordinates[2]);
        //    filledXs.push(tileX);
        //    filledYs.push(tileY);
        //});




        var data = {
            "playedTiles": submission
        };

        //var data = {
        //    "ID": 2,
        //    "GameID": 1
        //};

        $.ajax({
            url: '/Scrabble/Index',
            //contentType: "application/json",
            async: true,
            type: "POST",
            data: data,
        }).done(function (view) {
            //xxx;
            var viewBody = view.substring(
                view.lastIndexOf("<body>"),
                view.lastIndexOf("</body>")
            );
            $("#statusMessage").html("Success");
            $("body").html(viewBody);
            $("body").removeClass("Start");
            //console.log(view);
        }).fail(function (jqXHR, textStatus) {
            // xxx;
        });
    });

    $(document).on("click", "#showAnchors", function () {
        $('*[id*=tile_]:visible').each(function () {
            if ($(this).hasClass("locked")) {
                var tileCoordinates = $(this).attr('id').split("_");
                var tileX = parseInt(tileCoordinates[1]);
                var tileY = parseInt(tileCoordinates[2]);
                var tileOnLeft = ($("#tile_" + (tileX - 1) + "_" + tileY));
                var tileOnTop = ($("#tile_" + tileX + "_" + (tileY - 1)));
                var tileOnRight = ($("#tile_" + (tileX + 1) + "_" + tileY));
                var tileOnBottom = ($("#tile_" + tileX + "_" + (tileY + 1)));
                if (tileOnLeft != null && !(tileOnLeft.hasClass("locked"))) {
                    if (!tileOnLeft.hasClass("anchor")) { tileOnLeft.toggleClass("anchor"); }
                }
                if (tileOnTop != null && !(tileOnTop.hasClass("locked"))) {
                    if (!tileOnTop.hasClass("anchor")) { tileOnTop.toggleClass("anchor"); }
                }
                if (tileOnRight != null && !(tileOnRight.hasClass("locked"))) {
                    if (!tileOnRight.hasClass("anchor")) { tileOnRight.toggleClass("anchor"); }
                }
                if (tileOnBottom != null && !(tileOnBottom.hasClass("locked"))) {
                    if (!tileOnBottom.hasClass("anchor")) { tileOnBottom.toggleClass("anchor"); }
                }
            }
        });
    });

    //var originalRack = $("#rack").text();
    //var originalActiveTile = null;
    //var availableBoardTile = "";
    //var usedBoardTiles = "";
    //var activeTile = null;
    //var inputLetters = "";
    //var wordLimit = 0;
    //var typeOfPlay = "horizontalPlay";

    //$('.rack_chartile').on('keydown', function (e) {
    //    var originalRackTemp = originalRack;
    //    var usedBoardTilesTemp = usedBoardTiles;
    //    $('#inputErrors').text("");

    //$('#wordInput').on('keydown', function (e) {
    //    var originalRackTemp = originalRack;
    //    var usedBoardTilesTemp = usedBoardTiles;
    //    $('#inputErrors').text("");

    //    if (activeTile == null) {
    //        $('#inputErrors').text("Please select tile");
    //        e.preventDefault();
    //        return;
    //    }

    //    if (e.keyCode == 8) {
    //        $(this).val("");
    //        $("#rack").text(originalRack);
    //        originalActiveTile.trigger("click", [typeOfPlay]);
    //        return;
    //    }

    //    if (wordLimit < 0) {
    //        $('#inputErrors').text("Cannot put more characters.");
    //        e.preventDefault();
    //        return;
    //    }

    //    if (e.keyCode < 65 || e.keyCode > 90) {
    //        $('#inputErrors').text("Please use only letters");
    //        e.preventDefault();
    //        return;
    //    }

    //    var inputLetter = String.fromCharCode(e.which);

    //    if (activeTile.html().length == 1 && inputLetter != activeTile.html()) {
    //        $('#inputErrors').text("You must the letter from the selected tile as input");
    //        e.preventDefault();
    //        return;
    //    }

    //    var rack = $("#rack").text();
    //    inputLetters = $(this).val() + inputLetter;
    //    inputLetters = inputLetters.toUpperCase();
    //    for (var i = 0; i < inputLetters.length; i++) {
    //        if (availableBoardTile.includes(inputLetters[i])) {
    //            availableBoardTile = availableBoardTile.replace(inputLetters[i], "");
    //            usedBoardTiles = usedBoardTiles + inputLetters[i];          
    //        } else if (usedBoardTilesTemp.includes(inputLetters[i])) {
    //            usedBoardTilesTemp = usedBoardTilesTemp.replace(inputLetters[i], "_");
    //        } else if (originalRackTemp.includes(inputLetters[i])) {
    //            originalRackTemp = originalRackTemp.replace(inputLetters[i], "_");
    //        } else if (e.keyCode != 8 && e.keyCode != 32) {
    //            if (usedBoardTilesTemp.includes(inputLetters[i])) {
    //                usedBoardTilesTemp = usedBoardTilesTemp.replace(inputLetters[i], "");
    //            }
    //            console.log("Used tiles: " + usedBoardTiles);
    //            $('#inputErrors').text("Original rack and used tiles do not contain input letter " + inputLetters[i]);
    //            e.preventDefault();
    //            return;
    //            //}
    //        }
    //    }
    //    $("#rack").text(originalRackTemp);

    //    activeTile.text(inputLetter);
    //    wordLimit = wordLimit - 1;
    //    if (typeOfPlay == "verticalPlay") {
    //        var index = activeTile.index();
    //        activeTile = activeTile.parent().next("div").children("div .grid-item").eq(index);
    //    }
    //    else {
    //        activeTile = activeTile.next("div .grid-item");
    //    }
    //    activeTile.trigger("selectNextTile");
    //    /*if (rack.includes(inputLetter)) {
    //        $("#rack").text(rack.replace(inputLetter, "_"));
    //    }
    //    $(this).val($(this).val().toUpperCase());
    //    var wordInput = $("#wordInput").val();
    //    for (var i = 0; i < originalRack.length; i++) {
    //        if (rack[i] == "_" && !wordInput.includes(originalRack[i])) {
    //            $("#rack").text(rack.replace("_", originalRack[i]));
    //            rack = $("#rack").text();
    //        }
    //    }*/
    //});

    //$('.grid-item').on('selectNextTile', function (e) {
    //    availableBoardTile = "";
    //    activeTile = $(this);
    //    if (activeTile.html().length == 1) {
    //        availableBoardTile = availableBoardTile + activeTile.html();
    //    }
    //    console.log("Used tiles: " + usedBoardTiles);
    //    $(".grid-item").removeClass("currently-selected-tile");
    //    $(".grid-item").removeClass("currently-foreseen-tile");
    //    $(this).toggleClass("currently-selected-tile");
    //    if (typeOfPlay == "horizontalPlay") {
    //        $(this).next("div .grid-item").toggleClass("currently-foreseen-tile");
    //    } else {
    //        var index = $(this).index();
    //        $(this).parent().next("div").children("div .grid-item").eq(index).toggleClass("currently-foreseen-tile");
    //    }
    //});

    //$('.grid-item').on('click', function (e, param) {
    //    usedBoardTiles = "";
    //    $(".grid-item").removeClass("currently-selected-tile");
    //    $(".grid-item").removeClass("currently-foreseen-tile");
    //    if (param != undefined) {
    //        typeOfPlay = param;
    //    }
    //    else if (originalActiveTile != null) {
    //        if ($(this).attr('id') == originalActiveTile.attr('id')) {
    //            if (typeOfPlay == "verticalPlay") {
    //                typeOfPlay = "horizontalPlay";
    //            } else {
    //                typeOfPlay = "verticalPlay";
    //            }
    //        } else {
    //            typeOfPlay = "horizontalPlay";
    //        }
    //    }
    //    originalActiveTile = $(this);
    //    availableBoardTile = "";
    //    activeTile = $(this);
    //    if (activeTile.html().length == 1) {
    //        availableBoardTile = availableBoardTile + activeTile.html();
    //    }

    //    $(this).toggleClass("currently-selected-tile");
    //    if (typeOfPlay == "horizontalPlay") {
    //        wordLimit = $(this).nextAll().length;
    //        $(".grid-item").removeClass("currently-foreseen-tile");
    //        $(this).next("div .grid-item").toggleClass("currently-foreseen-tile");
    //    } else {
    //        wordLimit = $(this).parent().nextAll().length;
    //        $(".grid-item").removeClass("currently-foreseen-tile");
    //        var index = $(this).index();
    //        $(this).parent().next("div").children("div .grid-item").eq(index).toggleClass("currently-foreseen-tile");
    //    }
    //    $('#wordInput').val("");
    //    $("#rack").text(originalRack);
    //    $(".grid-item").not(".locked").html('&nbsp;&nbsp;');
    //    $("#wordInput").focus();
    //    //$(".grid-item").removeClass("filled");              
    //});

});