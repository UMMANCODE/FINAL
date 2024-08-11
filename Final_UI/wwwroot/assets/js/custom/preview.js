$(document).ready(function () {
  $(".img-input").change(function (e) {
    let box = $(this).parent().find(".preview-box");
    $(box).find(".preview-img").remove();

    for (var i = 0; i < e.target.files.length; i++) {

      let img = document.createElement("img");
      img.style.width = "200px";
      img.classList.add("preview-img");

      let reader = new FileReader();
      console.log(e.target.nextElementSibling);
      reader.readAsDataURL(e.target.files[i]);
      reader.onload = () => {
        img.setAttribute("src", reader.result);
        $(box).append(img)
      }
    }
  })
});

$(document).ready(function () {
  $('.remove-img-icon').click(function () {
    var imgBox = $(this).closest('.img-box');
    var imageId = imgBox.find('input[type="hidden"]').val();

    // Create a new hidden input for each ID to delete
    var hiddenInput = $('<input>').attr({
      type: 'hidden',
      name: 'IdsToDelete',
      value: imageId
    });

    // Append the hidden input to the container
    $('#idsToDeleteContainer').append(hiddenInput);

    // Remove the image from the preview box
    imgBox.remove();
  });
});
