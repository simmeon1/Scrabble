$(document).ready(function () {

    var activeTile = "";
    var foreseenTile = "";
    var directionOfPlay = "right";
    var anchorsShown = false;
    var anchorUsed = false;

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
                activeTile.html("<button id=board_" + this.id + " class='btn btn-secondary board_rack_chartile'><span class='board_rack_chartile_letter'>" + $(this).find('.rack_charTile_letter:first').text() + "</span><span class='board_rack_chartile_score small'>" + $(this).find('.rack_charTile_score:first').text() + "</span></button>");
                toggleRackCharTileSelection($(this));
            }
        } else {
            var parentId = $("#board_" + this.id).parent().attr("id");
            $("#board_" + this.id).remove();
            $("#" + parentId).html('<span id="board_chartile_0"><br/></span >');
            toggleRackCharTileSelection($(this));
        }
    });

    $(document).on("click", "#clearPlacements", function () {
        $('*[id*=board_rack_chartile]:visible').each(function () {
            var rackCharTileId = $(this).attr("id").replace("board_", "");
            var parentId = $(this).parent().attr("id");
            $("#board_" + this.id).remove();
            $("#" + parentId).html('<span id="board_chartile_0"><br/></span >');
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
        $(".grid-item").removeClass("currently-foreseen-tile");
        var tileCoordinates = activeTile.attr("id").split("_");
        var x = parseInt(tileCoordinates[1]);
        var y = parseInt(tileCoordinates[2]);
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
        foreseenTile.toggleClass("currently-foreseen-tile");
    });

    $(document).on("click", "#submit", function () {       
       
        showAnchors(true);
        var anchoredTilesCounter = 0;
        $('*[class*=anchor]:visible').each(function () {
            if ($(this).has('.board_rack_chartile').length == 1) {
                anchorUsed = true;
                anchoredTilesCounter++;
            }                
        });

        showAnchors(false);
        if (anchoredTilesCounter == 0) {
            anchorUsed = false;
        }

        var startTile = $('div[class*="Start"]');

        if (!anchorUsed && startTile.hasClass("locked")) {
                updateStatusMessage("Anchor not used.", "danger");
                return;
        }

        var rowsUsed = [];
        var columnsUsed = [];
        $('*[id*=board_rack_chartile]:visible').each(function () {
            var tileCoordinates = $(this).parent().closest('div').attr('id').split("_");
            var rowIndex = parseInt(tileCoordinates[1]); //13
            var columnIndex = parseInt(tileCoordinates[2]); //5
            if (!columnsUsed.includes(columnIndex)) {
                columnsUsed.push(columnIndex);
            }
            if (!rowsUsed.includes(rowIndex)) {
                rowsUsed.push(rowIndex);
            }
            var charTilesDetails = $(this).attr("id").split("_");
            var charTileId = charTilesDetails[4];
        });

        if (!((columnsUsed.length == 1 && rowsUsed.length >= 1) || (columnsUsed.length >= 1 && rowsUsed.length == 1))) {
            updateStatusMessage("Please use only one row or column.", "danger");
            return;
        } else {
            var indexesOfPlay = columnsUsed.length > rowsUsed.length ? columnsUsed : rowsUsed;
            var secondaryIndexOfPlay = columnsUsed.length > rowsUsed.length ? rowsUsed : columnsUsed;
            var typeOfPlay = columnsUsed.length > rowsUsed.length ? "horizontal" : "vertical";
            var tilesBetweenFirstAndLastTiles = (indexesOfPlay[indexesOfPlay.length - 1] - indexesOfPlay[0] + 1);
            for (var i = indexesOfPlay[0]; i < indexesOfPlay[indexesOfPlay.length - 1]; i++) {
                var checkedTile = typeOfPlay == "horizontal" ? $("#tile_" + secondaryIndexOfPlay[0] + "_" + i) : $("#tile_" + i + "_" + secondaryIndexOfPlay[0]);
                if (!checkedTile.hasClass("locked") && !checkedTile.children().first().hasClass("board_rack_chartile")) {
                    updateStatusMessage("Play is not connected.", "danger");
                    return;
                }
           }           
        }

        var rowsCount = $("#board").children().length;
        var columnsCount = $("#board").children().first().children().length;
        var boardArray = Array.from({ length: rowsCount }, () =>
            Array.from({ length: columnsCount }, () => false)
        );
        for (var i = 0; i < rowsCount; i++) {
            for (var j = 0; j < columnsCount; j++) {
                var textInTile = $("#board").children().eq(i).children().eq(j).children().first();
                if (textInTile.attr("id").includes("board_chartile_0")) {
                    boardArray[i][j] = "";
                } else {
                    var tileDetails = textInTile.attr("id").split("_");
                    var boardTileDetails = textInTile.parent().attr("id").split("_");
                    var boardTileRow = boardTileDetails[1];
                    var boardTileColumn = boardTileDetails[2];
                    boardArray[i][j] = boardTileRow + "_" + boardTileColumn;
                    if (tileDetails.includes("rack")) {
                        var charTileId = tileDetails[4];
                        boardArray[i][j] += "_" + charTileId + "_rack";
                    } else {
                        var charTileId = tileDetails[2];
                        boardArray[i][j] += "_" + charTileId;
                    }
                }
            }
        }

        var detectedWord = [];
        var totalMovesMade = 0;
        var listOfWordsOnBoard = [];
        var tilesNotInHorizontalPlay = [];
        var tilesNotInVerticalPlay = [];
        checkForWordsAndPlays(boardArray, true, tilesNotInHorizontalPlay, totalMovesMade, listOfWordsOnBoard);
        checkForWordsAndPlays(boardArray, false, tilesNotInVerticalPlay, totalMovesMade, listOfWordsOnBoard);


        function checkForWordsAndPlays(boardArray, isHorizontal, tilesNotInPlay, movesMade, listOfWords) {
            var boardArrayCopy = boardArray.slice(0);
            var movesMadeTemp = movesMade;
            if (!isHorizontal) {
                boardArrayCopy = rotateArrayCounterClockwise(boardArrayCopy);
            }
            for (var i = 0; i < boardArrayCopy.length; i++) {
                detectedWord = [];
                for (var j = 0; j < boardArrayCopy[i].length; j++) {
                    while (boardArrayCopy[i][j] != null && boardArrayCopy[i][j] != "") {
                        detectedWord.push(boardArrayCopy[i][j]);
                        j++;
                        continue;
                    } if (detectedWord.length == 1) {
                        tilesNotInPlay.push(detectedWord);
                    } else {
                        if (detectedWord.length > 1) {
                            listOfWords.push(detectedWord);
                        }
                    }
                    detectedWord = [];
                }
            }
            totalMovesMade = movesMadeTemp;
        }

        function rotateArrayCounterClockwise(array) {
            var rotatedArray = [];
            for (var i = 0; i < array[0].length; i++) {
                var boardColumnAsARow = [];
                for (var j = 0; j < array.length; j++) {
                    boardColumnAsARow.push(array[j][i]);
                }
                rotatedArray.push(boardColumnAsARow);
            }
            rotatedArray.reverse();
            return rotatedArray;
        }

        if ((columnsUsed.length == 1 && rowsUsed.length >= 1) || (columnsUsed.length >= 1 && rowsUsed.length == 1)) {
        } else {
            updateStatusMessage("Please use only one row or column.", "danger");
            return;
        }

        var listOfWordsMadeNow = [];
        for (var i = 0; i < listOfWordsOnBoard.length; i++) {
            for (var j = 0; j < listOfWordsOnBoard[i].length; j++) {
                if (listOfWordsOnBoard[i][j].includes("_rack")) {
                    listOfWordsMadeNow.push(listOfWordsOnBoard[i]);
                    break;
                }
            }
        }

        if (listOfWordsMadeNow.length == 0) {
            updateStatusMessage("You have not made any words.", "danger");
            return;
        }

        for (var i = 0; i < tilesNotInHorizontalPlay.length; i++) {
            for (var j = 0; j < tilesNotInVerticalPlay.length; j++) {
                if (tilesNotInVerticalPlay[j][0].includes(tilesNotInHorizontalPlay[i][0])) {
                    updateStatusMessage("Tile " + tilesNotInHorizontalPlay[i] + " is not in a valid play.", "danger");
                    return;
                }
            }
        }
       
        if (startTile.length == 1 && !startTile.hasClass("locked")) {
            var startTileDetails = $("body").find(".Start").first().attr("id").split("_");
            var startTileX = parseInt(startTileDetails[1]);
            var startTileY = parseInt(startTileDetails[2]);
            if (!(listOfWordsMadeNow[0][0].startsWith(startTileX + "_" + startTileY)) && (listOfWordsMadeNow[0][0].endsWith("_rack"))) {
                updateStatusMessage("Invalid starting move.", "danger");
                return;
            }
        }

        var data = {
            "playerWords": listOfWordsMadeNow
        };
        updateStatusMessage("Loading...", "info");
        $.ajax({
            url: '/Scrabble/Index',
            async: true,
            type: "POST",
            data: data,
        }).done(function (view) {
            var viewBody = view.substring(
                view.lastIndexOf("<body>"),
                view.lastIndexOf("</body>")
            );
            animateHtmlUpdates($("body"), viewBody);
            $("#output").height($("#board").height());
            anchorUsed = false;
            updateStatusMessage("Success :)", "success");
        }).fail(function (jqXHR) {
            updateStatusMessage(jqXHR.responseText, "danger");
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
        });
    }

    function animateHtmlUpdates(jqueryObject, message) {
        jqueryObject.fadeOut(200, function () {
            jqueryObject.html(message).fadeIn(200);
            
        });
        $("#output").height($("#board").height());
    }

    function toggleRackCharTileSelection(tile) {
        tile.toggleClass("btn-default");
        tile.toggleClass("btn-secondary");
        anchorUsed = false;
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