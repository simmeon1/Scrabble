$(document).ready(function () {

    var activeTile = "";
    var foreseenTile = "";
    var directionOfPlay = "right";
    var anchorsShown = false;
    var anchorUsed = false;
    var movesTable;

    refreshElementSizes();
    $("#validMovesButton").hide();

    $(window).on('resize', function () {
        $(".grid-item").height($(".grid-item").width());
        $(".rack_chartile").height($(".rack_chartile").width());
        $(".board_rack_chartile").height($(".board_rack_chartile").parent().height() - 2);
    });

    // / );
    //$("#statusMessage").hide();

    $(window).resize(function () {
        // This will fire each time the window is resized:
        if ($(window).width() < 768) {
            $("#leftOutput").insertAfter("#board");
        } else if ($(window).width() >= 768 && $(window).width() <= 992) {
            // if smaller
            $("#leftOutput").insertBefore("#board");
        }

    }).resize();

    $(document).on("click", ".grid-item", function () {
        if ($(this).hasClass("locked")) {
            return;
        }
        activeTile = $(this);
        $("*").removeClass("moveMarked");
        $(".grid-item").removeClass("currently-selected-tile");
        $(this).toggleClass("currently-selected-tile");
        $(".grid-item").removeClass("currently-foreseen-tile");
        var tileCoordinates = activeTile.attr("id").split("_");
        var x = parseInt(tileCoordinates[1]);
        var y = parseInt(tileCoordinates[2]);
        //switch (directionOfPlay) {
        //    case "up":
        //        foreseenTile = $('#tile_' + (x - 1) + "_" + y);
        //        break;
        //    case "left":
        //        foreseenTile = $('#tile_' + x + "_" + (y - 1));
        //        break;
        //    case "down":
        //        foreseenTile = $('#tile_' + (x + 1) + "_" + y);
        //        break;
        //    case "right":
        //        foreseenTile = $('#tile_' + x + "_" + (y + 1));
        //        break;
        //}
        //foreseenTile.toggleClass("currently-foreseen-tile");
    });

    $(document).on("click", ".rack_chartile", function () {
        var rack_chartile_details = this.id.split("_");
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
                if (parseInt(rack_chartile_details[3]) == 1) {
                    var blankLetter = prompt("Please enter a single letter for blank tile");
                    while (blankLetter != null && (blankLetter.length != 1 || Number.isInteger(parseInt(blankLetter)))) {
                        blankLetter = prompt("Please enter a single letter for blank tile");
                    }
                    if (blankLetter != null) {
                        blankLetter = blankLetter.toUpperCase();
                        activeTile.html("<button id=board_" + this.id + "_" + blankLetter + " class= 'btn btn-block btn-secondary board_rack_chartile blank'> <span class='board_rack_chartile_letter'>" + blankLetter + "</span> <span class='board_rack_chartile_score'>" + $(this).find('.rack_chartile_score:first').text() + "</span></button > ");
                        toggleRackCharTileSelection($(this));
                    }
                }
                else {
                    activeTile.html("<button id=board_" + this.id + " class='btn btn-block btn-secondary board_rack_chartile'><span class='board_rack_chartile_letter'>" + $(this).find('.rack_chartile_letter:first').text() + "</span><span class='board_rack_chartile_score'>" + $(this).find('.rack_chartile_score:first').text() + "</span></button>");
                    toggleRackCharTileSelection($(this));
                }
            }
        } else {
            var parentId = null;
            if (parseInt(rack_chartile_details[3]) == 1) {
                parentId = $(".blank").first().parent().attr("id");
            } else {
                parentId = $("#board_" + this.id).parent().attr("id");
            }
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
            toggleRackCharTileSelection($(".rack_chartile"));
        });
    });

    $(document).on("click", "#getMoves", function () {
        var boardArray = getBoardArray(false);
        var data = {
            "boardArray": boardArray
        };
        updateStatusMessage("Loading...", "info");
        $('button').prop('disabled', true);
        $.ajax({
            url: '/Scrabble/GetMoves',
            async: true,
            type: "POST",
            data: data,
        }).done(function (results) {
            populateTable(results);
            //$("#getMoves").replaceWith($("#validMovesButton"));
            $("#validMovesButton").show();
            updateStatusMessage("Success :)", "success");
            $('button').prop('disabled', false);
        }).fail(function (jqXHR) {
            populateTable(results);
            updateStatusMessage(jqXHR.responseText, "danger");
            $('button').prop('disabled', false);
        });
    });

    $(document).on("click", "#flipBoard", function () {
        var data = {
            "flipBoard": $("#board").hasClass("flipped") ? false : true
        };
        updateStatusMessage("Loading...", "info");
        $('button').prop('disabled', true);
        $.ajax({
            url: '/Scrabble/FlipBoard',
            async: true,
            type: "POST",
            data: data,
        }).done(function (view) {
            var viewBody = view.substring(
                view.lastIndexOf("<body>"),
                view.lastIndexOf("</body>")
            );
            $('button').prop('disabled', false);
            animateHtmlUpdates($("body"), viewBody);
        }).fail(function (jqXHR) {
            updateStatusMessage(jqXHR.responseText, "danger");
            $('button').prop('disabled', false);
        });
    });

    $(document).on("click", "#redraw", function () {
        updateStatusMessage("Loading...", "info");
        $('button').prop('disabled', true);
        $.ajax({
            url: '/Scrabble/Redraw',
            type: "POST"
        }).done(function (view) {
            var viewBody = view.substring(
                view.lastIndexOf("<body>"),
                view.lastIndexOf("</body>")
            );
            $('button').prop('disabled', false);
            animateHtmlUpdates($("body"), viewBody);
        }).fail(function (jqXHR) {
            updateStatusMessage(jqXHR.responseText, "danger");
            $('button').prop('disabled', false);
        });
    });

    $(document).on("click", "#skip", function () {
        updateStatusMessage("Loading...", "info");
        $('button').prop('disabled', true);
        $.ajax({
            url: '/Scrabble/Skip',
            type: "POST"
        }).done(function (view) {
            var viewBody = view.substring(
                view.lastIndexOf("<body>"),
                view.lastIndexOf("</body>")
            );
            $('button').prop('disabled', false);
            animateHtmlUpdates($("body"), viewBody);
        }).fail(function (jqXHR) {
            updateStatusMessage(jqXHR.responseText, "danger");
            $('button').prop('disabled', false);
        });
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
        }
        var indexesOfPlay = columnsUsed.length > rowsUsed.length ? columnsUsed : rowsUsed;
        var secondaryIndexOfPlay = columnsUsed.length > rowsUsed.length ? rowsUsed : columnsUsed;
        var typeOfPlay = columnsUsed.length > rowsUsed.length ? "normal" : "transposed";
        for (var i = indexesOfPlay[0]; i < indexesOfPlay[indexesOfPlay.length - 1]; i++) {
            var checkedTile = typeOfPlay == "normal" ? $("#tile_" + secondaryIndexOfPlay[0] + "_" + i) : $("#tile_" + i + "_" + secondaryIndexOfPlay[0]);
            if (!checkedTile.hasClass("locked") && !checkedTile.children().first().hasClass("board_rack_chartile")) {
                updateStatusMessage("Play is not connected.", "danger");
                return;

            }
        }

        var rackTilesPlayed = [];
        var boardArray = getBoardArray(true, rackTilesPlayed);

        var totalMovesMade = 0;
        var listOfWordsOnBoard = [];
        var tilesNotInUntransposedPlay = [];
        var tilesNotInTransposedPlay = [];
        checkForWordsAndPlays(boardArray, true, tilesNotInUntransposedPlay, totalMovesMade, listOfWordsOnBoard);
        checkForWordsAndPlays(boardArray, false, tilesNotInTransposedPlay, totalMovesMade, listOfWordsOnBoard);

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

        if (startTile.length == 1 && !startTile.hasClass("locked")) {
            var startTileDetails = $("body").find(".Start").first().attr("id").split("_");
            var startTileX = parseInt(startTileDetails[1]);
            var startTileY = parseInt(startTileDetails[2]);
            if (!(listOfWordsMadeNow[0][0].startsWith(startTileX + "_" + startTileY)) && (listOfWordsMadeNow[0][0].endsWith("_rack"))) {
                updateStatusMessage("Invalid starting move.", "danger");
                return;
            }
        }

        for (var i = 0; i < tilesNotInUntransposedPlay.length; i++) {
            for (var j = 0; j < tilesNotInTransposedPlay.length; j++) {
                if (tilesNotInTransposedPlay[j][0].includes(tilesNotInUntransposedPlay[i][0])) {
                    updateStatusMessage("Tile " + tilesNotInUntransposedPlay[i] + " is not in a valid play.", "danger");
                    return;
                }
            }
        }

        var data = {
            "playedRackTiles": rackTilesPlayed,
            "playedWords": listOfWordsMadeNow
        };
        updateStatusMessage("Loading...", "info");
        $('.button').prop('disabled', true);
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
            //$("#validMovesButton").replaceWith($("#getMoves"));
            $("#validMovesButton").hide();
            animateHtmlUpdates($("body"), viewBody);
            //$("#output").height($("#board").height());
            anchorUsed = false;
            updateStatusMessage("Success :)", "success");
            $('.button').prop('disabled', false);
        }).fail(function (jqXHR) {
            updateStatusMessage(jqXHR.responseText, "danger");
            $('.button').prop('disabled', false);
        });
    });

    $(document).on("click", "#showAnchors", function () {
        showAnchors(!anchorsShown);
    });

    function updateStatusMessage(message, type) {
        $("#statusMessage").html(`<div class="alert alert-` + type + `">
            <button type="button" class="close" data-dismiss="alert">x</button>` + message + `</div>`);
        if (type == "info") {
            $('#statusMessage').fadeIn(200);
        } else {
            $('#statusMessage').fadeIn(200).delay(500).fadeOut(200);
        }
    }

    function animateHtmlUpdates(jqueryObject, message) {
        jqueryObject.fadeOut(200, function () {
            jqueryObject.html(message).fadeIn(200, function () {
                refreshElementSizes();
                $("#validMovesButton").hide();
            });
        });
        //$("#output").height($("#board").height());
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

    function getBoardArray(includingPlayedRackTiles, rackTilesPlayedList) {
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
                        if (includingPlayedRackTiles && rackTilesPlayedList != null) {
                            var charTileId = tileDetails[4];
                            if (charTileId == 1) {
                                var blankLetter = tileDetails[5];
                                boardArray[i][j] += "_" + charTileId + "_" + blankLetter + "_rack";
                            } else {
                                boardArray[i][j] += "_" + charTileId + "_rack";
                            }
                            rackTilesPlayedList.push(boardArray[i][j]);
                        } else {
                            boardArray[i][j] = "";
                        }
                    } else {
                        var charTileId = tileDetails[2];
                        boardArray[i][j] += "_" + charTileId;
                    }
                }
            }
        }
        return boardArray;
    }

    function transposeArray(array) {
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
        //var w = array.length;
        //var h = array[0].length;

        //var result = [];

        //for (var i = 0; i < w; i++) {
        //    for (var j = 0; j < h; j++) {
        //        result[j, i] = array[i, j];
        //    }
        //}

        //return result;
    }

    function checkForWordsAndPlays(boardArray, isNotTransposed, tilesNotInPlay, movesMade, listOfWords) {
        var boardArrayCopy = boardArray.slice(0);
        var movesMadeTemp = movesMade;
        if (!isNotTransposed) {
            boardArrayCopy = transposeArray(boardArrayCopy);
        }
        for (var i = 0; i < boardArrayCopy.length; i++) {
            var detectedWord = [];
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

    function populateTable(json) {
        var table = `<table id="movesTable" class="table table-striped table-condensed table-hover">
                <thead>
                    <tr>
                        <th>Word</th>
                        <th>Direction</th>
                        <th>Extra Words</th>
                        <th>Row/Column Start</th>
                        <th>Row/Column End</th>
                        <th>Row/Column Index</th>
                        <th>Score</th>
                    </tr>
                </thead >
                <tbody>`
        for (var i = 0; i < json.length; i++) {
            var obj = json[i];
            table += `<tr id="move_` + obj["Word"] + "_" + obj["Start"] + "_" + obj["Anchor"] + "_" + obj["End"] + "_" + obj["Direction"] + `" class="moveRow">
                    <td>`+ obj["Word"] + `</td>
                    <td>`+ obj["Direction"] + `</td>
                    <td>`+ obj["Extra Words"] + `</td>
                <td>`+ obj["Start"] + `</td>
                        <td>`+ obj["End"] + `</td>
                        <td>`+ obj["Anchor"] + `</td>
                    <td>`+ obj["Score"] + `</td>
                </tr>`
        }
        table += `</tbody></table>`
        $(".modal-body").html(table);
        movesTable = $('#movesTable').DataTable();

        $('#movesTable').on('click', 'tr', function () {
            var moveDetails = $(this).attr("id").split("_");
            var start = parseInt(moveDetails[2]);
            var secondaryIndex = parseInt(moveDetails[3]);
            var end = parseInt(moveDetails[4]);
            var direction = moveDetails[5];
            $("*").removeClass("moveMarked");
            for (var i = start; i <= end; i++) {
                direction == "Horizontal" ? $("#tile_" + secondaryIndex + "_" + i).addClass("moveMarked")
                    : $("#tile_" + i + "_" + secondaryIndex).addClass("moveMarked");
            }
            $('#movesModal').modal('toggle');
        });
    }


    $(document).ready(function () {
        $('#movesTable').DataTable();
    });

    function refreshElementSizes() {
        $(".grid-item").height($(".grid-item").width());
        $(".rack_chartile").height($(".rack_chartile").width());
        $(".board_rack_chartile").height($(".board_rack_chartile").parent().height() - 2);
        $(".playerLog").height($("#board").height() / 2);
        $("#rowIndexes").height($("#board").height());
        $(".rowIndex").height($(".grid-item").height() + 1);
    }
});