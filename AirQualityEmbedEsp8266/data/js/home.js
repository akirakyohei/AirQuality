function logout() {
    console.log("logout");
    window.open("/logout", "_self");
}

function updateMqtt() {
    console.log('mqtt');
    var deviceId = $('#device-id').val();
    var token = $('#token').val();
    var form = new FormData();
    form.append("deviceId", deviceId);
    form.append("token", token);
    $.ajax({
        type: "POST",
        url: "http://airquality.local/updateMQTT",
        data: form,
        processData: false,
        contentType: false,
        success: function (response) {
            var isSuccess = false;
            isSuccess = response.success;
            if (isSuccess) {
                $('#alert-notifier').html("<div>Update success!</div><small>Close to reset device.</smal>");
                $('#alert').addClass('alert-success');
                $('#alert').addClass('show');
                $(".alert").alert();
                $.ajax({
                    type: "GET",
                    url: "http://airquality.local/config.json",
                    success: function (result) {
                        console.log(result);
                        var obj = result;
                        $('#user-value').html(obj['username']);
                        $('#pass-value').html(obj['password']);
                        $('#devi-value').html(obj['device_id']);
                        $('#toke-value').html(obj['token']);
                    }
                });
            } else {

                $('#alert-notifier').html("<div>Update failed!</div><small>Try dot it</smal>");
                $('#alert').addClass('alert-danger');
                $('#alert').addClass('show');
                $(".alert").alert();
            }
        },
        error: function (jqXHR, exception) {
            console.log(exception);
        }
    });

}

function updateAccount() {
    console.log('account');
    var username = $('#username').val();
    var pass = $('#password').val();
    console.log(username);
    console.log(pass);
    var form = new FormData();
    form.append("username", username);
    form.append("password", pass);
    console.log(username);
    console.log(pass);
    $.ajax({
        type: "POST",
        url: "http://airquality.local/updateAccount",
        data: form,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log(response);
            try {

                var isSuccess = false;
                isSuccess = response.success;
                console.log(isSuccess);
                if (isSuccess) {
                    $('#alert-notifier').html("<div>Update success!</div><small>Close to reset device.</smal>");
                    $('#alert').addClass('alert-success');
                    $('#alert').addClass('show');
                    $(".alert").alert();
                    $.ajax({
                        type: "GET",
                        url: "http://airquality.local/config.json",
                        success: function (result) {
                            console.log(result);
                            var obj = result;
                            $('#user-value').html(obj['username']);
                            $('#pass-value').html(obj['password']);
                            $('#devi-value').html(obj['device_id']);
                            $('#toke-value').html(obj['token']);
                        }
                    });
                }
                console.log(isSuccess);
            } catch (error) {
                console.error(error)
            }

        }
    });

}

function reset() {

    $.ajax({
        type: "GET",
        url: "http://airquality.local/reset",
        success: function (response) {
            console.log("reset");
            alert("reset success");
        }
    });
}


var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");
var requestOptions = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow'
};

fetch("http://airquality.local/config.json", requestOptions)
    .then(response => response.text())
    .then(result => {
        console.log(result);
        var obj = JSON.parse(result);
        $('#user-value').html(obj['username']);
        $('#pass-value').html(obj['password']);
        $('#devi-value').html(obj['device_id']);
        $('#toke-value').html(obj['token']);


    })
    .catch(error => console.log('error', error));


// $('.alert').alert('close')
$(document).ready(function () {


});