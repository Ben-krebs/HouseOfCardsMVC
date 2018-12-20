//// SignalR Setup
var hub = null;
$(function () {
    // Reference the auto-generated proxy for the hub.  
    hub = $.connection.gameHub;
    // Create a function that the hub can call back to display messages.
    //chat.client.addNewMessageToPage = function (name, message) {
    //    // Add the message to the page. 
    //    $('#discussion').append('<li><strong>' + htmlEncode(name)
    //        + '</strong>: ' + htmlEncode(message) + '</li>');
    //};

    // Functions
    hub.client.redirectToUrl = function (url) {
        window.location.href = url;
    };

    hub.client.alertOnJoin = function (name) {
        $('#players').append("<p>" + name + "</p>");
    };

    hub.client.alertOnLeave = function (name) {
        $('#players').append("<p>" + name + "</p>");
    };  

    hub.client.alertOnAccuse = function (name, id) {
        $('#Accuse_Ids_Div').append("<p class='Accuse-" + id + "'>" + name + "</p>");
    };

    hub.client.alertOnAcquit = function (id) {
        $('#Accuse_Ids_Div .Accuse-' + id).remove();
    };  

    // Start the connection.  
    $.connection.hub.start().done(function () {
        if (typeof (connectionStart) === 'function') {
            connectionStart();
        }     
    });
});

function AccuseAlert(name, id) {
    hub.server.alertAccuse(name, id);
}

function AcquitAlert(id) {
    hub.server.alertAcquit(id);
}

function JoinGameAlert() {
    hub.server.joinGroup(GlobalGameId, $.connection.hub.id, GlobalPlayerName);
}

function LeaveGame_Button() {
    hub.server.leaveGroup(GlobalGameId, $.connection.hub.id, GlobalPlayerName);
    window.location.href = '/Game/';
}


// Home page, used to create a new game
function CreateGame_Button() {
    // First validate the input
    $('input').removeClass('Empty-Input');
    if ($('#PlayerName_Input').val() === '') {
        $('#RoomCode_Input').addClass('Empty-Input');
        return;
    }

    $.ajax({
        url: '/Game/CreateGameHandler',
        type: "POST",
        datatype: JSON,
        data: { Player_Name: $('#PlayerName_Input').val() },
        success: function () {
            window.location.href = '/Game/';
        }
    });
}

// Home page, used to join an existing game
function JoinGame_Button() {
    // First validate the inputs
    $('input').removeClass('Empty-Input');
    if ($('#RoomCode_Input').val() === '') {
        $('#RoomCode_Input').addClass('Empty-Input');
        return;
    }
    else if ($('#PlayerName_Input').val() === '') {
        $('#RoomCode_Input').addClass('Empty-Input');
        return;
    }

    $.ajax({
        url: '/Game/JoinGameHandler',
        type: "POST",
        datatype: JSON,
        data: { Game_Id: $('#RoomCode_Input').val(), Player_Name: $('#PlayerName_Input').val() },
        success: function (data) {
            if (data === 'True') {
                window.location.href = '/Game/';
            }
            else {
                $('#InvalidCode_Div').show();
            }
        }
    });
}

// Start page, used to begin the first round
function StartGame_Button() {
    $.ajax({
        url: '/Game/StartGameHandler',
        type: "POST",
        datatype: JSON,
        data: {Game_Id: GlobalGameId},
        success: function () {
            hub.server.redirect('/Game/');
        }
    });
}

function ConfirmTarget_Button() {
    $.ajax({
        url: '/Game/ReadyHandler',
        type: "POST",
        datatype: JSON,
        data: { Game_Id: GlobalGameId, Player_Id: GlobalPlayerId, Phase: GlobalPhaseId, Selected_Card_Id: GlobalSelectedCard, Selected_Target_Id: GlobalSelectedTarget },
        success: function (data) {
            if (data === '0') {
                hub.server.redirect('/Game/');
            }
            else {
                $('#Ready_Count').html(data);
                ToggleBody('Ready_Body');
            }        
        }
    });
}

