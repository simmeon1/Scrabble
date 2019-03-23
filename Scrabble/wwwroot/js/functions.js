$(document).ready(function () {

    var activeTile = "";
    var foreseenTile = "";
    var directionOfPlay = "right";
    var anchorsShown = false;

    $("#output").height($("#board").height());

    $(document).on("click", ".rack_chartile", function () {
        if (!$(this).hasClass("btn-secondary")) {
            if (activeTile == "") {
                updateStatusMessage("Please select a tile", "danger");
                return;
            } else {
                if (activeTile.html().includes("button")) {
                    var rack_chartile = activeTile.children(":first").attr("id");
                    rack_chartile = rack_chartile.replace("board_", "");
                    toggleRackCharTileSelection($("#" + rack_chartile));
                }
                //animateHtmlUpdates(activeTile, "<button id=board_" + this.id + " class='btn btn-secondary board_rack_chartile'><span class='board_rack_chartile_letter'>" + $(this).find('.rack_charTile_letter:first').text() + "</span><span class='board_rack_chartile_score small'>" + $(this).find('.rack_charTile_score:first').text() + "</span></button>");
                activeTile.html("<button id=board_" + this.id + " class='btn btn-secondary board_rack_chartile'><span class='board_rack_chartile_letter'>" + $(this).find('.rack_charTile_letter:first').text() + "</span><span class='board_rack_chartile_score small'>" + $(this).find('.rack_charTile_score:first').text() + "</span></button>");
                toggleRackCharTileSelection($(this));
            }
        } else {
            var parentId = $("#board_" + this.id).parent().attr("id");
            $("#board_" + this.id).remove();
            //animateHtmlUpdates($("#" + parentId),"&nbsp; &nbsp;");
            $("#" + parentId).html("&nbsp; &nbsp;");
            toggleRackCharTileSelection($(this));
        }
    });

    /*$(document).on("click", ".changeDirection", function () {
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
    });*/

    $(document).on("click", "#clearPlacements", function () {
        $('*[id*=board_rack_chartile]:visible').each(function () {
            var rackCharTileId = $(this).attr("id").replace("board_", "");
            var parentId = $(this).parent().attr("id");
            $("#board_" + this.id).remove();
            //animateHtmlUpdates($("#" + parentId),"&nbsp; &nbsp;");
            $("#" + parentId).html("&nbsp; &nbsp;");
            toggleRackCharTileSelection($("#" + rackCharTileId));
        });
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
    });

    $(document).on("click", "#submit", function () {
        var startTile = $('div[class*="Start"]');
        if (startTile.length == 1) {
            if (!startTile.hasClass("locked")) {
                var startTileDetails = $("body").find(".Start").first().attr("id").split("_");
                var startTileX = parseInt(startTileDetails[1]);
                var startTileY = parseInt(startTileDetails[2]);
                var startTileCharTileId = parseInt(startTileDetails[3]);
                if (startTile.has('.board_rack_chartile').length == 0 || $("#tile_" + (startTileX - 1) + "_" + startTileY + "_" + startTileCharTileId).has('.board_rack_chartile').length == 1 || $("#tile_" + startTileX + "_" + (startTileY - 1) + "_" + startTileCharTileId).has('.board_rack_chartile').length == 1) {
                    console.log("#tile_" + (startTileX - 1) + "_" + startTileY);
                    console.log("#tile_" + startTileX + "_" + (startTileY - 1));
                    updateStatusMessage("Invalid starting move.", "danger");
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
        });
        //console.log(columnsUsed.length);
        //console.log(rowsUsed.length);

        if ((columnsUsed.length == 1 && rowsUsed.length >= 1) || (columnsUsed.length >= 1 && rowsUsed.length == 1)) {
            //console.log(submission);
        } else {
            updateStatusMessage("Please use only one row or column.", "danger");
            return;
        }

        var filledTilesCoordinates = columnsUsed.length > 1 ? columnsUsed : rowsUsed;
        var typeOfPlay = columnsUsed.length > 1 ? "vertical" : "horizontal";
        var secondaryCoordinate = columnsUsed.length == 1 ? columnsUsed[0] : rowsUsed[0];
        var playIsConnected = true;
        filledTilesCoordinates = filledTilesCoordinates.sort(function (a, b) { return a - b });
        var startOfPlay = filledTilesCoordinates[0];
        var endOfPlay = filledTilesCoordinates[filledTilesCoordinates.length - 1];
        var startOfPlayAncestor = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + (startOfPlay - 1)) : $("#tile_" + (startOfPlay - 1) + "_" + secondaryCoordinate);
        while (startOfPlayAncestor.hasClass("locked")) {
            startOfPlay = startOfPlay - 1;
            startOfPlayAncestor = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + (startOfPlay - 1)) : $("#tile_" + (startOfPlay - 1) + "_" + secondaryCoordinate);
        }
        var endOfPlaySuccessor = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + (endOfPlay + 1)) : $("#tile_" + (endOfPlay + 1) + "_" + secondaryCoordinate);
        while (endOfPlaySuccessor.hasClass("locked")) {
            endOfPlay = endOfPlay + 1;
            endOfPlaySuccessor = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + (endOfPlay + 1)) : $("#tile_" + (endOfPlay + 1) + "_" + secondaryCoordinate);
        }
        for (var i = startOfPlay; i <= endOfPlay; i++) {
            //var tile = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + i) : $("#tile_" + i + "_" + secondaryCoordinate);
            var tile = typeOfPlay == "horizontal" ? $("#tile_" + secondaryCoordinate + "_" + i) : $("#tile_" + i + "_" + secondaryCoordinate);
            console.log(tile.attr("id"));
            if (!tile.has('.board_rack_chartile').length == 1) {
                if (!tile.hasClass("locked")) {
                    playIsConnected = false;
                    break;
                    //} else {
                    //    typeOfPlay == "horizontal" ? submission.push(secondaryCoordinate + "_" + i + "_" + charTileId) : submission.push(i + "_" + secondaryCoordinate + "_" + charTileId);
                    //}
                }
            }
            //var boardTileDetails = tile.attr("id").split("_");
            //var boardTileX = boardTileDetails[1];
            //var boardTileY = boardTileDetails[2];
            var boardCharTileId = "";
            var boardCharTile = tile.children().first();
            var boardCharTileDetails = boardCharTile.attr("id").split("_");
            if (boardCharTile.is("button")) {
                boardCharTileId = boardCharTileDetails[4];
            } else {
                boardCharTileId = boardCharTileDetails[2];
            }
            typeOfPlay == "horizontal" ? submission.push(secondaryCoordinate + "_" + i + "_" + boardCharTileId) : submission.push(i + "_" + secondaryCoordinate + "_" + boardCharTileId);
        }

        if (!playIsConnected) {
            updateStatusMessage("Play is not connected.", "danger");
            return;
        }

        var anchorUsed = false;
        showAnchors(true);
        $('*[class*=anchor]:visible').each(function () {
            if ($(this).has('.board_rack_chartile').length == 1) {
                anchorUsed = true;
            }
            showAnchors(false);
        });

        if (!anchorUsed) {
            if (!startTile.hasClass("Start")) {
                updateStatusMessage("Anchor not used.", "danger");
                //if (anchorsShown) {
                //    $("#showAnchors").trigger("click");
                //}
                return;
            }
        }

        var data = {
            "playedTiles": submission
        };
        updateStatusMessage("Loading...", "info");
        $.ajax({
            url: '/Scrabble/Index',
            async: true,
            type: "POST",
            data: data,
        }).done(function (view) {
            //xxx;
            var viewBody = view.substring(
                view.lastIndexOf("<body>"),
                view.lastIndexOf("</body>")
            );
            animateHtmlUpdates($("body"), viewBody);
            //$("body").html(viewBody);
            $("body").removeClass("Start");
            $("#output").height($("#board").height());
            //console.log(view);
        }).fail(function (jqXHR, textStatus) {
            // xxx;
        });
    });

    $(document).on("click", "#showAnchors", function () {
        showAnchors(!anchorsShown);
    });

    function updateStatusMessage(message, type) {
        $('#statusMessage').fadeOut(200, function () {
            $('#output').removeClass();
            $('#output').addClass("panel panel-" + type);
            $("#statusMessage").html("<span>" + message + "</span>");
            $('#statusMessage').fadeIn(200);
            //$('#output').html("<span>" + message + "</span>").fadeIn(200);
        });
    }

    function animateHtmlUpdates(jqueryObject, message) {
        jqueryObject.fadeOut(200, function () {
            jqueryObject.html(message).fadeIn(200);
            //$("#output").height($("#board").height());
        });
    }

    function toggleRackCharTileSelection(tile) {
        tile.toggleClass("btn-default");
        tile.toggleClass("btn-secondary");
    }

    function showAnchors(yesOrNo) {
        if (yesOrNo) {
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
            anchorsShown = true;
        } else {
            $("*").removeClass("anchor");
            anchorsShown = false;
        }
    }
});