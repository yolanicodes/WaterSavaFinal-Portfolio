function showAlert(message){
    alert(message);
}
window.onload = function () {
    var messageField = document.getElementById('<%= hiddenMessage.ClientID %>');
    if (messageField.value) {
        showAlert(messageField.value);
        messageField.value = ''; // Clear the message after showing it
    }
}