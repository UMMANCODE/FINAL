$(document).ready(function () {
  var message = $('#temp-message').val();
  var type = $('#temp-type').val();

  if (message && type) {
    switch (type.toLowerCase()) {
    case 'success':
      toastr.success(message);
      break;
    case 'error':
      toastr.error(message);
      break;
    case 'warning':
      toastr.warning(message);
      break;
    case 'info':
      toastr.info(message);
      break;
    default:
      toastr.info(message);
    }
  }
});