var CardHeading = '';
// select a particular card for this round
function PlayCard_Button(id, target) {
    GlobalSelectedCard = id;
    GlobalSelectedTarget = null;

    if (target === 'Other') {
        CardHeading = $('#SelectCard_Heading').html();
        Open_Partial_Div('Game', 'Game/Partials/Card_Target', GlobalGameId);
    }
    else {       
        ConfirmTarget_Button();
    }
}

// Accuse page, no vote reached
function NoVote_Button() {

    $.ajax({
        url: '/Game/CreateGameHandler',
        type: "POST",
        datatype: JSON,
        data: { Player_Name: $('#PlayerName_Input').val() },
        success: function () {
            window.location.href = '/Game/';
        }
    });
}

// Accuse page vote confirmed
function ConfirmVote_Button(count) {
    // First validate the input
    var elms = $('#Player_Votes .active');

    if ($(elms).length !== count) {
        return;
    }
    var vote_ids = "";
    $.each(elms, function () {
        vote_ids += "," + $(this).data('id');
    });

    $.ajax({
        url: '/Game/EndRoundHandler',
        type: "POST",
        datatype: JSON,
        data: { Game_Id: GlobalGameId, Vote_Ids: vote_ids },
        success: function (data) {
            Open_Partial_Div('String', 'Game/Partials/Vote_Result', data)

            setTimeout(function () {
                window.location.href = '/Game/';
            }, 6000);
       
        }
    });
}

function Accuse_Button(elm) {
    if ($(elm).hasClass('active')) {
        AccuseAlert($(elm).data('name'), $(elm).data('id'));
    }
    else {
        AcquitAlert($(elm).data('id'));
    }
    $($(elm).toggleClass('active'));
}

//////// Load new divs

function Load_Game_Partial(partial, div) {
    $.ajax({
        url: '/Home/Partial_Game',
        type: "POST",
        datatype: JSON,
        data: { Game_Id: GlobalGameId, Partial: partial },
        success: function (data) {
            $(div).html(data);
        }
    });
}

function Load_Player_Partial(partial, div) {
    $.ajax({
        url: '/Home/Partial_Player',
        type: "POST",
        datatype: JSON,
        data: { Partial: partial },
        success: function (data) {
            $(div).html(data);
        }
    });
}

function Load_Card_Partial(partial, div, id) {
    $.ajax({
        url: '/Home/Partial_Card',
        type: "POST",
        datatype: JSON,
        data: { Card_Id: id, Partial: partial },
        success: function (data) {
            $(div).html(data);
        }
    });
}

function Load_String_Partial(partial, div, id) {
    $.ajax({
        url: '/Home/Partial_String',
        type: "POST",
        datatype: JSON,
        data: { Data: id, Partial: partial },
        success: function (data) {
            $(div).html(data);
        }
    });
}

function Open_Partial_Div(type, partial, id) {
    var div = '#Partial_Body';
    switch (type) {
        case "Card":
            Load_Card_Partial(partial, div, id);
            break;
        case "Player":
            Load_Player_Partial(partial, div);
            break;
        case "Game":
            Load_Game_Partial(partial, div);
            break;
        case "String":
            Load_String_Partial(partial, div, id);
            break;
    }
    ShowPartialDiv();
}


function HidePartialDiv() {
    $('#Game_Body').fadeIn(200);
    $('#Partial_Body').hide();
}

function ShowPartialDiv() {
    $('#Game_Body').hide();
    $('#Partial_Body').fadeIn(200);
}

function ToggleBody(id) {
    $('.Body-Div').hide();
    $('#' + id).fadeIn(200);
}

//function Open_Partial_Modal(type, partial, id) {
//    var div = '#PartialModal_Content';
//    switch (type) {
//        case "Card":
//            Load_Card_Partial(partial, div, id);
//            break;
//        case "Player":
//            Load_Player_Partial(partial, div);
//            break;
//        case "Game":
//            Load_Game_Partial(partial, div);
//            break;
//    }
//    $('#PartialModal').modal();
//}