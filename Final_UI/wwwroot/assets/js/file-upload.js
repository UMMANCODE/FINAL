(function ($) {
  'use strict';
  $(function () {
    $('.file-upload-browse').on('click', function () {
      var file = $(this).parent().parent().parent().find('.file-upload-default');
      file.trigger('click');
    });

    $('.file-upload-default').on('change', function () {
      var fileInput = $(this)[0];
      var fileNames = [];
      for (var i = 0; i < fileInput.files.length; i++) {
        fileNames.push(fileInput.files[i].name);
      }
      $(this).parent().find('.form-control').val(fileNames.join(', '));
    });
  });
})(jQuery);
